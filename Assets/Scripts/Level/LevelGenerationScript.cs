using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelCollection
{
	public string name;
	public float spawnRate;
	public List<GameObject> prefabs;
}

public class LevelGenerationScript : MonoBehaviour
{
	public enum LevelType
	{
		Easy = 0,
		Normal,
		Hard,
		DeadEnd,
		Total
	}

	public int roomSize;
	public List<LevelCollection> levelPrefabs;
	public List<GameObject> spawnedLevels;

	// Use this for initialization
	void Start ()
	{
		for(int i = 0; i < roomSize; i++)
		{
			float random = Random.Range(0.0f, 1.0f);
			float lowLimit;
			float highLimit = 0;

			Debug.Log(random);

			for(int j = 0; j < levelPrefabs.Count; j++)
			{
				lowLimit = highLimit;
				highLimit += levelPrefabs[j].spawnRate;
				if(random >= lowLimit && random <= highLimit)
				{
					Debug.Log(levelPrefabs[j].name);
//					Transform l_obj = spawnPoints[Random.Range(0, spawnPoints.Length)];
//					Instantiate(enemies[l_enemy].enemy, l_obj.position, l_obj.rotation);
//					currentEnemies += 1;
//					enemiesSpawnedThisWave += 1;
					break;
				}
			}
		}
	}
}