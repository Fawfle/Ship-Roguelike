using UnityEngine;


/// <summary>
/// abstract class for objects that move in a specified direction
/// </summary>
public abstract class MoveInDirectionBase : MonoBehaviour
{
    public abstract void SetDirection(Vector2 direction);
    public abstract void SetDirection(float direction);
}
