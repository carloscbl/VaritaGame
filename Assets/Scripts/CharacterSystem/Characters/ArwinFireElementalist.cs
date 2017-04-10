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
        gameObject.AddComponent<PlayerMovController>();
        //Set position
        gameObject.transform.Translate(new Vector2(2, 256), Space.World);
        projectileSystem = GameObject.Find("ProjectileSystem").GetComponent<ProjectileSystem>();
        MyFaction = Faction.Player;
        data = new ProjectileSystem.ProjectileData();
        data.owner = this.gameObject;
        data.factionAffectedByThisProjectile = Faction.EnemysPlusWildLife;
        data.KindOfProjectile = Projectile.KindOfProjectile.FireOrb;
        data.OriginPosition = Vector3.zero;
        data.TargetPosition = Vector3.zero;
        data.size = 0.75f;
        data.damage = 25;
        data.Velocity = 18;
        data.Pierceability = 0;
        data.AreaAttachedRadious = 0;
        data.LifeTime = 4;
        data.behaviour = Projectile.ProjectileBehaviour.exponential;

    }

    ProjectileSystem.ProjectileData data;
    protected override void Update()
    {
        base.Update();
        if (Input.GetMouseButtonDown(0))
        {
            data.KindOfProjectile = Projectile.KindOfProjectile.FireOrb;
            data.behaviour = Projectile.ProjectileBehaviour.exponential;
            data.size = 0.75f;
            data.Pierceability = 2500;
            data.OriginPosition = this.WeaponProjectileHelper.transform.position;
            data.TargetPosition = InputUtils.getMousePosition();
            ProjectileSystem.thisSystem.ShootProjectile(data);
            //projectileSystem.ShootProjectile(data);
        }else if (Input.GetMouseButtonDown(1))
        {
            data.KindOfProjectile = Projectile.KindOfProjectile.Bullet;
            data.size = 1;
            data.behaviour = Projectile.ProjectileBehaviour.linear;
            data.Pierceability = 2;
            data.OriginPosition = this.WeaponProjectileHelper.transform.position;
            data.TargetPosition = InputUtils.getMousePosition();
            ProjectileSystem.thisSystem.ShootProjectile(data);
        }

    }
}
