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

        
        projectilesList[19].GetComponent<Projectile>().setProperties(new Vector2(1,1),new Vector2(60,20),1,25,5,0,0,180,Projectile.ProjectileBehaviour.mouseControlled);
        projectilesList[19].SetActive(true);

    }

    private void instantiatePoolOfProjectiles()
    {
        projectilesList = new List<GameObject>();
        for (int i = 0; i < MaxProjectilesPoolSize; i++)
        {
            GameObject temp = GameObject.Instantiate((Resources.Load("Projectiles/ProjectileBase")) as GameObject, this.transform);
            temp.name = i.ToString();
            projectilesList.Add(temp);
            freeProjectilesList = new List<GameObject>(projectilesList);
        }
    } 

    public void reportAsFree(GameObject projectile)
    {

    }
}

