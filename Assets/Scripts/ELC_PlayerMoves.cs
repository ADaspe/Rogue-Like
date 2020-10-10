﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ELC_PlayerMoves : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private Vector3 playerMoves;

    public Transform player;

    public readonly Vector3 lastDirection;

    public AXD_PlayerAttack playerAttack;

    private Animator playerAnimator;
    private SpriteRenderer playerSpriteRenderer;
    
    [SerializeField]
    private LayerMask bodyHitMask;

    //Sliders pour régler les raycasts
    [Range(0f, 1.5f)][SerializeField]
    private float raycastLenght;
    [Range(0f, 1.5f)][SerializeField]
    private float raycastWidth;

    private enum Sides { Front, RightFront, LeftFront, Back, RightBack, LeftBack, Left, Right};
    [SerializeField]
    private Sides PlayerSide;

    [SerializeField] private bool isTouchingLeft;
    [SerializeField] private bool isTouchingRight;
    [SerializeField] private bool isTouchingTop;
    [SerializeField] private bool isTouchingDown;

    private bool canTurn = true;

    public bool canMove;
    public bool playerIsImmobile;


    [Header("Dash Characteristics")]

    public float dashDistance;
    public float dashTime;
    public bool isDashing;
    //public bool isGashDashing;
    //public bool isThrustDashing;
    private float nextDash;
    public float dashCooldown;
    public float stopDash;
    private bool isDashingInWall;

    private Vector3 dashVector;

    private void Start()
    {
        playerAnimator = this.GetComponent<Animator>();
        playerSpriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        

        if ((Input.GetKeyDown(KeyCode.A) && !isDashing) || isDashing) Dash(dashDistance, dashTime);

        if (canMove) Walk();

        PlayerTurnDetector();
        AnimationsManagement();
    }

    void Walk()
    {
        //détecte les inputs
        playerMoves.x = Input.GetAxis("Horizontal") * speed;
        playerMoves.y = Input.GetAxis("Vertical") * speed;

        //traite la vitesse
        playerMoves = Vector3.ClampMagnitude(playerMoves, speed);

        //Empêche le joueur de traverser les murs
        MovementClampIfCollidingWalls(speed, "playerMoves");

        IsPlayerImmobile();

        transform.Translate(playerMoves * Time.deltaTime);
    }

    void MovementClampIfCollidingWalls(float speed, string vectorToClamp)
    {
        Raycasts();
        
        if(vectorToClamp == "playerMoves")
        { 
            if (isTouchingLeft) playerMoves.x = Mathf.Clamp(playerMoves.x, 0, speed/2);
            if (isTouchingRight) playerMoves.x = Mathf.Clamp(playerMoves.x, -speed/2, 0);
            if (isTouchingDown) playerMoves.y = Mathf.Clamp(playerMoves.y, 0, speed/2);
            if (isTouchingTop) playerMoves.y = Mathf.Clamp(playerMoves.y, -speed/2, 0);
        }
        else if (vectorToClamp == "dashVector")
        {
            if (isTouchingLeft) dashVector.x = Mathf.Clamp(dashVector.x, 0, speed/2);
            if (isTouchingRight) dashVector.x = Mathf.Clamp(dashVector.x, -speed/2, 0);
            if (isTouchingDown) dashVector.y = Mathf.Clamp(dashVector.y, 0, speed/2);
            if (isTouchingTop) dashVector.y = Mathf.Clamp(dashVector.y, -speed/2, 0);
        }
    }

    void PlayerTurnDetector()
    {
        float playerDirectionAngle = Vector3.Angle(Vector3.right, lastDirection);

        if (playerMoves.y >= 0 && playerIsImmobile == false && playerMoves.sqrMagnitude > 0.05f)
        {
            if (playerDirectionAngle <= 22.5f) PlayerSide = Sides.Right;
            else if (playerDirectionAngle <= 67.5f) PlayerSide = Sides.RightBack;
            else if (playerDirectionAngle <= 112.5f) PlayerSide = Sides.Back;
            else if (playerDirectionAngle <= 157.5f) PlayerSide = Sides.LeftBack;
            else if (playerDirectionAngle <= 180f) PlayerSide = Sides.Left;
        }
        else if (playerMoves.y < 0 && playerIsImmobile == false && playerMoves.sqrMagnitude > 0.05f)
        {
            if (playerDirectionAngle <= 22.5f) PlayerSide = Sides.Right;
            else if (playerDirectionAngle <= 67.5f) PlayerSide = Sides.RightFront;
            else if (playerDirectionAngle <= 112.5f) PlayerSide = Sides.Front;
            else if (playerDirectionAngle <= 157.5f) PlayerSide = Sides.LeftFront;
            else if (playerDirectionAngle <= 180f) PlayerSide = Sides.Left;
        }
    }

    void Raycasts()
    {
        //Display direction raycast
        Debug.DrawRay(this.transform.position, transform.TransformDirection(Vector3.ClampMagnitude(playerMoves, 1)), Color.green);

        //DashVector raycast
        RaycastHit2D dashRaycast = Physics2D.Raycast(transform.position, transform.TransformDirection(dashVector), raycastLenght, bodyHitMask);
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.ClampMagnitude(dashVector * 100, raycastLenght)), Color.green);

        if (dashRaycast) isDashingInWall = true;
        else isDashingInWall = false;

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

    void IsPlayerImmobile()
    {
        if (playerMoves.sqrMagnitude < 0.005f) playerIsImmobile = true;
        else
        {
            playerIsImmobile = false;
            lastDirection = playerMoves; //On enregistre ici la dernière direction du joueur
        }
    }

    void AnimationsManagement()
    {

        playerAnimator.SetBool("IsImmobile", playerIsImmobile);

        if (canTurn)
        {
            //Le numéro 1 de PlayerSide correspond aux anim de Front, le 2 aux anims de SideFront, le 3 aux anims de Back, le 4 aux anims de Sideback et le 5 aux anims de Sides
            if (PlayerSide == Sides.Front) playerAnimator.SetInteger("PlayerSide", 1);
            else if (PlayerSide == Sides.RightFront || PlayerSide == Sides.LeftFront) playerAnimator.SetInteger("PlayerSide", 2);
            else if (PlayerSide == Sides.Back) playerAnimator.SetInteger("PlayerSide", 3);
            else if (PlayerSide == Sides.RightBack || PlayerSide == Sides.LeftBack) playerAnimator.SetInteger("PlayerSide", 4);
            else playerAnimator.SetInteger("PlayerSide", 5);

            //Vu qu'il y a qu'une anim de côté droit, il faut flip le sprite pour qu'elle fasse aussi anim du côté gauche
            if (PlayerSide == Sides.Left || PlayerSide == Sides.LeftBack || PlayerSide == Sides.LeftFront)
            {
                if (playerSpriteRenderer.flipX == false) playerSpriteRenderer.flipX = true;
            }
            else
            {
                if (playerSpriteRenderer.flipX == true) playerSpriteRenderer.flipX = false;
            }
        }
    }

    public IEnumerator PlayAnimation(string name, float duration, bool canMoveDuringIt, bool canTurnDuringIt)
    {
        playerAnimator.SetBool(name, true);
        canMove = canMoveDuringIt;
        canTurn = canTurnDuringIt;
        yield return new WaitForSeconds(duration);
        playerAnimator.SetBool(name, false);
        canMove = !canMoveDuringIt;
        canTurn = !canTurnDuringIt;
    }

    public Vector3 getPlayerMoves()
    {
        //return playerMoves;
        return lastDirection;
    }
    public void Dash(float distance, float time)
    {
        //On règle la durée du dash ici, cette valeur sera enclenchée qu'une fois par appel de la fonction
        if (!isDashing)
        {
            Debug.Log("Start dash");
            stopDash = Time.time + time;
            isDashing = true;
            canMove = false;
        }

        //Calcul du vecteur du dash
        dashVector = lastDirection.normalized * (distance / time) * Time.deltaTime;

        //On détecte si y'a un mur
        Raycasts();
        PlayerTurnDetector();
        MovementClampIfCollidingWalls(distance/time, "dashVector");
        MovementClampIfCollidingWalls(speed, "playerMoves");

        //Conditions d'arrêt du dash
        if (Time.time > stopDash || isDashingInWall)
        {
            isDashing = false;
            canMove = true;
        }
        else if(isDashing) player.Translate(dashVector); //Ici on bouge si tout va bien
    }

    //public void AttackDash(float distance, float time)
    //{
    //    player.Translate(lastDirection.normalized * (distance / time) * Time.deltaTime);
    //    Debug.Log("Dash Attack CD : " + (stopDash - Time.time));
    //    if (Time.time > stopDash)
    //    {
    //        isGashDashing = false;
    //        isThrustDashing = false;
    //        canMove = true;
    //    }
    //}
}
