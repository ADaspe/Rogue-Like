using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ELC_Enemy : MonoBehaviour
{
    [SerializeField]
    public ELC_EnemySO enemyStats;
    public ELC_PassivesProperties passiveScript;
    [HideInInspector]
    public Collider2D enemyCollider;
    private SpriteRenderer spriteRenderer;
    private Transform playerTransform;
    [HideInInspector]
    public Animator enemyAnimator;
    public GameObject Coins;

    [SerializeField]
    public int currentHealth;
    public float speed;
    private bool canMove = true;
    public bool isDead;
    public bool isDashing;
    public bool isSpawning;
    public bool isDying;
    public float stopDashing;
    private float attackCooldown; // le cooldown entre chaque attaque
    public float dashCooldown; //Valable que pour les ennemis qui ont l'attaque de base + l'attaque à distance
    public bool canDashAttack; //Valable que pour les ennemis qui ont l'attaque de base + l'attaque à distance
    public Vector3 movesTowardPlayer;
    private Vector3 fleePlayer;
    private Vector3 directionToDash;
    private Vector3 lastDirection;
    private Vector3 MoveAwayOtherEnemies;
    private bool isPreparingAttack;
    public bool isAttacking;
    public bool isDistanceAttacking;
    public bool isHit;

    public Material basicMat;
    public Material dissolveMaterial;
    public Material getHitMaterial;
    public Material deathMaterial;
    public float dissolveValue = 1;

    private Vector3 currentDashDirection;
    private float currentDashDistance;
    private float currentDashTime;

    private const float knockbackTime = 0.2f;
    public bool isStun;
    private bool canBeStun = true;
    private bool stopDashDamage = false;
    public bool isTmpInvulnerable = false;
    public bool isInvulnerable = false;
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
    public float distanceToStay;
    private float marginForDistanceToStay = 0.02f; //La marge dans laquelle peut être l'ennemi avant de s'approcher ou de reculer
    private enum EnemyDistance { TooFar, AtDistance, TooClose };
    private EnemyDistance distanceFromPlayer;

    void Start()
    {
        dissolveValue = 0;
        passiveScript = FindObjectOfType<ELC_PassivesProperties>();
        enemyCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyAnimator = GetComponent<Animator>();
        isTmpInvulnerable = false;
        dashCooldown = Time.time + enemyStats.DashCooldown;
        
        if (enemyStats != null)
        {
            currentHealth = enemyStats.MaxHealth;
            speed = enemyStats.MovementSpeed;
            distanceToStay = enemyStats.LimitDistanceToStay;
        }
        else
        {
            Debug.Log("Enemy Stats null on " + this.gameObject.name);
        }
        
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        StartCoroutine("Spawn");
    }


    void Update()
    {
        if(!isDead) VerifyIfIsAtDistance();
        
        if(isSpawning) // pour le shader de spawn
        {
            dissolveValue += Time.deltaTime * 5 / enemyStats.SpawnTime;
            spriteRenderer.material.SetFloat("_DissolveLevel", dissolveValue);
        }
        else if(isDying)
        {
            dissolveValue -= Time.deltaTime * 5 / enemyStats.DeathTime;
            spriteRenderer.material.SetFloat("_DissolveLevel", dissolveValue);
            //Debug.Log(dissolveValue);
        }

        if (!isStun && !isDead)
        {
            EnemyAttackCheck();

            if (canSeePlayer || !isTouchingSomething) tryToGo = Direction.Nowhere;

            if (isTouchingDown && isTouchingRight || isTouchingDown && isTouchingLeft || isTouchingTop && isTouchingRight || isTouchingTop && isTouchingLeft || isTouchingRight && isTouchingLeft || isTouchingTop && isTouchingDown)
            {
                EscapeWhenIsInWall();
            }
            else if (isTouchingSomething && !canSeePlayer && !playerIsInWall)
            {
                Raycasts();
                TryToGetOutOfWall(movesTowardPlayer);
                playerIsInWall = false;
            }
            else if (canMove && !isDashing)
            {
                if(enemyStats == null)
                {
                    Debug.Log("Encore un bug ici tiens");
                }
                EnemyMoves(enemyStats.EnemyPath.ToString());
                playerIsInWall = false;
            }
        }

        if (isDashing && !isDead) Dash(currentDashDirection, currentDashTime, currentDashDistance);

        if (lastDirection.x > 0) spriteRenderer.flipX = true;
        else spriteRenderer.flipX = false;
    }

    IEnumerator Spawn()
    {
        isSpawning = true;
        StartCoroutine(ApplyShader(enemyStats.SpawnTime, dissolveMaterial));
        canMove = false;
        yield return new WaitForSeconds(enemyStats.SpawnTime);
        canMove = true;
        isSpawning = false;
    }

    void EnemyMoves(string EnemyPathBehaviour)
    {
        if(EnemyPathBehaviour == "FollowPlayer" || EnemyPathBehaviour == "StayAtDistance") //Vérifie si l'ennemi a bien le caractère FollowPlayer
        {
            CalculateVectorTowardPlayer();
            //Raycasts();
            if (distanceFromPlayer == EnemyDistance.TooFar)
            {
                transform.Translate(movesTowardPlayer + MoveAwayOtherEnemies);
                lastDirection = movesTowardPlayer;
                CalculateDirectionForAnimator(movesTowardPlayer);
            }
            else if(distanceFromPlayer == EnemyDistance.TooClose)
            {
                transform.Translate(fleePlayer);
                lastDirection = fleePlayer;
                CalculateDirectionForAnimator(fleePlayer);
            }
        }
        else if (EnemyPathBehaviour == "FleePlayer")
        {
            CalculateVectorTowardPlayer();
            if (distanceFromPlayer == EnemyDistance.TooClose)
            {
                transform.Translate(fleePlayer);
                lastDirection = fleePlayer;
                CalculateDirectionForAnimator(fleePlayer);
            }
        }
    }

    void CalculateDirectionForAnimator(Vector3 vectorToUse)
    {
        enemyAnimator.SetFloat("MovesX", Mathf.Clamp(vectorToUse.x, -1, 1));
        enemyAnimator.SetFloat("MovesY", Mathf.Clamp(vectorToUse.y, -1, 1));

    }

    void EscapeWhenIsInWall()
    {
        Raycasts();
        playerIsInWall = true;
        Debug.Log(enemyStats.Name + " is trying to escape from a wall.");
        Vector3 vectorDirection = new Vector3(0,0);
        //if (isTouchingRight) vectorDirection.x = -speed;
        //if (isTouchingLeft) vectorDirection.x = speed;
        //if (isTouchingTop) vectorDirection.y = -speed;
        //if (isTouchingDown) vectorDirection.y = speed;

        //if (isTouchingTop && isTouchingLeft && isTouchingRight) vectorDirection = new Vector3(0, speed);
        //if (isTouchingDown && isTouchingLeft && isTouchingRight) vectorDirection = new Vector3(0, speed);
        //if (isTouchingLeft && isTouchingDown && isTouchingTop) vectorDirection = new Vector3(speed, 0);
        //if (isTouchingRight && isTouchingDown && isTouchingTop) vectorDirection = new Vector3(-speed, 0);

        CalculateVectorTowardPlayer();

        transform.Translate(movesTowardPlayer /** Time.deltaTime*/);
    }

    void EnemyAttackCheck()
    {
        if (!isAttacking)
        {
            if ((distanceFromPlayer == EnemyDistance.AtDistance || distanceFromPlayer == EnemyDistance.TooClose) && Time.time >= attackCooldown && canSeePlayer)
            {
                canMove = false;
                attackCooldown = Time.time + enemyStats.AttackCooldown;
                //Debug.Log(enemyStats.Name + " charge son attaque");
                StartCoroutine(Attack(false));

                if (enemyStats.DashOnPlayer) directionToDash = movesTowardPlayer;
            }
            else if (canDashAttack)
            {
                dashCooldown = Time.time + enemyStats.DashCooldown;
                StartCoroutine(Attack(true));
                directionToDash = movesTowardPlayer;
            }
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

        CheckDistanceFromOtherEnemies();

        fleePlayer = -movesTowardPlayer;

        if (!playerIsInWall)
        {
            movesTowardPlayer = ClampIfTouchSomething(movesTowardPlayer, speed);
            
        }
        fleePlayer = ClampIfTouchSomething(fleePlayer, speed);
    }

    private IEnumerator Attack(bool isDashing = false)
    {
        enemyAnimator.SetBool("IsPreparingForAttack", true);
        if (isDashing)
        {
            enemyAnimator.SetBool("DashAttack", true);
            enemyAnimator.SetBool("BasicAttack", false);
        }
        else
        {
            enemyAnimator.SetBool("BasicAttack", true);
            enemyAnimator.SetBool("DashAttack", false);
        }
        yield return new WaitForSeconds(enemyStats.WaitBeforeAttack);
        //Debug.Log(enemyStats.name + " attaque !");
        enemyAnimator.SetBool("IsPreparingForAttack", false);
        enemyAnimator.SetBool("IsAttacking", true);
        isAttacking = true;
        //Debug.Log("Attack");
        AXD_HydraHead head = GetComponent<AXD_HydraHead>();
        if(head != null)
        {
            if (head.hydra.currentPhase == AXD_Hydra.BossPhase.Phase1)
            {
                enemyAnimator.SetBool("Phase1", true);
                enemyAnimator.SetBool("Phase2", false);
                enemyAnimator.SetBool("Phase3", false);
            }
            else if (head.hydra.currentPhase == AXD_Hydra.BossPhase.Phase2)
            {
                enemyAnimator.SetBool("Phase1", false);
                enemyAnimator.SetBool("Phase2", true);
                enemyAnimator.SetBool("Phase3", false);
            }
            else if (head.hydra.currentPhase == AXD_Hydra.BossPhase.Phase3)
            {
                enemyAnimator.SetBool("Phase1", false);
                enemyAnimator.SetBool("Phase2", false);
                enemyAnimator.SetBool("Phase3", true);
            }
        }
        if((enemyStats.DashAndCloseCombatAttack && isDashing) || (enemyStats.DashOnPlayer && !enemyStats.DashAndCloseCombatAttack))
        {
            Dash(directionToDash, enemyStats.DashTime, enemyStats.DistanceToRun);
            HitPlayer(true);
        }
        else if (enemyStats.DistanceAttack)
        {
            DistanceAttack();
            isDistanceAttacking = true;      
        }
        else HitPlayer();

        canMove = true;
        if (enemyStats.DashAndCloseCombatAttack && isDashing) yield return new WaitForSeconds(enemyStats.DashTime);
        else yield return new WaitForSeconds(enemyStats.AttackAnimationTime);
        enemyAnimator.SetBool("IsAttacking", false);
        if (isDashing) enemyAnimator.SetBool("DashAttack", false);
        else enemyAnimator.SetBool("BasicAttack", false);
        isAttacking = false;
        isDistanceAttacking = false;
    }

    private void DistanceAttack()
    {
        CalculateVectorTowardPlayer();
        GameObject projectile;
        projectile = Instantiate(enemyStats.Projectile, this.transform.position, Quaternion.identity);
        projectile.GetComponent<ELC_Projectiles>().direction = movesTowardPlayer;
        projectile.GetComponent<ELC_Projectiles>().lifeDuration = enemyStats.ProjectileDurability;
        projectile.GetComponent<ELC_Projectiles>().strenght = enemyStats.ProjectileStrenght;
        projectile.GetComponent<ELC_Projectiles>().speed = enemyStats.ProjectileSpeed;
        projectile.GetComponent<ELC_Projectiles>().StartCoroutine("LifeDuration");
    }

    private void VerifyIfIsAtDistance()
    {
        float distance = Vector3.Distance(this.transform.position, playerTransform.position);
        float minDistance = distanceToStay - marginForDistanceToStay;
        float maxDistance = distanceToStay + marginForDistanceToStay;


        if (distance < minDistance) distanceFromPlayer = EnemyDistance.TooClose;
        else if (distance > maxDistance) distanceFromPlayer = EnemyDistance.TooFar;
        else distanceFromPlayer = EnemyDistance.AtDistance;

        if (enemyStats.DashAndCloseCombatAttack && distanceFromPlayer == EnemyDistance.TooFar && distance < enemyStats.MaxDistanceToTriggerDash && distance > enemyStats.MinDistanceToTriggerDash && Time.time > dashCooldown) canDashAttack = true;
        else canDashAttack = false;
    }

    private void CheckDistanceFromOtherEnemies()
    {
        Collider2D[] hitColliders = null;
        MoveAwayOtherEnemies = Vector3.zero;
        hitColliders = Physics2D.OverlapCircleAll(this.transform.position, enemyStats.EnemyWidth, LayerMask.GetMask("Enemies"));
        if(hitColliders != null && hitColliders.Length > 0)
        {
            foreach(Collider2D collider in hitColliders)
            {
                MoveAwayOtherEnemies += this.transform.position - collider.gameObject.transform.position;
            }
            MoveAwayOtherEnemies = MoveAwayOtherEnemies.normalized * speed * Time.deltaTime;
            MoveAwayOtherEnemies = ClampIfTouchSomething(MoveAwayOtherEnemies, speed);
        }
    }

    private void Dash(Vector3 direction, float dashTime, float dashDistance)
    {
        Raycasts();
        if (!isDashing)
        {
            stopDashing = Time.time + dashTime;
            isDashing = true;
            currentDashDirection = direction;
            currentDashDistance = dashDistance;
            currentDashTime = dashTime;
        }
        direction = direction.normalized * (dashDistance / dashTime) * Time.deltaTime;
        
        direction = ClampIfTouchSomething(direction, dashDistance / dashTime);

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

    IEnumerator Stun(float time, bool invulnerable = false)
    {
        
        canBeStun = false;
        if (invulnerable)
        {
            isTmpInvulnerable = true;
        }
        yield return new WaitForSeconds(enemyStats.noStunTime);
        
        isStun = true;
        
        yield return new WaitForSeconds(time);
        if (isTmpInvulnerable)
        {
            isTmpInvulnerable = false;
        }
        isStun = false;

        yield return new WaitForSeconds(enemyStats.noStunTime);
        
        canBeStun = true;
    }

    IEnumerator ApplyShader(float time, Material mat)
    {
        spriteRenderer.material = mat;
        //spriteRenderer.material.shader = mat.shader;
        yield return new WaitForSeconds(time);
        spriteRenderer.material = basicMat;
        //spriteRenderer.material.shader = basicMat.shader;

    }

    IEnumerator HitSound()
    {
        isHit = true;
        yield return new WaitForSeconds(0.01f);
        isHit = false;
    }
    public void GetHit(int Damage, Vector3 directionToFlee, float knockbackDistance = 0, float stunTime = 0, bool invulnerable = false)
    {
        //DropCoins(5);
        Debug.Log("Enemy hit");
        if (!isTmpInvulnerable && !isInvulnerable)
        {
            currentHealth -= Damage;
            StartCoroutine(HitSound());
            Dash(-directionToFlee, knockbackTime, knockbackDistance);
            if (!isStun && !isPreparingAttack && canBeStun == true)
            {
                StartCoroutine(Stun(stunTime, invulnerable));
                
                StopCoroutine("Attack");
                enemyAnimator.SetBool("IsPreparingForAttack", false);
                enemyAnimator.SetBool("IsAttacking", false);
                canMove = true;
            }
        }
        if(currentHealth <= 0)
        {
            if(enemyStats.achievements!=null && enemyStats.achievements.Count >= 0)
            {
                foreach(AXD_AchievementSO achivement in enemyStats.achievements)
                {
                    achivement.AddDefeated();
                }
            }
            AXD_Hydra tmpHydra = GetComponent<AXD_Hydra>();
            if (tmpHydra == null){
                StartCoroutine(Death());
            }
            else
            {
                StartCoroutine(tmpHydra.Death());
            }
            
        }
        else StartCoroutine(ApplyShader(0.05f, getHitMaterial));
    }

    IEnumerator Death()
    {
        Debug.Log("Oui");
        isDying = true;
        dissolveValue = 5;
        enemyCollider.enabled = false;
        isDead = true;
        StopCoroutine("Attack");
        enemyAnimator.SetBool("IsPreparingForAttack", false);
        enemyAnimator.SetBool("IsAttacking", false);
        StartCoroutine(ApplyShader(enemyStats.DeathTime, deathMaterial));
        yield return new WaitForSeconds(enemyStats.DeathTime - 0.3f);
        DropCoins((int)FindObjectOfType<ELC_PlayerStatManager>().MoneyMultiplicatorPU * enemyStats.MoneyEarnWhenDead);

        if (passiveScript.ActualPassiveScriptableObject != null)
        {
            if (passiveScript.ActualPassiveScriptableObject.PassiveName == "Corne D'Abondance" && Random.Range(0, 101) < passiveScript.CorneAbondancePercentageChanceDropPowerUp) Instantiate(passiveScript.PowerUpsGenerator, this.transform.position, Quaternion.identity);
            else if (passiveScript.ActualPassiveScriptableObject.PassiveName == "Faux De Chronos") FindObjectOfType<ELC_PowerUpManager>().StopFlow();
        }
        Destroy(this.gameObject);
        yield return null;
    }

    public void DropCoins(int moneyValue)
    {
        int numberToDrop = Mathf.FloorToInt(moneyValue / Coins.GetComponent<ELC_Coins>().value);

        for (int i = 0; i < numberToDrop; i++)
        {
            Instantiate(Coins, this.transform.position, Quaternion.identity);
        }
    }

    public void HitPlayer(bool dashAttack = false)
    {
        Collider2D[] hitColliders = null;


        if (dashAttack == false) //basic attack
        {
            hitColliders = Physics2D.OverlapCircleAll(this.transform.position + lastDirection.normalized * enemyStats.AttackRange, enemyStats.AttackRange, LayerMask.GetMask("Player"));
            if (hitColliders != null && hitColliders.Length > 0)
            {
                ELC_PlayerStatManager playerStats =  FindObjectOfType<ELC_PlayerStatManager>();
                hitColliders[0].gameObject.GetComponent<PlayerHealth>().GetHit((int)(enemyStats.AttackStrenght *  (1 / playerStats.DefenseMultiplicatorPU) * playerStats.FilAresDamagesTakenMultiplicator));
                //Debug.Log("Close combat Hit");
            }
        }
        else //dashAttack
        {
            StartCoroutine("DashAttack");
        }
    }

    private IEnumerator DashAttack()
    {
        //Debug.Log("DashAttack");
        Collider2D[] hitColliders = null;
        
        bool hitPlayer = false;
        if (!stopDashDamage)
        {
            while (Time.time <= stopDashing && hitPlayer == false && !stopDashDamage)
            {
                hitColliders = null;
                hitColliders = Physics2D.OverlapBoxAll(this.transform.position + directionToDash.normalized * 0.5f, new Vector2(enemyStats.DashColliderWidth, enemyStats.DashColliderWidth), Vector2.Angle(Vector2.up, directionToDash), LayerMask.GetMask("Player"));
                if (hitColliders != null && hitColliders.Length > 0)
                {
                    hitColliders[0].gameObject.GetComponent<PlayerHealth>().GetHit((int)enemyStats.DashStrenght);
                    //Debug.Log("Dash Hit");
                    hitPlayer = true;
                    stopDashDamage = true;
                }
                yield return null;
            }
        }
        stopDashDamage = false;
    }


    private void OnDrawGizmosSelected()
    {
        //Gizmos.DrawWireSphere(this.transform.position + lastDirection.normalized * enemyStats.AttackRange, enemyStats.AttackRange);
        //Gizmos.DrawCube(this.transform.position + directionToDash.normalized * 0.5f, new Vector3(enemyStats.DashColliderWidth, enemyStats.DashColliderWidth, 0));
    }

}
