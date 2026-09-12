using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {


    public static GameManager Instance { get; private set; }
    private static int levelNumber = 1;
    private static int totalScore = 0;
    public static void ResetStaticData() {
        levelNumber = 1;
        totalScore = 0;
    }

    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;


    [SerializeField] private List<GameLevel> gameLevelList;

    [SerializeField] private CinemachineCamera cinemachineCamera;

    
    private int score;
    private float time;
    private bool isTimerActive;

    private void Awake() {
        Instance = this;
    }
    private void Start() {
        Lander.Instance.OnCoinPickup += Lander_OnCoinPickup;
        Lander.Instance.OnLanded += Lander_OnLanded;
        Lander.Instance.OnStateChanged += Lander_OnStateChanged;

        GameInput.Instance.OnMenuButtonPressed += Game_OnMenuButtonPressed;
        loadCurrentLevel();
    }

    private void Game_OnMenuButtonPressed(object sender, System.EventArgs e) {
        PauseUnpauseGame();
    }
    private void Lander_OnStateChanged(object sender, Lander.OnStateChangedEventArgs e) {
        isTimerActive = e.state == Lander.State.Normal;

        if (e.state == Lander.State.Normal) {
            cinemachineCamera.Target.TrackingTarget = Lander.Instance.transform;
            CinemachineCameraZoom2D.Instance.SetNormalOrthographicSize();
            
        }
    }

    private void loadCurrentLevel() {
       GameLevel gameLevel = GetGameLevel();
       GameLevel spawnedGameLevel = Instantiate(gameLevel, Vector3.zero, Quaternion.identity);
       Lander.Instance.transform.position = spawnedGameLevel.GetLanderStartPosition();
       cinemachineCamera.Target.TrackingTarget = spawnedGameLevel.GetCameraStartTargetTransform();
       CinemachineCameraZoom2D.Instance.SetTargetOrthoGraphicSize(spawnedGameLevel.GetZoomedOutOrthographicSize());

    }

    private GameLevel GetGameLevel() {
        foreach (GameLevel gameLevel in gameLevelList) {
            if (gameLevel.GetLevelNumber() == levelNumber) {
                return gameLevel;
            }
        }
        return null; 
    } 

    private void Update() {
        if (isTimerActive) {
            time += Time.deltaTime;
        }
    }

    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e) {
        AddScore(e.score);
    }

    private void Lander_OnCoinPickup(object sender, System.EventArgs e) {
        AddScore(500);
    }

    public void AddScore(int addScoreAmount) {
        score += addScoreAmount;
        Debug.Log(score);
    }

    public int GetScore() {
        return score;
    }

    public float GetTime() {
        return time;
    }

    public int GetTotalScore() {
        return totalScore;
    }

    public void GoToNextLevel() {
        levelNumber++;
        totalScore += score;

        if (GetGameLevel() == null) {
            //No More Levels
            SceneLoader.LoadScene(SceneLoader.Scene.GameOverScene);
        } else {
            // Still have more levels
            SceneLoader.LoadScene(SceneLoader.Scene.GameScene);
        }
    } 

    public void RetryLevel() {
        SceneLoader.LoadScene(SceneLoader.Scene.GameScene);
    }

    public int GetLevelNumber() {
        return levelNumber;
    }

    public void PauseUnpauseGame() {
        if (Time.timeScale == 1f) {
            PauseGame();
        } else {
            UnpauseGame();
        }
    } 

    public void PauseGame() {
        Time.timeScale = 0f;
        OnGamePaused?.Invoke(this, EventArgs.Empty);
    }


    public void UnpauseGame() {
        Time.timeScale = 1f;
        OnGameUnpaused?.Invoke(this, EventArgs.Empty);
    }

    public static void setLevelNumber(int levelNumber) {
        GameManager.levelNumber = levelNumber;
    }

}

