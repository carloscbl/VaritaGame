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

        GameObject mainplayer = GameObject.Find("CharacterSystem").GetComponent<CharacterSystem>().getMainCharacter();

        float tiempo = Time.realtimeSinceStartup;
        for (int i = 0; i < 5; i++)
        {
            projectilesList[i].GetComponent<Projectile>().setProperties(mainplayer, Character.Faction.EnemysPlusWildLife,
            new Vector2(1, 1), new Vector2(20, 5 + (1* i)), 1, 25, 5, 0, 0, 180, Projectile.ProjectileBehaviour.linear);
            projectilesList[i].SetActive(true);
        }
        float tiempoFinal = Time.realtimeSinceStartup;
        print(tiempoFinal - tiempo);

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

