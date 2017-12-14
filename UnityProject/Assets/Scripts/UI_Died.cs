using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI_Died : MonoBehaviour {

	public bool Init = false;
	public GameObject Player;
	public GameObject Text_TimePoints;
	public GameObject Text_BaddiePoints;
	public GameObject Text_MeleePoints;
	public GameObject Text_AccuracyPoints;
	public GameObject Text_TotalPoints;
	public GameObject Text_TotalDollars;

	// Use this for initialization
	void Start () {
	
	}

	public void Initialize()
	{
		Init = false;
		Text_TimePoints.GetComponent<Text>().text = Player.GetComponent<Player>().TimeEarned.ToString();
		Text_BaddiePoints.GetComponent<Text>().text = Player.GetComponent<Player>().StatsBaddiesKilled.ToString();
		Text_MeleePoints.GetComponent<Text>().text = Player.GetComponent<Player>().StatsMeleeHits.ToString();
		Text_AccuracyPoints.GetComponent<Text>().text = ((int)((float) Player.GetComponent<Player>().StatsAccuracy / 10)).ToString();
		Text_TotalPoints.GetComponent<Text>().text = Player.GetComponent<Player>().StatsTotalPointsEarned.ToString();
		Text_TotalDollars.GetComponent<Text>().text = Player.GetComponent<Player>().PlayerTotalPoints.ToString();
	}

	// Update is called once per frame
	void Update () 
	{
		if(Init) Initialize();
	}
}
