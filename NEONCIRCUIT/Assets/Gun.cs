using UnityEngine;

public class Gun : MonoBehaviour
{

    public float damage = 10f;
    public float range = 100f;
    public float costs = 1f;
    public Camera cam;
    public GameObject impactEffect;
    public float impactForce = 30f;
    public double firetime = 0;
    public double firerate = 0.3;
    private GameObject _player;
    public Enemy.AssignedColors color;
    private Quaternion v;
    public Playerstats playerstats;
    public AudioClip[] gsounds;
    private AudioSource asource;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        playerstats.ChangePrimaryColor(color);
        asource = GetComponent<AudioSource>();
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
        if (Physics.Raycast(transform.position, cam.transform.forward, out hit, range))
        {
            if(playerstats.CanFirePrimary()) {
                playerstats.AddPrimary(-costs);
                if (hit.transform.tag == "Hitbox")
                {
                    var hbox = hit.transform.GetComponent<Hitbox>();
                    hbox.Damage(_player, playerstats.primaryColor, damage);
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
    }

    void playSound()
    {
        var i = Random.Range(1, gsounds.Length);
        asource.clip = gsounds[i];
        asource.PlayOneShot(asource.clip);
        gsounds[i] = gsounds[0];
        gsounds[0] = asource.clip;
    }
}
