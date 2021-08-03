using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Attack/Sniper")]
public class Sniper : AttackPattern
{
    public GameObject Projectile;
    public float Lifetime = 10;
    public float startOffset = 5;
    public float velocity = 0f;
    public float homingSpeed = 1f;
    public float homingRotation = 1f;
    public float minFightingDistance = 5;

    public float damage = 30f;
    // uses only one weapon!
    public Projectile ShootProjectile(Transform me, Transform target, GameObject Weapon, Color HPIndic)
    {

        var trans = Weapon.transform;
        var angle = Quaternion.FromToRotation(Vector3.forward, trans.forward);
        var pos = trans.position + new Vector3(trans.forward.x * startOffset, trans.forward.y * startOffset, trans.forward.z * startOffset);
        var spawn = Instantiate(Projectile, pos, angle);
        var stats = spawn.GetComponentInChildren<SniperBullet>();
        stats.GetComponent<Rigidbody>().AddForce(stats.transform.forward * velocity);
        stats.GetComponent<Renderer>().material.SetColor("_EmissionColor", HPIndic);
        stats.lifetime = Lifetime;
        stats.targetID = target.gameObject.tag;
        stats.homingspeed = homingSpeed;
        stats.Rotationspeed = homingRotation;
        stats.target = target;
        stats.source = me.gameObject;
        stats.Damage = damage;
        return stats;
    }

    //wird nicht aufgerufen.
    public override void Execute(Transform me, Transform target, GameObject[] Weapon, GameObject Target, Color HPIndic)
    {
        me.transform.LookAt(target.position);
    }
}
