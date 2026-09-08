using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour {


    [SerializeField] private Button mainMenuButton;
    [SerializeField] private TextMeshProUGUI scoreTextMesh;

    private void Awake() {
        mainMenuButton.onClick.AddListener(() => {
            SceneLoader.LoadScene(SceneLoader.Scene.MainMenuScene);
        });
    }

    private void Start() {
        scoreTextMesh.text = "FINAL SCORE: " + GameManager.Instance.GetTotalScore().ToString();

        mainMenuButton.Select();
    } 

}
