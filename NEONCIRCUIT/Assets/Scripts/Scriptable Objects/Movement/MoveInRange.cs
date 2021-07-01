using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "Scriptable Objects/Movement/MoveInRange")]
public class MoveInRange : MovementPattern
{
    [Range(0, 10)]
    public float minDistance = 0f;
    [Range(0, 20)]
    public float maxDistanceObstacle = 0f;
    [Range(0, 50)]
    public float rotationSpeed = 10f;
    public bool needsCover = true;
    private bool _isInCover = false;
    // is changed by attackscript. He stands still for a few moments to prepare and then shoot.

    public float obsDistance = 1;


    public override void Execute(Transform me, Transform target, GameObject[] obstacles, NavMeshAgent _agent)
    {

        var pos = new Vector3(0, 0, 0);
        //TODO: Find nearest point to target
        pos = target.position;
        if (!((me.position - pos).magnitude > minDistance))
        {
            //Add artificial rotation, as the target is not the player from the view of the agent.
            var q = Quaternion.LookRotation(target.position - me.position);
            me.rotation = Quaternion.Lerp(me.rotation, q, Time.deltaTime * rotationSpeed);
            pos = me.position;
        }
        
        _agent.SetDestination(pos);

        //TODO: Find nearest Obstacle and run to opposite side.

        //TODO: When attack is ready, get in line of position

        //TODO: Prohibit enemies walking into eachother.
    }


}
