using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public Playerstats playerstat;
    public float restore;

    //dictates how fast the pickup rotates around itself. 1 -> one rotation around y axis per second.
    [Range(0, 2)]
    public float rotationSpeed = .5f;

    private void Update()
    {
        this.transform.Rotate(new Vector3(0, rotationSpeed * 360 * Time.deltaTime, 0 ));
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerstat.AddHP(restore);
            Destroy(this.gameObject);
        }
    }
}
