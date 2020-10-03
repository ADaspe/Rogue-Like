﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ELC_PlayerMoves : MonoBehaviour
{
    [SerializeField]
    private float speed;

    public bool canMove;

    private Vector3 playerMoves;

    public Transform player;

    private Vector3 lastDirection;

    public AXD_PlayerAttack playerAttack;

    
    [SerializeField]
    private LayerMask bodyHitMask;

    //Sliders pour régler les raycasts
    [Range(0f, 1.5f)][SerializeField]
    private float raycastLenght;
    [Range(0f, 1.5f)][SerializeField]
    private float raycastWidth;

    private bool isTouchingLeft;
    private bool isTouchingRight;
    private bool isTouchingTop;
    private bool isTouchingDown;

    [Header("Dash Characteristics")]

    public float dashDistance;
    public float dashTime;
    public bool isDashing;
    public bool isGashDashing;
    public bool isThrustDashing;
    private float nextDash;
    public float dashCooldown;
    public float stopDash;

    void Update()
    {
        if (canMove)
        {
            Moves();
        }
        
        if(Input.GetAxisRaw("Dash") == 1 && Time.time > nextDash && canMove)
        {
            canMove = false;
            isDashing = true;
            nextDash = Time.time + dashCooldown;
            stopDash = Time.time + dashTime;
        }
        if (isGashDashing)
        {
            AttackDash(playerAttack.gashDashDistance, playerAttack.gashDashTime);

        }else if (isThrustDashing)
        {
            AttackDash(playerAttack.thrustDashDistance, playerAttack.thrustDashTime);
        }
        if (isDashing)
        {
            Dash();
        }

        Raycasts();
    }

    void Moves()
    {
        playerMoves.x = Input.GetAxis("Horizontal") * speed;
        playerMoves.y = Input.GetAxis("Vertical") * speed;
        playerMoves = Vector3.ClampMagnitude(playerMoves, speed);

        if (playerMoves != Vector3.zero)
        {
            lastDirection = playerMoves;
        }

        IsPlayerCollidingWalls();

        transform.Translate(playerMoves * Time.deltaTime);
    }

    void IsPlayerCollidingWalls()
    {
        if (isTouchingLeft) playerMoves.x = Mathf.Clamp(playerMoves.x, 0, speed);
        if (isTouchingRight) playerMoves.x =  Mathf.Clamp(playerMoves.x, -speed, 0);
        if (isTouchingDown) playerMoves.y =  Mathf.Clamp(playerMoves.y, 0, speed);
        if (isTouchingTop) playerMoves.y =  Mathf.Clamp(playerMoves.y, -speed, 0);
    }

    void Raycasts()
    {
        //Display direction raycast
        Debug.DrawRay(this.transform.position, transform.TransformDirection(Vector3.ClampMagnitude(playerMoves, 1)), Color.green);

        //Right raycast
        Vector3 rightRaycastStart = new Vector3(this.transform.position.x + 0.5f * raycastWidth, this.transform.position.y + 0.5f * raycastLenght);
        RaycastHit2D rightHit = Physics2D.Raycast(rightRaycastStart, transform.TransformDirection(Vector2.down), raycastLenght, bodyHitMask);
        Debug.DrawRay(rightRaycastStart, transform.TransformDirection(Vector2.down) * raycastLenght, Color.red);
        if(rightHit) isTouchingRight = true;
        else isTouchingRight = false;

        //Left raycast
        Vector3 leftRaycastStart = new Vector3(this.transform.position.x - 0.5f * raycastWidth, this.transform.position.y - 0.5f * raycastLenght);
        RaycastHit2D leftHit = Physics2D.Raycast(leftRaycastStart, transform.TransformDirection(Vector2.up), raycastLenght, bodyHitMask);
        Debug.DrawRay(leftRaycastStart, transform.TransformDirection(Vector2.up) * raycastLenght, Color.red);
        if (leftHit) isTouchingLeft = true;
        else isTouchingLeft = false;

        //Top raycast
        Vector3 topRaycastStart = new Vector3(this.transform.position.x - 0.5f * raycastLenght, this.transform.position.y + 0.5f * raycastWidth);
        RaycastHit2D topHit = Physics2D.Raycast(topRaycastStart, transform.TransformDirection(Vector2.right), raycastLenght, bodyHitMask);
        Debug.DrawRay(topRaycastStart, transform.TransformDirection(Vector2.right) * raycastLenght, Color.red);
        if (topHit) isTouchingTop = true;
        else isTouchingTop = false;

        //Down raycast
        Vector3 downRaycastStart = new Vector3(this.transform.position.x + 0.5f * raycastLenght, transform.position.y - 0.5f * raycastWidth);
        RaycastHit2D downHit = Physics2D.Raycast(downRaycastStart, transform.TransformDirection(Vector2.left), raycastLenght, bodyHitMask);
        Debug.DrawRay(downRaycastStart, transform.TransformDirection(Vector2.left) * raycastLenght, Color.red);
        if (downHit) isTouchingDown = true;
        else isTouchingDown = false;
    }

    public Vector3 getPlayerMoves()
    {
        return playerMoves;
    }
    public void Dash()
    {
        player.Translate(lastDirection.normalized * (dashDistance / dashTime) * Time.deltaTime);

        if(Time.time > stopDash)
        {
            isDashing = false;
            canMove = true;
        }
    }

    public void AttackDash(float distance, float time)
    {
        player.Translate(lastDirection.normalized * (distance / time) * Time.deltaTime);
        Debug.Log("Dash Attack CD : " + (stopDash - Time.time));
        if (Time.time > stopDash)
        {
            isGashDashing = false;
            isThrustDashing = false;
            canMove = true;
        }
    }
}
