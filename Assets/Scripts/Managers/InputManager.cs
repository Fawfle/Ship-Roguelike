using UnityEngine;
using UnityEngine.InputSystem;

public static class InputManager
{
    private static readonly string PLAYER_MOVE_KEY = "Move";
    private static readonly string PLAYER_DASH_KEY = "Dash";
    private static readonly string PLAYER_SHOOT_KEY = "Shoot";
    private static readonly string PLAYER_SNEAK_KEY = "Sneak";

    public static Vector2 MoveInput => InputSystem.actions.FindAction(PLAYER_MOVE_KEY).ReadValue<Vector2>();

    public static InputAction Dash => InputSystem.actions.FindAction(PLAYER_DASH_KEY);

    public static InputAction Shoot => InputSystem.actions.FindAction(PLAYER_SHOOT_KEY);
    public static InputAction Sneak => InputSystem.actions.FindAction(PLAYER_SNEAK_KEY);
}
