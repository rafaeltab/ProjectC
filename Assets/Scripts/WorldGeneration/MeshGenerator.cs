using Assets.Scripts;
using NoiseTest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

//TODO:
//-optimize
//-implement Marching Cubes

public class MeshGenerator : MonoBehaviour
{
    private KeyCode wup;
    private KeyCode wdown;

    public KeyCode WUp
    {
        get
        {
            return wup;
        }
    }
    public KeyCode WDown
    {
        get
        {
            return wdown;
        }
    }

    public MeshGenModel model;
    public ComputeShader compute;
    private MeshGenModel previous;

    private bool generated = false;
    private Vector3Int oldLoadChunk;

    private Stopwatch watch = new Stopwatch();

    public double time { get
        {
            return watch.Elapsed.TotalSeconds;
        }
    }

    /// <summary>
    /// Do everything that needs to be done when this gameobject is created
    /// </summary>
    void Start()
    {
        ListenToSettings();

        watch.Start();
        if (!generated)
        {
            if (model.template != null)
            {
                AssureObjects();

                Generate(true);
                previous = model;
                generated = true;
            }
        }        
    }

    /// <summary>
    /// make sure we are listening to the SettingsManager for updates in our controls
    /// </summary>
    private void ListenToSettings()
    {
        Setting wu = SettingsManager.GetInstance().Settings[0].GetSetting("wup");
        Setting wd = SettingsManager.GetInstance().Settings[0].GetSetting("wdown");
        wu.changeEvent += (sender, value) =>
        {
            wup = (KeyCode)value;
        };

        wd.changeEvent += (sender, value) =>
        {
            wup = (KeyCode)value;
        };

        wup = (KeyCode)wu.Value;
        wdown = (KeyCode)wd.Value;
    }

    /// <summary>
    /// Do everything that needs to be done every frame like updating the 
    /// </summary>
    void Update()
    {
        DoInputs();

        Vector3Int currentLoad = new Vector3Int((int) (model.loadPoint.position.x/model.size)+1, (int)(model.loadPoint.position.y / model.size), (int)(model.loadPoint.position.z / model.size) + 1);

        if (currentLoad != oldLoadChunk)
        {
            generated = false;
            StopCurrentGen();
        }

        if (!generated)
        {
            AssureObjects();

            Generate(genFull);
            genFull = false;
            previous = model;
            generated = true;
        }

        previous = new MeshGenModel(model);
        oldLoadChunk = currentLoad;
    }

    public bool gen = true;
    private Coroutine currentGen;
    private bool genFull = false;    

    /// <summary>
    /// Alter the wPos based on the current inputs
    /// </summary>
    private void DoInputs()
    {
        if (Input.GetKey(wup))
        {
            if (watch.Elapsed.TotalSeconds > model.wPosUpdateRate)
            {
                StopCurrentGen();
                watch.Restart();
            }
            model.wPos += 0.05f;
            generated = false;
            genFull = true;
            
        }
        if (Input.GetKey(wdown))
        {
            if (watch.Elapsed.TotalSeconds > model.wPosUpdateRate)
            {
                StopCurrentGen();
                watch.Restart();
            }
            model.wPos -= 0.05f;
            generated = false;
            genFull = true;
            
        }
    }

    /// <summary>
    /// Check if the right ammount of Game Objects have been created for this render distance
    /// </summary>
    private void AssureObjects()
    {
        int loadAmount = (int)Math.Pow(1 + model.renderDistance * 2, 3);
        if (model.template != null && loadAmount != chunks.Count)
        {
            for (int i = 0; i < chunks.Count; i++)
            {
                Destroy(chunks[i]);
            }
            chunks.Clear();
            for (int i = 0; i < loadAmount; i++)
            {
                GameObject go = Instantiate(model.template,transform);
                chunks.Add(go);
                chunkLocs.Add(null);
                wPosses.Add(float.MaxValue);
            }
        }
    }

