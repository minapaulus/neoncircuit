using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime = 0;
    private float timeelapsed = 0;
    public float corrector = 0;
    public string targetID = null;
    public GameObject source = null;
    public Transform target;
    public float Velocity = 0;

    private void Update()
    {
        if (timeelapsed > lifetime)
        {
            Destroy(this.transform.parent.gameObject);
        }
        else
        {
            CorrectPath();
            timeelapsed += Time.deltaTime;
        }
    }

    private void CorrectPath()
    {
        var direction = this.transform.position - target.position;
        var newdir = this.transform.forward - direction;
        newdir.Normalize();
        GetComponent<Rigidbody>().AddForce(newdir* corrector * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == targetID)
        {
            var stats = collision.gameObject.GetComponent<Playerstats>();
            stats.AddHP(-10); 
        }
        Destroy(this.transform.parent.gameObject);
    }
}
