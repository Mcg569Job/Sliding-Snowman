using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameStatus { Null, Normal, GameOver, Pause }

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager instance = null;
    private void Awake()
    {
        if (instance == null) instance = this;
    }
    #endregion

    [SerializeField] private SwipeControl swipe;
    [SerializeField] private Player player;
    [SerializeField] private CameraFollow cameraFollow;
    [HideInInspector] public GameStatus gameStatus;
    
    public bool gameStatusIsNormal() { return gameStatus == GameStatus.Normal; }

    private float _score, _bestScore;

    private int _coin;

    private void Start()
    {
        Menu();
        GetData();
        AddCoin(0);
    }
    private void GetData()
    {
        _coin = PlayerPrefs.GetInt("coin");
        _bestScore = PlayerPrefs.GetFloat("best");
    }

    public void Menu()
    {
        UI_Manager.instance.Menu(true);
        player.ResetPlayer();
        gameStatus = GameStatus.Null;
        cameraFollow.ResetCamera();
    }

    public void Play()
    {
        TinySauce.OnGameStarted();
        AddScore(0);
        WayManager.instance.ResetWays();
        UI_Manager.instance.Menu(false);
        gameStatus = GameStatus.Normal;
    }
    public void Resume()
    {
        WayManager.instance.ResetWays(false);
        cameraFollow.ResetCamera();
        UI_Manager.instance.Menu(false);
        player.ResetPlayer(false);
        gameStatus = GameStatus.Normal;
    }
    public void GameOver()
    {
        TinySauce.OnGameFinished(levelNumber: "score", _score);
        AudioManager.instance.PlaySound(AT.GameOver);
        AudioManager.instance.Vibrate();
        gameStatus = GameStatus.GameOver;
        UI_Manager.instance.GameOver(true);
        cameraFollow.ShakeCamera(.5f, .5f);
        swipe.Hold(false);
    }

    public void SetSensitivity() => swipe.SetSensitivity();


    public float GetScore() { return _score; }
    public void AddScore(float value)
    {
        _score += value;
        if (value == 0) _score = 0;

        if ((int)_score % 750 == 0)
        {
            player.UpdateSpeedByScore();
            WayManager.instance.UpdateLevelByScore();
        }

        if (_score > _bestScore)
        {
            _bestScore = _score;
            PlayerPrefs.SetFloat("best", _bestScore);
        }

        UI_Manager.instance.UpdateScore(_score);
    }

    public void AddCoin(int amount)
    {
        if (amount > 0)
        {
            AudioManager.instance.PlaySound(AT.Coin);
            AudioManager.instance.Vibrate();
        }
        _coin += amount;
        PlayerPrefs.SetInt("coin", _coin);
        UI_Manager.instance.UpdateCoin(_coin);
    }
}
