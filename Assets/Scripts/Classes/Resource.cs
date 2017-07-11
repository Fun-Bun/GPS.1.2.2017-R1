[System.Serializable]
public class Resource
{
	public int value;
	public int max;

	public float GetPercent()
	{
		return (float)value / (float)max;
	}


	public int Extend(int amount)
	{
		value += amount;
		if(value > max)
		{
			int remainder = value - max;
			value = max;
			return remainder;
		}
		else return 0;
	}

	public int Reduce(int amount)
	{
		value -= amount;
		if(value < 0)
		{
			int remainder = -value;
			value = 0;
			return remainder;
		}
		else return 0;
	}
}
