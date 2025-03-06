using UnityEngine;

[System.Serializable]
public struct UpdatableFloat
{
    public float value;

    public float add;
    public float multiplier;

    public bool updatable;

    public UpdatableFloat(float val)
    {
        value = val;
        add = 0f;
        multiplier = 1f;

        updatable = false;
    }

	public UpdatableFloat(float val, float a, float multi)
	{
		value = val;
		add = a;
		multiplier = multi;

        updatable = true;
	}

    public void Update(float deltaTime)
    {
        if (!updatable) return;

        value += add * deltaTime;
        value = Mathf.Lerp(value, value * multiplier, deltaTime);
    }

    // operator overloading is based
    public static implicit operator UpdatableFloat(float v) => new UpdatableFloat(v);
}