using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackPattern : ScriptableObject
{
    public float Attackspeed;


    public abstract void Execute(Transform me, Transform target, GameObject[] Weapon, GameObject Target, Color Hpindic);
}
