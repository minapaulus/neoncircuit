using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceTrigger1 : MonoBehaviour
{
    public Animator Bossroom;
    private Playerstats playerstat;

    private void Start()
    {
        playerstat = GameObject.FindGameObjectWithTag("Player").GetComponent<Playerstats>();
        //Debug.Log(playerstat.gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerstat.TriggerCheckPoint();
            Bossroom.SetTrigger("Trigger1");
            Destroy(this.gameObject);
        }
    }
}
