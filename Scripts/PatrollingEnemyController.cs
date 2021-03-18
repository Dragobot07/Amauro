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
    private float enemySpeed = 1;


    public override void Start()
    {
        base.Start();
        InvokeRepeating("EnemyMove", enemySpeed, enemySpeed);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        switch (collision.name)
        {
            case "EnemyUpTile":
                nextDirection = up;
                break;
            case "EnemyDownTile":
                nextDirection = down;
                break;
            case "EnemyLeftTile":
                nextDirection = left;
                break;
            case "EnemyRightTile":
                nextDirection = right;
                break;
            default:
                Debug.Log("Error: Collided with non-enemy-directing tile:"+collision.name);
                break;

        }

    }//*/
    public void EnemyMove()
    {
        entityImage.position += nextDirection * stepSize;
    }


}
