using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Flying : MonoBehaviour
{
    public float HP = 100f;

    public ShootWhenRdy attack;
    public AttackPattern support;
    public MovementPattern move;

    private Animator _anim;
    public float atkDuration = 15f;
    private float _atktimer = 0f;

    public float downtime = 2f; 
    //additionally the first weapon should be the one with which the enemy aims at the player. We check if there is a clear line of sight before attacking.
    public GameObject[] weapons;

    private GameObject _playerTarget;
    private Material _myMat = null;
    private Color _init = Color.red;

    //Ausgelagert
    public bool isRdy = true;
    public bool isAttacking = false;
    //TODO: Raycast


    private NavMeshAgent _agent;

    private float _lastfired = 0;
    // Start is called before the first frame update
    void Start()
    {
        _myMat = transform.GetChild(0).GetComponent<Renderer>().material;
        _init = _myMat.GetColor("_EmissionColor");
        _agent = GetComponent<NavMeshAgent>();
        _anim = GetComponent<Animator>();


    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(_agent.velocity.magnitude);
        if (HP <= 0) Die();
        if (_playerTarget == null) return;

        // Color only the material of the selected object.
        Color HPindic = _init * (HP / 100f);
        _myMat.SetColor("_EmissionColor", HPindic);

        //If the enemy is in range and has an initial clear FOV, he will initiate the attack. First a wind up animation is played and if it is finished the attack will commence while an attack rotation animation will be played.
        // after each Attack or Support move a short downtime is applied, so the player can breath.
 
        if (attack && weapons.Length > 0)
        {
            Debug.Log("Not seen");
            /*
            if ((Time.time - _lastfired) > (1.0 / attack.Attackspeed) && (transform.position - _playerTarget.transform.position).magnitude <= attack.minFightingDistance && PlayerInSight())
            {
                Debug.Log("I CAN ATTACK");
                // With isAttacking we tell our NPC to rotate until he is looking directly at the Player.
                isAttacking = true;
                //RayCast
                if (LookAtPlayer())
                {
                    Debug.Log("I ATTACK");
                    attack.Execute(transform, _playerTarget.transform, Weapon, _playerTarget);
                    _lastfired = Time.time;
                    isAttacking = false;
                }
            }
            else isAttacking = false;
            
            */
            // Initiate Attack the player, if no Attack has yet been initiated
            if (!isAttacking && (transform.position - _playerTarget.transform.position).magnitude <= attack.minFightingDistance && PlayerInSight())
            {
                _agent.isStopped = true;
                isAttacking = true;
                _anim.SetTrigger("Attack");
                _atktimer = 0f;

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
                            attack.Execute(this.transform, _playerTarget.transform, weapons, _playerTarget);
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
        if (move && !isAttacking && _anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            _agent.isStopped = false;
            move.Execute(transform, _playerTarget.transform, null, _agent);
        }

        //When do you need to rush and when do you need to take cover?

    }

    private bool PlayerInSight()
    {
        RaycastHit hit;
        var rayDir =  _playerTarget.transform.position - transform.position;
        if (Physics.Raycast(this.transform.position, rayDir, out hit))
        {
            if (hit.transform == _playerTarget.transform) {
                Debug.Log("In sight");
                return true; }
            else return false;
        }
        else return false;
    }

    private void Die()
        {
            // Only Destroy for now. Later a shader animation
            Destroy(this.gameObject);
        }


        //Beim eintreten den Spieler als Target auswählen.
        private void OnTriggerEnter(Collider other)
        {
            if (other != null && other.gameObject.tag == "Player")
            {
                _playerTarget = other.gameObject;
            }
        }

        public void Damage(float dmg)
        {
            HP -= dmg;
        }
    }

