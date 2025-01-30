using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    private GameObject _heavyOther;
    public Vector3 forceVector;
    [SerializeField] float forceMultipler = 2f;
    private Rigidbody2D myRb;
    private bool doGravity;
    [SerializeField] bool doFollowRotation;
    private GameScript _gameScript;
    private AudioSource _audioSource;
    void Start()
    {
        _heavyOther = GameObject.FindGameObjectWithTag("Planet");
        myRb = GetComponent<Rigidbody2D>();
        doGravity = true;
        _gameScript = FindObjectOfType<GameScript>();
        _audioSource = _gameScript._audioSource;
    }
    
    void FixedUpdate()
    {
        if (doGravity)
        {
            forceVector = UpdateGravityForce(transform.position);
            myRb.AddForce(forceVector, ForceMode2D.Impulse);
        }

        if (doFollowRotation)
        {
            SetRotation();
        }
    }

    public Vector3 UpdateGravityForce(Vector3 pos2)
    {
        Vector3 newForceVector = Vector3.zero;
        newForceVector += (forceMultipler * myRb.mass * _heavyOther.GetComponent<Rigidbody2D>().mass / Vector3.Distance(_heavyOther.transform.position, pos2))
                          * (_heavyOther.transform.position - transform.position);
        
        return newForceVector;
    }

    void SetRotation()
    {
        transform.rotation = Quaternion.LookRotation((_heavyOther.transform.position - transform.position).normalized, Vector3.up);
                if (transform.position.x < 0)
                    transform.rotation = Quaternion.Euler(0f, 0f, 90f -transform.rotation.eulerAngles.x);
                else
                    transform.rotation = Quaternion.Euler(0f, 0f, -90f + transform.rotation.eulerAngles.x);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Planet"))
        {
            _audioSource.Play();
            _audioSource.pitch = Random.Range(0.5f, 1.5f);
            
            Destroy(gameObject);
            _gameScript.UpdatePlanetLives(-1);
        }
        else if (collision.gameObject.CompareTag("Asteroid"))
        {
            Destroy(collision.gameObject);
            _gameScript.UpdateScore(1f);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            _gameScript.UpdateAlienLives(-1);
            
            PlayerController pc = collision.gameObject.GetComponent<PlayerController>();
            
            collision.gameObject.GetComponent<SpriteRenderer>().sprite =
                pc._hurtImage;

            pc.StartCoroutine(pc.ChangeToReg());
        }
    }
}
