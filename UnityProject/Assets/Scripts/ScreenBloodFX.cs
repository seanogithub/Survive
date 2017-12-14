using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBloodFX : MonoBehaviour {

	public Texture[] BloodSplatter;
	public float FadeTimer = 0.25f;
	private float FadeTimerValue;

	// Use this for initialization
	void Start () 
	{
		GetComponent<MeshRenderer>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(FadeTimerValue > 0)
		{
			GetComponent<MeshRenderer>().enabled = true;
			FadeTimerValue-= Time.deltaTime;
			Color FadeColor =  new Color ( 1.0f, 1.0f, 1.0f, FadeTimerValue / FadeTimer);
			GetComponent<MeshRenderer>().material.SetColor("_Color", FadeColor);
		}
		if(FadeTimerValue < 0)
		{
			GetComponent<MeshRenderer>().enabled = false;
		}
	}

	public void ShowBlood()
	{
		GetComponent<MeshRenderer>().enabled = true;
		if(BloodSplatter.Length > 0)
		{
			int RandomTextureIndex = Random.Range(0,  BloodSplatter.Length );
			GetComponent<MeshRenderer>().material.SetTexture("_MainTex", BloodSplatter[RandomTextureIndex]);
		}
		FadeTimerValue = FadeTimer;
	}

}
