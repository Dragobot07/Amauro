using System;
using UnityEngine;

public class PatrollingEnemyController : Entity
{

    private Vector3 right = new Vector3(1, 0);
    private Vector3 left = new Vector3(-1, 0);
    private Vector3 up = new Vector3(0, 1);
    private Vector3 down = new Vector3(0, -1);
    private Vector3 nextDirection = new Vector3(1, 1);
    //private bool reverseDirection;
    private float stepSize = .4f;
    private float enemySpeed = 3;

    public LayerMask goLeft;
    public LayerMask goRight;
    public LayerMask goUp;
    public LayerMask goDown;
    public LayerMask playerLayer;

    public PlayerController Player;
    private int distanceFromPlayer;
    

    public override void Start()
    {
        base.Start();
        //Debug.Log(entityImage.position);
        InvokeRepeating("EnemyMove", enemySpeed, enemySpeed);// TODO replace with coroutine which can be paused when game is unfocused.

    }
    private void Update()
    { 
        if (Player.GetIsOnEnemyPath() && distanceFromPlayer==-1) //TODO PROBLEM* doesnt take into account multiple path loops and thus will repeatedly find player distance on those loops
        {
            FindPlayerDistance();
        }
    }

    private void UpdateNextDirection(Vector3 vectorToDirect)
    {
        if (Physics2D.OverlapCircle(vectorToDirect, .1f, goLeft))
            nextDirection = left;
        else if (Physics2D.OverlapCircle(vectorToDirect, .1f, goRight))
            nextDirection = right;
        else if (Physics2D.OverlapCircle(vectorToDirect, .1f, goUp))
            nextDirection = up;
        else if (Physics2D.OverlapCircle(vectorToDirect, .1f, goDown))
            nextDirection = down;
        else
            Debug.LogError("Error: Enemy on non-enemy-directing tile");
    }

    public void EnemyMove()

    {
        UpdateNextDirection(entityImage.position);
        entityImage.position += nextDirection * stepSize;
        FindPlayerDistance();
    }

    public void FindPlayerDistance()// sets the Player's Distance from the Enemy to -1 unless the player is on it's path, then sets it equal to the enemy's distance in tiles from the player
    {
        distanceFromPlayer = -1;
        UpdateNextDirection(entityImage.position);
        Vector3 testPosition = entityImage.position + nextDirection * stepSize;
        int distanceTested = 1;
        for (int i = 0; testPosition != entityImage.position && i<21;i++)
        {
            if (Physics2D.OverlapCircle(testPosition, .1f, playerLayer))
            {
                distanceFromPlayer = distanceTested;
            }
            UpdateNextDirection(testPosition);
            testPosition +=nextDirection * stepSize;
            distanceTested+=1;
            if (i == 20)
            {
                Debug.LogError("maxed out the distance loop");
            }
        }
        //Debug.Log(distanceFromPlayer);
        Player.SetDistanceFromEnemy(distanceFromPlayer);
    
    }
}
