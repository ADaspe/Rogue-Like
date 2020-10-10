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
    private Vector3 enemyMoves;



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
        EnemyMoves(enemyStats.EnemyPath.ToString());
    }

    void EnemyMoves(string EnemyPathBehaviour)
    {
        if(EnemyPathBehaviour == "FollowPlayer") //Vérifie si l'ennemi a bien le caractère FollowPlayer
        {
            enemyMoves.x = playerTransform.position.x - this.transform.position.x;
            enemyMoves.y = playerTransform.position.y - this.transform.position.y;
            enemyMoves.Normalize();
            enemyMoves = enemyMoves * speed * Time.deltaTime;

            if (distanceFromPlayer == EnemyDistance.TooFar)
            {
                transform.Translate(enemyMoves);
            }
            else if(distanceFromPlayer == EnemyDistance.TooClose)
            {
                transform.Translate(-enemyMoves);
            }
        }
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
}
