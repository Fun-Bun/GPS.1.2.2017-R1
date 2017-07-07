[System.Serializable]
public class Depletable
{
	public Resource resource;

	public int depleteAmount;
	public float depleteDuration;
	public float depleteRate;
	private float depleteTimer;

	public void Update(float time)
	{
		depleteTimer += time * depleteRate;
		if(depleteTimer >= depleteDuration)
		{
			resource.Reduce(depleteAmount);
			depleteTimer = 0f;
		}
	}
}