using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceTrigger2 : MonoBehaviour
{
    public Animator Bossroom;
    public Boss Spherephiroth;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Bossroom.SetTrigger("Trigger2");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Bossroom.GetCurrentAnimatorStateInfo(0).IsName("End"))
        {
            Spherephiroth.TriggerBoss();
            Destroy(this.gameObject);
        }
    }
}
