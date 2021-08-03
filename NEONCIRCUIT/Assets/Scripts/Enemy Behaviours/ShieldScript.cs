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
    private GameObject _protector = null;

    protected void Update()
    {   if (_activated && _protector != null) {
            var direction = shield.transform.position - _protector.transform.position;
            shield.transform.LookAt(_protector.transform.position);
                }
    }


    public void Activate(Transform protector)
    {
        if (!_activated && _protector == null)
        {
            ////Debug.Log(protector.gameObject.name);
            _activated = true;
            shield.SetActive(true);
            shieldanim.SetTrigger("Activate");
            _protector = protector.gameObject;
        }
    }

    public void DeActivate(Transform protector)
    {
        if (_protector == protector.gameObject)
        {
            _activated = false;
            shield.SetActive(false);
            shieldanim.SetTrigger("DeActivate");
            _protector = null;
        }
    }

    public bool getActivation()
    {
        return _activated;
    }
}
