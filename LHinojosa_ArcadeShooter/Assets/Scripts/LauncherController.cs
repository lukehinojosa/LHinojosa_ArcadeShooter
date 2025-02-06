using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class LauncherController : MonoBehaviour
{
    [SerializeField] GameObject _projectilePrefab;
    private GameObject currentProjectile;
    [SerializeField] private float _deathTime = 2f;
    [SerializeField] float _forceMultiplier = 1f;
    [SerializeField] float _spawnDistance = 1.25f;
    private Vector3 _mySpawnOffset;

    private GameObject _heavyOther;
    
    private HUD _hud;
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _shootSound;

    void Start()
    {
        _heavyOther = GameObject.FindGameObjectWithTag("Planet");
        _hud = FindObjectOfType<HUD>();
        _audioSource = _hud.GetComponent<AudioSource>();
    }
    void Update()
    {
        _mySpawnOffset = -transform.up * _spawnDistance;
        Controls();
        SetRotation();
    }

    private void Controls()
    {
        if (GameScript.PlanetLives > 0 && GameScript.AlienLives > 0)
        {
            if (currentProjectile == null && Input.GetKeyDown(KeyCode.Space))
            {
                currentProjectile = Instantiate(_projectilePrefab, transform.position + _mySpawnOffset,
                    Quaternion.identity);
                currentProjectile.GetComponent<Rigidbody2D>()
                    .AddForce(_forceMultiplier * -transform.up, ForceMode2D.Impulse);
                Destroy(currentProjectile, _deathTime);
                
                _audioSource.volume = 0.1f;
                _audioSource.pitch = Random.Range(0.5f, 0.75f);
                _audioSource.PlayOneShot(_shootSound);
            }
        }
    }

    private void SetRotation()
    {
        transform.rotation = Quaternion.LookRotation((_heavyOther.transform.position - transform.position).normalized, Vector3.up);
        if (transform.position.x < 0)
            transform.rotation = Quaternion.Euler(0f, 0f, -transform.rotation.eulerAngles.x);
        else
            transform.rotation = Quaternion.Euler(0f, 0f, 180f + transform.rotation.eulerAngles.x);
    }
}