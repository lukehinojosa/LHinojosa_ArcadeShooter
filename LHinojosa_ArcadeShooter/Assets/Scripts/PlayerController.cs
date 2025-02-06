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

    private bool _up;
    private bool _down;
    private bool _right;
    private bool _left;
    private bool _canWarp = true;
    
    private Camera _mainCamera;

    private float _sizeSteps = 0.2f;
    private float _smallSize = 0.01f;
    private float _sizeTimeMultiplier = 0.1f;
    
    private HUD _hud;
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _teleportSound;

    void Start()
    {
        _rB = GetComponent<Rigidbody2D>();
        _sR = GetComponent<SpriteRenderer>();
        _mainCamera = Camera.main;
        _hud = FindObjectOfType<HUD>();
        _audioSource = _hud.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!GameScript.SpawnEnemies)
            return;
        
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            _up = true;
        else
            _up = false;
        
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            _down = true;
        else
            _down = false;
        
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            _right = true;
        else
            _right = false;
        
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            _left = true;
        else
            _left = false;

        if (_canWarp && (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)))
            StartCoroutine(Co_Warp());
    }
    
    void FixedUpdate()
    {
        if (!GameScript.SpawnEnemies)
            return;
        
        CheckBounds(); 
            
        if (transform.position.y < GameScript.CameraCorners[3])
        {
            if (_up)
                _rB.AddForce(Vector3.up * _moveSpeed);
        }
        else
            transform.position = new Vector3(transform.position.x, GameScript.CameraCorners[3], transform.position.z);

        if (transform.position.y > GameScript.CameraCorners[2])
        {
            if (_down)
                _rB.AddForce(Vector3.down * _moveSpeed);
        }
        else
            transform.position = new Vector3(transform.position.x, GameScript.CameraCorners[2], transform.position.z);

        if (transform.position.x < GameScript.CameraCorners[1])
        {
            if (_right)
                _rB.AddForce(Vector3.right * _moveSpeed);
        }
        else
            transform.position = new Vector3(GameScript.CameraCorners[1], transform.position.y, transform.position.z);

        if (transform.position.x > GameScript.CameraCorners[0])
        {
            if (_left)
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

    private IEnumerator Co_Warp()
    {
        _canWarp = false;
        
        _audioSource.volume = 0.1f;
        _audioSource.pitch = 1f;
        _audioSource.PlayOneShot(_teleportSound);
        
        //Shrink
        while (transform.localScale != Vector3.one * _smallSize)
        {
            yield return new WaitForSeconds(Time.deltaTime * _sizeTimeMultiplier);
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * _smallSize, _sizeSteps);
        }
        
        _sR.enabled = false;
        
        //Warp
        transform.position = new Vector3(-transform.position.x, -transform.position.y, 0f);
        
        //Grow
        while (transform.localScale != Vector3.one)
        {
            if (transform.localScale.x > 0.1f)
                _sR.enabled = true;
            
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, _sizeSteps);
            yield return new WaitForSeconds(Time.deltaTime * _sizeTimeMultiplier);
        }
        
        _canWarp = true;
    }
}
