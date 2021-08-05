using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Playerstats : MonoBehaviour
{
    //public Camera playerCamera;

    public float HP = 100;
    private float HPmax;
    public Image HPCount;

    public float PrimaryAmmo = 100;
    private float PrimaryAmmoMax;
    public Image PrimaryAmmoCount;

    public Renderer primaryWeaponRenderer;
    private List<Material> _primaryWeaponMats = new List<Material>();
    public Renderer primaryWeaponRendererSphere;
    private List<Material> _primaryWeaponSphereMat = new List<Material>();

    public Material primaryUI;

    public Enemy.AssignedColors primaryColor;
    private Color _neonColor; 

    public float SecondaryAmmo = 2;
    private float SecondaryAmmoMax;
    public Image SecondaryAmmoCount;

    private void Awake()
    {
        //Error if none is selected.

        HPmax = HP;
        PrimaryAmmoMax = PrimaryAmmo;
        SecondaryAmmoMax = SecondaryAmmo;
        foreach (Material ma in primaryWeaponRenderer.materials)
        {
            _primaryWeaponMats.Add(ma);
            Debug.Log(ma.name);
        }
        foreach (Material mat in primaryWeaponRendererSphere.materials)
        {
            _primaryWeaponSphereMat.Add(mat);
            Debug.Log(mat.name);
        }
    }

    public bool CanFirePrimary()
    {
        return (PrimaryAmmo >= 1); 
    }

    public bool CanFireSecondary()
    {
        return (SecondaryAmmo >= 1);
    }

    public bool CanFireGrenade()
    {
        return (SecondaryAmmo >= 1);
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
            //make ball smaller and color in Neoncolor
            PrimaryAmmo = PrimaryAmmoMax;
        }
        if (PrimaryAmmo <= 0)
        {
            PrimaryAmmo = 0;
        }
        PrimaryAmmoCount.fillAmount = PrimaryAmmo / PrimaryAmmoMax;
        foreach (Material mat in _primaryWeaponMats)
        {
            mat.SetColor("_EmissionColor", _neonColor * PrimaryAmmo / 20);
        }
    }

    public void ChangePrimaryColor(Enemy.AssignedColors newcolor)
    {
        primaryColor = newcolor;
        _neonColor = Enemy.ChooseColor(primaryColor);
        Debug.Log(_primaryWeaponSphereMat.Count);
        foreach (Material ma in _primaryWeaponSphereMat)
        {
            ma.SetColor("_EmissionColor", _neonColor * 1);
        }
        foreach (Material mat in _primaryWeaponMats)
        {
            mat.SetColor("_EmissionColor", _neonColor * PrimaryAmmo / 20);
        }
        primaryUI.SetColor("_EmissionColor", _neonColor);
        // Change renderer of weapon. 

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
