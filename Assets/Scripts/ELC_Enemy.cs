using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ELC_Enemy : MonoBehaviour
{
    [SerializeField]
    private ELC_EnemySO enemyStats;

    private Collider2D enemyCollider;
    private SpriteRenderer spriteRenderer;
    private Transform playerTransform;

    [SerializeField]
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
    [SerializeField]
    private bool isTouchingSomething;
    private bool playerIsInWall;
    [SerializeField]
    private bool canSeePlayer; //regardes si il y a un mur entre l'ennemi et le player
    
    private enum Direction { Left, Right, Top, Down, Nowhere};
    [SerializeField]
    private Direction tryToGo = Direction.Nowhere;

    [SerializeField]
    private LayerMask obstaclesLayerMask;



    [Header ("StayAtDistanceFromPlayer")]
    private float distanceToStay;
    private float marginForDistanceToStay = 0.02f; //La marge dans laquelle peut être l'ennemi avant de s'approcher ou de reculer
    private enum EnemyDistance { TooFar, AtDistance, TooClose };
    private EnemyDistance distanceFromPlayer;

    void Start()
    {
        enemyCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        actualLives = enemyStats.MaxHealth;
        speed = enemyStats.MovementSpeed;
        distanceToStay = enemyStats.LimitDistanceToStay;
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }


    void Update()
    {
        VerifyIfIsAtDistance();
        EnemyAttackCheck();

        if (isTouchingDown && isTouchingRight || isTouchingDown && isTouchingLeft || isTouchingTop && isTouchingRight || isTouchingTop && isTouchingLeft)
        {
            EscapeWhenIsInWall();
        }
        else playerIsInWall = false;

        if (canSeePlayer || !isTouchingSomething) tryToGo = Direction.Nowhere;
        
        if (isTouchingSomething && !canSeePlayer && !playerIsInWall)
        {
            Raycasts();
            TryToGetOutOfWall(movesTowardPlayer);
        }
        else if(canMove && !isDashing) EnemyMoves(enemyStats.EnemyPath.ToString());
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

    void EscapeWhenIsInWall()
    {
        playerIsInWall = true;
        Debug.Log(enemyStats.Name + " is trying to escape from a wall.");
        Vector3 vectorDirection = new Vector3(0,0);
        if (isTouchingRight) vectorDirection.x = -speed;
        if (isTouchingLeft) vectorDirection.x = speed;
        if (isTouchingTop) vectorDirection.y = -speed;
        if (isTouchingDown) vectorDirection.y = speed;
        transform.Translate(vectorDirection * Time.deltaTime);
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

        float raycastWidth = 0.9f;
        float raycastLenght = 0.8f;
        float moveRaycastLenght = 2;
        GameObject WallTouched;

        //Right
        Vector3 rightRaycastStart = new Vector3(this.transform.position.x + 0.5f * raycastWidth, this.transform.position.y + 0.5f * raycastLenght);
        RaycastHit2D rightHit = Physics2D.Raycast(rightRaycastStart, transform.TransformDirection(Vector2.down), raycastLenght, obstaclesLayerMask);
        Debug.DrawRay(rightRaycastStart, transform.TransformDirection(Vector2.down) * raycastLenght, Color.red);
        if (rightHit)
        {
            isTouchingRight = true;
            WallTouched = rightHit.transform.gameObject;
        }
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


        //DirectionVectors
        Vector3 vectorTowardPlayer = new Vector3(playerTransform.position.x - this.transform.position.x, playerTransform.position.y - this.transform.position.y).normalized * enemyStats.LimitDistanceToStay;
        RaycastHit2D directionRaycastHit = Physics2D.Raycast(this.transform.position, transform.TransformDirection(vectorTowardPlayer), moveRaycastLenght, obstaclesLayerMask);
        Debug.DrawRay(this.transform.position, transform.TransformDirection(vectorTowardPlayer), Color.blue);
        RaycastHit2D directionRaycastHit2 = Physics2D.Raycast(this.transform.position + new Vector3(0.5f, 0), transform.TransformDirection(vectorTowardPlayer), moveRaycastLenght, obstaclesLayerMask);
        Debug.DrawRay(this.transform.position + new Vector3(0.4f, 0), transform.TransformDirection(vectorTowardPlayer), Color.blue);
        RaycastHit2D directionRaycastHit3 = Physics2D.Raycast(this.transform.position + new Vector3(0, 0.5f), transform.TransformDirection(vectorTowardPlayer), moveRaycastLenght, obstaclesLayerMask);
        Debug.DrawRay(this.transform.position + new Vector3(0, 0.4f), transform.TransformDirection(vectorTowardPlayer), Color.blue);
        RaycastHit2D directionRaycastHit4 = Physics2D.Raycast(this.transform.position + new Vector3(0, -0.5f), transform.TransformDirection(vectorTowardPlayer), moveRaycastLenght, obstaclesLayerMask);
        Debug.DrawRay(this.transform.position + new Vector3(0, -0.4f), transform.TransformDirection(vectorTowardPlayer), Color.blue);
        RaycastHit2D directionRaycastHit5 = Physics2D.Raycast(this.transform.position + new Vector3(-0.5f, 0), transform.TransformDirection(vectorTowardPlayer), moveRaycastLenght, obstaclesLayerMask);
        Debug.DrawRay(this.transform.position + new Vector3(-0.4f, 0), transform.TransformDirection(vectorTowardPlayer), Color.blue);

        if (directionRaycastHit || directionRaycastHit2 || directionRaycastHit3 || directionRaycastHit4 || directionRaycastHit5) canSeePlayer = false;
        else canSeePlayer = true;

        if (isTouchingDown || isTouchingLeft || isTouchingRight || isTouchingTop) isTouchingSomething = true;
        else isTouchingSomething = false;
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

    private void TryToGetOutOfWall(Vector3 directionVector)
    {
        Raycasts();
        
        if (isTouchingRight || isTouchingLeft)
        {
            if (directionVector.y > 0 && !isTouchingTop /*&& tryToGo != Direction.Down*/)
            {
                directionVector.y = speed;//Mathf.Clamp(movesTowardPlayer.y, 0.05f, speed / 2);
                tryToGo = Direction.Top;
            }
            else if (directionVector.y < 0 && !isTouchingDown /*&& tryToGo != Direction.Top*/)
            {
                directionVector.y = -speed;//Mathf.Clamp(movesTowardPlayer.y, -speed / 2, -0.05f);
                tryToGo = Direction.Down;
            }
        }
        else if(isTouchingDown || isTouchingTop)
        {
            if (directionVector.x > 0 && !isTouchingRight /*&& tryToGo != Direction.Left*/)
            {
                directionVector.x = speed;//Mathf.Clamp(movesTowardPlayer.x, 0.05f, speed / 2);
                tryToGo = Direction.Right;
                
            }
            else if (directionVector.x < 0 && !isTouchingLeft /*&& tryToGo != Direction.Right*/)
            {
                directionVector.x = -speed;//Mathf.Clamp(movesTowardPlayer.x, -speed / 2, -0.05f);
                tryToGo = Direction.Left;
            }
        }

        if(!isTouchingSomething)
        {
            tryToGo = Direction.Nowhere;
        }

        transform.Translate(directionVector * Time.deltaTime);
    }
    public void GetHit(int Damage)
    {
        actualLives -= Damage;

        if(actualLives <= 0)
        {
            enemyCollider.enabled = false;
            spriteRenderer.enabled = false;
            Debug.Log(enemyStats.Name + " is dead, so saaaaad :(");
        }
    }
}
