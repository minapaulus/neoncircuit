using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperBullet : Projectile
{
    public float homingspeed;
    public float Rotationspeed;
    private float timeelapsed = 0;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    override protected void Update()
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
        if (target != null)
        {
            Vector3 dir = target.position - this.transform.position;
            dir.Normalize();
            Vector3 RotAmoung = Vector3.Cross(transform.forward, dir);
            rb.angularVelocity = RotAmoung * Rotationspeed;
        }
        rb.velocity = transform.forward * homingspeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == targetID)
        {
            var stats = collision.gameObject.GetComponent<Playerstats>();
            stats.AddHP(- base.Damage);
        }
        Destroy(this.transform.parent.gameObject);
    }

}
