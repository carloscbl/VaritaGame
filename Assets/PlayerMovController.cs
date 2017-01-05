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
    public float fsecondsDelayTeleport = 0.5f;
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

        if (Input.GetKeyUp(KeyCode.T))
        {
            StartCoroutine(Teleport());
        }
    }

    private IEnumerator Teleport()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10;//if z == 0 then we get the camera position, setting 10 units from the camera we get the desired position
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        newPosition.z = 0;

        yield return new WaitForSeconds(fsecondsDelayTeleport);

        //check if we can move to the desired position
        //We create the 2 points of the capsule at the desired point to teleport
        Vector3 p1, p2;
        CapsuleCollider capsule = GetComponent<CapsuleCollider>();
        p1 = newPosition + capsule.center;
        float h = capsule.height / 2 - capsule.radius;
        p2 = p1;
        p2.y += h;
        p1.y -= h;
        if (!Physics.CheckCapsule(p1, p2, capsule.radius))
        {
            transform.position = newPosition;
        }        
    }
}