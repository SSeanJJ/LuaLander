using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LandedUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI titleTextMesh;
    [SerializeField] private TextMeshProUGUI statsTextMesh;
    [SerializeField] private TextMeshProUGUI nextButtonTextMesh;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button retryButton;


    private Action nextButtonClickAction;

    private void Awake() {
        nextButton.onClick.AddListener(() => {
            nextButtonClickAction();
        });

        retryButton.onClick.AddListener(() => { 
            GameManager.Instance.RetryLevel();
        }); 
    }


    private void Start() {
        Lander.Instance.OnLanded += Lander_OnLanded;

        Hide(); // We Hide the landing screen because it would be sitting on top of the game.
    }

    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e) {
        if (e.landingType == Lander.LandingType.Success) {
            titleTextMesh.text = "SUCCESSFUL LANDING!";
            nextButtonTextMesh.text = "CONTINUE"; // changes the next button text mesh to continue on successful landing.
            nextButtonClickAction = GameManager.Instance.GoToNextLevel;
            retryButton.gameObject.SetActive(true); // Show Retry Button on Successful Landing.

        } else {
            titleTextMesh.text = "<color=#ff0000>CRASH!</color>";
            nextButtonTextMesh.text = "RETRY"; // changes the next button text mesh to retry the level on crash.
            nextButtonClickAction = GameManager.Instance.RetryLevel;
            retryButton.gameObject.SetActive(false); // Hide Retry Button on Crash.
        }

        statsTextMesh.text =
            Mathf.Round(e.landingSpeed * 2f) + "\n" +
            Mathf.Round(e.dotVector * 100f) + "\n" +
            "x" + e.scoreMultiplier + "\n" +
            e.score;

        Show();

    }
    private void Show() {
        gameObject.SetActive(true);
        nextButton.Select();
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
