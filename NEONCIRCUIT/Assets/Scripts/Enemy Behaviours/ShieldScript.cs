using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldScript : MonoBehaviour
{
    /* This script will be attached to all enemies.
     * It triggers a shield and an animation for the shield. The shield has the purpose to guard the enemy from the players attacks. As long as a Flyer enemy protects said enenmy.
     * */
    public GameObject shield;
    public Animator shieldanim;
    private bool _activated = false;
    private Transform _protector = null;

    public void Update()
    {   if (_activated && _protector != null) {
            var direction = shield.transform.position - _protector.position;
            shield.transform.LookAt(_protector.position);
                }
    }


    public void Activate(Transform protector)
    {
        shield.SetActive(true);
        shieldanim.SetTrigger("Activate");
        _protector = protector;
        _activated = true;
    }

    public void DeActivate()
    {
        shield.SetActive(false);
        shieldanim.SetTrigger("DeActivate");
        _protector = null;
        _activated = false;
    }
}
