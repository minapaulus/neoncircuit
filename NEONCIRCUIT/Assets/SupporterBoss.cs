using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupporterBoss : Enemy
{
    public GameObject Boss;

    protected override void Start()
    {
        base.Start();
        int i = (int) UnityEngine.Random.Range(0f, 1.9f);
        base.ChangeAssignedColor((Enemy.AssignedColors)i);
        Boss = GameObject.Find("Boss-Spherephiroth");
    }

    // Update is called once per frame
    protected override void Update()
    {
        this.transform.LookAt(Boss.transform.GetChild(0).transform.position);
    }

    public override void DamageHP(float dmg)
    {
        healthPoints -= dmg;
        ChangeColor(initcolor);



        if (healthPoints <= 0)
        {
            Boss.GetComponent<Boss>().SupporterKilled(this.gameObject);
            base.Die();
        }
    }
}
