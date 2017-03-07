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


        //Dav ini: Muevo el enemigo cerca para probar

        //No se porque se le pone al player la capa obstacle
        gameObject.layer = 0;

        Debug.Log("ArwinFireElementalist.cs:Moviendo a los enemigos a su sitio y persiguiendo player");
        
        GameObject enemyTest_3D;
        enemyTest_3D = GameObject.Find("EnemyTest_3D");
        enemyTest_3D.transform.Translate(new Vector3(30, 310,0), Space.World);

        //Enemigos
        enemyTest_3D = GameObject.Find("EnemyTest2_3D_volador");
        enemyTest_3D.transform.Translate(new Vector3(30, 300,0), Space.World);
        enemyTest_3D.GetComponent<EnemyPathfinding3D>().setTarget("Arwin");
        enemyTest_3D = GameObject.Find("EnemyTest2_3D_terrestre");
        enemyTest_3D.transform.Translate(new Vector3(30, 350,0), Space.World);
        enemyTest_3D.GetComponent<EnemyPathfinding3D>().setTarget("Arwin");

        ;

        //Dav fin
    }

    protected override void Update()
    {
        base.Update();
    }
}
