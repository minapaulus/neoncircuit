using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "Scriptable Objects/Movement/MoveInCoverNearPlayer")]
public class MoveInCoverNearPlayer : MovementPattern
{
    [Range(0, 10)]
    public float minDistance = 0f;
    [Range(0, 20)]
    public float maxDistanceObstacle = 0f;
    [Range(0, 50)]
    public float rotationSpeed = 10f;
    public bool isInCover = false;
    // is changed by attackscript. He stands still for a few moments to prepare and then shoot.
    public bool isAttacking = false;

    public float obsDistance = 1;


    public override void Execute(Transform me, Transform target, GameObject[] obstacles, NavMeshAgent _agent)
    {   // Kann in Script ausgelagert werden und Moves weitergegeben werden.

        var pos = new Vector3(0, 0, 0);
        //Debug.Log(obstacles.Length);
        RaycastHit hit;
        var rayDirection = target.position - me.position;
        if (Physics.Raycast(me.position, rayDirection, out hit))
        {
            //Debug.Log(hit.ToString());
            if (hit.transform == target)
            {
                // enemy can see the player!
                isInCover = false;
            }
            else isInCover = true;
        }

        if (isInCover)
        {
            var q = Quaternion.LookRotation(target.position - me.position);
            me.rotation = Quaternion.Lerp(me.rotation, q, Time.deltaTime * rotationSpeed);
            pos = me.position;
        }
        else
        {
            pos = SearchCover(target, obstacles);
            // If no obstacle is near our NPC, he engages headon.
            if ((me.position - pos).magnitude > maxDistanceObstacle)
            {
                
            
                pos = target.position;
                if (!((me.position - pos).magnitude > minDistance) || isAttacking)
                {
                    //Add artificial rotation, as the target is not the player from the view of the agent.
                    var q = Quaternion.LookRotation(target.position - me.position);
                    me.rotation = Quaternion.Lerp(me.rotation, q, Time.deltaTime * rotationSpeed);
                    pos = me.position;
                    isInCover = false;
                }

            }
        }
        _agent.SetDestination(pos);

        //TODO: Find nearest Obstacle and run to opposite side.

        //TODO: When attack is ready, get in line of position

        //TODO: Prohibit enemies walking into eachother.
    }


    private Vector3 SearchCover(Transform target, GameObject[] obstacles)
    {
        var q = new Vector3(0, 0, 0);
        GameObject closest = null;
        float smallestDistance = float.MaxValue;
        Vector3 sdirection = new Vector3(0, 0, 0);
        foreach (GameObject obs in obstacles)
        {
            Debug.Log(obs.ToString());
            Vector3 diff = obs.transform.position - target.position;
            var distance = diff.sqrMagnitude;
            if (distance < smallestDistance)
            {
                sdirection = new Vector3(diff.x, 0, diff.z);
                closest = obs;
                smallestDistance = distance;
            }
        }
        q = closest.transform.position + sdirection.normalized * obsDistance;

        return q;
    }

}
