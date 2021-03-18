using UnityEngine;
public class PlayerController : Entity
{
    
    /*
    public float moveSpeed = 5f; //speed of moment to target
    public Transform movePoint; //targets movements\
    
    // needed to make move pretty */

    private Vector3 direction = new Vector3(1,0);
    private float stepSize = .4f;
    public LayerMask whatStopsMovement;
    public LayerMask whatKillsPlayer;
    public Transform entityParent;

    private void Update()
    {
        if (Physics2D.OverlapCircle(entityImage.position, .1f, whatKillsPlayer))
        {
            Kill();
        }
    }
    public void PlayerMove ()
    {
        if (!Physics2D.OverlapCircle(entityImage.position + direction * stepSize, .1f, whatStopsMovement)){
            entityImage.position += direction * stepSize;
           
        }

    }

    public void RotateLeft()
    {
        entityImage.Rotate(new Vector3(0, 0, 1), -90);
        direction = new Vector3(-direction.y, direction.x);
    }

    public void RotateRight()
    {
        entityImage.Rotate(new Vector3(0, 0, 1), -90);
        direction = new Vector3(direction.y, -direction.x);
    }

    public void Kill()
    {
        for (int n = 0; n<=entityParent.childCount; n++)
        {
            Transform possibleEntity=entityParent.GetChild(n);
            possibleEntity.GetComponent<Entity>().Reset();
        }
    }
    

}
