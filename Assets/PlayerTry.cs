using UnityEngine;
using System.Collections;
using System;

public class PlayerTry : MonoBehaviour
{

    //Floats
    public float speed = 50f;
    public float maxSpeed = 3;
    public float jumpPower = 150f;
    public float trueSpeed;
    public float control;

    //Booleans

    public bool canDoubleJump;
    public bool justJumped = false;

    public LayerMask whatIsGround;
    public Transform GroundCheck;

    public Vector3 GroundDirection = new Vector3(0, -1, 0);
    //References
    private Rigidbody rb2d;
    //public bool grounded ;

    // Use this for initialization
    void Start()
    {

        rb2d = gameObject.GetComponent<Rigidbody>();
        GroundCheck = transform.FindChild("GroundCheck");


    }
    bool grounded()
    {
        return Convert.ToBoolean(Physics.OverlapSphere(GroundCheck.position, 0.15f, whatIsGround).Length);
    }
    // Update is called once per frame
    void Update()
    {

        trueSpeed = rb2d.velocity.x;
        

        if (Input.GetButtonDown("Jump"))
        {
            //Debug.LogWarning(Physics.Raycast(GroundCheck.position, GroundDirection, 50f, whatIsGround).ToString());
            if (grounded())
            {
                //Debug.LogWarning("Jumping");

                rb2d.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
                canDoubleJump = true;
                justJumped = true;
            }

        }
        // Debug.LogWarning(grounded);

    }
    bool hmov;
    void FixedUpdate()
    {
        //fixes the slip and slide in unity
        

        float h = Input.GetAxisRaw("Horizontal");
        //Fake Friction / Easing x speed
        /*
        if (h != 0 && (rb2d.velocity.x > -0.1f || rb2d.velocity.x < 0.1f))
        {
            rb2d.velocity = new Vector2(0.0f, rb2d.velocity.y);
            hmov = false;
        }
        else
        {
            hmov = true;
        }
        */
        if (grounded())
        {
            //Moving the player
            rb2d.velocity = new Vector2(h * speed + rb2d.velocity.x, rb2d.velocity.y);
            //Limiting the speed of the player
            if (rb2d.velocity.x > maxSpeed)
            {
                rb2d.velocity = new Vector2(maxSpeed, rb2d.velocity.y);
            }
            else if (rb2d.velocity.x < -maxSpeed)
            {
                rb2d.velocity = new Vector2(-maxSpeed, rb2d.velocity.y);
            }
            if (h == 0)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x * 0.75f, rb2d.velocity.y);
            }

        }
        else
        {
            //Moving the player
            rb2d.velocity = new Vector2(h * speed + rb2d.velocity.x, rb2d.velocity.y);
            //Limiting the speed of the player
            if (rb2d.velocity.x > maxSpeed)
            {
                rb2d.velocity = new Vector2(maxSpeed, rb2d.velocity.y);
            }
            else if (rb2d.velocity.x < -maxSpeed)
            {
                rb2d.velocity = new Vector2(-maxSpeed, rb2d.velocity.y);
            }
            if (h == 0)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x * 0.75f, rb2d.velocity.y);
            }
        }
        




    }
}