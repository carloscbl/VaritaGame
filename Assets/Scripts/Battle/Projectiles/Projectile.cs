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
    
    private void OnEnable()
    {
        counter = 0;
        currentLerpTime = 0;

    }
    private void OnDisable()
    {
        StatsWereSet = false;
        //Register as free
        transform.GetComponentInParent<ProjectileSystem>().registerAsFree(this.gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        //print("Colision");
        //print(other.gameObject.name);
        if (other.gameObject.tag == "Player")
        {

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
        this.transform.localScale *= this.Size;
        this.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Art/Textures/Projectiles/Materials/testProjectile");

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

            if(LifeTime <= 0)
            {
                this.gameObject.SetActive(false);
            }

            if(Behaviour == ProjectileBehaviour.mouseControlled)
            {
                mouseControlledBehaviour();
            }
            else if(Behaviour == ProjectileBehaviour.linear)
            {
                linearBehaviour();
            }
            else if(Behaviour == ProjectileBehaviour.exponential)
            {
                exponentialBehaviour();
            }
        }
    }

    public void setProperties(GameObject owner,Character.Faction FactionAffectedByThisProjectile, Vector2 Origin,Vector2 target,float size,float Damage,float Velocity, uint Pierceability, float AreaAttachedRadious, float LifeTime, ProjectileBehaviour behaviour)
    {
        this.owner = owner;
        this.AffectedByThisProjectile = AffectedByThisProjectile;
        this.OriginPos = Origin;
        this.TargetPos = target;
        this.Size = size;
        this.Damage = Damage;
        this.Velocity = Velocity;
        this.Pierceability = Pierceability;
        this.AreaAttachedRadious = AreaAttachedRadious;
        this.LifeTime = LifeTime;
        this.Behaviour = behaviour;
        InitializeProperties();
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

    float lerpTime = 3f;
    float currentLerpTime;
    private void linearBehaviour()
    {
        currentLerpTime += Time.deltaTime;
        if (currentLerpTime > lerpTime)
        {
            currentLerpTime = lerpTime;
        }
        float t = currentLerpTime / lerpTime;
        this.transform.position = Vector3.Lerp(OriginPos,TargetPos,t);
        Debug.DrawLine(OriginPos, TargetPos, Color.red);
        if (counter == 0)
        {
            
            transform.Rotate(Vector3.forward, Mathf.Atan2(((TargetPos - OriginPos).y), ((TargetPos - OriginPos).x)) * Mathf.Rad2Deg, Space.World);
            counter += 1;
        }
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
       
        transform.position = new Vector3(Mathf.SmoothStep(OriginPos.x,TargetPos.x,t), Mathf.SmoothStep(OriginPos.y, TargetPos.y, t), 0);
        if (counter == 0)
        {
            transform.Rotate(Vector3.forward, Mathf.Atan2(((TargetPos - OriginPos).y), ((TargetPos - OriginPos).x)) * Mathf.Rad2Deg, Space.World);
            counter += 1;
        }

    }

   

}

