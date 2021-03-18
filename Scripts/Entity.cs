using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Transform entityImage;
    private Vector3 startposition;
    public virtual void Start()
    {
        startposition = entityImage.position;
    }
    public void Reset()
    {
        entityImage.position = startposition;
    }
    public Transform getEntityImage()
    {
        return entityImage;
    }
}//