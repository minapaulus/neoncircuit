using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float healthPoints = 100f;


    public enum AssignedColors { Color1, Color2, Color3};
    protected Color HPindic;

    public Renderer[] ren;
    private List<Material> _myMats = new List<Material>();  

    public AssignedColors assignedColor;

    protected Color initcolor; 
    // Start is called before the first frame update.
    // Every Renderer with a Neoncolor must be assigned publicly. Including circles and so forth.
    protected virtual void Start()
    {
        switch (assignedColor)
        {
            case AssignedColors.Color1:
                initcolor = Color.green;
                break;
            case AssignedColors.Color2:
                initcolor = Color.blue;
                break;
            case AssignedColors.Color3:
                initcolor = Color.red;
                break;
            default:
                initcolor = Color.white;
                break;
                }

        foreach(Renderer re in ren)
        {
            foreach(Material ma in re.materials)
            {
                _myMats.Add(ma);
            }
        }
    }

    protected virtual void Update()
    {
        if (healthPoints <= 0) Die();

        HPindic = initcolor * (healthPoints / 100f);
        foreach(Material mymat in _myMats)
        {
            mymat.SetColor("_EmissionColor", HPindic);
        }
    }

    private void Die()
    {
        // Only Destroy for now. Later a shader animation
        Destroy(this.gameObject);
    }

    public void DamageHP(float dmg)
    {
        healthPoints -= dmg;
    }
}
