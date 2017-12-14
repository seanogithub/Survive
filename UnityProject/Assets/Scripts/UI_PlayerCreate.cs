using UnityEngine;
using System.Collections;
using System.Text;
using UnityEngine.UI;
using Valve.VR;
using Valve;


public class UI_PlayerCreate : MonoBehaviour {

	public bool Init = false;
	public GameObject PlayerManager;
	public GameObject PlayerText;
	public string PlayerNameText = "";

	// Use this for initialization
	void Start () {
	}

	public void Initialize()
	{
		Init = false;
		PlayerNameText = "";

		//SteamVR_Utils.Event.Listen("KeyboardCharInput", OnKeyboard);
		//SteamVR_Utils.Event.Listen("KeyboardClosed", OnKeyboardClosed);

		//SteamVR_Events.System(EVREventType.VREvent_KeyboardCharInput).Listen(OnKeyboard);
		//SteamVR_Events.System(EVREventType.VREvent_KeyboardClosed).Listen(OnKeyboardClosed);

		//SteamVR.instance.overlay.ShowKeyboard(0,0,"Test",256,"",false, 0);
	}

		// Update is called once per frame
	void Update () 
	{
		if(Init) Initialize();
		PlayerText.GetComponent<Text>().text = PlayerNameText;
	}

	/*
	private void OnKeyboard(Valve.VR.VREvent_t args) //(object[] args)
	{
		StringBuilder stringBuilder = new StringBuilder(256);
		SteamVR.instance.overlay.GetKeyboardText(stringBuilder, 256);
		PlayerNameText = stringBuilder.ToString(); 
		PlayerText.GetComponent<Text>().text = PlayerNameText;
	}

	private void OnKeyboardClosed(Valve.VR.VREvent_t args) //(object[] args)
	{
		Debug.Log("Keyboard Closed");
	}
	*/

}
