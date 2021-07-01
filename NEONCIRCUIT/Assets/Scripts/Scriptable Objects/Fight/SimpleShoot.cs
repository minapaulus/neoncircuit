using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Attack/SimpleShoot")]
public class SimpleShoot : AttackPattern
{
    public GameObject Projectile;
    public float Lifetime = 50;
    public float startOffset = 5;
    public float velocity = 1000;
    private int weaponindice = 0;


    public override void Execute(Transform me, Transform target, GameObject[] Weapon, GameObject Target)
    {
        // Projectile ausrichten
        //foreach (GameObject o in Weapon)
        {
            var trans = Weapon[weaponindice].transform;
            var angle = Quaternion.FromToRotation(Vector3.forward, trans.forward);
            var pos = trans.position + new Vector3 (trans.forward.x * startOffset, trans.forward.y * startOffset, trans.forward.z * startOffset);
            var spawn = Instantiate(Projectile, pos, angle);
            var stats = spawn.GetComponentInChildren<Projectile>();
            stats.GetComponent<Rigidbody>().AddForce(stats.transform.forward * velocity);
            stats.lifetime = Lifetime;
            stats.targetID = Target.tag;
            stats.source = me.gameObject;
            weaponindice = (weaponindice + 1) % 2;
        }
    }
}
