using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameScript : MonoBehaviour
{
    private float _score = 0;
    private float _timer = 0;
    private float _time = 2.5f;
    public int _planetLives = 3;
    public int _alienLives = 3;

    public float _xL;
    public float _xR;
    public float _yT;
    public float _yB;

    [SerializeField] TextMeshProUGUI _scoreText;
    [SerializeField] TextMeshProUGUI _loseText;

    [SerializeField] private GameObject _AsteroidPrefab;

    [SerializeField] private Image[] _lifeImages;
    [SerializeField] private Image[] _alienHeadImages;

    public AudioSource _audioSource;
    private Camera _mainCamera;

    void Start()
    {
        _mainCamera = Camera.main;
        _audioSource = GetComponent<AudioSource>();
        CheckAspectRatio();
    }

    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _time)
        {
            SpawnAsteroid();
            _timer = 0;
        }
    }

    void SpawnAsteroid()
    {
        CheckAspectRatio();

        if (_planetLives > 0 && _alienLives > 0)
        {
            int chance = Random.Range(0, 4);
            switch (chance)
            {
                case 0: //Spawn at top
                    Instantiate(_AsteroidPrefab, new Vector3(Random.Range(_xL, _xR), _yT + 1f, 0f), Quaternion.identity);
                    break;
                case 1: //Spawn at bottom
                    Instantiate(_AsteroidPrefab, new Vector3(Random.Range(_xL, _xR), _yB - 1f, 0f), Quaternion.identity);
                    break;
                case 2: //Spawn at left
                    Instantiate(_AsteroidPrefab, new Vector3(_xL - 1f, Random.Range(_yB, _yT), 0f), Quaternion.identity);
                    break;
                default: //Spawn at right
                    Instantiate(_AsteroidPrefab, new Vector3(_xR + 1f, Random.Range(_yB, _yT), 0f), Quaternion.identity);
                    break;
            }
        }
        else
            _loseText.gameObject.SetActive(true);
    }

    void CheckAspectRatio()
    {
        _xL = _mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        _xR = _mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        _yB = _mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
        _yT = _mainCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;
    }

public void UpdateScore(float scoreValue)
    {
        _score += scoreValue;
        _scoreText.text = "SCORE\n" + _score;
    }

    public void UpdatePlanetLives(int lifeValue)
    {
        if (lifeValue < 0f && _planetLives > 0)
        {
            _lifeImages[_planetLives - 1].gameObject.SetActive(false);
        }
        
        _planetLives += lifeValue;
    }

    public void UpdateAlienLives(int lifeValue)
    {
        if (lifeValue < 0f && _alienLives > 0)
        {
            _alienHeadImages[_alienLives - 1].gameObject.SetActive(false);
        }
        
        _alienLives += lifeValue;
    }
    
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}