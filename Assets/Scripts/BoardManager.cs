using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {
	[Serializable]
	public class Count
	{
		public int minimum, maximum;

		public Count(int min, int max){
			minimum = min;
			maximum = max;
		}

		public int RandomInRange{
			get{
				return Random.Range(minimum, maximum + 1);
			}
		}
	}

	public int rows, columns = 8;
	public Count wallCount = new Count(5,9);
	public Count foodCount = new Count(1,5);

	public GameObject exit;

	public GameObject[] floorTiles, wallTiles, foodTiles, enemyTiles, outerWallTiles;

	private Transform boardHolder;
	private List<Vector3> gridPositions = new List<Vector3>();

	void Start(){
		SetupScene(8);
	}

	void SetupScene(int level){
		InitializePositions();
		BoardSetup();
		LayoutObjectsAtRandoom(foodTiles, foodCount);
		LayoutObjectsAtRandoom(wallTiles, wallCount);
		var enemiesCount = (int)Mathf.Log(level, 2f);
		LayoutObjectsAtRandoom(enemyTiles, new Count(enemiesCount, enemiesCount));
		Instantiate(exit, new Vector3(rows - 1, columns - 1, 0f), Quaternion.identity);
	}


	void InitializePositions(){
		gridPositions.Clear();

		for(int i = 1; i < columns - 1; i++)
			for(int j = 1; j < rows - 1; j++)
				gridPositions.Add(new Vector3(i, j, 0f));
	}

	void BoardSetup(){
		boardHolder = new GameObject("Board").transform;

		for(int i = -1; i < columns + 1; i++){
			for(int j = -1; j < rows + 1; j++){
				GameObject prefab;
				if(i == -1 || i == columns || j == -1 || j == rows)
					prefab = wallTiles[Random.Range(0, wallTiles.Length)];
				else
					prefab = floorTiles[Random.Range(0, floorTiles.Length)];

				var instance = Instantiate(prefab, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;
				instance.transform.parent = boardHolder;
			}
		}
	}

	private void LayoutObjectsAtRandoom(GameObject[] layoutObjects, Count counter){
		var currentCount = counter.RandomInRange;

		for(int i = 0; i < currentCount; i++){
			var toInstantiate = layoutObjects[Random.Range(0, layoutObjects.Length)];
			Instantiate(toInstantiate, RandomUniqPosition, Quaternion.identity);
		}
	}

	private Vector3 RandomUniqPosition{
		get{
			var randomIndex = Random.Range(0, gridPositions.Count);
			var position = gridPositions[randomIndex];
			gridPositions.RemoveAt(randomIndex);
			return position;
		}
	}

}
