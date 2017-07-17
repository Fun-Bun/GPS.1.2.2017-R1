using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelCollection
{
	public string name;
	public int spawnSize;
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

	// Use this for initialization
	void Start ()
	{
		//BUGGGED!! LOOP CRASHES THE GAME!
//		List<int> roomsLeft = new List<int>();
//
//		for(int i = 0; i < (int)LevelType.Total; i++)
//		{
//			roomsLeft.Add(levelPrefabs[i].spawnSize);
//		}
//
//		int total = (int)LevelType.Total;
//
//		for(int i = 0; i < roomSize; i++)
//		{
//			int rand = Random.Range(0, total);
//
//			for(int j = rand; j < (int)LevelType.Total; i++)
//			{
//				if(roomsLeft[j] <= 0)
//					rand++;
//			}
//
//			if(rand >= (int)LevelType.Total)
//				break;
//
//			GameObject newLevel = Instantiate(levelPrefabs[rand].prefabs[Random.Range(0, levelPrefabs[rand].prefabs.Count)]);
//			roomsLeft[rand]--;
//
//			if(roomsLeft[rand] <= 0)
//				total--;
//		}
	}
}