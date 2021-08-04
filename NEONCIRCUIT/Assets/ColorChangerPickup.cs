using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChangerPickup : MonoBehaviour
{
    public Playerstats playerstat;
    public float restorePrimary;
    public float restoreSecondary;
    public Enemy.AssignedColors targetColor;
    private Color _paintColor;

    public Renderer[] ren;
    private List<Material> _mats = new List<Material>();

    //dictates how fast the pickup rotates around itself. 1 -> one rotation around y axis per second.
    [Range(0, 2)]
    public float rotationSpeed = .5f;

    public float downtime = 10f;
    private float _downduration = 0f;

    private void Start()
    {   
        foreach(Renderer re in ren)
        {
            foreach(Material mat in re.materials)
            {
                _mats.Add(mat);
            }
        }
        ChangeTargetColor(targetColor);
    }

    private void Update()
    {
        this.transform.Rotate(new Vector3(0, rotationSpeed * 360 * Time.deltaTime, 0));

        /* TestZwecke
        if (Input.GetMouseButtonDown(0))
        {
            ChangeTargetColor((Enemy.AssignedColors)(((int)targetColor + 1) % 4));
        }*/
    }

    public void ChangeTargetColor(Enemy.AssignedColors color)
    {
        targetColor = color;
        _paintColor = Enemy.ChooseColor(targetColor);
        if (this.gameObject.GetComponent<BoxCollider>().enabled)
        {
            PaintColor(_paintColor);
        }
    }

    private void PaintColor(Color x)
    {
        foreach (Material mat in _mats)
        {
            mat.SetColor("_EmissionColor", x * 3);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        playerstat.AddPrimary(restorePrimary);
        playerstat.AddSecondary(restoreSecondary);

        playerstat.ChangePrimaryColor(targetColor);

        StartCoroutine(ReactivateAfterXSec(downtime));
        PaintColor(Color.black);
        this.gameObject.GetComponent<BoxCollider>().enabled = false ;
    }

    private IEnumerator ReactivateAfterXSec(float x)
    {
        _downduration = 0f; 
        while(_downduration < downtime)
        {
            _downduration += Time.deltaTime;
            yield return null;
        }
        this.gameObject.GetComponent<BoxCollider>().enabled = true;
        ChangeTargetColor(targetColor);

    }
}
