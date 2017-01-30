using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Dummy : Character
{
    protected override void Start()
    {
        base.Start();
        MyFaction = Faction.NPCEnemys;
        tag = "Living";
        hp = 200;
        body_main.GetComponent<SpriteRenderer>().color = Color.red;
    }
    public override uint hurtMe(uint dmg) {
        print(dmg);
        return hp -= dmg;
    }
    protected override void Update()
    {
        if(hp <= 0)
        {
            Debug.LogWarning(name + "is Dead");
            this.gameObject.transform.Rotate(0, 0, 180);
            hp = 250;
        }
    }
}

