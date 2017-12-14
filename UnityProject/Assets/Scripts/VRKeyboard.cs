using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRKeyboard : MonoBehaviour {

	public GameObject[] KeyboardButtonArray;
	public bool KeyboardShift = true;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void ShiftButtonPressed()
	{
		KeyboardShift = !KeyboardShift;
		for(int i=0; i< KeyboardButtonArray.Length; i++)
		{
			string Char = KeyboardButtonArray[i].GetComponent<MenuButton>().Data;
			if (KeyboardShift)
			{
				Char = Char.ToUpper();
			}
			else
			{
				Char = Char.ToLower();
			}
			KeyboardButtonArray[i].GetComponent<MenuButton>().Data = Char;
			KeyboardButtonArray[i].GetComponentInChildren<Text>().text = Char;
		}
	}
}
