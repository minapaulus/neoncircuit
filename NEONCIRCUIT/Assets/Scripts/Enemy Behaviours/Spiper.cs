using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spiper : Enemy
{

    public Animator _anim;

    public MoveInRange Rush;
    public Sniper Snip; 
    public GameObject Weapon;

    private bool _shot = false;
    private Projectile _projectile;

    private GameObject _playerTarget;

    private NavMeshAgent _agent;

    public LayerMask IgnoreLayer;

    private bool _aiming = false;

    private LineRenderer _lineR;

    private bool move = true;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _agent = GetComponent<NavMeshAgent>();
        //_anim = GetComponent<Animator>();

        //For creating line renderer object
        _lineR = Weapon.AddComponent<LineRenderer>();
        _lineR.startColor = base.HPindic;
        _lineR.endColor = base.HPindic;
        _lineR.startWidth = 0.01f;
        _lineR.endWidth = 0.01f;
        _lineR.positionCount = 2;
        _lineR.useWorldSpace = true;


    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        //if (base.healthPoints <= 0) Die();
        if (_playerTarget == null) return;
        _anim.SetFloat("Vertical", _agent.velocity.z);
        _anim.SetFloat("Horizontal", _agent.velocity.x);

        if (Snip)
        {
            //Just face the player
            //Debug.Log(PlayerInSight());
            if ((transform.position - _playerTarget.transform.position).magnitude < Snip.minFightingDistance && PlayerInSight())
            {
                Snip.Execute(this.transform, _playerTarget.transform, null, null, Color.red);
                _aiming = true;
                _agent.isStopped = true;
                if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                {
                    _anim.SetTrigger("Attack");
                    _shot = false;
                }
                if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                {
                    //Draw Line to Player 
                    //For drawing line in the world space, provide the x,y,z values
                    _lineR.enabled = true;
                    _lineR.SetPosition(0, Weapon.transform.position); //x,y and z position of the starting point of the line
                    _lineR.SetPosition(1, _playerTarget.transform.position); //x,y and z position of the end point of the line

                    if (!_shot)
                    {
                        _projectile = Snip.ShootProjectile(this.transform, _playerTarget.transform, Weapon, base.HPindic);
                        _shot = true;
                    }
                    if (_projectile == null)
                    {
                        _lineR.enabled = false;
                        _anim.SetTrigger("Attack-end");
                        _shot = false;
                        _projectile.LoseTarget();
                        _projectile = null;
                        _aiming = false;

                    }
                }
            }
            else
            {
                _lineR.enabled = false;
                if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") && _projectile != null)
                {
                    _anim.SetTrigger("Attack-end");
                    _shot = false;
                    _projectile.LoseTarget();
                    _projectile = null;
                    _aiming = false;
                }
            }
        }

        if (Rush && (_playerTarget.transform.position - this.transform.position).magnitude >= Rush.minDistance && !_aiming)
        {
            _agent.isStopped = false;
            _anim.SetBool("move", true);
            Rush.Execute(transform, _playerTarget.transform, null, _agent);
        }
        else
        {
            _agent.isStopped = true;
            _anim.SetBool("move", false);
        }
        
    }

    //returns if the enemy is right now looking directly at the player.
    private bool LookAtPlayer()
    {
        RaycastHit hit;
        var rayDir = Weapon.transform.forward;
        if (Physics.Raycast(Weapon.transform.position, rayDir, out hit))
        {
            //Debug.Log(hit.transform.name);
            if (hit.transform == _playerTarget.transform) return true;
            else return false;
        }
        else return false;
    }

    //returns if the player is in sight of the enemy.
    private bool PlayerInSight()
    {
        RaycastHit hit;
        var rayDir = _playerTarget.transform.position - transform.position;
        if (Physics.Raycast(this.transform.position, rayDir, out hit, Mathf.Infinity, IgnoreLayer))
        {
            if (hit.transform == _playerTarget.transform) return true;
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
