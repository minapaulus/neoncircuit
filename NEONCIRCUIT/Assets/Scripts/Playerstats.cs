using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Playerstats : MonoBehaviour
{   
    public float HP = 100;
    private float HPmax;
    public Image HPCount;

    public float PrimaryAmmo = 100;
    private float PrimaryAmmoMax;
    public Image PrimaryAmmoCount;

    public float SecondaryAmmo = 2;
    private float SecondaryAmmoMax;
    public Image SecondaryAmmoCount;

    private void Start()
    {
        //Error if none is selected.

        HPmax = HP;
        PrimaryAmmoMax = PrimaryAmmo;
        SecondaryAmmoMax = SecondaryAmmo;
    }

    public void AddHP(float i)
    {   
        HP += i;
        if (HP > HPmax)
        {
            HP = HPmax;
        }
        if (HP <= 0)
        {
            //Death
            HP = 0;
        }
        HPCount.fillAmount = HP / HPmax;

    }
    public void AddPrimary(float i)
    {
        PrimaryAmmo += i;
        if (PrimaryAmmo > PrimaryAmmoMax)
        {
            PrimaryAmmo = PrimaryAmmoMax;
        }
        if (PrimaryAmmo <= 0)
        {
            PrimaryAmmo = 0;
        }
        PrimaryAmmoCount.fillAmount = PrimaryAmmo / PrimaryAmmoMax;
    }
    public void AddSecondary(float i)
    {
        SecondaryAmmo += i;
        if (SecondaryAmmo > SecondaryAmmoMax)
        {
            SecondaryAmmo = SecondaryAmmoMax;
        }
        if (SecondaryAmmo <= 0)
        {
            SecondaryAmmo = 0;
        }
        SecondaryAmmoCount.fillAmount = SecondaryAmmo / SecondaryAmmoMax;
    }
}
