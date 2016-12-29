using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class ArwinFireElementalist : Character
{
    protected override void Start()
    {
        base.Start();
        //Set position
        gameObject.transform.Translate(new Vector2(20, 400), Space.World);

    }
    protected override void Update()
    {
        base.Update();
    }
}
