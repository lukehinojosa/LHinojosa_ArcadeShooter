using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScript : MonoBehaviour
{
    private static int _score;
    public static int Score
    {
        get
        {
            return _score;
        }
        set
        {
            if (value == _score)
                return;
            
            _score = value;
            ScoreChanged?.Invoke(_score);
        }
    }
    
    private static int _planetLives = 3;
    public static int PlanetLives
    {
        get
        {
            return _planetLives;
        }
        set
        {
            if (value == _planetLives || value < 0f)
                return;
            
            _planetLives = value;
            
            if (value < 3)
                PlanetLivesChanged?.Invoke(_planetLives, false);
        }
    }
    
    private static int _alienLives = 3;
    public static int AlienLives
    {
        get
        {
            return _alienLives;
        }
        set
        {
            if (value == _alienLives || value < 0f)
                return;
            
            _alienLives = value;
            
            if (value < 3)
                AlienLivesChanged?.Invoke(_alienLives, false);
        }
    }

    private static float[] _cameraCorners = new float[0];//xL, xR, yB, yT
    public static float[] CameraCorners
    {
        get
        {
            return _cameraCorners;
        }
        set
        {
            if (_cameraCorners.SequenceEqual(value)) 
                return;

            _cameraCorners = value;
        }
    }

    private static bool _spawnEnemies = true;
    public static bool SpawnEnemies
    {
        get
        {
            return _spawnEnemies;
        }
        set
        {
            _spawnEnemies = value;
        }
    }

    public static event Action<int> ScoreChanged;
    public static event Action<int, bool> PlanetLivesChanged;
    public static event Action<int, bool> AlienLivesChanged;
    
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void StartGame()
    {
        Score = 0;
        PlanetLives = 3;
        AlienLives = 3;
        SpawnEnemies = true;
        
        SceneManager.LoadScene("MainScene");
    }
    
    public static IEnumerator EndScreen()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("EndScreen");
    }
    
    public void MainMenu()
    {
        Debug.Log("Test");
        SceneManager.LoadScene("StartScene");
    }
}