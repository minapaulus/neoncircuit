using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public Enemy targetScript;
    public float damageMultiplier = 1f;

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
    public void Damage(GameObject me, Enemy.AssignedColors atkColor)
    {
        if (me.tag == "Player" && atkColor == targetScript.assignedColor)
        {
            //TODO: Here the damage from the gun is given into a var which is then multiplied with the multiplier. 
            var gunDamage = 10f;
            var damage = damageMultiplier * gunDamage;
            targetScript.DamageHP(damage);

        }
    }
}
