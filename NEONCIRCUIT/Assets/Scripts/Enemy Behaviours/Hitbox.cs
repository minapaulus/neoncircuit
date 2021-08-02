using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public Enemy targetScript;
    public float damageMultiplier = 1f;

    public enum HitBoxType { basic, critical, indestructible, normal }

    // The type will be useful for registration. Sounds and behaviour of Explosives. ONLY ONE Hitbox shall have assigned basic per Enemy. This guarantees explosive damage is triggered once.
    public HitBoxType type;

    public Enemy.AssignedColors color; 

    public void OnTriggerEnter(Collider other)
    {
        //Das ist pseudoCode!! Muss man mit Mina noch absprechen wie die Schüsse wirklich funktionieren.
        if (other.tag != "Projectile" && targetScript.assignedColor == color)
        {
            //TODO: Here the damage from the gun is given into a var which is then multiplied with the multiplier. 
            var gunDamage = 10f;
            var damage = damageMultiplier * gunDamage;
            targetScript.DamageHP(damage);

        }
    }

    /* As we use raycasts, this might be an alternative to OnTriggerEnter*/
    public void Damage(GameObject me, Enemy.AssignedColors atkColor, float dmg)
    {
        if (me.tag == "Player" && (atkColor == targetScript.assignedColor || atkColor == Enemy.AssignedColors.Color4))
        {
            //TODO: Here the damage from the gun is given into a var which is then multiplied with the multiplier. 
            var damage = damageMultiplier * dmg;
            targetScript.DamageHP(damage);

        }
    }
}
