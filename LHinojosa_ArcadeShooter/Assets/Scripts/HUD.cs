using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _scoreText;
    [SerializeField] private Image[] _lifeImages;
    [SerializeField] private Image[] _alienHeadImages;
    [SerializeField] private GameObject _AsteroidPrefab;
    
    private float _spawnDelay = 2.5f;
    public AudioSource _audioSource;
    private Camera _mainCamera;
    
    private void OnEnable()
    {
        GameScript.ScoreChanged += GameScript_ScoreChanged;
        GameScript.PlanetLivesChanged += GameScript_PlanetLivesChanged;
        GameScript.AlienLivesChanged += GameScript_AlienLivesChanged;
    }

    private void OnDisable()
    {
        GameScript.ScoreChanged -= GameScript_ScoreChanged;
        GameScript.PlanetLivesChanged -= GameScript_PlanetLivesChanged;
        GameScript.AlienLivesChanged -= GameScript_AlienLivesChanged;
    }
    
    void Start()
    {
        _mainCamera = Camera.main;
        _audioSource = GetComponent<AudioSource>();

        GameScript_ScoreChanged(GameScript.Score);

        switch (GameScript.PlanetLives)
        {
            case 3:
                GameScript_PlanetLivesChanged(2, true);
                GameScript_PlanetLivesChanged(1, true);
                GameScript_PlanetLivesChanged(0, true);
                break;
            case 2:
                GameScript_PlanetLivesChanged(2, false);
                GameScript_PlanetLivesChanged(1, true);
                GameScript_PlanetLivesChanged(0, true);
                break;
            case 1:
                GameScript_PlanetLivesChanged(2, false);
                GameScript_PlanetLivesChanged(1, false);
                GameScript_PlanetLivesChanged(0, true);
                break;
            case 0:
                GameScript_PlanetLivesChanged(2, false);
                GameScript_PlanetLivesChanged(1, false);
                GameScript_PlanetLivesChanged(0, false);
                break;
        }
        
        switch (GameScript.AlienLives)
        {
            case 3:
                GameScript_AlienLivesChanged(2, true);
                GameScript_AlienLivesChanged(1, true);
                GameScript_AlienLivesChanged(0, true);
                break;
            case 2:
                GameScript_AlienLivesChanged(2, false);
                GameScript_AlienLivesChanged(1, true);
                GameScript_AlienLivesChanged(0, true);
                break;
            case 1:
                GameScript_AlienLivesChanged(2, false);
                GameScript_AlienLivesChanged(1, false);
                GameScript_AlienLivesChanged(0, true);
                break;
            case 0:
                GameScript_AlienLivesChanged(2, false);
                GameScript_AlienLivesChanged(1, false);
                GameScript_AlienLivesChanged(0, false);
                break;
        }
        
        StartCoroutine(Co_SpawnAsteroid());
    }

    private void GameScript_ScoreChanged(int score)
    {
        _scoreText.text = "SCORE\n" + score;
    }

    private void GameScript_PlanetLivesChanged(int lifeValue, bool isActive)
    {
        _lifeImages[lifeValue].gameObject.SetActive(isActive);
        
        if (!isActive && lifeValue == 0)
            GameScript.SpawnEnemies = false;
    }

    private void GameScript_AlienLivesChanged(int lifeValue, bool isActive)
    {
        _alienHeadImages[lifeValue].gameObject.SetActive(isActive);
        
        if (!isActive && lifeValue == 0)
            GameScript.SpawnEnemies = false;
    }
    
    private IEnumerator Co_SpawnAsteroid()
    {
        while (GameScript.SpawnEnemies)
        {
            yield return new WaitForSeconds(_spawnDelay);

            int chance = Random.Range(0, 4);
            switch (chance)
            {
                case 0: //Spawn at top
                    Instantiate(_AsteroidPrefab,
                        new Vector3(Random.Range(GameScript.CameraCorners[0], GameScript.CameraCorners[1]),
                            GameScript.CameraCorners[3] + 1f, 0f), Quaternion.identity);
                    break;
                case 1: //Spawn at bottom
                    Instantiate(_AsteroidPrefab,
                        new Vector3(Random.Range(GameScript.CameraCorners[0], GameScript.CameraCorners[1]),
                            GameScript.CameraCorners[2] - 1f, 0f), Quaternion.identity);
                    break;
                case 2: //Spawn at left
                    Instantiate(_AsteroidPrefab,
                        new Vector3(GameScript.CameraCorners[0] - 1f,
                            Random.Range(GameScript.CameraCorners[2], GameScript.CameraCorners[3]), 0f),
                        Quaternion.identity);
                    break;
                default: //Spawn at right
                    Instantiate(_AsteroidPrefab,
                        new Vector3(GameScript.CameraCorners[1] + 1f,
                            Random.Range(GameScript.CameraCorners[2], GameScript.CameraCorners[3]), 0f),
                        Quaternion.identity);
                    break;
            }
        }

        StartCoroutine(GameScript.EndScreen());
    }
}
