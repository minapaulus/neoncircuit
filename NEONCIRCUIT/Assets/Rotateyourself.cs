using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotateyourself : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        this.transform.RotateAround(this.transform.position, this.transform.up, -9f * Time.deltaTime);
        
    }
}
