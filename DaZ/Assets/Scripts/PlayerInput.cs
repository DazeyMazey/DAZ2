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
    private Vector3 collisionDown;
    private Vector3 collisionUp;
    private Vector3 collisionLeft;
    private Vector3 collisionRight;
    private Vector3 max_V;

    // character properties
    float width;
    float height;

    // Start is called before the first frame update
    void Start()
    {
        JumpVelocity = new Vector3(0, 1) * JUMPPOWER;
        HorizontalMovement = Vector3.zero;
        max_V = new Vector3(0, -MAX_FALLSPEED);

        Fall = true;
        acceleration = new Vector3(0, 1) * GRAVITYMOD;
        object_velocity = Vector3.zero;
        T_minus_1 = transform.position;

        width = GetComponent<SpriteRenderer>().bounds.size.x / 2;
        height = GetComponent<SpriteRenderer>().bounds.size.y / 2;
        detectiondistanceY = height + 0.05f;
        detectiondistanceX = width;

        collisionDown = new Vector3(0, -1);
        collisionUp = new Vector3(0, 1);
        collisionLeft = new Vector3(-1, 0);
        collisionRight = new Vector3(1, 0);

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
        RaycastHit2D hit;
        Fall = true;

        Vector3 temp = new Vector3(width, 0);
        // checks for falling
        if (hit = Physics2D.Raycast(transform.position, collisionDown, detectiondistanceY))
        {
            OnGroundPhysics(hit);
        }
        else if (hit = Physics2D.Raycast(transform.position + temp, collisionDown, detectiondistanceY))
        {
            OnGroundPhysics(hit);
        }
        else if (hit = Physics2D.Raycast(transform.position - temp, collisionDown, detectiondistanceY))
        {
            OnGroundPhysics(hit);
        }

        // checking for left walls
        if (hit = Physics2D.Raycast(transform.position, collisionLeft, detectiondistanceX))
        {
            OnWallPhysics(hit);
        }
        else if (hit = Physics2D.Raycast(transform.position, collisionLeft, detectiondistanceX))
        {
            OnWallPhysics(hit);
        }
        else if (hit = Physics2D.Raycast(transform.position, collisionLeft, detectiondistanceX))
        {
            OnWallPhysics(hit);
        }

        // checking for right walls
        if (hit = Physics2D.Raycast(transform.position, collisionRight, detectiondistanceX))
        {
            OnWallPhysics(hit);
        }
        else if (hit = Physics2D.Raycast(transform.position, collisionRight, detectiondistanceX))
        {
            OnWallPhysics(hit);
        }
        else if (hit = Physics2D.Raycast(transform.position, collisionRight, detectiondistanceX))
        {
            OnWallPhysics(hit);
        }

        // checks for ceiling
        if (hit = Physics2D.Raycast(transform.position, collisionUp, detectiondistanceY))
        {
            OnCeilingPhysics(hit);
        }
        else if (hit = Physics2D.Raycast(transform.position + temp, collisionUp, detectiondistanceY))
        {
            OnCeilingPhysics(hit);
        }
        else if (hit = Physics2D.Raycast(transform.position - temp, collisionUp, detectiondistanceY))
        {
            OnCeilingPhysics(hit);
        }


        // set previous frame
        T_minus_1 = this.transform.position;

        this.transform.position += HorizontalMovement * WALKSPEED;
        this.transform.position += object_velocity;


        if (Fall)
        {
            GravCalc();
        }


    }

    private void OnWallPhysics(RaycastHit2D hit)
    {
        if (hit.collider.tag == "Ground" || hit.collider.tag == "Ceiling")
        {
            HorizontalMovement = Vector3.zero;
            this.transform.position = T_minus_1;
        }
    }

    private void OnCeilingPhysics(RaycastHit2D hit)
    {
        if (hit.collider.tag == "Ground" || hit.collider.tag == "Ceiling")
        {
            object_velocity = Vector3.zero;
            this.transform.position = T_minus_1;
        }
    }

    private void OnGroundPhysics(RaycastHit2D hit)
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
        max_V *= -1;

        JumpVelocity *= -1;
        collisionDown *= -1;
        collisionUp *= -1;

        object_velocity += acceleration;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Colliding with object in OnCollision");

        if (collision.collider.gameObject.tag == "EnemyProjectile")
        {

        }
    }
}
