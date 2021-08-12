using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Playerstats : MonoBehaviour
{
    //public Camera playerCamera;
    // Max Variables made public, so we can easily modify current HP and ammo on Load of savefile in Start methods.

    public float HP = 100;
    public float HPmax;
    public Image HPCount;

    public int deaths = 0;

    public float time = 0f;
    private bool succeeded = false;

    public bool Checkpoint = false;

    public float CptHP;
    public float CptPrimary;
    public float CptSecondary;
    public Enemy.AssignedColors CptColor = Enemy.AssignedColors.Color1;

    public GameObject CheckpointObject;

    public float PrimaryAmmo = 100;
    public float PrimaryAmmoMax;
    public Image PrimaryAmmoCount;

    public Renderer primaryWeaponRenderer;
    private List<Material> _primaryWeaponMats = new List<Material>();
    public Renderer primaryWeaponRendererSphere;
    private List<Material> _primaryWeaponSphereMat = new List<Material>();

    private List<Renderer> ParticleEffects = new List<Renderer>();
    private GameObject _ammoSphere;
    private Vector3 _originalScale;

    public Image primaryUI;
    public Material[] HoloMaterials;

    public Enemy.AssignedColors primaryColor;
    private Color _neonColor; 

    public float SecondaryAmmo = 2;
    public float SecondaryAmmoMax;
    public Image SecondaryAmmoCount;
    private bool _dead = false;

    private void Awake()
    {
        //Error if none is selected.
        /*
        HPmax = HP;
        PrimaryAmmoMax = PrimaryAmmo;
        SecondaryAmmoMax = SecondaryAmmo;
        */
        foreach (Material ma in primaryWeaponRenderer.materials)
        {
            _primaryWeaponMats.Add(ma);
            //Debug.Log(ma.name);
        }
        foreach (Material mat in primaryWeaponRendererSphere.materials)
        {
            _primaryWeaponSphereMat.Add(mat);
            //Debug.Log(mat.name);
        }

        _ammoSphere = primaryWeaponRendererSphere.gameObject;
        _originalScale = _ammoSphere.transform.localScale;
        SetInitColors();
    }

    private void Update()
    {
        if (!succeeded)
        {
            time += Time.deltaTime;
            //Debug.Log(time);
            //Debug.Log(deaths);
        }
    }
    public void ChangeColorOfparticle(GameObject particle)
    {
        particle.GetComponent<Renderer>().material.SetColor("_EmissionColor", _neonColor * 1);
        ParticleEffects = new List<Renderer>();

        for (int i = 0; i < particle.transform.childCount; i++)
        {
            ParticleEffects.Add(particle.transform.GetChild(i).GetComponent<Renderer>());
        }
        foreach (Renderer ren in ParticleEffects)
        {
            foreach (Material mat in ren.materials)
            {
                mat.SetColor("_EmissionColor", _neonColor * 1);
            }
        }

    }

    void SetInitColors()
    {
        for(int i = 0; i < HoloMaterials.Length; i++)
        {
            HoloMaterials[i].SetColor("_EmissionColor", Enemy.ChooseColor((Enemy.AssignedColors) i));
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


    public bool AddHP(float i)
    {
        // if full life return false so health pickup does not get destroyed
        if (HP == HPmax && i > 0) return false;

        HP += i;
        if (HP > HPmax)
        {
            HP = HPmax;
        }
        if (HP <= 0)
        {
            //Death
            HP = 0;
            Die();
        }
        HPCount.fillAmount = HP / HPmax;
        return true;
    }

    private void Die()
    {
        if (!_dead)
        {
            deaths++;
            _dead = true;
            SavePlayer();
        }
        //Activate Death UI;
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

        _ammoSphere.transform.localScale = _originalScale * (PrimaryAmmo/PrimaryAmmoMax);
    }

    public void ChangePrimaryColor(Enemy.AssignedColors newcolor)
    {
        primaryColor = newcolor;
        _neonColor = Enemy.ChooseColor(primaryColor);
        //Debug.Log(_primaryWeaponSphereMat.Count);
        foreach (Material ma in _primaryWeaponSphereMat)
        {
            ma.SetColor("_EmissionColor", _neonColor * 1);
        }
        foreach (Material mat in _primaryWeaponMats)
        {
            mat.SetColor("_EmissionColor", _neonColor * PrimaryAmmo / 20);
        }

        primaryUI.material = HoloMaterials[(int)newcolor];
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

    public void LoadPlayer ()
    {
        //Debug.Log("Load");
        PlayerData data = SaveSystem.LoadPlayer();

        if (data != null)
        {

            Checkpoint = data.hasReachedCheckpoint;
            Debug.Log(Checkpoint);

            if (Checkpoint && CheckpointObject != null)
            {
                this.gameObject.GetComponent<CharacterController>().enabled = false;
                this.gameObject.transform.position = CheckpointObject.transform.position;
                this.gameObject.GetComponent<CharacterController>().enabled = true;
                this.gameObject.transform.rotation = CheckpointObject.transform.rotation;

                primaryColor = (Enemy.AssignedColors)data.assignedColor;
                PrimaryAmmo = data.PrimAmmo;
                SecondaryAmmo = data.secAmmo;
                ChangePrimaryColor(primaryColor);
                AddHP(0);
                AddPrimary(0);
                AddSecondary(0);
            }

            /*Testzwecke: 

            this.gameObject.transform.position = new Vector3(data.playerPos[0],data.playerPos[1], data.playerPos[2]);
            this.gameObject.transform.rotation = Quaternion.Euler(new Vector3(data.playerRot[0], data.playerRot[1], data.playerRot[2]));
            */
            deaths = data.deathCount;
            time = data.time;

        }

    }

    public void SavePlayer()
    {
        //Debug.Log("Save");
        SaveSystem.SavePlayer(this);
    }

    public void TriggerCheckPoint()
    {
        // enable shortly UI for Checkpoint saved. 
        //When the checkpoint is saved, we save the HP,Ammocounts and colors for said checkpoint.
        if (!Checkpoint)
        {
            Checkpoint = true;
            CptHP = HP;
            CptPrimary = PrimaryAmmo;
            CptSecondary = SecondaryAmmo;
            CptColor = primaryColor;

            SavePlayer();
        }
    }
    private void EndGame()
    {
        //is called when Boss is dead.
        succeeded = true;
        //Enable UI where we set Textfields for Death and Time with two buttons to get to main menu or restart or Quit and deactivate pause menu object.
    }
}
