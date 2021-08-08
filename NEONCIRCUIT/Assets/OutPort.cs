using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutPort : MonoBehaviour
{
    public ShootWhenRdy weapon;
    public GameObject PortalOut;
    public GameObject[] weapons;

    public Boss Boss;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Projectile")
        {
            Transform _playerTarget = GameObject.FindGameObjectWithTag("Player").transform;
            Destroy(other.gameObject);
            if(weapon)
            {
                weapon.Execute(weapons[0].transform,_playerTarget, weapons, _playerTarget.gameObject, Boss.GetInitColor());
            }
        }
    }
}
