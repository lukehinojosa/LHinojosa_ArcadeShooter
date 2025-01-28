using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    private GameObject _heavyOther;
    public Vector3 forceVector;
    private float forceMultipler = 2f;
    public Rigidbody2D myRb;
    public bool doGravity;
    void Start()
    {
        _heavyOther = GameObject.FindGameObjectWithTag("Planet");
        myRb = GetComponent<Rigidbody2D>();
        doGravity = true;
    }
    
    void FixedUpdate()
    {
        if (doGravity)
        {
            forceVector = UpdateGravityForce(transform.position);
            myRb.AddForce(forceVector, ForceMode2D.Impulse);
        }
    }

    public Vector3 UpdateGravityForce(Vector3 pos2)
    {
        Vector3 newForceVector = Vector3.zero;
        newForceVector += (forceMultipler * myRb.mass * _heavyOther.GetComponent<Rigidbody2D>().mass / Vector3.Distance(_heavyOther.transform.position, pos2))
                          * (_heavyOther.transform.position - transform.position);
        
        return newForceVector;
    }
}