    /// <summary>
    /// Generate the chunks only if not currently genning
    /// </summary>
    private void Generate(bool full)
    { 
        if (gen)
        {
            int offsetX = (int) (model.loadPoint.position.x / model.size) - model.renderDistance;
            int offsetY = (int) (model.loadPoint.position.y / model.size) - model.renderDistance;
            int offsetZ = (int) (model.loadPoint.position.z / model.size) - model.renderDistance;

            Vector3Int offset = new Vector3Int(offsetX,offsetY,offsetZ);
            currentGen = StartCoroutine(AllChunks(offset,full));
        }
    }  
    
    /// <summary>
    /// Stop current generation
    /// </summary>
    public void StopCurrentGen()
    {
        if (currentGen != null && !genFull)
        {
            StopCoroutine(currentGen);
            gen = true;
        }
    }

    private List<GameObject> chunks = new List<GameObject>();
    private List<Vector3Int?> chunkLocs = new List<Vector3Int?>();
    private List<float> wPosses = new List<float>();

    /// <summary>
    /// Generate all chunks
    /// </summary>
    /// <param name="offset">Location offset</param>
    /// <param name="full">generate everything or only new chunks NOT IMPLEMENTED</param>
    /// <returns></returns>
    private IEnumerator AllChunks(Vector3Int offset,bool full)
    {
        gen = false;

        List<Vector3Int> ToBeSet = new List<Vector3Int>();

        int s = 1 + model.renderDistance * 2;
        for (int x = 0; x < s; x++)
        {
            for (int y = 0; y < s; y++)
            {
                for (int z = 0; z < s; z++)
                {
                    Vector3Int loc = new Vector3Int(x + offset.x, y + offset.y, z + offset.z);
                    ToBeSet.Add(loc);
                }
            }
        }
        ToBeSet = Helper.Order(ToBeSet, model.size, model.loadPoint).ToList();
        var cac = GetChangeChunks(ToBeSet, full);
        var gos = cac.Item1;
        var locs = cac.Item2;
        var linxeses = cac.Item3;

        for (int i = 0; i < gos.Count; i++)
        {
            Chunk c = new Chunk(locs[i], model.wPos, 1, model.threshold,compute,this,model.visualizer, model.size, model.noiseScale);
            c.Display(gos[i]);
            chunkLocs[linxeses[i]] = locs[i];
            wPosses[linxeses[i]] = model.wPos;
            yield return null;
        }

        gen = true;
    }

    /// <summary>
    /// Get the chunks that can be changed and the chunkLocations that need to be changed
    /// </summary>
    /// <param name="ToBeSet">Locations of chunks that are in render distance</param>
    /// <param name="fullRegen">Whether or not to fully regen</param>
    private Tuple<List<GameObject>, List<Vector3Int>, List<int>> GetChangeChunks(List<Vector3Int> ToBeSet,bool fullRegen)
    {
        //get all the locations that are in tbs but not in cl        
        List<Vector3Int> ToBeGenned = new List<Vector3Int>();

        List<Vector3Int> cl = Helper.GetNotNulls(chunkLocs).ToList();
        int ind = 0;
        foreach (var tbs in ToBeSet)
        {
            if (!cl.Contains(tbs) || fullRegen)
            {
                ToBeGenned.Add(tbs);
            }
            else
            {
                if (wPosses[chunkLocs.IndexOf(tbs)] != model.wPos)
                {
                    ToBeGenned.Add(tbs);
                }
            }
            ind++;
        }

        //get all the indexes of locations that are in cl but not in tbs
        List<GameObject> gos = new List<GameObject>();
        List<int> indexes = new List<int>();
        ind = 0;
        
        foreach (var c in chunkLocs)
        {
            if (!c.HasValue || fullRegen || wPosses[ind] != model.wPos)
            {
                gos.Add(chunks[ind]);
                indexes.Add(ind);
            }
            else
            {                
                if (!ToBeSet.Contains(c.Value))
                {
                    gos.Add(chunks[ind]);
                    indexes.Add(ind);
                }
            }
            ind++;
        }
        return Tuple.Create(gos, ToBeGenned,indexes);
    }
}

/// <summary>
/// A way to say if a certain function (coroutine or async) should stop executing
/// </summary>
public class StopToken
{
    public bool stop { get; set; } = false;
}
