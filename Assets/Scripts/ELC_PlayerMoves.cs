using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ELC_PlayerMoves : MonoBehaviour
{

    private Vector3 playerMoves;
    public Transform player;
    public Vector3 lastDirection = new Vector3(0, -1);
    public Vector3 attackPoint;
    public const float animationTime = 0.4f;
    public ELC_PlayerStatManager playerStats;
    public PlayerHealth playerHealth;
    [SerializeField]
    private Animation sponkAnimation;
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
    public float nextSwichAttackTime;
    public float nextSponkAttackTime;

    //Anti spam variables
    private bool dashButtonDown;
    private bool swichButtonDown;
    private bool sponkButtonDown;

    [Header("Dash Characteristics")]
    
    public bool isDashing;
    private float nextDash;
    public float dashCooldown;
    public float stopDash;
    private bool isDashingInWall;
    public float currentDistance;
    public float currentTime;

    private Vector3 dashVector;

    private void Start()
    {
        playerAnimator = this.GetComponent<Animator>();
        playerSpriteRenderer = this.GetComponent<SpriteRenderer>();

        lastDirection = new Vector3(0, -1);
        playerAnimator.SetFloat("DirectionAxeX", 0);
        playerAnimator.SetFloat("DirectionAxeY", -1);
    }

    void Update()
    {
        attackPoint = transform.position + lastDirection.normalized*playerStats.SwichAreaRadius;
        if (Input.GetAxisRaw("Dash") != 1)
        {
            dashButtonDown = false;
        }
        if (Input.GetAxisRaw("Swich") != 1)
        {
            swichButtonDown = false;
        }
        if (Input.GetAxisRaw("Sponk") != 1)
        {
            sponkButtonDown = false;
        }

        if ((Input.GetAxisRaw("Dash") == 1 && !isDashing && !dashButtonDown) || isDashing)
        {
            Dash(playerStats.DashDistance, playerStats.DashTime); // Utilise l'input manager bordel à couille O'Clavier 
            dashButtonDown = true;
        }
        if(Input.GetAxisRaw("Swich") == 1 && Time.time > nextSwichAttackTime && !swichButtonDown)
        {
            swichButtonDown = true;
            Dash(playerStats.SwichDashDistance, playerStats.SwichDashTime);
            StartCoroutine(PlayAnimation("SwishAttack", playerStats.AnimationSwichTime, false, false));
            nextSwichAttackTime = Time.time + 1f / playerStats.SwichAttackRate;

        }else if(Input.GetAxisRaw("Sponk") == 1 && Time.time > nextSponkAttackTime && !sponkButtonDown)
        {
            sponkButtonDown = true;
            StartCoroutine("SponkAttackAnimation");
            
        }
        
        if(Input.GetAxisRaw("Heal") != 0)
        {
            playerHealth.Heal(playerStats.healingRate * Input.GetAxisRaw("Heal"));
        }

        if (canMove) Walk();

        PlayerTurnDetector();
        AnimationsManagement();
    }

    void Walk()
    {
        //détecte les inputs
        playerMoves.x = Input.GetAxis("Horizontal") * playerStats.Speed;
        playerMoves.y = Input.GetAxis("Vertical") * playerStats.Speed;

        //traite la vitesse
        playerMoves = Vector3.ClampMagnitude(playerMoves, playerStats.Speed);

        //Empêche le joueur de traverser les murs
        MovementClampIfCollidingWalls(playerStats.Speed, "playerMoves");

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
        if (playerMoves.sqrMagnitude < 0.01f) playerIsImmobile = true; // mettre une variable plutôt qu'un chiffre en dur
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
            playerAnimator.SetFloat("DirectionAxeX", Mathf.Clamp(lastDirection.x, -1, 1));
            playerAnimator.SetFloat("DirectionAxeY", Mathf.Clamp(lastDirection.y, -1, 1));
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

    public IEnumerator PlayAnimation(string name, float time, bool canMoveDuringIt, bool canTurnDuringIt)
    {
        
        playerAnimator.SetBool(name, true);
        canMove = canMoveDuringIt;
        canTurn = canTurnDuringIt;
        if (name.Equals("SwishAttack") || name.Equals("SponkAttack"))
        {
            yield return new WaitForSeconds(time /playerAnimator.GetFloat("AnimationSpeedMultiplier")); // Sert à arrêter l'animation au bon moment, peu importe sa vitesse

        }else
        {
            yield return new WaitForSeconds(time);
        }
        
        playerAnimator.SetBool(name, false);
        canMove = true;
        canTurn = true;
    }
    public void StopAnimation(string name)
    {
        playerAnimator.SetBool(name, false);
        canMove = true;
        canTurn = true;
    }

    public void Dash(float distance, float time)
    {
        //On règle la durée du dash ici, cette valeur sera enclenchée qu'une fois par appel de la fonction
        if (!isDashing)
        {
            StartCoroutine(PlayAnimation("isDashing", playerStats.AnimationDashTime, false, false));
            playerStats.invulnerability = true;
            currentDistance = distance;
            currentTime = time;
            stopDash = Time.time + time;
            isDashing = true;
            canMove = false;
        }

        //Calcul du vecteur du dash
        dashVector = lastDirection.normalized * (currentDistance / currentTime) * Time.deltaTime;

        //On détecte si y'a un mur
        Raycasts();
        PlayerTurnDetector();
        MovementClampIfCollidingWalls(distance / time, "dashVector");
        MovementClampIfCollidingWalls(playerStats.Speed, "playerMoves");

        //Conditions d'arrêt du dash
        if (Time.time > stopDash || isDashingInWall)
        {
            StopAnimation("isDashing");
            isDashing = false;
            canMove = true;
            playerStats.invulnerability = false;
        }
        else if (isDashing) player.Translate(dashVector); //Ici on bouge si tout va bien
    }


    IEnumerator SponkAttackAnimation()
    {

        StartCoroutine(PlayAnimation("SponkAttack", playerStats.AnimationSponkTime, false, false));
        nextSponkAttackTime = Time.time + 1f / playerStats.SponkAttackRate;
        yield return new WaitForSeconds(playerAnimator.GetCurrentAnimatorStateInfo(0).length * 1 / 4);
        Dash(playerStats.ThrustDashDistance, playerStats.ThrustDashTime);

    }
    private void OnDrawGizmosSelected()
    {

        if (attackPoint != null)
        {
            //Gizmos.DrawWireCube(attackPoint, new Vector3(thrustWidth, thrustlength, 0));
            Gizmos.DrawWireSphere(attackPoint, playerStats.SwichAreaRadius);
        }

    }
}
