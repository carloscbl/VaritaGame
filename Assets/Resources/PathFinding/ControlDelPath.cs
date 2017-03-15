using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Hacer que el AstarPath cambie cuando se vaya a salir de él.
//      

public class ControlDelPath : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("0"))//Creamos la malla del A*
        {
            Debug.Log("Creando el Astar");

            //¿Iria en CharacterSystem.cs?
            //Creamos el AstarPath
            Debug.Log("ControlDelPath:Creando el A_star a mano.");
            GameObject newA_star = Instantiate(Resources.Load("A_star")) as GameObject;
            newA_star.transform.position = new Vector3(0, 0, 0);
            newA_star.transform.SetParent(this.transform);

            //Esto en el AstarPath.cs ->GraphUpdateObject guo = new graphupdateobject(mybounds);
            //TODO: En Chunk.cs anyado la capa obstacle cuando se crea el objeto para que se excluya
                  //del pathfinding.
            


        }
        if (Input.GetKeyDown("9"))//Clonar enemigo
        {
            Debug.Log("ControlDelPath:");

            //int maxX = 30;
            //int maxZ = 310;

            //float x = Random.value * maxX;
            float x = 30;
            //float z = Random.value * maxZ;
            //float y = 0;
            float y = 320;
            
            //Transform enemigo = (Transform)Instantiate((Object)GameObject.Find("EnemyTest2_3D_volador"), new Vector3(x, y, 0),Quaternion.identity);
            
            
        }

        if (Input.GetKeyDown("8")) //reescaneamos el A*
        {
            Debug.Log("ControlDelPath:");
            AstarPath.active.Scan();

        }
        if (Input.GetKeyDown("7"))
        {
            Debug.Log("ControlDelPath:");

        }

    }
}
