using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime = 0;
    private float timeelapsed = 0;
    public string targetID = null;
    public GameObject source = null;
    public float Velocity = 0;

    private void Update()
    {
        if (timeelapsed > lifetime)
        {
            Destroy(this.transform.parent.gameObject);
        }
        else
        {
            timeelapsed += Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            var stats = collision.gameObject.GetComponent<Playerstats>();
            stats.AddHP(-10); 
        }
        Destroy(this.transform.parent.gameObject);
    }
}
