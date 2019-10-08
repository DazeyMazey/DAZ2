﻿using System;
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
    public int GRAVITY_USES = 5;

    // player movement vectors
    private Vector2 JumpVelocity;
    private Vector2 HorizontalMovement;
    private int curr_jumps = 0;

    // private gravity variables
    private Vector2 acceleration;
    private Vector2 object_velocity;
    private Vector2 T_minus_1_Y;
    private Vector2 T_minus_1_X;

    // collision variables
    private float detectiondistanceY;
    private float detectiondistanceX;
    private Vector2 collisionDown;
    private Vector2 collisionUp;
    private Vector2 collisionLeft;
    private Vector2 collisionRight;
    private Vector2 max_V;

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
        T_minus_1_Y = new Vector3(0, transform.position.y);
        T_minus_1_X = new Vector3(transform.position.x, 0);

        width = (GetComponent<SpriteRenderer>().bounds.size.x / 2);
        height = GetComponent<SpriteRenderer>().bounds.size.y / 2;

        detectiondistanceY = height;
        detectiondistanceX = width + 0.3f;

        collisionDown = new Vector2(0, -1);
        collisionUp = new Vector2(0, 1);
        collisionLeft = new Vector2(-1, 0);
        collisionRight = new Vector2(1, 0);
}


    private void Update()
    {
        HorizontalMovement.x = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && curr_jumps < MAX_JUMPS) // handles the jumping
        {
            Jump();
            this.transform.Translate(JumpVelocity);
        }
        if (Input.GetKeyDown(KeyCode.Q)) // handles the gravity
        {
            // Check to see if you have uses of gravity
            SwitchGravity();
        }



    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Move Character
        this.transform.Translate((HorizontalMovement * WALKSPEED) + object_velocity);


        // Detecting Collisions
        RaycastHit2D hit;
        Vector3 temp = new Vector3(width, 0);

        // checking for left walls
        if (hit = Physics2D.Raycast(transform.position, collisionLeft, detectiondistanceX))
        {
            OnWallLeftPhysics(hit);
        }
        //else if (hit = Physics2D.Raycast(transform.position, collisionLeft, detectiondistanceX))
        //{
        //    OnWallPhysics(hit);
        //}
        //else if (hit = Physics2D.Raycast(transform.position, collisionLeft, detectiondistanceX))
        //{
        //    OnWallPhysics(hit);
        //}

        // checking for right walls
        if (hit = Physics2D.Raycast(transform.position, collisionRight, detectiondistanceX))
        {
            OnWallRightPhysics(hit);
        }
        //else if (hit = Physics2D.Raycast(transform.position, collisionRight, detectiondistanceX))
        //{
        //    OnWallPhysics(hit);
        //}
        //else if (hit = Physics2D.Raycast(transform.position, collisionRight, detectiondistanceX))
        //{
        //    OnWallPhysics(hit);
        //}


        // checks for falling
        if (hit = Physics2D.Raycast(this.transform.position, collisionDown, detectiondistanceY))
        {
            OnGroundPhysics(hit);
        }
        else if (hit = Physics2D.Raycast(this.transform.position + temp, collisionDown, detectiondistanceY))
        {
            OnGroundPhysics(hit);
        }
        else if (hit = Physics2D.Raycast(this.transform.position - temp, collisionDown, detectiondistanceY))
        {
            OnGroundPhysics(hit);
        }
        else
        {
            Fall = true;
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
        T_minus_1_Y.y = this.transform.position.y;
        T_minus_1_X.x = this.transform.position.x;


        if (Fall)
        {
            GravCalc();
        }


    }

    private void OnWallLeftPhysics(RaycastHit2D hit)
    {
        if (hit.collider.tag == "Ground")
        {
            HorizontalMovement = Vector2.zero;
            transform.position = new Vector3(hit.point.x + detectiondistanceX, transform.position.y);
        }
    }

    private void OnWallRightPhysics(RaycastHit2D hit)
    {
        if (hit.collider.tag == "Ground")
        {
            HorizontalMovement = Vector2.zero;
            transform.position = new Vector3(hit.point.x - detectiondistanceX, transform.position.y);
        }
    }

    private void OnCeilingPhysics(RaycastHit2D hit)
    {
        if (hit.collider.tag == "Ground" || hit.collider.tag == "Ceiling")
        {
            object_velocity = Vector2.zero;
            transform.position = new Vector3(transform.position.x, hit.point.y - height);
        }
    }

    private void OnGroundPhysics(RaycastHit2D hit)
    {
        if (hit.collider.tag == "Ground" || hit.collider.tag == "Ceiling")
        {
            Fall = false;
            object_velocity = Vector2.zero;
            curr_jumps = 0;

            Debug.Log("Player: " + transform.position);
            Debug.Log("Hit Point: " + hit.point);

            transform.position = new Vector3(transform.position.x, hit.point.y + height);
        }
    }


    private void Jump()
    {
        // add positive adder to object_velocity
        object_velocity = Vector2.zero;
        object_velocity += JumpVelocity;
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
        height *= -1;

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
