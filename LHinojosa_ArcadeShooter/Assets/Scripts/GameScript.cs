using System;
using System.Linq;
using UnityEngine;

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
            PlanetLivesChanged?.Invoke(_planetLives);
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
            AlienLivesChanged?.Invoke(_alienLives);
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
    public static event Action<int> PlanetLivesChanged;
    public static event Action<int> AlienLivesChanged;
}