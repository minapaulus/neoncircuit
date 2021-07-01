using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "Scriptable Objects/Movement/RotationOnly")]
public class RotationOnly : MovementPattern
{
    public float RotationSpeed = 1;

    public override void Execute(Transform me, Transform target, GameObject[] obstacles, NavMeshAgent _agent )
    {
        var dir = Quaternion.LookRotation(target.position - me.position);
        me.rotation = Quaternion.Lerp(me.rotation, dir, Time.deltaTime * RotationSpeed);
    }
}
