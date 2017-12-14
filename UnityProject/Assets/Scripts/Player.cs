using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public GameObject UIManager;
	public GameObject PlayerManager;
	public GameObject UIInventoryStore;
	public GameObject ScreenBloodFX;
	public bool Init = false;
	public bool Alive = false;
	public int HitPoints = 100;
	public int PlayerTotalPoints = 0;
	public float TotalTime = 0;
	public int TimeEarned = 0;
	public float PointTimeMultiplier = 0.5f;
	public float CheckDifficultyTimer = 10f;
	private float DifficultyTimer = 10f;

	public int StatsShotsFired = 0;
	public int StatsBaddieHits = 0;
	public int StatsBaddiesKilled = 0;
	public int StatsMeleeHits = 0;
	public int StatsAccuracy = 0;
	public int StatsTotalPointsEarned = 0;


	void Awake()
	{
		UIManager = GameObject.Find("UI_Manager_Prefab");
		PlayerManager = GameObject.Find("PlayerManager_Prefab");
		UIInventoryStore = GameObject.Find("UI_InventoryStore_Prefab");
	}

	// Use this for initialization
	void Start () 
	{
	}

	public void Initialize()
	{
		Init = false;
		Alive = true;
		TotalTime = 0;
		TimeEarned = 0;
		HitPoints = 100;
		StatsShotsFired = 0;
		StatsBaddieHits = 0;
		StatsBaddiesKilled = 0;
		StatsMeleeHits = 0;
		StatsAccuracy = 0;
		StatsTotalPointsEarned = 0;
		DifficultyTimer = CheckDifficultyTimer;
	}

	// Update is called once per frame
	void Update () 
	{
		if(Init) Initialize();

		if(Alive)
		{
			TotalTime += Time.deltaTime;

			DifficultyTimer -= Time.deltaTime;

			// Make the game harder or easier based on the amount of points the player has gotten.
			if(DifficultyTimer < 0f)
			{
				DifficultyTimer = CheckDifficultyTimer;
				float CurrentTimePoints = (int)(TotalTime/PointTimeMultiplier);

				if(HitPoints < 50 && CurrentTimePoints < ((float)UIInventoryStore.GetComponent<InventoryStore>().TotalCost * 0.5f))
				{
					ChangeGameDifficulty(0.5f);
				}
				else
				{
					if (CurrentTimePoints > ((float)UIInventoryStore.GetComponent<InventoryStore>().TotalCost * 0.8f))
					{
						ChangeGameDifficulty(1.7f);
					}
				}
			}
		}

		if(HitPoints <= 0 && Alive)
		{
			Debug.Log("Player Died");
			// destroy the inventory and the baddies.
			string UICurrentState = UIManager.GetComponent<UI_Manager>().UICurrentState;
			switch (UICurrentState)
			{

			// need to not hard code this so much

			case "Room_01State":
				GameObject Room_01State = UIManager.GetComponent<UI_Manager>().UIRoom_01;
				Room_01State.GetComponent<InventoryManager>().SendMessage("DestroyAllInventory");
				Room_01State.GetComponent<BaddieManager>().SendMessage("DestroyAllBaddies");
				break;	
			case "Room_02State":
				GameObject Room_02State = UIManager.GetComponent<UI_Manager>().UIRoom_02;
				Room_02State.GetComponent<InventoryManager>().SendMessage("DestroyAllInventory");
				Room_02State.GetComponent<BaddieManager>().SendMessage("DestroyAllBaddies");
				break;	
			case "Room_03State":
				GameObject Room_03State = UIManager.GetComponent<UI_Manager>().UIRoom_03;
				Room_03State.GetComponent<InventoryManager>().SendMessage("DestroyAllInventory");
				Room_03State.GetComponent<BaddieManager>().SendMessage("DestroyAllBaddies");
				break;	
			case "Room_04State":
				GameObject Room_04State = UIManager.GetComponent<UI_Manager>().UIRoom_04;
				Room_04State.GetComponent<InventoryManager>().SendMessage("DestroyAllInventory");
				Room_04State.GetComponent<BaddieManager>().SendMessage("DestroyAllBaddies");
				break;	
			}
			Alive = false;
			// calculate stats
			CalculateStats();

			UIManager.GetComponent<UI_Manager>().SwitchStates("DiedState");
		}
	}

	public void ChangeGameDifficulty(float MaxBaddies)
	{
		// MaxBaddies = the percentage of 
		string UICurrentState = UIManager.GetComponent<UI_Manager>().UICurrentState;
		switch (UICurrentState)
		{

		// need to not hard code this so much

		case "Room_01State":
			GameObject Room_01State = UIManager.GetComponent<UI_Manager>().UIRoom_01;
			if ((int)(Room_01State.GetComponent<BaddieManager>().MaxNumberOfBaddies * MaxBaddies) >= 1 &&
				(int)(Room_01State.GetComponent<BaddieManager>().MaxNumberOfBaddies * MaxBaddies) <= 12)
			{
				Room_01State.GetComponent<BaddieManager>().MaxNumberOfBaddies = (int)(Room_01State.GetComponent<BaddieManager>().MaxNumberOfBaddies * MaxBaddies);
				Debug.Log("Changed Game Difficulty. Max Baddies =  " + Room_01State.GetComponent<BaddieManager>().MaxNumberOfBaddies);
			}
			break;	
		case "Room_02State":
			GameObject Room_02State = UIManager.GetComponent<UI_Manager>().UIRoom_02;
			if ((int)(Room_02State.GetComponent<BaddieManager>().MaxNumberOfBaddies * MaxBaddies) >= 1 &&
				(int)(Room_02State.GetComponent<BaddieManager>().MaxNumberOfBaddies * MaxBaddies) <= 12)
			{
				Room_02State.GetComponent<BaddieManager>().MaxNumberOfBaddies = (int)(Room_02State.GetComponent<BaddieManager>().MaxNumberOfBaddies * MaxBaddies);
				Debug.Log("Changed Game Difficulty. Max Baddies =  " + Room_02State.GetComponent<BaddieManager>().MaxNumberOfBaddies);
			}
			break;	
		case "Room_03State":
			GameObject Room_03State = UIManager.GetComponent<UI_Manager>().UIRoom_03;
			if ((int)(Room_03State.GetComponent<BaddieManager>().MaxNumberOfBaddies * MaxBaddies) >= 1 &&
				(int)(Room_03State.GetComponent<BaddieManager>().MaxNumberOfBaddies * MaxBaddies) <= 12)
			{
				Room_03State.GetComponent<BaddieManager>().MaxNumberOfBaddies = (int)(Room_03State.GetComponent<BaddieManager>().MaxNumberOfBaddies * MaxBaddies);
				Debug.Log("Changed Game Difficulty. Max Baddies =  " + Room_03State.GetComponent<BaddieManager>().MaxNumberOfBaddies);
			}
			break;	
		case "Room_04State":
			GameObject Room_04State = UIManager.GetComponent<UI_Manager>().UIRoom_04;
			if ((int)(Room_04State.GetComponent<BaddieManager>().MaxNumberOfBaddies * MaxBaddies) >= 1 &&
				(int)(Room_04State.GetComponent<BaddieManager>().MaxNumberOfBaddies * MaxBaddies) <= 12)
			{
				Room_04State.GetComponent<BaddieManager>().MaxNumberOfBaddies = (int)(Room_04State.GetComponent<BaddieManager>().MaxNumberOfBaddies * MaxBaddies);
				Debug.Log("Changed Game Difficulty. Max Baddies =  " + Room_04State.GetComponent<BaddieManager>().MaxNumberOfBaddies);
			}
			break;	
		}

	}

	public void CalculateStats()
	{
		StatsAccuracy = 0;
		if(StatsShotsFired > 0)
		{
			StatsAccuracy = (int)((float)StatsBaddieHits / (float)StatsShotsFired * 100);
		}
		TimeEarned = (int)(TotalTime/PointTimeMultiplier); // dollar per seconds

		// give points
		StatsTotalPointsEarned = 0;
		StatsTotalPointsEarned+= TimeEarned;
		StatsTotalPointsEarned+= StatsBaddiesKilled; // baddies killed bonus
		StatsTotalPointsEarned+= (int)((float)StatsMeleeHits / 4); // melee hits bonus
		StatsTotalPointsEarned+= (int)((float)StatsAccuracy / 10) ; // accuracy bonus

		// update stats to the player manager
		PlayerManager.GetComponent<PlayerManager>().CurrentPlayer.PlayerDollars += StatsTotalPointsEarned;
		PlayerTotalPoints = PlayerManager.GetComponent<PlayerManager>().CurrentPlayer.PlayerDollars;

		PlayerManager.GetComponent<PlayerManager>().CurrentPlayer.PlayerStatsTotalTime += TotalTime;
		PlayerManager.GetComponent<PlayerManager>().CurrentPlayer.PlayerStatsTotalNights += 1;
		PlayerManager.GetComponent<PlayerManager>().CurrentPlayer.PlayerStatsTotalShotsFired += StatsShotsFired;
		PlayerManager.GetComponent<PlayerManager>().CurrentPlayer.PlayerStatsTotalBaddieHits += StatsBaddieHits;
		PlayerManager.GetComponent<PlayerManager>().CurrentPlayer.PlayerStatsTotalBaddiesKilled += StatsBaddiesKilled;
		PlayerManager.GetComponent<PlayerManager>().CurrentPlayer.PlayerStatsTotalMeleeHits += StatsMeleeHits;
		PlayerManager.GetComponent<PlayerManager>().CurrentPlayer.PlayerStatsTotalAccuracy = (int)(((float)PlayerManager.GetComponent<PlayerManager>().CurrentPlayer.PlayerStatsTotalBaddieHits / (float) PlayerManager.GetComponent<PlayerManager>().CurrentPlayer.PlayerStatsTotalShotsFired) * 100f);

		// save updated data
		PlayerManager.GetComponent<PlayerManager>().UpdateCurrentPlayer(PlayerManager.GetComponent<PlayerManager>().CurrentPlayerIndex);
		PlayerManager.GetComponent<PlayerManager>().SavePlayer();

	}

	public void TakeDamage(int amount)
	{
		HitPoints-= amount;
		//SteamVR_Fade.Start(new Color(1.0f, 0f, 0f, 0.25f), 0);
		//SteamVR_Fade.Start(Color.clear, 1);
		if(ScreenBloodFX)
		{
			ScreenBloodFX.GetComponent<ScreenBloodFX>().SendMessage("ShowBlood");
		}
	}

}
