using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ELC_PlayerMoves : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private Vector3 playerMoves;

    public Transform player;

    private Vector3 lastDirection;

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

    private bool isTouchingLeft;
    private bool isTouchingRight;
    private bool isTouchingTop;
    private bool isTouchingDown;

    public bool canMove;
    [SerializeField]
    private bool playerIsImmobile;


    [Header("Dash Characteristics")]

    public float dashDistance;
    public float dashTime;
    public bool isDashing;
    public bool isGashDashing;
    public bool isThrustDashing;
    private float nextDash;
    public float dashCooldown;
    public float stopDash;

    private void Start()
    {
        playerAnimator = this.GetComponent<Animator>();
        playerSpriteRenderer = this.GetComponent<SpriteRenderer>();
    }

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

        AnimationsManagement();

        Raycasts();
    }

    void Moves()
    {
        playerMoves.x = Input.GetAxis("Horizontal") * speed;
        playerMoves.y = Input.GetAxis("Vertical") * speed;
        playerMoves = Vector3.ClampMagnitude(playerMoves, speed);

        //Déterminer dans quelle direction le joueur est
        //if (Input.GetAxis("Vertical") > Input.GetAxis("Horizontal") && Input.GetAxis("Vertical") > -Input.GetAxis("Horizontal")) PlayerSide = Sides.Back;
        //else if (Input.GetAxis("Vertical") < Input.GetAxis("Horizontal") && Input.GetAxis("Vertical") < -Input.GetAxis("Horizontal")) PlayerSide = Sides.Front;
        //else if (Input.GetAxis("Horizontal") > Input.GetAxis("Vertical") && Input.GetAxis("Horizontal") > -Input.GetAxis("Vertical")) PlayerSide = Sides.Right;
        //else if (Input.GetAxis("Horizontal") < Input.GetAxis("Vertical") && Input.GetAxis("Horizontal") < -Input.GetAxis("Vertical")) PlayerSide = Sides.Left;

        float playerDirectionAngle = Vector3.Angle(Vector3.right, playerMoves);

        if(playerMoves.y >= 0 && playerIsImmobile == false && playerMoves.sqrMagnitude > 0.05f)
        {
            if (playerDirectionAngle <= 22.5f) PlayerSide = Sides.Right;
            else if (playerDirectionAngle <= 67.5f) PlayerSide = Sides.RightBack;
            else if (playerDirectionAngle <= 112.5f) PlayerSide = Sides.Back;
            else if (playerDirectionAngle <= 157.5f) PlayerSide = Sides.LeftBack;
            else if (playerDirectionAngle <= 180f) PlayerSide = Sides.Left;
        }
        else if(playerMoves.y < 0 && playerIsImmobile == false && playerMoves.sqrMagnitude > 0.05f)
        {
            if (playerDirectionAngle <= 22.5f) PlayerSide = Sides.Right;
            else if (playerDirectionAngle <= 67.5f) PlayerSide = Sides.RightFront;
            else if (playerDirectionAngle <= 112.5f) PlayerSide = Sides.Front;
            else if (playerDirectionAngle <= 157.5f) PlayerSide = Sides.LeftFront;
            else if (playerDirectionAngle <= 180f) PlayerSide = Sides.Left;
        }

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

    void AnimationsManagement()
    {
        if (playerMoves.sqrMagnitude < 0.005f) playerIsImmobile = true;
        else playerIsImmobile = false;
        playerAnimator.SetBool("IsImmobile", playerIsImmobile);

        //Le numéro 1 de PlayerSide correspond aux anim de Front, le 2 aux anims de SideFront, le 3 aux anims de Back, le 4 aux anims de Sideback et le 5 aux anims de Sides
        if (PlayerSide == Sides.Front) playerAnimator.SetInteger("PlayerSide", 1);
        else if (PlayerSide == Sides.RightFront || PlayerSide == Sides.LeftFront) playerAnimator.SetInteger("PlayerSide", 2);
        else if (PlayerSide == Sides.Back) playerAnimator.SetInteger("PlayerSide", 3);
        else if (PlayerSide == Sides.RightBack || PlayerSide == Sides.LeftBack) playerAnimator.SetInteger("PlayerSide", 4);
        else playerAnimator.SetInteger("PlayerSide", 5);

        //Vu qu'il y a qu'une anim de côté droit, il faut flip le sprite pour qu'elle fasse aussi anim du côté gauche
        if (PlayerSide == Sides.Left || PlayerSide == Sides.LeftBack || PlayerSide == Sides.LeftFront)
        {
            if(playerSpriteRenderer.flipX == false)playerSpriteRenderer.flipX = true;
        }
        else
        {
            if (playerSpriteRenderer.flipX == true) playerSpriteRenderer.flipX = false;
        }
    }

    public IEnumerator PlayAnimation(string name, float duration)
    {
        playerAnimator.SetBool(name, true);
        yield return new WaitForSeconds(duration);
        playerAnimator.SetBool(name, false);
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
