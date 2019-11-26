using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Behavior : MonoBehaviour
{
    GameObject E_Object;

    // Got to get properties of the sprite
    private float width;
    private float height;
    private SpriteRenderer spriteR;

    private float detectiondistanceY;
    private float detectiondistanceX;

    // Collision vectors
    private Vector2 collisionDown;
    private Vector2 collisionUp;
    private Vector2 collisionLeft;
    private Vector2 collisionRight;

    private int PlayerLayer = (1 << 12);

    public bool InDialogue;
    public bool Talking;

    // Start is called before the first frame update
    void Start()
    {
        InDialogue = false;
        E_Object = this.transform.GetChild(0).gameObject;
        E_Object.SetActive(false);

        width = (GetComponent<BoxCollider2D>().bounds.size.x);
        height = (GetComponent<BoxCollider2D>().bounds.size.y);

        spriteR = gameObject.GetComponent<SpriteRenderer>();

        detectiondistanceY = height;
        detectiondistanceX = width;

        collisionDown = new Vector2(0, -1);
        collisionUp = new Vector2(0, 1);
        collisionLeft = new Vector2(-1, 0);
        collisionRight = new Vector2(1, 0);
    }

    private void FixedUpdate()
    {
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, collisionLeft, detectiondistanceX, PlayerLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, collisionRight, detectiondistanceX, PlayerLayer);
        RaycastHit2D hitDown = Physics2D.Raycast(transform.position, collisionDown, detectiondistanceY, PlayerLayer);
        RaycastHit2D hitUp = Physics2D.Raycast(transform.position, collisionUp, detectiondistanceY, PlayerLayer);

        if (hitLeft && !InDialogue)
        {
            if (hitLeft.collider.tag == "Player")
                ShowE();
        }
        else if (hitRight && !InDialogue)
        {
            if (hitRight.collider.tag == "Player")
                ShowE();
        }
        else if (hitDown && !InDialogue)
        {
            if (hitDown.collider.tag == "Player")
                ShowE();
        }
        else if (hitUp && !InDialogue)
        {
            if (hitUp.collider.tag == "Player")
                ShowE();
        }
        else
        {
            HideE();
        }
    }

    private void ShowE()
    {
        E_Object.SetActive(true);
    }

    private void HideE()
    {
        E_Object.SetActive(false);
    }
}
