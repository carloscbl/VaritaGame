using UnityEngine;
using System.Collections;
using System;

public class PlayerMovController
    : MonoBehaviour
{
    float mHorizontalAxis;
    private Rigidbody mRigidBody;
    private bool mMoving;

    public int mJumpImpulse = 250;
    private void Start()
    {
        mRigidBody = gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        mHorizontalAxis = Input.GetAxisRaw("Horizontal");
        //Lerps Here Please
        if(Input.GetButton("Horizontal"))
            mRigidBody.velocity = new Vector2(mHorizontalAxis*5, mRigidBody.velocity.y);

        if (Input.GetButtonDown("Jump")){
            mRigidBody.AddForce((new Vector2(0, mJumpImpulse)), ForceMode.Force);
        }
        
    }

  
}