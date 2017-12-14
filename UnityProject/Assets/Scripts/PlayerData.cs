using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerData  {

	public string PlayerName = "";
	public int PlayerDollars = 200;
	//public bool[] PlayerAchievments;

	public float PlayerStatsTotalTime = 0;
	public int PlayerStatsTotalNights = 0;
	public int PlayerStatsTotalShotsFired = 0;
	public int PlayerStatsTotalBaddieHits = 0;
	public int PlayerStatsTotalBaddiesKilled = 0;
	public int PlayerStatsTotalMeleeHits = 0;
	public int PlayerStatsTotalAccuracy = 0;
	public string CartItem01 = "";
	public string CartItem02 = "";
	public string CartItem03 = "";
	public string CartItem04 = "";
	public string CartItem05 = "";
	public string CartItem06 = "";
	public string CartItem07 = "";
	public string CartItem08 = "";
	public string CartItem09 = "";
	public string CartItem10 = "";

	public PlayerData DeepCopy (PlayerData data)
	{
		PlayerData NewData = new PlayerData();
		NewData.PlayerName = data.PlayerName;
		NewData.PlayerDollars = data.PlayerDollars;
		//NewData.PlayerAchievments = data.PlayerAchievments;

		NewData.PlayerStatsTotalTime = data.PlayerStatsTotalTime;
		NewData.PlayerStatsTotalNights = data.PlayerStatsTotalNights;
		NewData.PlayerStatsTotalShotsFired = data.PlayerStatsTotalShotsFired;
		NewData.PlayerStatsTotalBaddieHits = data.PlayerStatsTotalBaddieHits;
		NewData.PlayerStatsTotalBaddiesKilled = data.PlayerStatsTotalBaddiesKilled;
		NewData.PlayerStatsTotalMeleeHits = data.PlayerStatsTotalMeleeHits;
		NewData.PlayerStatsTotalAccuracy = data.PlayerStatsTotalAccuracy;
		NewData.CartItem01 = data.CartItem01;
		NewData.CartItem02 = data.CartItem02;
		NewData.CartItem03 = data.CartItem03;
		NewData.CartItem04 = data.CartItem04;
		NewData.CartItem05 = data.CartItem05;
		NewData.CartItem06 = data.CartItem06;
		NewData.CartItem07 = data.CartItem07;
		NewData.CartItem08 = data.CartItem08;
		NewData.CartItem09 = data.CartItem09;
		NewData.CartItem10 = data.CartItem10;

		return NewData;
	}
}
