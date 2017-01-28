using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class ArwinFireElementalist : Character
{
    public readonly Faction MyFaction = Faction.Player;
    protected override void Start()
    {
        base.Start();
        //Set position
        gameObject.transform.Translate(new Vector2(20, 400), Space.World);
        projectileSystem = GameObject.Find("ProjectileSystem").GetComponent<ProjectileSystem>();

        data = new ProjectileSystem.ProjectileData();
        data.owner = this.gameObject;
        data.factionAffectedByThisProjectile = Faction.EnemysPlusWildLife;
        data.OriginPosition = this.ArmRight.transform.position;
        data.TargetPosition = Vector3.zero;
        data.size = 1f;
        data.damage = 25;
        data.Velocity = 3;
        data.Pierceability = 0;
        data.AreaAttachedRadious = 0;
        data.LifeTime = 3;
        data.behaviour = Projectile.ProjectileBehaviour.linear;

    }

    ProjectileSystem.ProjectileData data;
    protected override void Update()
    {
        base.Update();
        if (Input.GetMouseButtonDown(0))
        {
            data.OriginPosition = this.ArmRight.transform.position;
            data.TargetPosition = InputUtils.getMousePosition();
            ProjectileSystem.thisSystem.ShootProjectile(data);
            //projectileSystem.ShootProjectile(data);
        }

    }
}
