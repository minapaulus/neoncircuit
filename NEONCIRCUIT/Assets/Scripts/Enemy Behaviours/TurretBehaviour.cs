using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBehaviour : Enemy
{
    public AttackPattern attack;
    public MovementPattern Move;
    public GameObject[] Weapon;
    private GameObject[] obstacles;


    public Renderer lightPillar; 

    private GameObject PlayerTarget; 

    private float lastfired = 0;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        HPindic = base.initcolor * (base.healthPoints / 100f);
        lightPillar.material.SetColor("_EmissionColor", HPindic);
        if (PlayerTarget == null) return;
        if (Move) Move.Execute(transform, PlayerTarget.transform, null, null);

        if (attack && Weapon.Length > 0) {
            if ((Time.time - lastfired) > (1.0/attack.Attackspeed)) {
                attack.Execute(transform, PlayerTarget.transform, Weapon, PlayerTarget, base.HPindic);
                lastfired = Time.time;
            }
        }
    }


    //Beim eintreten den Spieler als Target auswählen.
    private void OnTriggerEnter(Collider other)
    {
        if (other != null && other.gameObject.tag == "Player")
        {
            PlayerTarget = other.gameObject;
        }
    }
    // Beim austreten das Target auf null setzen, somit aktionen unterbinden. Kann man vllt ganz weglassen. Depends, schätze ich ob man Gegner haben will die immer jagen. Bei einem Turret hingegen macht das Sinn..
    private void OnTriggerExit(Collider other)
    {
        if (other != null && other.gameObject.tag == "Player")
        {
            PlayerTarget = null;
        }
    }
}
