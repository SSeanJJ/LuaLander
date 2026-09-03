using UnityEngine;

public class GameLevel : MonoBehaviour {
    [SerializeField] private int levelNumber;
    [SerializeField] private Transform landerStartPositionTransform;
    [SerializeField] private Transform cameraStartTargetTransform;
    [SerializeField] private float zoomedOutOrthoraphicSize;


    public int GetLevelNumber() {
        return levelNumber;
    }

    public Vector3 GetLanderStartPosition() {
        return landerStartPositionTransform.position;
    }
    public Transform GetCameraStartTargetTransform() {
        return cameraStartTargetTransform;
    }

    public float GetZoomedOutOrthographicSize() {
        return zoomedOutOrthoraphicSize;
    }

}
