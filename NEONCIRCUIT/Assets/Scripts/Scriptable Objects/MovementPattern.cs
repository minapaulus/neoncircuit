using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class MovementPattern : ScriptableObject
{
    public abstract void Execute(Transform me, Transform target, GameObject[] obstacles, NavMeshAgent _agent);
}
