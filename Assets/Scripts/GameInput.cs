using System.Runtime.InteropServices;
using UnityEngine;

public class GameInput : MonoBehaviour {

    public static GameInput Instance { get; private set; }

    private InputActions inputActions;
    private void Awake() {
        Instance = this; 

        inputActions = new InputActions();
        inputActions.Enable();
    }
    private void OnDestroy() {
        inputActions.Disable();
    }
    public bool IsUpActionPressed() {
        return inputActions.Player.LanderUp.IsPressed();
    }

    public bool IsLeftActionPressed() {
        return inputActions.Player.LanderLeft.IsPressed();
    }

    public bool IsRightActionPressed() {
        return inputActions.Player.LanderRight.IsPressed();
    }

    public Vector2 GetMovementInputVector2() {
        return inputActions.Player.Movement.ReadValue<Vector2>();
    }
}