using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
/** VARIABLES **/
    // Globals accessed in Unity
    public float JUMPPOWER = 20f;
    public float WALKSPEED = 5f;
    public bool Fall;
    public float GRAVITYMOD = -9.8f;
    public float INITIALVELOCITY = 0;
    public float MAX_FALLSPEED = 1;
    public int MAX_JUMPS = 2;
    public bool GRAVITY_USE;
    public bool PlayerEnabled;

    // Game Objects
    public GameObject Bullet;

    // player movement vectors
    private Vector2 JumpVelocity;
    private Vector2 HorizontalMovement;
    private int curr_jumps = 0;

    // private gravity variables
    private Vector2 acceleration;
    private Vector2 object_velocity;

    // collision variables
    private float detectiondistanceY;
    private float detectiondistanceX;
    private Vector2 collisionDown;
    private Vector2 collisionUp;
    private Vector2 collisionLeft;
    private Vector2 collisionRight;
    private Vector2 max_V;
    private float collisionMod = 0.04f;

    RaycastHit2D hitDown;
    RaycastHit2D hitDown2;
    RaycastHit2D hitDown3;

    RaycastHit2D hitUp;
    RaycastHit2D hitUp2;
    RaycastHit2D hitUp3;

    // character properties
    float width;
    float height;

  
    
    
    
