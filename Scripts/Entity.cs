using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Transform entityImage;
    private Vector3 startPosition;
    public virtual void Start()
    {
        startPosition = entityImage.position;
    }
    public void Reset()
    {
        entityImage.position = startPosition;
    }
    public Transform getEntityImage()
    {
        return entityImage;
    }
}//