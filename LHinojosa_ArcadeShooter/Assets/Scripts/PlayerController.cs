using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float _moveSpeed = 15f;
    float _maxVelocity = 15f;

    private Rigidbody2D _rB;
    private SpriteRenderer _sR;
    
    [SerializeField] public Sprite _regularImage;
    [SerializeField] public Sprite _hurtImage;

    private bool up;
    private bool down;
    private bool right;
    private bool left;
    
    private Camera _mainCamera;

    void Start()
    {
        _rB = GetComponent<Rigidbody2D>();
        _sR = GetComponent<SpriteRenderer>();
        _mainCamera = Camera.main;
    }

    void Update()
    {
        if (!GameScript.SpawnEnemies)
            return;
        
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            up = true;
        else
            up = false;
        
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            down = true;
        else
            down = false;
        
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            right = true;
        else
            right = false;
        
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            left = true;
        else
            left = false;
    }
    
    void FixedUpdate()
    {
        if (!GameScript.SpawnEnemies)
            return;
        
        CheckBounds(); 
            
        if (transform.position.y < GameScript.CameraCorners[3])
        {
            if (up)
                _rB.AddForce(Vector3.up * _moveSpeed);
        }
        else
            transform.position = new Vector3(transform.position.x, GameScript.CameraCorners[3], transform.position.z);

        if (transform.position.y > GameScript.CameraCorners[2])
        {
            if (down)
                _rB.AddForce(Vector3.down * _moveSpeed);
        }
        else
            transform.position = new Vector3(transform.position.x, GameScript.CameraCorners[2], transform.position.z);

        if (transform.position.x < GameScript.CameraCorners[1])
        {
            if (right)
                _rB.AddForce(Vector3.right * _moveSpeed);
        }
        else
            transform.position = new Vector3(GameScript.CameraCorners[1], transform.position.y, transform.position.z);

        if (transform.position.x > GameScript.CameraCorners[0])
        {
            if (left)
                _rB.AddForce(Vector3.left * _moveSpeed);
        }
        else
            transform.position = new Vector3(GameScript.CameraCorners[0], transform.position.y, transform.position.z);

        if (_rB.velocity.magnitude > _maxVelocity)
            _rB.velocity = _rB.velocity.normalized * _maxVelocity;
    }
    
    public IEnumerator ChangeToReg()
    {
        yield return new WaitForSeconds(.5f);
        _sR.sprite = _regularImage;
    }
    
    private void CheckBounds()
    {
        GameScript.CameraCorners = new float[]
        {
            _mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x,
            _mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x,
            _mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y,
            _mainCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y
        };
    }
}
