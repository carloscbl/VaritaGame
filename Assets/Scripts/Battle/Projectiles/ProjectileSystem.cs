using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;

public class ProjectileSystem : MonoBehaviour
{
    private readonly uint MaxProjectilesPoolSize = 24;
    private uint timesResizedPool = 0;
    private List<GameObject> projectilesList;
    private List<GameObject> freeProjectilesList;

    public static ProjectileSystem thisSystem;

    private void Awake()
    {
        thisSystem = this;
    }
    private void Start()
    {
        //instantiate the pool;
        instantiatePoolOfProjectiles();
        
    }
    private void resizePoolAnother_24()
    {
        timesResizedPool += 1;
        for (int i = 0; i < 24; i++)
        {
            GameObject temp = GameObject.Instantiate((Resources.Load("Projectiles/ProjectileBase")) as GameObject, this.transform);
            temp.name = (projectilesList.Count + 1).ToString();
            projectilesList.Add(temp);
            freeProjectilesList.Add(temp);
        }
        
    }
    private GameObject GetOneFreePooledProjectile()
    {
        if(freeProjectilesList.Count == 0)
        {
            resizePoolAnother_24();
        }
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
        public Projectile.KindOfProjectile      KindOfProjectile;
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
            data.KindOfProjectile,
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

