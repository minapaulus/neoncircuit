using UnityEngine;

public class Shotgun : MonoBehaviour
{
    public float damage = 20f;
    public float range = 100f;
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
            for(int i = 0; i <= 25; i++)
            {
                var tilt = Quaternion.AngleAxis(Random.Range(0f, 45f), Vector3.up);
                var spin = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.forward);
                var tiltspin = spin * tilt;
                //var test = cam.transform.up * Random.Range(0.0f, 0.2f);
                //var vector = cam.transform.forward * 10f + Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), cam.transform.forward) * test;
                Ray ray = new Ray(transform.position, tiltspin * cam.transform.forward);
                RaycastHit hit;
                Debug.Log("test");
                if (Physics.Raycast(ray, out hit, range))
                {
                    Debug.Log(hit.transform.tag);
                    if (hit.transform.tag == "Hitbox")
                    {
                        var hbox = hit.transform.GetComponent<Hitbox>();
                        hbox.Damage(_player, color, damage);
                    }
                    if (hit.rigidbody != null)
                    {
                        hit.rigidbody.AddForce(-hit.normal * impactForce);
                    }
                    playSound();
                    GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(impact, 0.3f);
                }
            }
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

