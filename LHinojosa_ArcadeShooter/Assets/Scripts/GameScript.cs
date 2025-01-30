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

    private float _xL = -10f;
    private float _xR = 10f;
    private float _yT = 7f;
    private float _yB = -7f;

    [SerializeField] TextMeshProUGUI _scoreText;
    [SerializeField] TextMeshProUGUI _loseText;

    [SerializeField] private GameObject _AsteroidPrefab;

    [SerializeField] private Image[] _lifeImages;
    [SerializeField] private Image[] _alienHeadImages;
    
    public AudioSource _audioSource;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
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
        if (_planetLives > 0 && _alienLives > 0)
        {
            int chance = Random.Range(0, 4);
            switch (chance)
            {
                case 0: //Spawn at top
                    Instantiate(_AsteroidPrefab, new Vector3(Random.Range(_xL, _xR), _yT, 0f), Quaternion.identity);
                    break;
                case 1: //Spawn at bottom
                    Instantiate(_AsteroidPrefab, new Vector3(Random.Range(_xL, _xR), _yB, 0f), Quaternion.identity);
                    break;
                case 2: //Spawn at left
                    Instantiate(_AsteroidPrefab, new Vector3(_xL, Random.Range(_yB, _yT), 0f), Quaternion.identity);
                    break;
                default: //Spawn at right
                    Instantiate(_AsteroidPrefab, new Vector3(_xR, Random.Range(_yB, _yT), 0f), Quaternion.identity);
                    break;
            }
        }
        else
            _loseText.gameObject.SetActive(true);
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