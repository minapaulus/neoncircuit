using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Attack/ShootWhenRdy")]
public class ShootWhenRdy : AttackPattern
{
    public GameObject Projectile;
    public float Lifetime = 10;
    public float startOffset = 5;
    public float velocity = 2000;
    public float homingforce = 1f;
    public float minFightingDistance = 5;

    public float damage = 10f;
    // uses only one weapon!

    public override void Execute(Transform me, Transform target, GameObject[] Weapon, GameObject Target, Color HPIndic)
    {
        for (int i = 0; i < Weapon.Length; i++)
        {
            var weapon = Weapon[i];
            var trans = weapon.transform;
            var angle = Quaternion.FromToRotation(Vector3.forward, trans.forward);
            var pos = trans.position + new Vector3(trans.forward.x * startOffset, trans.forward.y * startOffset, trans.forward.z * startOffset);
            var spawn = Instantiate(Projectile, pos, trans.rotation);
            var stats = spawn.GetComponentInChildren<Projectile>();
            stats.GetComponent<Rigidbody>().AddForce(stats.transform.forward * velocity);
            stats.GetComponent<Renderer>().material.SetColor("_EmissionColor", HPIndic);
            stats.lifetime = Lifetime;
            stats.targetID = Target.tag;
            stats.homingforce = homingforce;
            stats.target = target;
            stats.source = me.gameObject;
            stats.Damage = damage;

            var mats = stats.transform.GetChild(0).GetComponent<Renderer>().materials;
            foreach(Material mat in mats)
            {
                mat.color = HPIndic;
                mat.SetColor("_EmissionColor", HPIndic * 10);
            }
        }  
    }    
}
