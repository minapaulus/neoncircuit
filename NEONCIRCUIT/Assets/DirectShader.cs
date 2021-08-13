using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectShader : MonoBehaviour
{
    public GameObject cam;
    public GameObject target;
    public Material mat; 

    // Update is called once per frame
    void Update()
    {
        
        var direction = (cam.transform.position - target.transform.position);
        //Debug.Log(direction);
        Vector2 matdir = new Vector2(direction.x, -direction.z);
        matdir.Normalize();
        mat.SetVector("Direction", matdir);
       
    }
}
