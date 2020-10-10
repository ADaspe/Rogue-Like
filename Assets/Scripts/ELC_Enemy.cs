using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ELC_Enemy : MonoBehaviour
{
    [SerializeField]
    private ELC_EnemySO enemyStats;

    
    private Transform playerTransform;

    private float actualLives;
    private float speed;
    private bool canMove = true;
    private bool isDashing;
    private float stopDashing;
    private float attackCooldown; // le cooldown entre chaque attaque
    private Vector3 movesTowardPlayer;
    private Vector3 fleePlayer;
    private Vector3 directionToDash;

 
    private bool isTouchingRight;
    private bool isTouchingLeft;
    private bool isTouchingTop;
    private bool isTouchingDown;
    private bool isTouchingSomething;
    [SerializeField]
    private bool canSeePlayer;

    [SerializeField]
    private LayerMask obstaclesLayerMask;
    [SerializeField]
    private LayerMask fieldOfViewLayerMask;



    [Header ("StayAtDistanceFromPlayer")]
    private float distanceToStay;
    private float marginForDistanceToStay = 0.02f; //La marge dans laquelle peut être l'ennemi avant de s'approcher ou de reculer
    private enum EnemyDistance { TooFar, AtDistance, TooClose };
    private EnemyDistance distanceFromPlayer;

    void Start()
    {
        actualLives = enemyStats.MaxHealth;
        speed = enemyStats.MovementSpeed;
        distanceToStay = enemyStats.LimitDistanceToStay;
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }


    void Update()
    {
        VerifyIfIsAtDistance();
        EnemyAttackCheck();

        if(canMove && !isDashing) EnemyMoves(enemyStats.EnemyPath.ToString());
        if (isDashing) Dash(directionToDash);
    }

    void EnemyMoves(string EnemyPathBehaviour)
    {
        if(EnemyPathBehaviour == "FollowPlayer" || EnemyPathBehaviour == "StayAtDistance") //Vérifie si l'ennemi a bien le caractère FollowPlayer
        {
            CalculateVectorTowardPlayer();
            //Raycasts();
            if (distanceFromPlayer == EnemyDistance.TooFar)
            {
                transform.Translate(movesTowardPlayer);
            }
            else if(distanceFromPlayer == EnemyDistance.TooClose)
            {
                transform.Translate(fleePlayer);
            }
        }
        else if (EnemyPathBehaviour == "FleePlayer")
        {
            CalculateVectorTowardPlayer();
            if (distanceFromPlayer == EnemyDistance.TooClose)
            {
                transform.Translate(fleePlayer);
            }
        }
    }

    void EnemyAttackCheck()
    {
        if((distanceFromPlayer == EnemyDistance.AtDistance || distanceFromPlayer == EnemyDistance.TooClose) && Time.time >= attackCooldown && canSeePlayer)
        {
            canMove = false;
            attackCooldown = Time.time + enemyStats.AttackCooldown;
            Debug.Log(enemyStats.Name + " charge son attaque");
            StartCoroutine("Attack");

            if(enemyStats.DashOnPlayer) directionToDash = movesTowardPlayer;
        }
    }

    Vector3 ClampIfTouchSomething(Vector3 vectorToClamp, float speed)
    {
        Raycasts();
        speed = speed * Time.deltaTime;

        if (isTouchingLeft)
        {
            vectorToClamp.x = Mathf.Clamp(vectorToClamp.x, 0, speed / 2);
            //if(!isTouchingDown && !isTouchingTop) vectorToClamp.x = vectorToClamp.normalized.y * speed;
        }


        if (isTouchingRight)
        {
            vectorToClamp.x = Mathf.Clamp(vectorToClamp.x, -speed / 2, 0);
            //if (!isTouchingDown && !isTouchingTop) vectorToClamp.x = vectorToClamp.normalized.y * speed;
        }

        if (isTouchingDown)
        {
            vectorToClamp.y = Mathf.Clamp(vectorToClamp.y, 0, speed / 2);
            //if(!isTouchingRight && !isTouchingLeft)vectorToClamp.x = vectorToClamp.normalized.x * speed;
        }

        if (isTouchingTop)
        {
            vectorToClamp.y = Mathf.Clamp(vectorToClamp.y, -speed / 2, 0);
            //if (!isTouchingRight && !isTouchingLeft) vectorToClamp.x =  vectorToClamp.normalized.x * speed;
        }

        Vector3 resultVector = new Vector3(vectorToClamp.x, vectorToClamp.y);
        return resultVector;
    }

    void Raycasts() //En cours
    {

        float raycastWidth = 1f;
        float raycastLenght = 0.8f;
        float moveRaycastLenght = 2;

        //Right
        Vector3 rightRaycastStart = new Vector3(this.transform.position.x + 0.5f * raycastWidth, this.transform.position.y + 0.5f * raycastLenght);
        RaycastHit2D rightHit = Physics2D.Raycast(rightRaycastStart, transform.TransformDirection(Vector2.down), raycastLenght, obstaclesLayerMask);
        Debug.DrawRay(rightRaycastStart, transform.TransformDirection(Vector2.down) * raycastLenght, Color.red);
        if (rightHit) isTouchingRight = true;
        else isTouchingRight = false;

        //Left
        Vector3 leftRaycastStart = new Vector3(this.transform.position.x - 0.5f * raycastWidth, this.transform.position.y - 0.5f * raycastLenght);
        RaycastHit2D leftHit = Physics2D.Raycast(leftRaycastStart, transform.TransformDirection(Vector2.up), raycastLenght, obstaclesLayerMask);
        Debug.DrawRay(leftRaycastStart, transform.TransformDirection(Vector2.up) * raycastLenght, Color.red);
        if (leftHit) isTouchingLeft = true;
        else isTouchingLeft = false;

        //Top
        Vector3 topRaycastStart = new Vector3(this.transform.position.x - 0.5f * raycastLenght, this.transform.position.y + 0.5f * raycastWidth);
        RaycastHit2D topHit = Physics2D.Raycast(topRaycastStart, transform.TransformDirection(Vector2.right), raycastLenght, obstaclesLayerMask);
        Debug.DrawRay(topRaycastStart, transform.TransformDirection(Vector2.right) * raycastLenght, Color.red);
        if (topHit) isTouchingTop = true;
        else isTouchingTop = false;


        //Down
        Vector3 downRaycastStart = new Vector3(this.transform.position.x + 0.5f * raycastLenght, transform.position.y - 0.5f * raycastWidth);
        RaycastHit2D downHit = Physics2D.Raycast(downRaycastStart, transform.TransformDirection(Vector2.left), raycastLenght, obstaclesLayerMask);
        Debug.DrawRay(downRaycastStart, transform.TransformDirection(Vector2.left) * raycastLenght, Color.red);
        if (downHit) isTouchingDown = true;
        else isTouchingDown = false;

        //DirectionVector
        RaycastHit2D directionRaycastHit = Physics2D.Raycast(this.transform.position, transform.TransformDirection(movesTowardPlayer.normalized * enemyStats.LimitDistanceToStay), moveRaycastLenght, fieldOfViewLayerMask);
        Debug.DrawRay(this.transform.position, transform.TransformDirection(movesTowardPlayer.normalized * enemyStats.LimitDistanceToStay), Color.blue);
        if(directionRaycastHit && directionRaycastHit.transform.gameObject.CompareTag("Player") == true) canSeePlayer = true;
        else canSeePlayer = false;

        if (isTouchingDown || isTouchingLeft || isTouchingRight || isTouchingTop) isTouchingSomething = true;
    }

    private void CalculateVectorTowardPlayer()
    {
        movesTowardPlayer.x = playerTransform.position.x - this.transform.position.x;
        movesTowardPlayer.y = playerTransform.position.y - this.transform.position.y;
        movesTowardPlayer.Normalize();
        movesTowardPlayer = movesTowardPlayer * speed * Time.deltaTime;

        fleePlayer = -movesTowardPlayer;

        movesTowardPlayer = ClampIfTouchSomething(movesTowardPlayer, speed);
        fleePlayer = ClampIfTouchSomething(fleePlayer, speed);
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(enemyStats.WaitBeforeAttack);
        Debug.Log(enemyStats.name + " attaque !");

        if (enemyStats.DashOnPlayer) Dash(directionToDash);
        canMove = true;
    }

    private void VerifyIfIsAtDistance()
    {
        float distance = Vector3.Distance(this.transform.position, playerTransform.position);
        float minDistance = distanceToStay - marginForDistanceToStay;
        float maxDistance = distanceToStay + marginForDistanceToStay;


        if (distance < minDistance) distanceFromPlayer = EnemyDistance.TooClose;
        else if (distance > maxDistance) distanceFromPlayer = EnemyDistance.TooFar;
        else distanceFromPlayer = EnemyDistance.AtDistance;
    }

    private void Dash(Vector3 direction)
    {
        if (!isDashing)
        {
            stopDashing = Time.time + enemyStats.DashTime;
            isDashing = true;
        }
        direction = direction.normalized * (enemyStats.DistanceToRun / enemyStats.DashTime) * Time.deltaTime;
        
        direction = ClampIfTouchSomething(direction, enemyStats.DistanceToRun / enemyStats.DashTime);

        if (Time.time >= stopDashing) isDashing = false;
        else transform.Translate(direction);
    }
}
