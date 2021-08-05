using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    private GameObject _playertarget;
    public Animator _anim;

    // is activated in second phase.
    public ColorChangerPickup red;
    // The mask widens with the health of the Boss. When it reaches maximum (10), it will trigger second phase.
    public Material[] Masks;
    private bool _secondPhase = false;

    public GameObject rollAttackSphere;
    public float rollAttackDMG = 20f;
    public float rollAttackDuration = 10f;

    public float rollAttackwindup = 2f; 

    public Hitbox HeadshotHitbox; 

    public GameObject[] Turretspawns;
    public GameObject[] Trackshooters;
    public GameObject[] Supporterpos;

    public GameObject SimplyForwardWeapon;
    public float laserBeamDMG = 30f;
    public float laserBeamDurationC2 = 10f;
    public GameObject[] ForwardWeapons;
    public GameObject[] AroundWeapons; 


    // Start is called before the first frame update
    protected override void Start()
    {   
        base.Start();
        SetMaskRadius(0f);
    }

    private void SetMaskRadius(float s)
    {
        foreach (Material mat in Masks)
        {
            mat.SetFloat("_MaskRadius",s);
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        // When damaged the red marks on the bottom should spread out and if red phase begins be fully extruded. With damage to the red phase, the mask will shrink again.

        // Debug
        if (Input.GetMouseButtonDown(0))
        {
            ChangeColor(0); 
        }
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
            if (!_secondPhase) _secondPhase = true;
            SetMaskRadius((base.healthPoints) / (base.initialHP/3) * 10);
        }
    }

}
