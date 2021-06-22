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
    private int weaponindice = 0;
    public float minFightingDistance = 5;
    // uses only one weapon!

    public override void Execute(Transform me, Transform target, GameObject[] Weapon, GameObject Target)
    {
        var weapon = Weapon[0];
        var trans = weapon.transform;
        var angle = Quaternion.FromToRotation(Vector3.forward, trans.forward);
        var pos = trans.position + new Vector3(trans.forward.x * startOffset, trans.forward.y * startOffset, trans.forward.z * startOffset);
        var spawn = Instantiate(Projectile, pos, angle);
        var stats = spawn.GetComponentInChildren<Projectile>();
        stats.GetComponent<Rigidbody>().AddForce(stats.transform.forward * velocity);
        stats.lifetime = Lifetime;
        stats.targetID = Target.tag;
        stats.source = me.gameObject;
                
            
        


        
    }
}
