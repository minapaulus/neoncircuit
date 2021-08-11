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
    public AudioClip sound1;
    private AudioSource ssource;
    public float Damage = 0f;

    private void Start()
    {
        ssource = GetComponent<AudioSource>();
        playSound(sound1);
    }

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

    void playSound(AudioClip sound)
    {
        ssource.clip = sound;
        ssource.loop = true;
        ssource.Play();
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
            if (collision.gameObject.tag == targetID)
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
