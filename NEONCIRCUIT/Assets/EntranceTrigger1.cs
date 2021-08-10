using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceTrigger1 : MonoBehaviour
{
    public Animator Bossroom;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Bossroom.SetTrigger("Trigger1");
            Destroy(this.gameObject);
        }
    }
}
