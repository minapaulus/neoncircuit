using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "Scriptable Objects/Movement/MoveinRangeOrCover")]
public class MoveinRangeOrCover : MovementPattern
{
    private NavMeshAgent _agent;
    [Range(0,10)]
    public float minDistance = 0f;
    [Range(0,20)]
    public float maxDistanceObstacle = 0f;
    [Range(0,50)]
    public float rotationSpeed = 10f;
    public bool needsCover = true;
    private bool _isInCover = false;
    // is changed by attackscript. He stands still for a few moments to prepare and then shoot.
    public bool isAttacking = false;

    public float obsDistance = 1;


    public override void Execute(Transform me, Transform target, GameObject[] obstacles)
    {
        /*if (_agent == null)
        {   //This line results in bugs with multiple objects... hooow
            _agent = me.gameObject.GetComponent<NavMeshAgent>();
        }*/
        _agent = me.gameObject.GetComponent<NavMeshAgent>();

        var pos = new Vector3(0,0,0);
        if (needsCover)
        {
            Debug.Log(obstacles.Length);
            _isInCover = false;
            if (_isInCover)
            {
                var q = Quaternion.LookRotation(target.position - me.position);
                me.rotation = Quaternion.Lerp(me.rotation, q, Time.deltaTime * rotationSpeed);
                pos = me.position;
            }
            else
            {
                pos = SearchCover(target, obstacles);
                if ((me.position - pos).magnitude <= obsDistance)
                {
                    //_isInCover = true;

                }
            }
        }
        else
        {
            //TODO: Find nearest point to target
            pos = target.position;
            if (!((me.position - pos).magnitude > minDistance) || isAttacking)
            {
                //Add artificial rotation, as the target is not the player from the view of the agent.
                var q = Quaternion.LookRotation(target.position - me.position);
                me.rotation = Quaternion.Lerp(me.rotation, q, Time.deltaTime * rotationSpeed);
                pos = me.position;
            }
        }
            _agent.SetDestination(pos);

            //TODO: Find nearest Obstacle and run to opposite side.

            //TODO: When attack is ready, get in line of position

        //TODO: Prohibit enemies walking into eachother.
        }
    

    private Vector3 SearchCover(Transform target, GameObject[] obstacles)
    {
        var q = new Vector3(0,0,0);
        GameObject closest = null;
        float smallestDistance = float.MaxValue;
        Vector3 sdirection = new Vector3(0,0,0);
        foreach(GameObject obs in obstacles)
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

    public void SetCover(bool cover)
    {
        _isInCover = cover;
    }
    public void Attacks(bool attack)
    {
        isAttacking = attack;
    }
}
