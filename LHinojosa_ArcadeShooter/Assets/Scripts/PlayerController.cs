using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float _moveSpeed = 15f;
    float _maxVelocity = 15f;

    private Rigidbody2D _myRb;
    
    [SerializeField] public Sprite _regularImage;
    [SerializeField] public Sprite _hurtImage;
    
    private GameScript _gameScript;

    private bool up;
    private bool down;
    private bool right;
    private bool left;

    void Start()
    {
        _myRb = GetComponent<Rigidbody2D>();
        _gameScript = FindObjectOfType<GameScript>();
    }

    void Update()
    {
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
        if (_gameScript._planetLives > 0 && _gameScript._alienLives > 0)
        {
            if (transform.position.y < _gameScript._yT)
            {
                if (up)
                    _myRb.AddForce(Vector3.up * _moveSpeed);
            }
            else
                transform.position = new Vector3(transform.position.x, _gameScript._yT, transform.position.z);

            if (transform.position.y > _gameScript._yB)
            {
                if (down)
                    _myRb.AddForce(Vector3.down * _moveSpeed);
            }
            else
                transform.position = new Vector3(transform.position.x, _gameScript._yB, transform.position.z);

            if (transform.position.x < _gameScript._xR)
            {
                if (right)
                    _myRb.AddForce(Vector3.right * _moveSpeed);
            }
            else
                transform.position = new Vector3(_gameScript._xR, transform.position.y, transform.position.z);

            if (transform.position.x > _gameScript._xL)
            {
                if (left)
                    _myRb.AddForce(Vector3.left * _moveSpeed);
            }
            else
                transform.position = new Vector3(_gameScript._xL, transform.position.y, transform.position.z);

            if (_myRb.velocity.magnitude > _maxVelocity)
                _myRb.velocity = _myRb.velocity.normalized * _maxVelocity;
        }
    }
    
    public IEnumerator ChangeToReg()
    {
        yield return new WaitForSeconds(.5f);
        GetComponent<SpriteRenderer>().sprite = _regularImage;
    }
}
