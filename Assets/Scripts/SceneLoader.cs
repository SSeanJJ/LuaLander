using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader {

    public enum Scene { 
        MainMenuScene,
        GameScene,
        GameOverScene,
        LevelSelectScene,
    } 

    public static void LoadScene(Scene scene) {
        Time.timeScale = 1f;
        SceneManager.LoadScene(scene.ToString());
    } 

}
