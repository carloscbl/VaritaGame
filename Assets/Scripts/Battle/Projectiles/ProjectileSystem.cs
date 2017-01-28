using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;

public class ProjectileSystem : MonoBehaviour
{
    private uint MaxProjectilesPoolSize = 128;
    private List<GameObject> projectilesList;
    private List<GameObject> freeProjectilesList;

    public static ProjectileSystem thisSystem;

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
            freeProjectilesList.Remove(projectilesList[i]);
        }
        float tiempoFinal = Time.realtimeSinceStartup;
        print(tiempoFinal - tiempo);
        thisSystem = this;
    }

    private GameObject GetOneFreePooledProjectile()
    {
        GameObject temp = freeProjectilesList[0];
        freeProjectilesList.Remove(temp);
        return temp;
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

    public void registerAsFree(GameObject projectile)
    {
        freeProjectilesList.Add(projectile);
    }

    public struct ProjectileData
    {
        public GameObject                       owner;
        public Character.Faction                factionAffectedByThisProjectile;
        public Vector2                          OriginPosition;
        public Vector2                          TargetPosition;
        public float                            damage;
        public float                            size;
        public float                            Velocity;
        public uint                             Pierceability;
        public float                            LifeTime;
        public Projectile.ProjectileBehaviour   behaviour;
        public float                            AreaAttachedRadious;

    }
    public void ShootProjectile(ProjectileData data)
    {
        GameObject projectile = GetOneFreePooledProjectile();
        projectile.GetComponent<Projectile>().setProperties(
            data.owner,
            data.factionAffectedByThisProjectile,
            data.OriginPosition,
            data.TargetPosition,
            data.size,
            data.damage,
            data.Velocity,
            data.Pierceability,
            data.AreaAttachedRadious,
            data.LifeTime,
            data.behaviour);
        projectile.SetActive(true);
    }
}

