using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentWeapon : MonoBehaviour
{
    public int currentWeapon = 0;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i == currentWeapon);
        }
    }

    // Update is called once per frame
    void Update()
    {
        int oldweapon = currentWeapon;
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (currentWeapon >= transform.childCount - 1)
            {
                currentWeapon = 0;
            }
            else currentWeapon++;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (currentWeapon < transform.childCount - 1)
            {
                currentWeapon = transform.childCount - 1;
            }
            else currentWeapon--;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentWeapon = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentWeapon = 1;
        }
        if (oldweapon != currentWeapon)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(i == currentWeapon);
            }
        }
    }
}
