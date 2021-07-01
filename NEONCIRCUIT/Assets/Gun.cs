using UnityEngine;

public class Gun : MonoBehaviour
{

    public float damage = 10f;
    public float range = 100f;
    public Camera cam;
    public ParticleSystem particleSystem;
    public GameObject impactEffect;
    public float impactForce = 30f;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        particleSystem.Play();

        RaycastHit hit;
        if(Physics.Raycast(transform.position, cam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            Target t = hit.transform.GetComponent<Target>();

            if(t != null)
            {
                t.TakeDamage(damage);
            }

            if(hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impact, 2f);
        }
    }
}
