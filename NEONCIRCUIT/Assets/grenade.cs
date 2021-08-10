using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grenade : MonoBehaviour
{
    public GameObject Throwable;
    public float throwStrength; 

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1)){
            var grenadina = Instantiate(Throwable, transform.position, transform.rotation);
            grenadina.GetComponent<Rigidbody>().AddForce(transform.forward * throwStrength, ForceMode.VelocityChange);
        }
    }
}