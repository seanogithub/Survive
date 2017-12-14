using UnityEngine;
using System.Collections;

public class BaddieManager : MonoBehaviour {

	public bool Init = false;
	public GameObject[] BaddieSourceArray;
	public GameObject[] SpawnPointArray;
	public GameObject[] BaddieArray;
	public int IDIndex = 1;
	public int NumberOfBaddiesToChange = 1;
	public int MinNumberOfBaddies = 0;
	public int MaxNumberOfBaddies = 6;
	public float NoBaddieTimer = 5.0f; 
	public float RandomExtraBaddiePercentage = 30f;


	// Use this for initialization
	void Start () {
		Init = true;
	}

	public void Initialize()
	{
		Init = false;
	}

	// Update is called once per frame
	void Update () {
		if(Init) Initialize();

		CheckForNoBaddies();
		EvaluateNumberOfBaddies();
	}

	void EvaluateNumberOfBaddies()
	{

		// decide if there needs to be more or less baddies
		// this will make a sine wave of increasing and decreasing number of baddies

		bool NullBaddies = AreThereNullBaddies();
		if(NullBaddies)
		{
			if(BaddieArray.Length >= MaxNumberOfBaddies)
			{
				NumberOfBaddiesToChange = -1;
			}
			if(BaddieArray.Length <= MinNumberOfBaddies)
			{
				NumberOfBaddiesToChange = 1;
			}

			if(NumberOfBaddiesToChange > 0)
			{
				// remove any null baddies
				RemoveNullBaddies();

				// add a baddie to replace the null baddie
				AddNullBaddie();
				SpawnBaddie(BaddieArray.Length-1);

				// increase the number of baddies by 1
				AddNullBaddie();
				SpawnBaddie(BaddieArray.Length-1);

				//randomly add an extra baddie
				float RandomExtraBaddie = Random.Range(0, 100);
				if(RandomExtraBaddie < RandomExtraBaddiePercentage)
				{
					// add an extra baddie
					AddNullBaddie();
					SpawnBaddie(BaddieArray.Length-1);
					Debug.Log("Spawning extra baddie!");
				}
			}
			else
			{
				RemoveNullBaddies();
			}
		}
	}

	void CheckForNoBaddies()
	{
		// catch for if there are no baddies based on NoBaddieTimer
		if(BaddieArray.Length == 0)
		{
			NoBaddieTimer -= Time.deltaTime;
		}
		if(NoBaddieTimer < 0)
		{
			NoBaddieTimer = 5.0f; 
			AddNullBaddie();
			SpawnBaddie(BaddieArray.Length-1);
			Debug.Log("No baddies alive. Baddie was spawned.");
		}
	}

	bool AreThereNullBaddies()
	{
		if(BaddieArray.Length < 1)
		{
			return true;
		}

		for(int i = 0; i < BaddieArray.Length; i ++)
		{
			if (BaddieArray[i] == null)
			{
				return true;
			}
		}
		return false;
	}

	void AddNullBaddie()
	{
		int BaddieCount = 0;

		// count the number of null baddies
		for(int i = 0; i < BaddieArray.Length; i ++)
		{
			if (BaddieArray[i] != null)
			{
				BaddieCount++;
			}
		}

		// copy the baddies to the new array
		GameObject[] NewBaddieArray = new GameObject[BaddieCount+1];
		for(int i = 0; i < BaddieArray.Length; i ++)
		{
			if (BaddieArray[i] != null)
			{
				NewBaddieArray[i] = BaddieArray[i];
			}
		}

		// copy the new array
		BaddieArray = NewBaddieArray;
	}

	void RemoveNullBaddies()
	{
		int BaddieCount = 0;

		// count the number of null baddies
		for(int i = 0; i < BaddieArray.Length; i ++)
		{
			if (BaddieArray[i] != null)
			{
				BaddieCount++;
			}
		}

		// copy the baddies to the new array
		GameObject[] NewBaddieArray = new GameObject[BaddieCount];
		int NewBaddieIndex = 0;
		for(int i = 0; i < BaddieArray.Length; i ++)
		{
			if (BaddieArray[i] != null)
			{
				NewBaddieArray[NewBaddieIndex] = BaddieArray[i];
				NewBaddieIndex++;
			}
		}

		// copy the new array
		BaddieArray = NewBaddieArray;
	}

	void SpawnBaddie(int BaddieIndex)
	{
		int RandSpawnPoint = Random.Range(0, (SpawnPointArray.Length - 0));
		int RandSpawnBaddie = Random.Range(0, (BaddieSourceArray.Length - 0));
		Vector3 SpawnPosition = SpawnPointArray[RandSpawnPoint].transform.position;
		float YRotation = Random.Range(0f, 360.0f);
		Vector3 SpawnRotation = new Vector3 (0,YRotation ,0);
		GameObject Baddie = BaddieSourceArray[RandSpawnBaddie];
		BaddieArray[BaddieIndex] = Instantiate(Baddie);
		BaddieArray[BaddieIndex].transform.position = SpawnPosition;
		BaddieArray[BaddieIndex].transform.Rotate(SpawnRotation);
		BaddieArray[BaddieIndex].GetComponent<Baddie>().ID = IDIndex;
		IDIndex++;
		//Debug.Log("SpawnBaddie");
		//Debug.Log(SpawnPosition);
		//Debug.Log(Baddie);
	}

	public void DestroyAllBaddies()
	{
		for (int i = 0; i < BaddieArray.Length; i++)
		{
			Destroy(BaddieArray[i]);
		}
	}
}
