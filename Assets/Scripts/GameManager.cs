using NUnit.Framework;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {


    public static GameManager Instance { get; private set; }
    private static int levelNumber = 1;
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

        loadCurrentLevel();
    }
    private void Lander_OnStateChanged(object sender, Lander.OnStateChangedEventArgs e) {
        isTimerActive = e.state == Lander.State.Normal;

        if (e.state == Lander.State.Normal) {
            cinemachineCamera.Target.TrackingTarget = Lander.Instance.transform;
            CinemachineCameraZoom2D.Instance.SetNormalOrthographicSize();
            
        }
    }

    private void loadCurrentLevel() {
        foreach (GameLevel gameLevel in gameLevelList) {
            if (gameLevel.GetLevelNumber() == levelNumber) {
                GameLevel spawnedGameLevel = Instantiate(gameLevel, Vector3.zero, Quaternion.identity);
                Lander.Instance.transform.position = spawnedGameLevel.GetLanderStartPosition();
                cinemachineCamera.Target.TrackingTarget = spawnedGameLevel.GetCameraStartTargetTransform();
                CinemachineCameraZoom2D.Instance.SetTargetOrthoGraphicSize(spawnedGameLevel.GetZoomedOutOrthographicSize());
            }
        }
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

    public void GoToNextLevel() {
        levelNumber++;
        SceneManager.LoadScene(0);
    }

    public void RetryLevel() {
        SceneManager.LoadScene(0);
    }

    public int GetLevelNumber() {
        return levelNumber;
    }

}

//Test for GitHub Fix on Visual Studio