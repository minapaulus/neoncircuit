using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Attack/Support")]
public class Support :AttackPattern
{
    // Start is called before the first frame update

    // Here we just spawn the Spheres and Planes around the nearest friendly (or enemy for the matter) objects and start there animation. It is called when the Flyer enemy type 
    // does trigger support moves. 

    // When the move is being cancelled, the Sphere should go away. There can be two reasons for this: 
    // 1. One Enemy is out of range. 
    // 2. Flyer cancels its animation. 

    private List<GameObject> _trackedTargets = new List<GameObject>();
    private Transform _me;

    private void OnEnable()
    {
        _trackedTargets = new List<GameObject>();
    }

    public override void Execute(Transform me, Transform target, GameObject[] friends , GameObject Target, Color HPindic)
    {
        _me = me;
       foreach(GameObject friend in friends)
        {
            // Only do something, if a shield needs to be activated.
            if (!_trackedTargets.Contains(friend))
            {
                _trackedTargets.Add(friend);
                //everything normal. But they have not yet been registered.
                if (_trackedTargets.Count <= 2)
                {
                    //Debug.Log(friend.name);
                    friend.GetComponent<ShieldScript>().Activate(me);
                }
                // Here, we have the case that a new friend is nearer than one who is being protected. In this case, we activate the new near friend and deactivate the far one.
                else
                {
                    Debug.Log("Wieso hier rein?");
                    friend.GetComponent<ShieldScript>().Activate(me);
                    _trackedTargets.Sort(SortByDistanceToMe);
                    GameObject _toDeactivate = _trackedTargets[_trackedTargets.Count - 1];
                    _trackedTargets.RemoveAt(_trackedTargets.Count - 1);
                    _toDeactivate.GetComponent<ShieldScript>().DeActivate(me);

                }
            }
        }
    }

    int SortByDistanceToMe(GameObject a, GameObject b)
    {
        float squaredRangeA = (a.transform.position - _me.position).sqrMagnitude;
        float squaredRangeB = (b.transform.position - _me.position).sqrMagnitude;
        return squaredRangeA.CompareTo(squaredRangeB);
    }

    public void EndSupport(GameObject[] friends, Transform me)
    {
        //Debug.Log("Ending Support");
        foreach(GameObject friend in friends)
        {
            friend.GetComponent<ShieldScript>().DeActivate(me);
        }
    }

    internal void StartSupportOne(GameObject friend, Transform me)
    {
        friend.GetComponent<ShieldScript>().Activate(me);
    }

    internal void EndSupportOne(GameObject friend, Transform me)
    {
        friend.GetComponent<ShieldScript>().DeActivate(me);
    }
}
