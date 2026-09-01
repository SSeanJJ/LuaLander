using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LandedUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleTextMesh;
    [SerializeField] private TextMeshProUGUI statsTextMesh;
    [SerializeField] private TextMeshProUGUI nextButtonTextMesh;
    [SerializeField] private Button nextButton;


    private Action nextButtonClickAction;

    private void Awake()
    {
        nextButton.onClick.AddListener(() =>
        {
            nextButtonClickAction();
        });
    }


    private void Start() {
        Lander.Instance.OnLanded += Lander_OnLanded;

        Hide();
    }

    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        if (e.landingType == Lander.LandingType.Success) {
            titleTextMesh.text = "SUCCESSFUL LANDING!";
            nextButtonTextMesh.text = "CONTINUE";
            nextButtonClickAction = GameManager.Instance.GoToNextLevel; 
        } else { 
            titleTextMesh.text = "<color=#ff0000>CRASH!</color>";
            nextButtonTextMesh.text = "RETRY";
            nextButtonClickAction = GameManager.Instance.RetryLevel;
        }

        statsTextMesh.text =
            Mathf.Round(e.landingSpeed *2f) + "\n" +
            Mathf.Round(e.dotVector * 100f) + "\n" +
            "x" + e.scoreMultiplier + "\n" +
            e.score;

        Show();

    }
    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
