using UnityEngine;

public static class MyUtils
{
	public static T[] GetComponents<T>(this GameObject[] objs) 
	{
		T[] res = new T[objs.Length];

		for (int i = 0; i < objs.Length; i++)
		{
			res[i] = objs[i].GetComponent<T>();
		}

		return res;
	}

	public static T[] GetComponents<T>(this MonoBehaviour[] bs)
	{
		T[] res = new T[bs.Length];

		for (int i = 0; i < bs.Length; i++)
		{
			res[i] = bs[i].gameObject.GetComponent<T>();
		}

		return res;
	}

	/// <summary>
	/// Creates a vector at the specified <param name="angle">
	/// </summary>
	/// <returns>the vector</returns>
	public static Vector2 Vector2FromAngle(float angle)
	{
		return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
	}

	/// <returns>the angle between the <paramref name="direction"/> vector and the right vector in degrees</returns>
	public static float ToAngle(this Vector2 direction)
	{
		return Vector2.SignedAngle(Vector2.right, direction);
	}
}
