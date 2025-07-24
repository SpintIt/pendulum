using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class EventSystem : MonoBehaviour
{
    private GameInput _gameInput;

    public Vector2 MousePosition { get; private set; }
    public string LastKey { get; private set; }

    public event UnityAction OnExit;

    public event UnityAction<Vector2> OnClick;
    public event UnityAction<Vector2> OnClickUp;

    public event UnityAction<string> PressAnyKey;

    public void Init()
    {
        _gameInput = new();
        _gameInput.Enable();

        _gameInput.GamePlay.Position.performed += OnPosition;

        _gameInput.GamePlay.Click.performed += OnGamePlayClick;
        _gameInput.GamePlay.Click.canceled += OnGamePlayClickUp;

        _gameInput.GamePlay.PressKey.performed += PressKey;

#if UNITY_ANDROID || UNITY_EDITOR

        _gameInput.UI.Exit.performed += (InputAction.CallbackContext context) =>
        {
            OnExit?.Invoke();
        };
#endif
    }

    private void PressKey(InputAction.CallbackContext context)
    {
        PressAnyKey?.Invoke(context.control.displayName);
    }

    private void OnGamePlayClick(InputAction.CallbackContext context)
    {
        OnClick?.Invoke(MousePosition);
    }

    private void OnGamePlayClickUp(InputAction.CallbackContext context)
    {
        OnClickUp?.Invoke(MousePosition);
    }

    private void OnPosition(InputAction.CallbackContext context)
    {
        MousePosition = GetMousePosition(context);
    }

    private Vector3 GetMousePosition(InputAction.CallbackContext context)
        => context.ReadValue<Vector2>();
}
