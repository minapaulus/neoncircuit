using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float healthPoints = 100f;
    public float initialHP;


    public enum AssignedColors { Color1, Color2, Color3, Color4};
    protected Color HPindic;

    public Renderer[] ren;
    private List<Material> _myMats = new List<Material>();  

    public AssignedColors assignedColor;

    protected Color initcolor; 
    // Start is called before the first frame update.
    // Every Renderer with a Neoncolor must be assigned publicly. Including circles and so forth.
    protected virtual void Start()
    {
        initialHP = healthPoints;
        initcolor = ChooseColor(assignedColor);

        foreach(Renderer re in ren)
        {
            foreach(Material ma in re.materials)
            {
                _myMats.Add(ma);
            }
        }
        ChangeColor(initcolor);
    }

    public static Color ChooseColor(AssignedColors assignedColor)
    {
        var _initcolor = new Color();
        switch (assignedColor)
        {
            case AssignedColors.Color1:
                _initcolor = Color.green;
                break;
            case AssignedColors.Color2:
                _initcolor = Color.blue;
                break;
            case AssignedColors.Color3:
                _initcolor = Color.red;
                break;
            case AssignedColors.Color4:
                _initcolor = Color.white;
                break;
            default:
                _initcolor = Color.white;
                break;
        }

        return _initcolor;
    }

    protected virtual void Update()
    {
        //if (healthPoints <= 0) Die();

        //IMPORTANT FOR BOSS: initcolor = ChooseColor(assignedColor);
        // TODO: Auskommentieren und in Damage nur rein.
        //ChangeColor(initcolor);
    }

    public void ChangeAssignedColor(Enemy.AssignedColors newColor)
    {
        assignedColor = newColor;
        initcolor = ChooseColor(assignedColor);
        ChangeColor(initcolor);
    }

    public void ChangeColor(Color color)
    {
        HPindic =  color * (healthPoints / initialHP);
        foreach (Material mymat in _myMats)
        {
            mymat.SetColor("_EmissionColor", HPindic);
        }
    }

    protected void Die()
    {
        // Only Destroy for now. Later a shader animation
        Destroy(this.gameObject);
    }

    // Virtual added, does it make a difference? Does it still work?
    public virtual void DamageHP(float dmg)
    {
        if (healthPoints - dmg <= initialHP) healthPoints -= dmg;
        else healthPoints = initialHP;
        ChangeColor(initcolor);

        if (healthPoints <= 0) Die();
    }
}
