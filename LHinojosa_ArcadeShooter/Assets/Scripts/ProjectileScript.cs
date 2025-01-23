using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    GameObject[] otherObjects;
    public Vector3 forceVector;
    [SerializeField] private float forceMultipler = 1f;
    Rigidbody2D myRb;
    public bool doGravity;
    void Start()
    {
        otherObjects = GameObject.FindGameObjectsWithTag("Planet");
        myRb = GetComponent<Rigidbody2D>();
        doGravity = false;
    }
    
    void Update()
    {
        if (doGravity)
        {
            forceVector = UpdateGameobjectForce(transform.position);
            myRb.AddForce(forceVector, ForceMode2D.Impulse);
        }
    }

    public Vector3 UpdateGameobjectForce(Vector3 pos2)
    {
        Vector3 newForceVector = Vector3.zero;
        for (int i = 0; i < otherObjects.Length; i++)
            newForceVector += (forceMultipler * myRb.mass * otherObjects[i].GetComponent<Rigidbody2D>().mass / Vector3.Distance(otherObjects[i].transform.position, pos2))
                           * (otherObjects[i].transform.position - transform.position);
        
        return newForceVector;
    }
}
