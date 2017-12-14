using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watch : MonoBehaviour {

	public GameObject Player;
	public GameObject WatchObject;
	public GameObject TimeSecondsOnes;
	public GameObject TimeSecondsTens;
	public GameObject TimeMinutesOnes;
	public GameObject TimeMinutesTens;

	private Animation HealthAnim;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () 
	{
		if(this.transform.parent.GetComponent<RobustController>().HoldableObject != null)
		{
			if(this.transform.parent.GetComponent<RobustController>().HoldableObject.name == "MenuController_Prefab(Clone)")
			{
				WatchObject.SetActive(false);
			}
			else
			{
				UpdateWatchAnimation();
			}
		}
		else
		{
			UpdateWatchAnimation();
		}
	}

	public void UpdateWatchAnimation()
	{
		WatchObject.SetActive(true);
		HealthAnim = this.GetComponentInChildren<Animation>();
		HealthAnim.Play();
		HealthAnim.Stop();
		HealthAnim["Health"].speed = 0f;
		HealthAnim["Health"].enabled = true;
		HealthAnim["Health"].time = (Player.GetComponent<Player>().HitPoints/ 30f );

		float TotalTime = Player.GetComponent<Player>().TotalTime;
		float TimeSeconds = (TotalTime % 60);
		float TimeMinutes = (TotalTime / 60);

		float TimeSecondsOnesValue = ((TimeSeconds / 10) - (int)(TimeSeconds / 10)) * 10;
		TimeSecondsOnes.GetComponent<WatchTimeDigit>().DigitValue = (int) TimeSecondsOnesValue;

		float TimeSecondsTensValue = ((TimeSeconds / 100) - (int)(TimeSeconds / 100)) * 10;
		TimeSecondsTens.GetComponent<WatchTimeDigit>().DigitValue = (int) TimeSecondsTensValue;

		float TimeMinutesOnesValue = ((TimeMinutes / 10) - (int)(TimeMinutes / 10)) * 10;
		TimeMinutesOnes.GetComponent<WatchTimeDigit>().DigitValue = (int) TimeMinutesOnesValue;

		float TimeMinutesTensValue = ((TimeMinutes / 100) - (int)(TimeMinutes / 100)) * 10;
		TimeMinutesTens.GetComponent<WatchTimeDigit>().DigitValue = (int) TimeMinutesTensValue;

	}
}
