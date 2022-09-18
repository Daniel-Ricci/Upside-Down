using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int _level;
    [SerializeField] private Goal _upGoal;
    [SerializeField] private Goal _downGoal;
    private static LevelManager _instance;
    public bool controlsEnabled {get; private set;} = true;
    public int numCrystals;
    private bool _levelEnded = false;
    private float _timer;

    private const int _numLevels = 5;

    public static LevelManager Instance
    {
        get{return _instance;}
    }

    private void Awake()
    {
        _instance = this;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            _timer = Time.time;
        }
        else if(Input.GetKey(KeyCode.R))
        {
            if(Time.time - _timer > 2.5f)
            {
                _timer = float.PositiveInfinity;
                RestartLevel();
            }
        }
        else
        {
            _timer = float.PositiveInfinity;
        }

        if(!_levelEnded && _upGoal.goalCleared && _downGoal.goalCleared)
        {
            LevelCleared();
        }
    }

    public void GameOver()
    {
        _levelEnded = true;
        controlsEnabled = false;
        Invoke(nameof(RestartLevel), 2.0f);
    }

    private void LevelCleared()
    {
        _levelEnded = true;
        controlsEnabled = false;
        FindObjectOfType<UIManager>().OnEndLevel();
        Invoke(nameof(NextLevel), 3.5f);
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene("Level" + _level, LoadSceneMode.Single);
    }

    private void NextLevel()
    {
        if(_level == _numLevels)
        {
            SceneManager.LoadScene("Outro", LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadScene("Level" + (_level + 1), LoadSceneMode.Single);
        }
    }
}