/** FUNCTIONS **/ 
    
    // Start is called before the first frame update
    void Start()
    {
        PlayerEnabled = true;
        JumpVelocity = new Vector3(0, 1) * JUMPPOWER;
        HorizontalMovement = Vector3.zero;
        max_V = new Vector3(0, -MAX_FALLSPEED);

        Fall = true;
        acceleration = new Vector3(0, 1) * GRAVITYMOD;
        object_velocity = Vector3.zero;

        width = (GetComponent<SpriteRenderer>().bounds.size.x / 2);
        height = (GetComponent<SpriteRenderer>().bounds.size.y / 2);

        detectiondistanceY = height;
        detectiondistanceX = width + collisionMod;

        collisionDown = new Vector2(0, -1);
        collisionUp = new Vector2(0, 1);
        collisionLeft = new Vector2(-1, 0);
        collisionRight = new Vector2(1, 0);

        Vector3 temp = new Vector3(width, 0);

        hitDown = Physics2D.Raycast(this.transform.position, collisionDown, detectiondistanceY);
        hitDown2 = Physics2D.Raycast(this.transform.position + temp, collisionDown, detectiondistanceY);
        hitDown3 = Physics2D.Raycast(this.transform.position - temp, collisionDown, detectiondistanceY);

        hitUp = Physics2D.Raycast(transform.position, collisionUp, detectiondistanceY);
        hitUp2 = Physics2D.Raycast(transform.position + temp, collisionUp, detectiondistanceY);
        hitUp3 = Physics2D.Raycast(transform.position - temp, collisionUp, detectiondistanceY);
}

    // update called as much as possible
    private void Update()
    {
        if (PlayerEnabled)
            HorizontalMovement.x = Input.GetAxis("Horizontal");
        else
            HorizontalMovement.x = 0;


        float aimY = Input.GetAxis("Vertical");
        float aimX = HorizontalMovement.x;

        if (Input.GetKeyDown(KeyCode.Space) && curr_jumps < MAX_JUMPS) // handles the jumping
        {
            Jump();
            this.transform.Translate(JumpVelocity);
        }
        if (Input.GetKeyDown(KeyCode.Q)) // handles the gravity
        {
            // Check to see if you have uses of gravity
            if (GRAVITY_USE && PlayerEnabled)
            {
                SwitchGravity();
                GRAVITY_USE = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && PlayerEnabled)
        {
            RaycastHit2D hit = InteractionChecker();
            if (hit)
            {
                InteractWithObject(hit);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Move Character
        this.transform.Translate((HorizontalMovement * WALKSPEED) + object_velocity);

        // Temp Vectors
        Vector3 temp = new Vector3(width, 0);
        Vector3 temp2 = new Vector3(0, height);

        // Detecting Collisions-- We're first casting the sides so we can do comparisons instead of prioritizing one side over another
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, collisionLeft, detectiondistanceX);
        RaycastHit2D hitUpLeft = Physics2D.Raycast(transform.position + temp2, collisionLeft, detectiondistanceX);
        RaycastHit2D hitDownLeft = Physics2D.Raycast(transform.position - temp2, collisionLeft, detectiondistanceX);
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, collisionRight, detectiondistanceX);
        RaycastHit2D hitUpRight = Physics2D.Raycast(transform.position + temp2, collisionRight, detectiondistanceX);
        RaycastHit2D hitDownRight = Physics2D.Raycast(transform.position - temp2, collisionRight, detectiondistanceX);

        //checking for left walls
        if (hitLeft)
        {
            OnWallLeftPhysics(hitLeft);
        }
        else if (hitUpLeft && !hitUpRight && !hitUp2)
        {
            OnWallLeftPhysics(hitUpLeft);
        }
        else if (hitDownLeft && !hitDownRight && !hitDown2)
        {
            OnWallLeftPhysics(hitDownLeft);
        }

        // checking for right walls
        if (hitRight)
        {
            OnWallRightPhysics(hitRight);
        }
        else if (!hitUpLeft && hitUpRight && !hitUp3)
        {
            OnWallRightPhysics(hitUpRight);
        }
        else if (!hitDownLeft && hitDownRight && !hitDown3)
        {
            OnWallRightPhysics(hitDownRight);
        }


        hitDown = Physics2D.Raycast(this.transform.position, collisionDown, detectiondistanceY);
        hitDown2 = Physics2D.Raycast(this.transform.position + temp, collisionDown, detectiondistanceY);
        hitDown3 = Physics2D.Raycast(this.transform.position - temp, collisionDown, detectiondistanceY);

        hitUp = Physics2D.Raycast(transform.position, collisionUp, detectiondistanceY);
        hitUp2 = Physics2D.Raycast(transform.position + temp, collisionUp, detectiondistanceY);
        hitUp3 = Physics2D.Raycast(transform.position - temp, collisionUp, detectiondistanceY);

        // checks for falling and bottom collisons
        if (hitDown)
        {
            OnGroundPhysics(hitDown);
        }
        else if (hitDown2)
        {
            OnGroundPhysics(hitDown2);
        }
        else if (hitDown3)
        {
            OnGroundPhysics(hitDown3);
        }
        else
        {
            Fall = true;
        }

        // checks for ceiling
        if (hitUp)
        {
            OnCeilingPhysics(hitUp);
        }
        else if (hitUp2)
        {
            OnCeilingPhysics(hitUp2);
        }
        else if (hitUp3)
        {
            OnCeilingPhysics(hitUp3);
        }



        // Calc Gravity if no downwards collisions were detected
        if (Fall)
        {
            GravCalc();
        }


    }
  
    
  
    
    /** Character Functions for movement and Enabling **/
    private void InteractWithObject(RaycastHit2D hit)
    {
        hit.collider.SendMessageUpwards("interact", SendMessageOptions.DontRequireReceiver);
        PausePlayer();
    }
   
    // Adds velocity to object after setting to zero, that way the jump always starts at 0
    // sets fall to true and adds to the current jumps that way we can monitor the number of jumps
    private void Jump()
    {
        // add positive adder to object_velocity
        object_velocity = Vector2.zero;
        object_velocity += JumpVelocity;
        Fall = true;
        curr_jumps++;
    }

    // Pause player horizontal and gravity manipulation movement
    public void PausePlayer()
    {
        PlayerEnabled = false;
    }
    // Re-enable player horizontal and gravity manipulation movement
    public void PlayPlayer()
    {
        PlayerEnabled = true;
    }



    
    /** Physics for platforming **/
    private void OnWallLeftPhysics(RaycastHit2D hit)
    {
        if (hit && hit.collider.tag == "Ground")
        {
            transform.position = new Vector3(hit.point.x + detectiondistanceX, transform.position.y);
        }
    }

    private void OnWallRightPhysics(RaycastHit2D hit)
    {
        if (hit && hit.collider.tag == "Ground")
        {
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
            //Debug.Log("Player: " + transform.position);
            //Debug.Log("Hit Point: " + hit.point);
            Fall = false;
            object_velocity = Vector2.zero;
            curr_jumps = 0;
            transform.position = new Vector3(transform.position.x, hit.point.y + height);
            GRAVITY_USE = true;
        }
    }

   
    /** Interactions with story **/
    private RaycastHit2D InteractionChecker()
    {
        Vector3 temp = new Vector3(width, 0);
        Vector3 temp2 = new Vector3(0, height);

        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, collisionLeft, detectiondistanceX);
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, collisionRight, detectiondistanceX);
        RaycastHit2D hitDowntemp = Physics2D.Raycast(this.transform.position, collisionDown, detectiondistanceY);
        RaycastHit2D hitUptemp = Physics2D.Raycast(transform.position, collisionUp, detectiondistanceY);

        if (hitLeft)
        {
            if (hitLeft.collider.tag == "interact")
                return hitLeft;
        }
        if (hitRight)
        {
            if (hitRight.collider.tag == "interact")
                return hitRight;
        }
        if (hitDowntemp)
        {
            if (hitDowntemp.collider.tag == "interact")
                return hitDowntemp;
        }
        if (hitUptemp)
        {
            if (hitUptemp.collider.tag == "interact")
                return hitUptemp;
        }

        return hitLeft;
    }



    /** Gravity Functions **/
    private void GravCalc()
     {
        if (object_velocity.magnitude < MAX_FALLSPEED)
            object_velocity += (acceleration);
        else
            object_velocity = max_V;

        
        // add a max cap to velocity and deltatime;
     }

    // Switches the way gravity accelerates and the way jumps push
    // also switches collisions so the ceiling of levels are now treated like floors
    // gets falling started by adding new acceleration to velocity
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

}
