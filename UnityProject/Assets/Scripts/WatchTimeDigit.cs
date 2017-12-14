using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchTimeDigit : MonoBehaviour {

	public int DigitValue;
	public Texture CurrentDigitTexture;
	public Texture DigitZero;
	public Texture DigitOne;
	public Texture DigitTwo;
	public Texture DigitThree;
	public Texture DigitFour;
	public Texture DigitFive;
	public Texture DigitSix;
	public Texture DigitSeven;
	public Texture DigitEight;
	public Texture DigitNine;
	public Material DigitMaterial;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		switch(DigitValue)
		{
		case 0:
			{
				CurrentDigitTexture = DigitZero;
				break;
			}
		case 1:
			{
				CurrentDigitTexture = DigitOne;
				break;
			}
		case 2:
			{
				CurrentDigitTexture = DigitTwo;
				break;
			}
		case 3:
			{
				CurrentDigitTexture = DigitThree;
				break;
			}
		case 4:
			{
				CurrentDigitTexture = DigitFour;
				break;
			}
		case 5:
			{
				CurrentDigitTexture = DigitFive;
				break;
			}
		case 6:
			{
				CurrentDigitTexture = DigitSix;
				break;
			}
		case 7:
			{
				CurrentDigitTexture = DigitSeven;
				break;
			}
		case 8:
			{
				CurrentDigitTexture = DigitEight;
				break;
			}
		case 9:
			{
				CurrentDigitTexture = DigitNine;
				break;
			}
		default:
			{
				CurrentDigitTexture = DigitZero;
				break;
			}
		}

		DigitMaterial.mainTexture = CurrentDigitTexture;
	}
}
