using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
/** VARIABLES **/
    // Globals accessed in Unity
    public float JUMPPOWER = 20f;
    public float WALKSPEED = 5f;
    public float GRAVITYMOD = -9.8f;
    public float INITIALVELOCITY = 0;
    public float MAX_FALLSPEED = 1;
    public int MAX_JUMPS = 2;
    public bool GRAVITY_USE;

    public bool Fall;
    public bool PlayerEnabled;

    public static int totalItemsCollected = 0;

    // animation global variables
    public bool Gravity_Inverted;
    public bool isJump;


    // Game Objects
    private Animator Animator;
    private Text num_item_text;

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

    private Vector3 widthVector;
    private Vector3 heightVector;
    
    // Physics layer stuff
    private Vector2 max_V;
    private int physicsLayer = (1 << 0);
    private int interactLayer = (1 << 8);
    private int collectLayer = (1 << 9);
    private int doorLayer = (1 << 10);
    private int hazardLayer = (1 << 11);
    private float hitboxMod = 0.2f;
    private float collision_mod = 0.65f;

    // Hits
    RaycastHit2D hitDown;
    RaycastHit2D hitDown2;
    RaycastHit2D hitDown3;

    RaycastHit2D hitUp;
    RaycastHit2D hitUp2;
    RaycastHit2D hitUp3;

    RaycastHit2D hitLeft;
    RaycastHit2D hitUpLeft;
    RaycastHit2D hitDownLeft;
    RaycastHit2D hitRight;
    RaycastHit2D hitUpRight;
    RaycastHit2D hitDownRight;

    // character properties
    float width;
    float height;
    private SpriteRenderer spriteR;
    private Vector2 PlayerSpawn;

    public bool PlayerCutSceneRight { get; set; }
    public bool PlayerCutSceneLeft { get; set; }


    /** FUNCTIONS **/

    // Start is called before the first frame update
    void Start()
    {
        PlayerSpawn = this.transform.position;
        PlayerEnabled = true;
        JumpVelocity = new Vector3(0, 1) * JUMPPOWER;
        HorizontalMovement = Vector3.zero;
        max_V = new Vector3(0, -MAX_FALLSPEED);

        Fall = true;
        acceleration = new Vector3(0, 1) * GRAVITYMOD;
        object_velocity = Vector3.zero;
        Gravity_Inverted = false;

        Animator = GetComponent<Animator>();
        num_item_text = GameObject.Find("Num_item").GetComponent<Text>();
        num_item_text.text = totalItemsCollected.ToString();

        width = (GetComponent<SpriteRenderer>().bounds.size.x / 2) * collision_mod;
        height = (GetComponent<SpriteRenderer>().bounds.size.y / 2);

        spriteR = gameObject.GetComponent<SpriteRenderer>();

        detectiondistanceY = height;
        detectiondistanceX = width;

        collisionDown = new Vector2(0, -1);
        collisionUp = new Vector2(0, 1);
        collisionLeft = new Vector2(-1, 0);
        collisionRight = new Vector2(1, 0);

        widthVector = new Vector3(width, 0);
        heightVector = new Vector3(0, height);
    }

    // update called as much as possible
    private void Update()
    {
        if (PlayerEnabled)
            HorizontalMovement.x = Input.GetAxis("Horizontal");
        else if (PlayerCutSceneRight)
            HorizontalMovement.x = 1;
        else if (PlayerCutSceneLeft)
            HorizontalMovement.x = -1;
        else
            HorizontalMovement.x = 0;

        if (HorizontalMovement.x > 0)
        {
            spriteR.flipX = true;
        }
        else if (HorizontalMovement.x < 0)
        {
            spriteR.flipX = false;
        }
        else
        {
            spriteR.flipX = spriteR.flipX;
        }

        Animator.SetFloat("Speed", Mathf.Abs(HorizontalMovement.x));

        if (Input.GetKeyDown(KeyCode.Space) && curr_jumps < MAX_JUMPS && PlayerEnabled) // handles the jumping
        {
            isJump = true;
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
            if (hit && hit.transform.tag == "interact")
            {
                Debug.Log(hit.transform.tag);
                InteractWithObject(hit);
            }
        }

        isJump = (object_velocity.y >= 0) ^ Gravity_Inverted;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Move Character
        this.transform.Translate((HorizontalMovement * WALKSPEED) + object_velocity);
        CheckCollisions();
        CheckDamage();
        CheckCollectables();
        CheckDoor();
    }

    // Raycast checkers
    private void CheckCollisions()
    {
        // Detecting Collisions-- We're first casting the sides so we can do comparisons instead of prioritizing one side over another
        hitLeft = Physics2D.Raycast(transform.position, collisionLeft, detectiondistanceX, physicsLayer);
        hitUpLeft = Physics2D.Raycast(transform.position + heightVector, collisionLeft, detectiondistanceX, physicsLayer);
        hitDownLeft = Physics2D.Raycast(transform.position - heightVector, collisionLeft, detectiondistanceX, physicsLayer);
        hitRight = Physics2D.Raycast(transform.position, collisionRight, detectiondistanceX, physicsLayer);
        hitUpRight = Physics2D.Raycast(transform.position + heightVector, collisionRight, detectiondistanceX, physicsLayer);
        hitDownRight = Physics2D.Raycast(transform.position - heightVector, collisionRight, detectiondistanceX, physicsLayer);


        // checking for left walls ---
        if (hitLeft)
        {
            OnWallLeftPhysics(hitLeft);
        }
        else if (hitUpLeft && !hitUpRight && Fall)
        {
            OnWallLeftPhysics(hitUpLeft);
        }
        else if (hitDownLeft && Fall && !hitDownRight)
        {
            OnWallLeftPhysics(hitDownLeft);
        }

        // checking for right walls ---
        if (hitRight)
        {
            OnWallRightPhysics(hitRight);
        }
        else if (!hitUpLeft && hitUpRight && Fall)
        {
            OnWallRightPhysics(hitUpRight);
        }
        else if (hitDownRight && Fall & !hitDownLeft)
        {
            //Debug.Log("DownLeft");
            OnWallRightPhysics(hitDownRight);
        }

        // checking for Ground and Ceiling ---
        hitDown = Physics2D.Raycast(this.transform.position, collisionDown, detectiondistanceY, physicsLayer);
        hitDown2 = Physics2D.Raycast(this.transform.position + widthVector * collision_mod, collisionDown, detectiondistanceY, physicsLayer);
        hitDown3 = Physics2D.Raycast(this.transform.position - widthVector * collision_mod, collisionDown, detectiondistanceY, physicsLayer);

        hitUp = Physics2D.Raycast(transform.position, collisionUp, detectiondistanceY, physicsLayer);
        hitUp2 = Physics2D.Raycast(transform.position + widthVector * collision_mod, collisionUp, detectiondistanceY, physicsLayer);
        hitUp3 = Physics2D.Raycast(transform.position - widthVector * collision_mod, collisionUp, detectiondistanceY, physicsLayer);
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
    public void CheckDamage()
    {
        hitLeft = Physics2D.Raycast(transform.position, collisionLeft, detectiondistanceX * hitboxMod, hazardLayer);
        hitUpLeft = Physics2D.Raycast(transform.position + heightVector * hitboxMod, collisionLeft, detectiondistanceX * hitboxMod, hazardLayer);
        hitDownLeft = Physics2D.Raycast(transform.position - heightVector * hitboxMod, collisionLeft, detectiondistanceX * hitboxMod, hazardLayer);
       
        hitRight = Physics2D.Raycast(transform.position, collisionRight, detectiondistanceX * hitboxMod, hazardLayer);
        hitUpRight = Physics2D.Raycast(transform.position + heightVector * hitboxMod, collisionRight, detectiondistanceX * hitboxMod, hazardLayer);
        hitDownRight = Physics2D.Raycast(transform.position - heightVector * hitboxMod, collisionRight, detectiondistanceX * hitboxMod, hazardLayer);


        if (hitLeft && hitLeft.collider.CompareTag("Hazard"))
        {
            DamagePlayer();
        }
        else if (hitUpLeft && hitUpLeft.collider.CompareTag("Hazard"))
        {
            DamagePlayer();
        }
        else if (hitDownLeft && hitDownLeft.collider.CompareTag("Hazard"))
        {
            DamagePlayer();
        }
        else if (hitRight && hitRight.collider.CompareTag("Hazard"))
        {
            DamagePlayer();
        }
        else if (hitUpRight && hitUpRight.collider.CompareTag("Hazard"))
        {
            DamagePlayer();
        }
        else if (hitDownRight && hitDownRight.collider.CompareTag("Hazard"))
        {
            DamagePlayer();
        }
    }
    private void CheckCollectables()
    {
        hitLeft = Physics2D.Raycast(transform.position, collisionLeft, detectiondistanceX, collectLayer);
        hitRight = Physics2D.Raycast(transform.position, collisionRight, detectiondistanceX, collectLayer);
        RaycastHit2D hitDowntemp = Physics2D.Raycast(transform.position, collisionDown, detectiondistanceY, collectLayer);
        RaycastHit2D hitUptemp = Physics2D.Raycast(transform.position, collisionUp, detectiondistanceY, collectLayer);

        if (hitLeft && hitLeft.collider.CompareTag("collectable"))
        {
            CollectCollectable(hitLeft);
        }
        else if (hitRight && hitRight.collider.CompareTag("collectable"))
        {
            Debug.Log("Collided with Object");
            CollectCollectable(hitRight);
        }
        else if (hitDowntemp && hitDowntemp.collider.CompareTag("collectable"))
        {
            Debug.Log("Collided with Object");
            CollectCollectable(hitDowntemp);
        }
        else if (hitUptemp && hitUptemp.collider.CompareTag("collectable"))
        {
            Debug.Log("Collided with Object");
            CollectCollectable(hitUptemp);
        }
    }
    private void CheckDoor()
    {
        hitLeft = Physics2D.Raycast(transform.position, collisionLeft, detectiondistanceX, doorLayer);
        hitRight = Physics2D.Raycast(transform.position, collisionRight, detectiondistanceX, doorLayer);
        RaycastHit2D hitDowntemp = Physics2D.Raycast(transform.position, collisionDown, detectiondistanceY, doorLayer);
        RaycastHit2D hitUptemp = Physics2D.Raycast(transform.position, collisionUp, detectiondistanceY, doorLayer);

        if (hitLeft && hitLeft.collider.CompareTag("Door"))
        {
            NextLevel(hitLeft);
        }
        else if (hitRight && hitRight.collider.CompareTag("Door"))
        {
            NextLevel(hitRight);
        }
        else if (hitDowntemp && hitDowntemp.collider.CompareTag("Door"))
        {
            NextLevel(hitDowntemp);
        }
        else if (hitUptemp && hitUptemp.collider.CompareTag("Door"))
        {
            NextLevel(hitUptemp);
        }
    }
    private RaycastHit2D InteractionChecker()
    {
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, collisionLeft, detectiondistanceX, interactLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, collisionRight, detectiondistanceX, interactLayer);
        RaycastHit2D hitDowntemp = Physics2D.Raycast(transform.position, collisionDown, detectiondistanceY, interactLayer);
        RaycastHit2D hitUptemp = Physics2D.Raycast(transform.position, collisionUp, detectiondistanceY, interactLayer);

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



    /** Character Functions for movement and Enabling **/
    private void InteractWithObject(RaycastHit2D hit)
    {
        hit.collider.SendMessageUpwards("interact", SendMessageOptions.DontRequireReceiver);
        PausePlayer();
    }
    private void CollectCollectable(RaycastHit2D hit)
    {
        hit.collider.SendMessageUpwards("destroy", SendMessageOptions.DontRequireReceiver);
        totalItemsCollected++;
        num_item_text.text = totalItemsCollected.ToString();
    }
    private void NextLevel(RaycastHit2D hit)
    {
        hit.collider.SendMessageUpwards("NextLevel", totalItemsCollected,SendMessageOptions.DontRequireReceiver);
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
    private void ResetPlayer()
    {
        this.transform.position = PlayerSpawn;
        object_velocity = Vector2.zero;
        HorizontalMovement = Vector2.zero;
        curr_jumps = 0;
        spriteR.flipY = false;
        GRAVITY_USE = false;

        Start();
    }

    public void DamagePlayer()
    {
        ResetPlayer();
        // play sound if need be here:
    }



    /** Physics for platforming **/
    private void OnWallLeftPhysics(RaycastHit2D hit)
    {
        if (hit.collider.CompareTag("Ground"))
        {
            transform.position = new Vector3(hit.point.x + detectiondistanceX, transform.position.y);
        }
    }
    private void OnWallRightPhysics(RaycastHit2D hit)
    {
        if (hit.collider.tag == "Ground")
        {
            transform.position = new Vector3(hit.point.x - detectiondistanceX, transform.position.y);
        }
    }
    private void OnCeilingPhysics(RaycastHit2D hit)
    {
        if (hit.collider.CompareTag("Ground"))
        {
            object_velocity = Vector2.zero;
            transform.position = new Vector3(transform.position.x, hit.point.y - height);
        }
    }
    private void OnGroundPhysics(RaycastHit2D hit)
    {
        if (hit.collider.CompareTag("Ground"))
        {
            Fall = false;
            object_velocity = Vector2.zero;
            curr_jumps = 0;
            transform.position = new Vector3(transform.position.x, hit.point.y + height);
            GRAVITY_USE = true;
            isJump = false;
        }
    }
 
    /** Gravity Functions **/
    private void GravCalc()
     {
        if (object_velocity.magnitude < MAX_FALLSPEED)
            object_velocity += (acceleration);
        else // Velocity Cap
            object_velocity = max_V;

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
        spriteR.flipY = !spriteR.flipY;
        Gravity_Inverted = !Gravity_Inverted;
    }

}
