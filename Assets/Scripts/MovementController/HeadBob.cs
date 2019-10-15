using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 

public class HeadBob : MonoBehaviour
{
    Vector2 LastPosition;
    Vector2 CurrentPosition;
 
    void FixedUpdate()
    {
    }
 
    void Update()
    {
 
    }
 
    private void LateUpdate()
    {
        CurrentPosition = -Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
 
        if (Input.GetMouseButton(0))
        {
            Vector2 pos = (CurrentPosition - LastPosition);
            this.transform.position += new Vector3(pos.x, pos.y, 0);
        }
 
        LastPosition = -Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane)); //This line was the problem LastPosition = CurrentPosition may have overwritten some values???
    }

}
