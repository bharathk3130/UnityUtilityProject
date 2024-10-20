using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "ScriptableObjects/Input Reader")]
public class InputReader : ScriptableObject, PlayerInputActions.IPlayerActions
{
    PlayerInputActions _inputActions;
    
    public Vector2 Move => _inputActions.Player.Move.ReadValue<Vector2>();

    void OnEnable()
    {
        if (_inputActions == null)
        {
            _inputActions = new PlayerInputActions();
            _inputActions.Player.SetCallbacks(this);
        }
    }

    public void Enable() => _inputActions.Enable();
    public void Disable() => _inputActions.Disable();

    public void OnMove(InputAction.CallbackContext context) { }

    public void OnLook(InputAction.CallbackContext context) { }

    public void OnFire(InputAction.CallbackContext context) { }
    public void OnMouseControlCamera(InputAction.CallbackContext context) { }

    public void OnRun(InputAction.CallbackContext context) { }

    public void OnJump(InputAction.CallbackContext context) { }
}