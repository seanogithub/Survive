using UnityEngine;
using System.Collections;

public class InventoryManager : MonoBehaviour {

	public bool Init = false;
	public GameObject[] InventorySourceArray;
	public GameObject[] SpawnPointArray;
	public GameObject[] InventoryArray;

	// Use this for initialization
	void Start () 
	{
		InventoryArray = new GameObject[SpawnPointArray.Length];
	}

	public void Initialize()
	{
		Init = false;
		for (int i = 0; i < InventorySourceArray.Length; i++)
		{
			if(InventorySourceArray[i] != null)
			{
				//int RandSpawnPoint = Random.Range(0, (SpawnPointArray.Length - 1));
				//Vector3 SpawnPosition = SpawnPointArray[RandSpawnPoint].transform.position;
				Vector3 SpawnPosition = SpawnPointArray[i].transform.position;
				float YRotation = Random.Range(0f, 360.0f);
				Vector3 SpawnRotation = new Vector3 (0,YRotation ,0);
				GameObject InventoryItem = InventorySourceArray[i];
				InventoryArray[i] = Instantiate(InventoryItem);
				InventoryArray[i].transform.position = SpawnPosition;
				InventoryArray[i].transform.Rotate(SpawnRotation);

				//Debug.Log(InventoryItem.name);
			}
		}
	}


	// Update is called once per frame
	void Update () {
		if(Init) Initialize();
	}

	public void DestroyAllInventory()
	{
		for (int i = 0; i < InventoryArray.Length; i++)
		{
			Destroy(InventoryArray[i]);
		}
	}
}
