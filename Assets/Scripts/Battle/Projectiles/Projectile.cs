using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    public enum ProjectileBehaviour
    {
        linear, exponential, rotatory, mouseControlled
    }
    public enum KindOfProjectile
    {
        Bullet,FireOrb,EnergyOrb,FireBullet
    }
    private Character.Faction AffectedByThisProjectile;
    private GameObject owner;
    private Vector2 OriginPos = new Vector2(0, 0); 
    private Vector2 TargetPos = new Vector2(0, 0);
    private float Size = 1;
    private float Damage = 0;
    private float Velocity = 0;
    private uint Pierceability = 0; // 0 no pierce, 1 1º collision wont stop the projectile, but yes the second one and so on
    private float AreaAttachedRadious = 0;
    private float LifeTime = 0;
    private bool StatsWereSet = false;
    private ProjectileBehaviour Behaviour;
    private KindOfProjectile kindOfPrefab;
    public bool shouldStop = false;
    private float forcedDestructionTime = 10;
    private Vector2 Direction;
    
    private void OnEnable()
    {
        this.GetComponent<SpriteRenderer>().color = Color.white;
        forcedDestructionTime = 10;
        shouldStop = false;
        counter = 0;
        currentLerpTime = 0;

    }
    private void OnDisable()
    {
        StatsWereSet = false;
        //Register as free
        transform.GetComponentInParent<ProjectileSystem>().registerAsFree(this.gameObject);
        counter = 0;
    }
    public void DestroyAnimationEnd()
    {
        this.gameObject.SetActive(false);
    }
    public void ProceedToEndProjectile()
    {
        //This call a track animator that make it invoke DestryoAnimationEnd()
        this.gameObject.GetComponent<Animator>().SetBool("destroy", true);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player" && other.gameObject.tag != "Projectiles")
        {
            if(other.tag == "Living" || other.tag == "Destructibles")
            {
                other.GetComponent<Character>().hurtMe((uint)this.Damage);
            }
            if(Pierceability == 0)
            {
                shouldStop = true;
            }else
            {
                Pierceability -= 1;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //print(collision.gameObject.name);
    }
    private void OnTriggerStay(Collider other)
    {
        //print("TrigerStay" + other.gameObject.name);
    }
    private void InitializeProperties()
    {
        this.transform.position = OriginPos;
        
        switch (kindOfPrefab)
        {
            case KindOfProjectile.Bullet:
                this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Art/Textures/Projectiles/testProjectile");
                this.GetComponent<Animator>().enabled = false;
                transform.localScale = new Vector3(1, 1, 0.25f);
                this.GetComponent<BoxCollider>().size = new Vector3(1, 0.25f, 0.25f);
                break;
            case KindOfProjectile.FireOrb:
                this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Art/Textures/Projectiles/Animations/FireOrb_traveler/_0000_FireOrb_travel1");
                this.GetComponent<Animator>().enabled = true;
                transform.localScale = new Vector3(1, 1, 0.25f);
                break;
            case KindOfProjectile.EnergyOrb:
                break;
            case KindOfProjectile.FireBullet:
                break;
            default:
                break;
        }
        this.transform.localScale *= this.Size;
        StatsWereSet = true;
    }

    Camera camera;
    private Vector3 mousePosition2;
    private Vector3 mousePosition;
    private uint counter = 0;
    Collider aa;
    private void Update()
    {
        if (StatsWereSet)
        {
            LifeTime -= Time.deltaTime;
            //Here we check LifeTime and movement

            if(LifeTime <= 0 || shouldStop == true)
            {
                ProceedToEndProjectile();

            }
            if (shouldStop)
            {
                forcedDestructionTime -= Time.deltaTime;
                if(forcedDestructionTime < 0)
                {
                    DestroyAnimationEnd();
                }
                return;
            }
            switch (Behaviour)
            {
                case ProjectileBehaviour.linear:
                    linearBehaviour();
                    break;
                case ProjectileBehaviour.exponential:
                    exponentialBehaviour();
                    break;
                case ProjectileBehaviour.rotatory:
                    break;
                case ProjectileBehaviour.mouseControlled:
                    mouseControlledBehaviour();
                    break;
            }
           
        }
    }

    public void setProperties(GameObject owner,Character.Faction FactionAffectedByThisProjectile,Projectile.KindOfProjectile KindOfProjectile,
        Vector2 Origin,Vector2 target,float size,float Damage,float Velocity, uint Pierceability, float AreaAttachedRadious, float LifeTime, ProjectileBehaviour behaviour)
    {
        this.owner = owner;
        this.AffectedByThisProjectile = FactionAffectedByThisProjectile;
        this.kindOfPrefab = KindOfProjectile;
        this.OriginPos = Origin;
        this.TargetPos = target;
        this.Size = size;
        this.Damage = Damage;
        this.Velocity = Velocity;
        this.Pierceability = Pierceability;
        this.AreaAttachedRadious = AreaAttachedRadious;
        this.LifeTime = LifeTime;
        this.Behaviour = behaviour;
        lerpTime = LifeTime;
        rotateTowardsTarget();
       
        //Init
        InitializeProperties();
    }
    private void rotateTowardsTarget()
    {
        transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(((TargetPos - OriginPos).y), ((TargetPos - OriginPos).x)) * Mathf.Rad2Deg);
    }
    private void mouseControlledBehaviour()
    {
        camera = GameObject.Find("CharacterSystem").GetComponent<CharacterSystem>().mainCharacter.transform.Find("Main Camera").gameObject.GetComponent<Camera>();
        mousePosition2 = Input.mousePosition;
        //Grab the current mouse position on the screen
        mousePosition = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z - camera.transform.position.z));
        //Rotates toward the mouse
        Vector3 target = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.x, Mathf.Atan2((mousePosition.y - transform.position.y), (mousePosition.x - transform.position.x)) * Mathf.Rad2Deg);
        gameObject.transform.eulerAngles = new Vector3(0, 0, Mathf.MoveTowardsAngle(transform.eulerAngles.z, target.z, 5));
        transform.position = Vector2.MoveTowards(transform.position, mousePosition, 0.1f);
    }

    float lerpTime = 2f;
    float currentLerpTime;
    private void linearBehaviour()
    {
        currentLerpTime += Time.deltaTime;
        if (currentLerpTime > lerpTime)
        {
            currentLerpTime = lerpTime;
        }
        float t = currentLerpTime / lerpTime;
        float radians = Mathf.Atan2(((TargetPos - OriginPos).y), ((TargetPos - OriginPos).x));
        float x = Mathf.Cos(radians) * Velocity;
        float y = Mathf.Sin(radians) * Velocity;
        Direction = new Vector2(x, y);
       
        this.transform.position = Vector3.Lerp(OriginPos, Direction + OriginPos, t);
        
    }
   
    private void exponentialBehaviour()
    {
        //Debug.DrawLine(OriginPos, TargetPos, Color.red);
        currentLerpTime += Time.deltaTime;
        if (currentLerpTime > lerpTime)
        {
            currentLerpTime = lerpTime;
        }
        float t = currentLerpTime / lerpTime;

        //S , smootherStep
        //t = t * t * t * (t * (6f * t - 15f) + 10f);

        //Exponential
        t = t * t;

        float radians = Mathf.Atan2(((TargetPos - OriginPos).y), ((TargetPos - OriginPos).x));
        float x = Mathf.Cos(radians) * Velocity;
        float y = Mathf.Sin(radians) * Velocity;
        Direction = new Vector2(x, y);

        this.transform.position = Vector3.Lerp(OriginPos, Direction + OriginPos, t);
        
    }

   

}

