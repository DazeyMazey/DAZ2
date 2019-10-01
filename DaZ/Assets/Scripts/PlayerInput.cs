using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float JUMPPOWER = 20f;
    public float WALKSPEED = 5f;
    public bool Fall;
    public float GRAVITYMOD = -9.8f;
    public float INITIALVELOCITY = 0;
    public float MAX_FALLSPEED = 1;
    public int MAX_JUMPS = 2;

    // player movement vectors
    private Vector3 JumpVelocity;
    private Vector3 HorizontalMovement;
    private int curr_jumps = 0;

    // private gravity variables
    private Vector3 acceleration;
    private Vector3 object_velocity;
    private Vector3 T_minus_1;

    // collision variables
    private float detectiondistanceY;
    private float detectiondistanceX;
    private float diagnol_detectiondistance;
    private Vector3 collisionDown;
    private Vector3 collisionUp;
    private Vector3 collisionLeft;
    private Vector3 collisionRight;
    private Vector3 collisionUpLeft;
    private Vector3 collisionUpRight;
    private Vector3 collisionDownLeft;
    private Vector3 collisionDownRight;
    private Vector3 max_V;


    // Start is called before the first frame update
    void Start()
    {
        JumpVelocity = new Vector3(0, 1) * JUMPPOWER;
        HorizontalMovement = Vector3.zero;
        max_V = new Vector3(0, MAX_FALLSPEED);

        Fall = true;
        acceleration = new Vector3(0, 1) * GRAVITYMOD;
        object_velocity = Vector3.zero;
        T_minus_1 = transform.position;

        float width = GetComponent<SpriteRenderer>().bounds.size.x / 2;
        float height = GetComponent<SpriteRenderer>().bounds.size.y / 2;
        detectiondistanceY = height + 0.05f;
        detectiondistanceX = width;
        diagnol_detectiondistance = (float)Math.Sqrt(Math.Pow(width, 2) + Math.Pow(height, 2)); // X^2 + Y^2 = Z^2 calculating the diagnol max distance for collision detection

        collisionDown = new Vector3(0, -1);
        collisionUp = new Vector3(0, 1);
        collisionLeft = new Vector3(-1, 0);
        collisionRight = new Vector3(1, 0);
        collisionUpLeft = new Vector3(-1, 1);
        collisionUpRight = new Vector3(1, 1);
        collisionDownLeft = new Vector3(-1, -1);
        collisionDownRight = new Vector3(1, -1);
}


    private void Update()
    {
        HorizontalMovement.x = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && curr_jumps < MAX_JUMPS) // handles the jumping
        {
            Jump();
            this.transform.position += object_velocity;
        }
        if (Input.GetKeyDown(KeyCode.Q)) // handles the gravity
        {
            SwitchGravity();
        }



    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Detecting Collisions
        RaycastHit hit;
        Fall = true;


        // checks for falling
        if (Physics.Raycast(transform.position, collisionDown, out hit, detectiondistanceY))
        {
            OnGroundPhysics(hit);
        }
        else if (Physics.Raycast(transform.position, collisionDownLeft, out hit, diagnol_detectiondistance))
        {
            OnGroundPhysics(hit);
        }
        else if (Physics.Raycast(transform.position, collisionDownRight, out hit, diagnol_detectiondistance))
        {
            OnGroundPhysics(hit);
        }
        T_minus_1 = this.transform.position;

        this.transform.position += HorizontalMovement * WALKSPEED;
        this.transform.position += object_velocity;

        // check for side walls

        if (Fall)
        {
            GravCalc();
        }


    }

    private void OnGroundPhysics(RaycastHit hit)
    {
        if (hit.collider.tag == "Ground" || hit.collider.tag == "Ceiling")
        {
            Fall = false;
            object_velocity = Vector3.zero;
            this.transform.position = T_minus_1;
            curr_jumps = 0;
        }
    }

    private void Jump()
    {
        // add positive adder to object_velocity
        object_velocity = Vector3.zero;
        object_velocity += (JumpVelocity);
        Fall = true;
        curr_jumps++;
    }

    private void GravCalc()
     {
        if (object_velocity.magnitude < MAX_FALLSPEED)
            object_velocity += (acceleration);
        else
            object_velocity = max_V;

        
        // add a max cap to velocity and deltatime;
     }





    public void SwitchGravity()
    {
        acceleration *= -1;

        JumpVelocity *= -1;
        collisionDown *= -1;
        collisionUp *= -1;
        collisionDownLeft *= -1;
        collisionDownRight *= -1;
        collisionUpLeft *= -1;
        collisionUpRight *= -1;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Colliding with object in OnCollision");

        if (collision.collider.gameObject.tag == "EnemyProjectile")
        {

        }
    }
}
