using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime = 0;
    private float timeelapsed = 0;
    public float homingforce = 0;
    public string targetID = null;
    public GameObject source = null;
    public Transform target;
    public float Velocity = 0;

    public float Damage = 0f;

    protected virtual void Update()
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
        if (target != null) this.transform.LookAt(target.position);
        GetComponent<Rigidbody>().AddForce(this.transform.forward * homingforce * Time.deltaTime);
    }

    public void LoseTarget()
    {
        target = null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag != "Projectile"){
            Debug.Log("Hit object With Tag " + collision.transform.tag);
            if (collision.gameObject.tag == "Player")
            {
                var stats = collision.gameObject.GetComponent<Playerstats>();
                stats.AddHP(-Damage);
            }
            Destroy(this.transform.parent.gameObject);
        } else
        {
                Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());            
        }
    }
}
