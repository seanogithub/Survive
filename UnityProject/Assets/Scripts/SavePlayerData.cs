using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SavePlayerData {

	//	public List<PlayerData> UserPlayerData = new List<PlayerData>(); 
	public PlayerData[] Player = new PlayerData[0];

	public SavePlayerData()
	{
		//		UserPlayerData = new List<PlayerData>(); 
		Player = new PlayerData[0];
	}	

	public void Add (PlayerData newItem)
	{
		PlayerData[] tempArray = new PlayerData[Player.Length+1];
		if (Player.Length > 0)
		{
			for(int i = 0; i < Player.Length; i++)
			{
				tempArray[i] = Player[i];
			}
			tempArray[tempArray.Length-1] = newItem;
			Player = tempArray;
		}
		else
		{
			tempArray[0] = newItem;
			Player = tempArray;
		}
	}


	public void RemoveAt (int Index)
	{
		if(Player.Length > 0)
		{
			PlayerData[] tempArray = new PlayerData[Player.Length-1];
			for(int i = 0; i < Index; i++)
			{
				tempArray[i] = Player[i];
			}
			for(int i = Index + 1; i < Player.Length; i++)
			{
				tempArray[i-1] = Player[i];
			}
			Player = tempArray;
		}
	}

	public void Clear ()
	{
		Player = new PlayerData[0];
	}

	public int Length ()
	{
		return Player.Length;
	}
}
