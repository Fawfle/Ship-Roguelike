using UnityEngine;

public static class WorldBounds
{
    public static bool IsInBounds(Vector2 position, float padding=0f)
    {
        float maxDistance = Camera.main.orthographicSize + padding;
        if (position.x > maxDistance) return false;
        if (position.x < -maxDistance) return false;
		if (position.y > maxDistance) return false;
		if (position.y < -maxDistance) return false;
        return true;
	}
}
