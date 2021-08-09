using UnityEngine;

public class Shotgun : MonoBehaviour
{
    public float damage = 20f;
    public float range = 300f;
    public Camera cam;
    public GameObject impactEffect;
    public float impactForce = 200f;
    public double firetime = 0;
    public double firerate = 2;
    private GameObject _player;
    public Enemy.AssignedColors color;
    private Quaternion v;
    public Playerstats playerstats;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        playerstats.ChangePrimaryColor(color);
    }

    // Update is called once per frame
    void Update()
    {
        firetime -= Time.deltaTime;
        if (Input.GetButton("Fire1"))
        {
            if (firetime <= 0)
            {
                Shoot();
                firetime = firerate;
            }
        }
    }

    void Shoot()
    {

        Debug.Log("SHOOOT");

        RaycastHit hit;
        if (Physics.Raycast(transform.position, cam.transform.forward, out hit, range))
        {
            if (playerstats.CanFireSecondary())
            {
                if (hit.transform.tag == "Hitbox")
                {
                    var hbox = hit.transform.GetComponent<Hitbox>();
                    hbox.Damage(_player, color, damage);
                }
                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * impactForce);
                }
                GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impact, 0.3f);
            }
        }
    }
}

