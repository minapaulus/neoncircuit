using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class throwable : MonoBehaviour
{
    private bool _cooked = false;
    public float cookingTime = 2f;
    private float _duration = 0f;

    public float Blastforce = 50f;

    public GameObject explosion;
    private bool _triggered = false;

    public float damage = 50f;

    public float Blastradius = 10f;

    private GameObject _player;

    public Enemy.AssignedColors color; 



    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (_cooked)
        {
            if(_duration >= cookingTime)
            { 
                Explode();
            } else
            {
                if(_duration >= cookingTime - 1f && !_triggered)
                {
                    _triggered = true;
                    var exp = Instantiate(explosion, transform.position, transform.rotation);
                    GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
                }
                _duration += Time.deltaTime;
            }
            
        }
        
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, Blastradius);

        foreach(Collider nearbyEnemy in colliders)
        {
            if (nearbyEnemy.tag == "Hitbox")
            {
                var hitboxenemy = nearbyEnemy.GetComponent<Hitbox>();
                if(hitboxenemy.type == Hitbox.HitBoxType.basic)
                {
                    hitboxenemy.Damage(_player, color, damage );
                }

            }

            if(nearbyEnemy.tag == "Player")
            {
                var dir = transform.position - nearbyEnemy.transform.position;
                dir.Normalize();
                nearbyEnemy.GetComponent<Rigidbody>().AddForce(dir * Blastforce);
            }
        }

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        _cooked = true; 
    }
}
