using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class ArwinFireElementalist : Character
{
    protected override void Update()
    {
        HatSkin.transform.Translate(0, 0.01f, 0);
        //Debug.Log("Hola");
    }
}
