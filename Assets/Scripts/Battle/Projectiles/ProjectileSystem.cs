using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;

class ProjectileSystem : MonoBehaviour
{
    private uint MaxProjectilesPoolSize = 128;
    private List<GameObject> projectilesList;
    private List<GameObject> freeProjectilesList;
    private void Start()
    {
        //instantiate the pool;
        instantiatePoolOfProjectiles();
    }

    private void instantiatePoolOfProjectiles()
    {
        projectilesList = new List<GameObject>();
        for (int i = 0; i < MaxProjectilesPoolSize; i++)
        {
            projectilesList.Add(GameObject.Instantiate((Resources.Load("Projectiles/ProjectileBase")) as GameObject,this.transform));
            freeProjectilesList = new List<GameObject>(projectilesList);
        }
    } 
}

