using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementPattern : ScriptableObject
{
    public abstract void Execute(Transform me, Transform target, GameObject[] obstacles);
}
