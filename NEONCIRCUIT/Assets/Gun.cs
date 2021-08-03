using UnityEngine;

public class Gun : MonoBehaviour
{

    public float damage = 10f;
    public float range = 100f;
    public Camera cam;
    public ParticleSystem particleSystem;
    public GameObject impactEffect;
    public float impactForce = 30f;
    public double firetime = 0;
    public double firerate = 0.3;
    private GameObject _player;
    public Enemy.AssignedColors color;
    private Quaternion v;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        v = particleSystem.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        firetime -= Time.deltaTime;
        if (Input.GetButton("Fire1"))
        {
            if(firetime <= 0)
            {
                Shoot();
                firetime = firerate;
                //test
            }
        }
    }

    void Shoot()
    {

        Debug.Log("SHOOOT");

        RaycastHit hit;
        if(Physics.Raycast(transform.position, cam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
            particleSystem.transform.LookAt(hit.transform.position, Vector3.up);
            particleSystem.Play();

            if (hit.transform.tag == "Hitbox")
            {
                var hbox = hit.transform.GetComponent<Hitbox>();
                hbox.Damage(_player, color, damage);
            }

            //Target t = hit.transform.GetComponent<Target>();

            //if(t != null)
            //{
            //    t.TakeDamage(damage);
            //}

            if(hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impact, 0.3f);
        }
        else
        {
            particleSystem.transform.rotation = v;
            particleSystem.Play();
        }
    }
}
