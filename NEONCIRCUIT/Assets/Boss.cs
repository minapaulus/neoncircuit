using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : Enemy
{
    private bool _triggered = false;
    private GameObject _playertarget;
    public GameObject robotSphere;
    public Animator _anim;
    private NavMeshAgent _nAgent;
    public float Walkspeed = 5f;
    public float Rollspeed = 8f;
    public Transform Spawnpoint;
    private Vector3 _localSpawnPoint;

    private bool _setPoint = false;

    public AudioClip sound_hochfliegen;
    public AudioClip sound_runterfliegen;
    public AudioClip sound_shield1;
    public AudioClip sound_shield2;
    public AudioClip sound_atmo;
    public AudioClip sound_shot;
    private AudioSource sasource;

    // is activated in second phase.
    public ColorChangerPickup red;
    // The mask widens with the health of the Boss. When it reaches maximum (10), it will trigger second phase.
    public Renderer[] Masks;
    private List<Material> _mats = new List<Material>();
    private bool _secondPhase = false;

    public Hitbox HeadshotHitbox;
    public Hitbox BasicHB;

    /*
    // Two options:
    // 1. Rolls around with Animation while centerpart is damaging on stay. 
    // 2. Rolls in the middle and aims at player untill it rolls forward. Here the aim of the player is to evade possibly directing the Boss into pillars which cause Stun effects.
    // 3. is one but with Turrets enabled. 
    // Chance: 3/20 
    // Colorchange after animation except C3.
    public GameObject rollAttackSphere;
    public float rollAttackDMG = 20f;
    public float rollAttackDuration = 10f;
    public float rollAttackwindup = 1f;
    private bool _rollAttack = false;
    */

    // Here Projectiles will be launched one after each other from Track spawnpoints who track (Script needed) the player. 
    // While this is happening Supporter drones will be spawned and protect the boss. 
    // for C1: Trackshooters are not enabled and the Boss heals itself. 
    // For C2: No Healing but the damage via Trackshooters.
    // For C3: Additional Turrets are spawned (two random). 
    // Chance: 3/20 
    // Colorchange after animation except C3.
    public GameObject[] Trackshooters;
    public ShootWhenRdy Trackingshots;
    public float minDistance = 10f;

    public GameObject[] Supporterpos;
    public GameObject SupporterPrefab;
    private int _aliveSups = 0;
    private List<GameObject> _listSup = new List<GameObject>();
    public float hOTValuePerSecond = 10f;
    private bool _supportset = false;

    private bool _TrackingSupportAttack = false;

    // FOr Red Phase. Secondphase.
    public GameObject[] Turretspawns;
    private bool _TurretspawnSupportAttack = false;
    public GameObject TurretPrefab; 
    public int turretCount = 2;

    //Laser Beam attack deals a lot of damage to the player and is disabled when it hits once. 
    // C1: The beam comes straight out of the Boss. 
    // C2: The beam comes from above the player and travels slowly. 
    // C3: C2 with Trackshooter attack.
    // Chance: 4/20 
    // Colorchange after animation except C3. 
    public GameObject SimplyForwardWeapon;
    public ShootWhenRdy BeamAttackObject;
    public GameObject BeamPortalIn;
    public float PortalSpeed;
    public GameObject BeamPortalOut;
    private bool _laserBeamAttack = false;
    public float laserBeamDMG = 30f;
    public GameObject beamSpawnAbovePlayer; 
    public float laserBeamDurationC2 = 10f;
    public float laserBeamDurationC1 = 2f;
    private float _normalBeamLastFired = 0f;

    // The normal attack. Not effected by Colorchanges. But also doesnt trigger a colorchange. Boss chases the Player and will Shoot when rdy. Similar to the robot enemies. Higher chance for this move to be triggered.
    // Chance: 6/20.
    // No Colorchange.
    public GameObject[] ForwardWeapons;
    private bool _normalAttack = false;
    private float _normalLastFired = 0f;
    public ShootWhenRdy NormalAttack;

    // Similar to Flying Enemy. Boss will trigger fast rolling change and roll to the Center. here he will fly up and Rotate and shoot many projectiles with low tracking capabilities.
    // Chance: 4/20
    // No Colorchange.
    public GameObject[] AroundWeapons;
    private bool _normalRotationAttack = false;
    private float _duration = 0f;
    public float maxDuration = 5f;
    public ShootWhenRdy RotationAttackObject;

    // Start is called before the first frame update
    protected override void Start()
    {   
        base.Start();

        foreach (Renderer ren in Masks)
        {
            foreach (Material mat in ren.materials)
            {
                _mats.Add(mat);
            }
        }

        SetMaskRadius(0f);
        _playertarget = GameObject.FindGameObjectWithTag("Player");
        _nAgent = GetComponent<NavMeshAgent>();
        _localSpawnPoint = Spawnpoint.position;
        this.GetComponent<ShieldScript>().Activate(this.transform);
        sasource = GetComponent<AudioSource>();
    }

    private void SetMaskRadius(float s)
    {
        foreach (Material mat in _mats)
        {
            mat.SetFloat("_MaskRadius",s);
        }
    }

    public void TriggerBoss()
    {
        if (!_triggered)
        {
            this.GetComponent<ShieldScript>().DeActivate(this.transform);
            HeadshotHitbox.gameObject.SetActive(true);
            BasicHB.gameObject.SetActive(true);
            _triggered = true;
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (_playertarget == null) return;

        Debuging();
        // When damaged the red marks on the bottom should spread out and if red phase begins be fully extruded. With damage to the red phase, the mask will shrink again.

        if (_triggered)
        {
            //ChooseAction();

            if (_normalAttack)
            {
                ChaseAttack();
            }

            if (_normalRotationAttack)
            {
                RotationAttack();
            }

            if (_TrackingSupportAttack)
            {
                SupportAttack();
            }

            if (_laserBeamAttack)
            {
                BeamAttack();
            }
        }
    }

    private void ChooseAction()
    {
        if(!(_TrackingSupportAttack || _laserBeamAttack || _normalAttack || _normalRotationAttack) && _anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            int rando = (int)UnityEngine.Random.Range(0, 20);

            if (rando < 9)
            {
                _normalAttack = true;
            }

            if (rando >= 9 && rando < 14)
            {
                _normalRotationAttack = true;
            }

            if (rando >= 14 && rando < 17)
            {
                _TrackingSupportAttack = true;
            }

            if (rando >= 17 && rando < 20)
            {
                _laserBeamAttack = true;
            }


        }
    }

    void playSound(AudioClip sound)
    {
        sasource.clip = sound;
        sasource.PlayOneShot(sasource.clip);
    }

    private void Debuging()
    {
        // Debug
        if (Input.GetKey("2"))
        {
            _normalAttack = true;
        }

        if (Input.GetKey("1"))
        {
            //Debug.Log("Pressed 1");
            _normalRotationAttack = true;
        }

        if (Input.GetKey("3"))
        {
            //Debug.Log("Pressed 1");
            _TrackingSupportAttack = true;
        }

        if (Input.GetKey("4"))
        {
            //Debug.Log("Pressed 1");
            _laserBeamAttack = true;
        }
    }

    private void BeamAttack()
    {
        WalkTo(_localSpawnPoint);

        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("WalkTo"))
        {
            if ((this.transform.position - _localSpawnPoint).magnitude <= 1.7f)
            {
                _anim.SetBool("BeamAttack", true);
                _nAgent.isStopped = true;
                robotSphere.transform.position = _localSpawnPoint;
                robotSphere.transform.rotation = this.transform.rotation;
                //Debug.Log("Attackpoint Reached");
            }
        }

        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("BeamAttack-Windup"))
        {
            robotSphere.transform.position = _localSpawnPoint;
            robotSphere.transform.rotation = this.transform.rotation;
            if (base.assignedColor == Enemy.AssignedColors.Color1) this.transform.LookAt(_playertarget.transform.position);
            if(base.assignedColor == Enemy.AssignedColors.Color2 || base.assignedColor == Enemy.AssignedColors.Color3)
            {
                this.transform.LookAt(BeamPortalIn.transform.position);
                BeamPortalIn.SetActive(true);
                BeamPortalOut.SetActive(true);
                //PortalLogic();
            }
            playSound(sound_shield2);
        }

        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("BeamAttack"))
        {

            if (base.assignedColor == Enemy.AssignedColors.Color1) NormalBeamAttack();
            if (base.assignedColor == Enemy.AssignedColors.Color2) TrackingBeamAttack();
            if (base.assignedColor == Enemy.AssignedColors.Color3) FucktThisPlayerInParticularBeamAttack();

        }


        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("BeamAttack-Windup-End"))
        {
            BeamPortalIn.SetActive(false);
            BeamPortalOut.SetActive(false);
            playSound(sound_shield1);
        }

        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("BeamAttack-End") && (_anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= .6f))
        {
            AfterFlyAttack();
            ResetAll();

        }

    }

    private void SupportAttack()
    {
        RollingAnimTo(_localSpawnPoint);
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Rolling"))
        {
            if ((this.transform.position - _localSpawnPoint).magnitude <= 1.7f)
            {
                _anim.SetBool("SupportAttack", true);
                _nAgent.isStopped = true;
                robotSphere.transform.position = _localSpawnPoint;
                robotSphere.transform.rotation = this.transform.rotation;
                //Debug.Log("Attackpoint Reached");
            }
        }

        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("SupportAttack-Windup"))
        {
            playSound(sound_hochfliegen);
        }

        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("SupportAttack"))
        {
            SpawnSupporters();
            //playSound(sound_atmo);
            if (SupportersAlive())
            {
                if (base.assignedColor == Enemy.AssignedColors.Color1) HealthSupportAttack();
                if (base.assignedColor == Enemy.AssignedColors.Color2) AttackSupportAttack();
                if (base.assignedColor == Enemy.AssignedColors.Color3) AggressiveSupportAttack();
            } else
            {
                _anim.SetBool("SupportAttack", false);
                this.GetComponent<ShieldScript>().DeActivate(this.transform);
            }
            
        }

        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("SupportAttack-Windup-End"))
        {
            playSound(sound_runterfliegen);
        }

        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Support-End") && (_anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= .6f))
        {
            AfterFlyAttack();
            ResetAll();

        }
    }

    private void FucktThisPlayerInParticularBeamAttack()
    {
        TrackingBeamAttack();
        AttackSupportAttack();
        SpawnTurrets();
    }

    private void TrackingBeamAttack()
    {
        PortalLogic();
        if (_duration <= laserBeamDurationC2)
        {
            robotSphere.transform.position = _localSpawnPoint;
            this.transform.LookAt(BeamPortalIn.transform.position);
            if (BeamAttackObject)
            {
                if ((Time.time - _normalBeamLastFired) > (1.0 / Trackingshots.Attackspeed))
                {
                    BeamAttackObject.Execute(transform, BeamPortalIn.transform, ForwardWeapons, _playertarget, base.initcolor);
                    _normalBeamLastFired = Time.time;
                }
            }
            _duration += Time.deltaTime;


        }
        else
        {
            _anim.SetBool("BeamAttack", false);
        }

    }

    private void PortalLogic()
    {
        // Out travels slowly behind player.
        // Sound des folgenden Portals.

        BeamPortalOut.transform.RotateAround(BeamPortalOut.transform.position, BeamPortalOut.transform.right, 180f * Time.deltaTime);
        BeamPortalIn.transform.RotateAround(BeamPortalIn.transform.position, BeamPortalIn.transform.right, 180f * Time.deltaTime);

        FollowPlayer(BeamPortalOut);
    }

    private void FollowPlayer(GameObject follower)
    {
        var direction = _playertarget.transform.position - follower.transform.position;
        direction.Normalize();
        follower.transform.position = new Vector3(follower.transform.position.x + (direction.x * PortalSpeed * Time.deltaTime), follower.transform.position.y, follower.transform.position.z + (direction.z * PortalSpeed * Time.deltaTime))  ;


    }

    private void NormalBeamAttack()
    {
        if(_duration <= laserBeamDurationC1)
        {
            robotSphere.transform.position = _localSpawnPoint;
            robotSphere.transform.rotation = this.transform.rotation;
            this.transform.LookAt(_playertarget.transform.position);
            if (BeamAttackObject)
            {
                if ((Time.time - _normalLastFired) > (1.0 / Trackingshots.Attackspeed))
                {
                    GameObject[] weapons = { SimplyForwardWeapon };
                    BeamAttackObject.Execute(transform, _playertarget.transform, ForwardWeapons, _playertarget, base.initcolor);
                    _normalLastFired = Time.time;
                }
            }
            _duration += Time.deltaTime;


        } else
        {
            _anim.SetBool("BeamAttack", false);
        }
    }

    public void SupporterKilled(GameObject supporter)
    {
        if (_listSup.Contains(supporter)) return;
        _listSup.Add(supporter);
        _aliveSups--;
        if (_aliveSups <= 0) _aliveSups = 0;
        Debug.Log("Supporter Killed, Left: " + _aliveSups);
        // Deactivate corresponding Shield.
    }

    private bool SupportersAlive()
    {
        return _aliveSups > 0;
    }

    private void AggressiveSupportAttack()
    {
        AttackSupportAttack();
        if (!_TurretspawnSupportAttack)
        {
            turretCount = 2;
            SpawnTurrets();
        }
    }

    private void AttackSupportAttack()
    {
        // Finde das n‰chste static shoot ding, was Sichth at aber auch nicht zu weit weg ist. 
        GameObject weapon = null;
        float nearest = Mathf.Infinity;
        foreach (GameObject obj in Trackshooters)
        {
            if (PlayerInSight(obj))
            {
                float distance = (obj.transform.position - _playertarget.transform.position).magnitude;
                if (distance < nearest && distance >= minDistance)
                {
                    nearest = distance;
                    weapon = obj;
                }
            }
        }

        if (Trackingshots)
        {
            if(weapon != null)
            {
                if ((Time.time - _normalLastFired) > (1.0 / Trackingshots.Attackspeed))
                {
                    GameObject[] weapons = { weapon };
                    weapon.transform.LookAt(_playertarget.transform.position);
                    Trackingshots.Execute(transform, _playertarget.transform, weapons, _playertarget, base.initcolor);
                    _normalLastFired = Time.time;
                }
            }
        }
        // Lass es schieﬂen. Simple as that.
        playSound(sound_shot);
        return;
    }

    private void HealthSupportAttack()
    {
        DamageHP(-(hOTValuePerSecond * Time.deltaTime));
        //Spawn a FX Effect which suggests Heals??
        Debug.Log("Health Support Attack");
        playSound(sound_shield2);
    }

    private void SpawnSupporters()
    {
        if (!_supportset)
        {
            foreach (GameObject obj in Supporterpos)
            {
                Instantiate(SupporterPrefab, obj.transform.position, obj.transform.rotation);
                _aliveSups++;
            }
            this.GetComponent<ShieldScript>().Activate(this.transform);
            _supportset = true;
        }
    }

    private void ChaseAttack()
    {
        _nAgent.SetDestination(_playertarget.transform.position);
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Idle")) {
            _anim.SetBool("Chase", true);
            Walk();   
        }

        // if Player in sight and in Range and facing Boss. Attack Trigger.

        if (NormalAttack) {
            if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Chasing") &&
                (Time.time - _normalLastFired) > (1.0 / NormalAttack.Attackspeed) && 
                (SimplyForwardWeapon.transform.position - _playertarget.transform.position).magnitude <= NormalAttack.minFightingDistance &&
                PlayerInSight(this.gameObject))
            {
                _nAgent.isStopped = true;
                _anim.SetBool("Chase", false);
                if (!_anim.GetBool("Attack")) { _anim.SetTrigger("Attack"); }
            }
            if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Attack-Windup"))
            {
                this.transform.LookAt(_playertarget.transform.position);
                playSound(sound_shield1);
            }

            if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") &&
                _anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.33 &&
                (Time.time - _normalLastFired) > (1.0 / NormalAttack.Attackspeed))
            {
                NormalAttack.Execute(transform, _playertarget.transform, ForwardWeapons, _playertarget, base.HPindic);
                _normalLastFired = Time.time;
                ResetAll();
            }
        }
    }
    
    private void RotationAttack()
    {
        //Debug.Log("RotationAttack engaged");
        RollingAnimTo(_localSpawnPoint);

        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Rolling"))
        {
            if ((this.transform.position - _localSpawnPoint).magnitude <= 1.7f)
            {
                _anim.SetBool("RotationAttack", true);
                _nAgent.isStopped = true;
                //Debug.Log("Attackpoint Reached");
            }
        }

        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("RotationAttack-Windup"))
        {
            playSound(sound_hochfliegen);
        }

        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("RotationAttack"))
        {
            //playSound(sound_atmo);

            //Flyer Code Attack.
            if (RotationAttackObject)
            {
               if ((Time.time - _normalLastFired) > (1.0 / RotationAttackObject.Attackspeed)){
                    RotationAttackObject.Execute(transform, _playertarget.transform, AroundWeapons, _playertarget, base.HPindic);
                    _normalLastFired = Time.time;
                }
            }

            if (!_TurretspawnSupportAttack && base.assignedColor == Enemy.AssignedColors.Color3)
            {
                SpawnTurrets();
            }

            // if Duration reached RotationAttack = false
            if(_duration >= maxDuration)
            {
                _anim.SetBool("RotationAttack", false);
                _duration = 0;
            } else
            {
                _duration += Time.deltaTime;
            }
        }

        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Rotation-End") && (_anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= .6f))
        {
            playSound(sound_runterfliegen);
            AfterFlyAttack();
            ResetAll();
            
        }
    }

    private void RollingAnimTo(Vector3 point)
    {
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            //Debug.Log((this.transform.position - _localSpawnPoint).magnitude);
            if (((this.transform.position - _localSpawnPoint).magnitude <= 10f))
            {
                _anim.SetTrigger("NormalBall");
            }
            else
            {
                _anim.SetTrigger("DynamicBall");
            }

            if (!_setPoint)
            {
                RollTo(_localSpawnPoint);
            }
        }
    }

    private bool SpawnTurrets()
    {
        // find all turrets with sight on Player.
        List<GameObject> goodTurrets = new List<GameObject>();

        foreach(GameObject turret in Turretspawns)
        {
            if (PlayerInSight(turret)) goodTurrets.Add(turret);
        }

        if (goodTurrets.Count >= turretCount)
        {
            for (int i = 0; i <= turretCount - 1; i++)
            {
                //Debug.Log("Player in Sight");
                int k = (int)UnityEngine.Random.Range(0, goodTurrets.Count -1);
                Instantiate(TurretPrefab, goodTurrets[k].transform.position, goodTurrets[k].transform.rotation);
                // Sound an Stelle.
                playSound(sound_shield1);
                Debug.Log("Spawn Turrets");
                goodTurrets.RemoveAt(k);
            }
            _TurretspawnSupportAttack = true;
            return true;
        }
        return false;
    }

    private void ResetAll()
    {
        _anim.SetBool("RotationAttack", false);
        _normalRotationAttack = false;
        _anim.ResetTrigger("NormalBall");
        _anim.ResetTrigger("DynamicBall");

        _normalAttack = false;
        _anim.ResetTrigger("Attack");
        _anim.SetBool("Chase", false);
        _TurretspawnSupportAttack = false;
        turretCount = 2;

        _supportset = false;
        _anim.SetBool("SupportAttack", false);
        _TrackingSupportAttack = false;

        _laserBeamAttack = false;
        _anim.ResetTrigger("Walkmid");
        _anim.SetBool("BeamAttack", false);
        _duration = 0f;
        _listSup = new List<GameObject>();

        BeamPortalOut.transform.localPosition = new Vector3(0, BeamPortalOut.transform.position.y, 0);

    }

    private void AfterFlyAttack()
    {
        robotSphere.transform.position = _localSpawnPoint;
        robotSphere.transform.rotation = this.transform.rotation;
        _setPoint = false;
        if (base.assignedColor == Enemy.AssignedColors.Color1) base.ChangeAssignedColor(Enemy.AssignedColors.Color2);
        else if(base.assignedColor == Enemy.AssignedColors.Color2) base.ChangeAssignedColor(Enemy.AssignedColors.Color1);
    }

    private void RollTo(Vector3 point)
    {
        _nAgent.SetDestination(point);
        _setPoint = true;
        _nAgent.speed = Rollspeed;
        _nAgent.isStopped = false;

    }

    private void Walk()
    {
        _nAgent.isStopped = false;
        _nAgent.speed = Walkspeed;
    }

    private void WalkTo(Vector3 point)
    {
        if (!_setPoint)
        {
            _anim.SetTrigger("Walkmid");
            _nAgent.SetDestination(point);
            _setPoint = true;
            Walk();
        }

    }
    private bool PlayerInFront( GameObject obj)
    {
        RaycastHit hit;
        var rayDir = _playertarget.transform.position - obj.transform.position;
        if (Physics.Raycast(obj.transform.position, obj.transform.forward, out hit))
        {
            if (hit.transform == _playertarget.transform)
            {
                Debug.Log("Hit him");
                return true;
            }
            else
            {
                Debug.Log("Not hit");
                return false;
            }
        }
        else return false;
    }

    private bool PlayerInSight(GameObject obj)
    {
        RaycastHit hit;
        var rayDir = _playertarget.transform.position - obj.transform.position;
        if (Physics.Raycast(obj.transform.position, rayDir, out hit))
        {
            if (hit.transform == _playertarget.transform)
            {
                //Debug.Log("Hit him");
                return true;
            }
            else
            {
                //Debug.Log("Not hit");
                return false;
            }
        }
        else return false;
    }

    private void ChangeColor(int color)
    {
        base.assignedColor = (Enemy.AssignedColors)color;
        Color buffer = new Color();
        buffer = ChooseColor(base.assignedColor);
        base.ChangeColor(buffer);
    }

    public override void DamageHP(float dmg)
    {
        base.DamageHP(dmg); 
        if(base.healthPoints > base.initialHP/3 && !_secondPhase)
        {
            // Here we are in the range of 333 until 999 HP.
            // We want to enlarge the mask until it reaches 10 at 333. Lower than 333 triggers second Phase.
            SetMaskRadius(( base.initialHP - base.healthPoints) / (base.initialHP) * 10);
        } else
        {
            if(base.healthPoints <= 0)
            {
                //Initiate End of Bossfight.
            }
            if (!_secondPhase)
            {
                _secondPhase = true;
                base.initialHP = base.initialHP / 3;
                ChangeAssignedColor(Enemy.AssignedColors.Color3);
                red.gameObject.SetActive(true);
            }
            SetMaskRadius((base.healthPoints) / (base.initialHP) * 10);
        }
    }
    
    public Color GetInitColor()
    {
        return base.initcolor;
    }
}
