using UnityEngine;
using UnityEngine.UI;

public class LevelSelectUI : MonoBehaviour
{
    [SerializeField] private Button MainMenuButton;
    [SerializeField] private Button LevelOneButton;
    [SerializeField] private Button LevelTwoButton;



    private void Awake() {
        MainMenuButton.onClick.AddListener(() => {
            SceneLoader.LoadScene(SceneLoader.Scene.MainMenuScene);
        });

        LevelOneButton.onClick.AddListener(() => { 
        GameManager.setLevelNumber(1);
            SceneLoader.LoadScene(SceneLoader.Scene.GameScene);
        });

        LevelTwoButton.onClick.AddListener(() => {
        GameManager.setLevelNumber(2);
            SceneLoader.LoadScene(SceneLoader.Scene.GameScene);
        });

    }

}
