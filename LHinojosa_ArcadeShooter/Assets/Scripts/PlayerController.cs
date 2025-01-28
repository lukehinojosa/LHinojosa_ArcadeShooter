using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float _moveSpeed = 15f;
    float _maxY = 4.4f;
    float _minY = -4.4f;
    float _maxX = 8f;
    float _minX = -8f;
    float _maxVelocity = 15f;

    private Rigidbody2D _myRb;

    void Start()
    {
        _myRb = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        if (transform.position.y < _maxY)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                _myRb.AddForce(Vector3.up * _moveSpeed);
        }
        else
            transform.position = new Vector3(transform.position.x, _maxY, transform.position.z);

        if (transform.position.y > _minY)
        {
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                _myRb.AddForce(Vector3.down * _moveSpeed);
        }
        else
            transform.position = new Vector3(transform.position.x, _minY, transform.position.z);
        
        if (transform.position.x < _maxX)
        {
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                _myRb.AddForce(Vector3.right * _moveSpeed);
        }
        else
            transform.position = new Vector3(_maxX, transform.position.y, transform.position.z);
        
        if (transform.position.x > _minX)
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                _myRb.AddForce(Vector3.left * _moveSpeed);
        }
        else
            transform.position = new Vector3(_minX, transform.position.y, transform.position.z);
        
        if (_myRb.velocity.magnitude > _maxVelocity)
            _myRb.velocity = _myRb.velocity.normalized * _maxVelocity;
    }
}
