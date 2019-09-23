using Assets.Scripts;
using NoiseTest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    public MeshGenModel model;
    private MeshGenModel previous;

    public GameObject template;
    public MeshGenerator()
    {
    }

    // Start is called before the first frame update
    async void Start()
    {
        if (!model.created)
        {
            model.created = true;
            await Gen();
        }
        previous = model;
    }

    // Update is called once per frame
    async Task Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            model.wPos += 0.05f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            model.wPos -= 0.05f;
        }

        if (!model.created)
        {
            model.created = true;
            await Gen();
        }
        else if (model.autoUpdate)
        {
            if (previous.wPos != model.wPos)
            {
                await Gen();
            }
        }
        previous = new MeshGenModel(model);
    }

    List<GameObject> chunks = new List<GameObject>(); 

    public async Task Gen()
    {
        Stopwatch watch = new Stopwatch();
        watch.Start();
        foreach (var chunk in chunks)
        {
            Destroy(chunk);
        }

        for (int x = 0; x < model.chunks.x; x++)
        {
            for (int y = 0; y < model.chunks.y; y++)
            {
                for (int z = 0; z < model.chunks.z; z++)
                {
                    StartCoroutine(GenSingleCoroutine(x,y,z));
                }
            }
        }
        watch.Stop();
        UnityEngine.Debug.Log($"Total Generation time was {watch.Elapsed}");

    }

    public float[,,] Generate(int chunkX, int chunkY, int chunkZ)
    {
        Stopwatch watch = new Stopwatch();
        watch.Start();

        SimplexNoise sn = new SimplexNoise(1);

        float[,,] values = new float[model.sizeX, model.sizeY, model.sizeZ];
        
        //go through each block position
        for (int x = 0; x < model.sizeX; x++)
        {
            for (int y = 0; y < model.sizeY; y++)
            {
                for (int z = 0; z < model.sizeZ; z++)
                {

                    //get value of the noise at given x, y, and z.
                    float noiseValue = ((float)sn.Evaluate((x+chunkX*model.sizeX) * model.noiseScale, (y + chunkY * model.sizeY) * model.noiseScale, (z + chunkZ * model.sizeZ) * model.noiseScale, model.wPos))/2+0.5f;
                    values[x, y, z] = noiseValue;
                    
                }
            }
        }
        watch.Stop();
        UnityEngine.Debug.Log($"Generation time was {watch.Elapsed}");
        return values;
    }
    
    public Mesh Visualise(float[,,] values)
    {
        Stopwatch watch = new Stopwatch();
        watch.Start();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        for (int x = 0; x < model.sizeX; x++)
        {
            for (int y = 0; y < model.sizeY; y++)
            {
                for (int z = 0; z < model.sizeZ; z++)
                {
                    if (Enabled(values[x,y,z]))
                    {
                        Vector3 current = new Vector3(x,y,z);
                        #region x
                        if (values.GetLength(0) == x + 1)
                        {                            
                            CreateFace(drawRight(),vertices,triangles, uvs, current);
                        }else if (!Enabled(values[x+1,y,z]))
                        {                            
                            CreateFace(drawRight(), vertices, triangles, uvs, current);
                        }

                        if (x == 0)
                        {                            
                            CreateFace(drawLeft(), vertices, triangles, uvs, current);
                        }else if (!Enabled(values[x - 1, y, z]))
                        {
                            CreateFace(drawLeft(), vertices, triangles, uvs, current);
                        }
                        #endregion x

                        #region y
                        if (values.GetLength(1) == y + 1)
                        {
                            
                            CreateFace(drawUp(), vertices, triangles, uvs, current);
                        }else if (!Enabled(values[x, y + 1, z]))
                        {
                            
                            CreateFace(drawUp(), vertices, triangles, uvs, current);
                        }

                        if (y == 0)
                        {
                            
                            CreateFace(drawDown(), vertices, triangles, uvs, current);
                        }else if (!Enabled(values[x, y - 1, z]))
                        {
                            
                            CreateFace(drawDown(), vertices, triangles, uvs, current);
                        }
                        #endregion y

                        #region z
                        if (values.GetLength(2) == z + 1)
                        {
                            CreateFace(drawFar(), vertices, triangles, uvs, current);
                        }else if (!Enabled(values[x, y, z + 1]))
                        {
                            CreateFace(drawFar(), vertices, triangles, uvs, current);
                        }

                        if (z == 0)
                        {
                            CreateFace(drawNear(), vertices, triangles, uvs, current);
                        }else if (!Enabled(values[x, y, z - 1]))
                        {
                            CreateFace(drawNear(),vertices,triangles, uvs, current);
                        }
                        #endregion z
                    }
                }
            }
        }

        Mesh mesh = new Mesh();
        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
        watch.Stop();
        UnityEngine.Debug.Log($"Visualisation time was {watch.Elapsed}");
        return mesh;
    }

    public IEnumerator GenSingleCoroutine(int x, int y, int z)
    {
        Mesh m = Visualise(Generate(x, y, z));

        template.transform.position = new Vector3(x * model.sizeX, y * model.sizeY, z * model.sizeZ);
        GameObject go = Instantiate(template,transform);
        go.GetComponent<MeshFilter>().mesh = m;
        chunks.Add(go);

        yield return null;
    }

    #region draw methods
    private static readonly Vector3 up = new Vector3() { x = 0, y = 1, z = 0 }; // up
    private static readonly Vector3 rightup = new Vector3() { x = 0, y = 1, z = 1 }; // rightup
    private static readonly Vector3 nearup = new Vector3() { x = 1, y = 1, z = 0 }; // nearup
    private static readonly Vector3 nearrightup = new Vector3() { x = 1, y = 1, z = 1 }; // nearrightup
    private static readonly Vector3 near = new Vector3() { x = 1, y = 0, z = 0 }; // near
    private static readonly Vector3 nearright = new Vector3() { x = 1, y = 0, z = 1 }; // nearright
    private static readonly Vector3 basis = new Vector3() { x = 0, y = 0, z = 0 }; // base
    private static readonly Vector3 right = new Vector3() { x = 0, y = 0, z = 1 }; // right


    public Vector3[] drawUp()
    {
        return new Vector3[] { up, rightup, nearup, nearrightup };
    }

    public Vector3[] drawDown()
    {
        return new Vector3[] { near, nearright, basis, right };
    }

    public Vector3[] drawLeft()
    {
        return new Vector3[] { basis, right, up, rightup };
    }


    public Vector3[] drawRight()
    {
        return new Vector3[] { nearup, nearrightup, near, nearright };
    }

    public Vector3[] drawNear()
    {
        return new Vector3[] { basis, up, near, nearup };
    }

    public Vector3[] drawFar()
    {
        return new Vector3[] { nearright, nearrightup, right, rightup };
    }

    public void CreateFace(Vector3[] verticesTBA,List<Vector3> vertices, List<int> triangles, List<Vector2> uvs, Vector3 pos)
    {
        verticesTBA[0] = Add(verticesTBA[0], pos);
        verticesTBA[1] = Add(verticesTBA[1], pos);
        verticesTBA[2] = Add(verticesTBA[2], pos);
        verticesTBA[3] = Add(verticesTBA[3], pos);

        int b = vertices.Count;

        foreach (Vector3 v in verticesTBA)
        {
            vertices.Add(v);
            uvs.Add(new Vector2(0,1));
        }
       
        triangles.Add(b);
        triangles.Add(b + 1);
        triangles.Add(b + 2);
        triangles.Add(b + 2);
        triangles.Add(b + 1);
        triangles.Add(b + 3);
    }
    #endregion draw methods

    public bool Enabled(float val)
    {
        return val >= model.threshold;
    }

    public Vector3 Add(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
    }
}
