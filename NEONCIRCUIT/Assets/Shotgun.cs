using UnityEngine;

public class Shotgun : MonoBehaviour
{
    public float damage = 20f;
    public float pellets = 25f;
    public float range = 100f;
    public float maxSpread = 10f;
    public Camera cam;
    public GameObject impactEffect;
    public float impactForce = 200f;
    public double firetime = 0;
    public double firerate = 2;
    private GameObject _player;
    public Enemy.AssignedColors color;
    public Playerstats playerstats;
    public AudioClip[] sgsounds;
    private AudioSource sasource;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        playerstats.ChangePrimaryColor(color);
        sasource = GetComponent<AudioSource>();
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

        if (playerstats.CanFireSecondary())
        {
            for(int i = 0; i < pellets; i++)
            {
                /*
                var offset = transform.up * Random.Range(0.0f, 5.0f);
                offset = Quaternion.AngleAxis(Random.Range(0.0f, 30.0f), transform.forward) * offset;
                var hitv = transform.forward * 10.0f + offset;
                var vector = hitv - transform.position;
                vector.Normalize();
                //Ray ray = new Ray(vector, transform.position);
                */

                //Try Niklas the first: 
                Quaternion originalCamRot = cam.transform.rotation;
                Vector3 vector = cam.transform.forward;
                Quaternion camAngle = cam.transform.rotation;

                Quaternion angle = Random.rotation;
                camAngle = Quaternion.RotateTowards(camAngle, angle, maxSpread);

                vector = camAngle * vector;
                vector.Normalize();

                cam.transform.rotation = camAngle;
                RaycastHit hit;
                if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range))
                {
                    Debug.Log(hit.transform.tag);
                    if (hit.transform.tag == "Hitbox")
                    {
                        var hbox = hit.transform.GetComponent<Hitbox>();
                        hbox.Damage(_player, color, damage/pellets);
                    }
                    if (hit.rigidbody != null)
                    {
                        hit.rigidbody.AddForce(-hit.normal * impactForce);
                    }
                    GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(impact, 0.3f);
                }
                cam.transform.rotation = originalCamRot;
            }

            playSound();
            playerstats.AddSecondary(-1);
            //if (Physics.Raycast(transform.position, cam.transform.forward, out hit, range))
        }
    }

    void playSound()
    {
        var i = Random.Range(1, sgsounds.Length);
        sasource.clip = sgsounds[i];
        sasource.PlayOneShot(sasource.clip);
        sgsounds[i] = sgsounds[0];
        sgsounds[0] = sasource.clip;
    }
}

