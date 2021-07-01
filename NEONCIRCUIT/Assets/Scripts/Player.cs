using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Camera playerCamera;
    public int ammo;
    public bool Killed;
    public float speed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        ammo = 6;
        Killed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            if (ammo > 0 && Killed == false)
            {
                ammo--;
                var bulletObject = Instantiate(bulletPrefab, gameObject.transform.position, gameObject.transform.rotation);
                Rigidbody bulletRB = bulletObject.GetComponent<Rigidbody>();
                bulletRB.AddForce(gameObject.transform.forward * speed);
                Destroy(bulletObject, 3f);
            }
    }
}