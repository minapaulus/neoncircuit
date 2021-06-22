using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBehaviour : MonoBehaviour
{
    public AttackPattern attack;
    public MovementPattern Move;
    public GameObject[] Weapon;
    private GameObject[] obstacles;

    private GameObject PlayerTarget; 

    private float lastfired = 0;
    // Start is called before the first frame update
    void Start()
    {
        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerTarget == null) return;
        if (Move) Move.Execute(transform, PlayerTarget.transform, obstacles, null);

        if (attack && Weapon.Length > 0) {
            if ((Time.time - lastfired) > (1.0/attack.Attackspeed)) {
                attack.Execute(transform, PlayerTarget.transform, Weapon, PlayerTarget);
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
