using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Flying : Enemy
{
    //public float HP = 100f;

    public ShootWhenRdy attack;
    public Support support;
    public MovementPattern move;
    private ShieldScript _shieldScript;

    private Animator _anim;
    public float atkDuration = 15f;
    private float _atktimer = 0f;

    public float downtime = 2f; 
    //additionally the first weapon should be the one with which the enemy aims at the player. We check if there is a clear line of sight before attacking.
    public GameObject[] weapons;

    private GameObject _playerTarget;
    //private Material _myMat = null;
    //private Color _init = Color.red;

    //It should priotize supporting. Changes priorization if there are no friends near, or if he lost health equal or greater than AggressionThreshold.
    private bool _prioSupport = true;
    private bool _supportTriggered = false;
    public float maxSupportRange = 30f;
    public float aggressionThreshold = 40f; 
    private List<GameObject> Enemies = new List<GameObject>();
    private List<GameObject> _trackedObjects = new List<GameObject>();
    private GameObject[] _nearest = { null, null };
    public bool isAttacking = false;
    //TODO: Raycast


    private NavMeshAgent _agent;

    private float _lastfired = 0;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _agent = GetComponent<NavMeshAgent>();
        _anim = GetComponent<Animator>();
        _anim.SetFloat("Exhaust-Speed", 1 / downtime);
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (obj.name != this.name)
            {
                Enemies.Add(obj);
            }
        }
        _shieldScript = GetComponent<ShieldScript>();
        //Debug.Log(Enemies.Count);


    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        //Debug.Log(_agent.velocity.magnitude);
        if (_playerTarget == null) return;

        _prioSupport = CheckForPrioSupport();

        if (_prioSupport && support && Enemies.Count > 0)
        {
            //Check if there are enemies which it can support. If not change priosupport.
            CheckForDeath();
            if (EnemiesinRange())
            {
                if (!_supportTriggered)
                {
                    _agent.isStopped = true;
                    _anim.SetTrigger("Support");
                    _supportTriggered = true;
                }
                if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Supporting"))
                {
                    //Debug.Log("Execute Support");
                    //support.Execute(this.transform, null, _nearest, null, Color.red);

                    foreach (GameObject friend in _nearest)
                    {
                        if (!_trackedObjects.Contains(friend))
                        {
                            _trackedObjects.Add(friend);
                            //everything normal. But they have not yet been registered.
                            if (_trackedObjects.Count <= 2)
                            {
                                //Debug.Log(friend.name);
                                support.StartSupportOne(friend, this.transform);
                            }
                            // Here, we have the case that a new friend is nearer than one who is being protected. In this case, we activate the new near friend and deactivate the far one.
                            else
                            {
                                //support.Execute(this.transform, null, _nearest, null, Color.red);
                                support.StartSupportOne(friend, this.transform);
                                _trackedObjects.Sort(SortByDistanceToMe);
                                GameObject _toDeactivate = _trackedObjects[_trackedObjects.Count - 1];
                                _trackedObjects.RemoveAt(_trackedObjects.Count - 1);
                                support.EndSupportOne(_toDeactivate, this.transform);

                            }
                        }
                    }

                }
            }
            else
            {
                if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Supporting"))
                {

                    _anim.SetTrigger("end-support");
                    support.EndSupport(_nearest,this.transform);
                    _trackedObjects.Clear();
                }
                _prioSupport = false;
            }
        }
        else _supportTriggered = false;
    
        //If the enemy is in range and has an initial clear FOV, he will initiate the attack. First a wind up animation is played and if it is finished the attack will commence while an attack rotation animation will be played.
        // after each Attack or Support move a short downtime is applied, so the player can breath.
 
        if (!_prioSupport && attack && weapons.Length > 0)
        {
            // Initiate Attack the player, if no Attack has yet been initiated
            if (!isAttacking) {
                 if ((transform.position - _playerTarget.transform.position).magnitude <= attack.minFightingDistance && PlayerInSight())
                    {
                        _agent.isStopped = true;
                        isAttacking = true;
                        _anim.SetTrigger("Attack");
                        _atktimer = 0f;

                    }
                }
            else
            {
                //if Attack is already initiated wait till the windup is over
                if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Attacking"))
                {
                    // Attack as long as atkDuration
                    if (_atktimer <= atkDuration)
                    {
                        _atktimer += Time.deltaTime;
                        // Shoot according to the attackspeed.
                        if ((Time.time - _lastfired) > (1.0 / attack.Attackspeed))
                        {
                            _lastfired = Time.time;
                            attack.Execute(this.transform, _playerTarget.transform, weapons, _playerTarget, base.HPindic);
                        }
                    }
                    else
                    {
                        _anim.SetTrigger("end-attack");
                        isAttacking = false;
                    }
                }
            }

        }
        else isAttacking = false;




        /* If no attack or support move is being performed, the agent has to move to the player or get into range. */
        if (!_supportTriggered && move && !isAttacking && _anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            _agent.isStopped = false;
            move.Execute(transform, _playerTarget.transform, null, _agent);
        }

        //When do you need to rush and when do you need to take cover?

    }

    private bool CheckForPrioSupport()
    {
        if (_shieldScript.getActivation() || base.healthPoints <= base.initialHP - aggressionThreshold) {
            if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Supporting"))
            {
                _anim.SetTrigger("end-support");
                support.EndSupport(_nearest,this.transform);
            }
            _supportTriggered = false;
            return false; }
        else return _prioSupport;
    }

    private void CheckForDeath()
    {
        try
        {
            foreach (GameObject en in Enemies)
            {
                if (en == null)
                {
                    Enemies.Remove(en);
                }
            }
        }
        catch
        {

        }
    }

    private bool EnemiesinRange()
    {
        // we search for the two nearest Enemies. If they are nearer than our MaxSupportRange, then we will trigger support.
        if (Enemies.Count < 2) return false;
        Enemies.Sort(SortByDistanceToMe);
        /*foreach(GameObject enem in Enemies)
        {
            Debug.Log((enem.transform.position - transform.position).magnitude);
        }*/
        _nearest[0] = Enemies[0]; 
        _nearest[1] = Enemies[1]; 


        if(_nearest[0] != null && _nearest[1] != null && (_nearest[0].transform.position - this.transform.position).magnitude < maxSupportRange && (_nearest[1].transform.position - this.transform.position).magnitude < maxSupportRange){ 
            return true; 
        } else return false;
    }

    int SortByDistanceToMe(GameObject a, GameObject b)
    {
        float squaredRangeA;
        float squaredRangeB;
        if (a != null)
        {
            squaredRangeA = (a.transform.position - transform.position).sqrMagnitude;
        }
        else squaredRangeA = 1000000f;
        if (b != null)
        {
            squaredRangeB = (b.transform.position - transform.position).sqrMagnitude;
        }
        else squaredRangeB = 1000000f;
        return squaredRangeA.CompareTo(squaredRangeB);
    }

    private bool PlayerInSight()
    {
        RaycastHit hit;
        var rayDir =  _playerTarget.transform.position - transform.position;
        if (Physics.Raycast(this.transform.position, rayDir, out hit))
        {
            if (hit.transform == _playerTarget.transform) {
               // Debug.Log("In sight");
                return true; }
            else return false;
        }
        else return false;
    }


        //Beim eintreten den Spieler als Target auswählen.
        private void OnTriggerEnter(Collider other)
        {
            if (other != null && other.gameObject.tag == "Player")
            {
                _playerTarget = other.gameObject;
            }
        }

    }

