using UnityEngine;

public class GameLevel : MonoBehaviour {
    [SerializeField] private int levelNumber;
    [SerializeField] private Transform landerStartPositionTransform;

    public int GetLevelNumber() {
        return levelNumber;
    }

    public Vector3 GetLanderStartPosition() {
        return landerStartPositionTransform.position;
    }

}
