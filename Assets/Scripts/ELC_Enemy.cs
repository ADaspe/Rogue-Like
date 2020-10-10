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
    private Vector3 directionToDash;



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
            if (distanceFromPlayer == EnemyDistance.TooFar)
            {
                transform.Translate(movesTowardPlayer);
            }
            else if(distanceFromPlayer == EnemyDistance.TooClose)
            {
                transform.Translate(-movesTowardPlayer);
            }
        }
        else if (EnemyPathBehaviour == "FleePlayer")
        {
            CalculateVectorTowardPlayer();
            if (distanceFromPlayer == EnemyDistance.TooClose)
            {
                transform.Translate(-movesTowardPlayer);
            }
        }
    }

    void EnemyAttackCheck()
    {
        if((distanceFromPlayer == EnemyDistance.AtDistance || distanceFromPlayer == EnemyDistance.TooClose) && Time.time >= attackCooldown)
        {
            canMove = false;
            attackCooldown = Time.time + enemyStats.AttackCooldown;
            Debug.Log(enemyStats.Name + " charge son attaque");
            StartCoroutine("Attack");

            if(enemyStats.DashOnPlayer) directionToDash = movesTowardPlayer;
        }
    }

    private void CalculateVectorTowardPlayer()
    {
        movesTowardPlayer.x = playerTransform.position.x - this.transform.position.x;
        movesTowardPlayer.y = playerTransform.position.y - this.transform.position.y;
        movesTowardPlayer.Normalize();
        movesTowardPlayer = movesTowardPlayer * speed * Time.deltaTime;
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

        if (Time.time >= stopDashing) isDashing = false;
        else transform.Translate(direction.normalized * (enemyStats.DistanceToRun / enemyStats.DashTime) * Time.deltaTime);
    }
}
