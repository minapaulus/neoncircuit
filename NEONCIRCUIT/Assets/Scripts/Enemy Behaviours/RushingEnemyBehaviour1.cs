using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RushingEnemyBehaviour1 : Enemy
{
    //public float HP = 100f;
    [Range(0, 100)]
    public float LifeReg = 10f;
    private bool _isRegenerating = false;
    public float fleeHP = 30;

    private Animator _anim;

    public GameObject materialReference;

    public ShootWhenRdy attack;
    public MoveInRange Rush;
    public MovementPattern Aim;
    public MoveInCoverNearPlayer Flee;
    public GameObject[] Weapon;
    private GameObject[] obstacles;

    private GameObject _playerTarget;
    //private Material _myMat = null;
    //private Color _init = Color.red;

    //Ausgelagert
    public bool isRdy = true;
    public bool isAttacking = false;
    //TODO: Raycast


    private NavMeshAgent _agent;

    private float _lastfired = 0;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        _agent = GetComponent<NavMeshAgent>();
        _anim = GetComponent<Animator>();


    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        //if (base.healthPoints <= 0) Die();
        if (_playerTarget == null) return;
        _anim.SetFloat("speed", _agent.velocity.magnitude);

        // Color only the material of the selected object.
        //Color HPindic = _init * (base.healthPoints / 100f);
        //_myMat.SetColor("_EmissionColor", HPindic);

        //Wenn der Gegner Angreifen kann und nicht in Cover ist -> Attack
        if (attack && Weapon.Length > 0)
        {
            if ((Time.time - _lastfired) > (1.0 / attack.Attackspeed) && (transform.position - _playerTarget.transform.position).magnitude <= attack.minFightingDistance && PlayerInSight())
            {
                //Debug.Log("I CAN ATTACK");
                // With isAttacking we tell our NPC to rotate until he is looking directly at the Player.
                
                //RayCast
                if (LookAtPlayer())
                {
                   // Debug.Log("I Initiate ATTACK");
                    _anim.SetBool("isAttacking", true);
                    if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Attacking") &&
                        _anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= .6f)
                    {
                        _agent.isStopped = false;
                        //Debug.Log("here we are");
                        attack.Execute(transform, _playerTarget.transform, Weapon, _playerTarget, base.HPindic);
                        _lastfired = Time.time;
                        isAttacking = false;
                        _anim.SetBool("isAttacking", false);
                    }
                }
            }
            else isAttacking = false;
        }
        else isAttacking = false;

        //When do you need to rush and when do you need to take cover?

        if (Rush && base.healthPoints > fleeHP && !isAttacking) {
            _agent.isStopped = false;
            Rush.Execute(transform, _playerTarget.transform, obstacles, _agent);
        }
        //Debug.Log((Weapon[0].transform.position - _playerTarget.transform.position).magnitude);
        if ((Aim && base.healthPoints > fleeHP && isAttacking)) {
            _agent.isStopped = true;
            Aim.Execute(transform, _playerTarget.transform, obstacles, _agent); }


        if (Flee && (base.healthPoints <= fleeHP || _isRegenerating)) {
            Flee.Execute(transform, _playerTarget.transform, obstacles, _agent);
            if (Flee.isInCover)
            {
                base.healthPoints += LifeReg * Time.deltaTime;
                _isRegenerating = true;
                if (base.healthPoints >= 100)
                {
                    base.healthPoints = 100;
                    _isRegenerating = false;
                }
                //prevent Actions except regenerating
                _lastfired = Time.time;
            }
        }
     }

    private bool LookAtPlayer()
    {
        RaycastHit hit;
        var rayDir = Weapon[0].transform.forward;
        if (Physics.Raycast(Weapon[0].transform.position, rayDir, out hit))
        {
            //Debug.Log(hit.transform.name);
            if (hit.transform == _playerTarget.transform) return true;
            else return false;
        }
        else return false;
    }

    private bool PlayerInSight()
    {
        RaycastHit hit;
        var rayDir = _playerTarget.transform.position - transform.position ;
        if (Physics.Raycast(this.transform.position, rayDir, out hit))
        {
            if (hit.transform == _playerTarget.transform) return true;
            else return false;
        }
        else return false;
    }

    private void Die()
        {
            // Only Destroy for now. Later a shader animation
            Destroy(this.gameObject);
        }


        //Beim eintreten den Spieler als Target ausw�hlen.
        private void OnTriggerEnter(Collider other)
        {
            if (other != null && other.gameObject.tag == "Player")
            {
                _playerTarget = other.gameObject;
            }
        }
    }

