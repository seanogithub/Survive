using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using System.Linq;
using System.Text.RegularExpressions;

public class UI_Manager : MonoBehaviour {


	public string UIPreviousState = "HomeState";
	public string UICurrentState = "HomeState";
	public GameObject UserBlobManager;
	public GameObject Player;
	public GameObject UISplashScreen;
	public GameObject UIHome;
	public GameObject UIInventoryStore;
	public GameObject UIRoom_01;
	public GameObject UIRoom_02;
	public GameObject UIRoom_03;
	public GameObject UIRoom_04;
	public GameObject UIDied;
	public GameObject UIPlayerSelect;
	public GameObject UIPlayerCreate;
	public GameObject UIRoomSelect;
	public GameObject UITutorial;
	public GameObject UIRoom;
	public GameObject RoomStore;

    public bool SplashScreenState = false;
	public bool HomeState = false;
	public bool InventoryStoreState = false;
	public bool Room_01State = false;
	public bool Room_02State = false;
	public bool Room_03State = false;
	public bool Room_04State = false;
	public bool DiedState = false;
	public bool PlayerSelectState = false;
	public bool PlayerCreateState = false;
	public bool RoomSelectState = false;
	public bool TutorialState = false;
	public bool UIRoomState = false;
	public bool RoomStoreState = false;

    public System.DateTime currentDate = new System.DateTime();

	public float FrameRateInterval = 0;

	void Awake()
	{
		UserBlobManager = GameObject.Find("UserBlob_Prefab");
		UISplashScreen = GameObject.Find("UI_SplashScreen_Prefab");
		UIHome = GameObject.Find("UI_Home_Prefab");
		UIInventoryStore = GameObject.Find("UI_InventoryStore_Prefab");
		UIRoom_01 = GameObject.Find("Room_01_Prefab");
		UIRoom_02 = GameObject.Find("Room_02_Prefab");
		UIRoom_03 = GameObject.Find("Room_03_Prefab");
		UIRoom_04 = GameObject.Find("Room_04_Prefab");
		UIDied = GameObject.Find("UI_Died_Prefab");
		UIPlayerSelect = GameObject.Find("UI_PlayerSelect_Prefab");
		UIPlayerCreate = GameObject.Find("UI_PlayerCreate_Prefab");
		UIRoomSelect = GameObject.Find("UI_RoomSelect_Prefab");
		UITutorial = GameObject.Find("UI_Tutorial_Prefab");
		UIRoom = GameObject.Find("UI_Room_Prefab");
		RoomStore = GameObject.Find("Room_Store_Prefab");
    }

    // Use this for initialization
    void Start () 
	{
		SetUIState();
	}


	public void ResetManagerData()
	{
	}
	
	public void SetCurrentDate(System.DateTime newDate)
	{
		currentDate = new System.DateTime(newDate.Year, newDate.Month, newDate.Day, newDate.Hour, newDate.Minute, newDate.Second);
	}
	
	public void SwitchStates(string myNewState)
	{
		// check for valid states
		if(	myNewState == "SplashScreenState" ||
			myNewState == "HomeState" ||
			myNewState == "InventoryStoreState" ||
		   	myNewState == "Room_01State" ||
			myNewState == "Room_02State" ||
			myNewState == "Room_03State" ||
			myNewState == "Room_04State" ||
		   	myNewState == "DiedState" ||
			myNewState == "PlayerSelectState" ||
			myNewState == "PlayerCreateState" ||
			myNewState == "RoomSelectState" ||
			myNewState == "TutorialState" ||
			myNewState == "UIRoomState" ||
			myNewState == "RoomStoreState"
		   )
		{
			UICurrentState = myNewState;
			SetUIState();
		}
		
	}

	void SetUIState () 
	{
		SplashScreenState = false;
		HomeState = false;
		InventoryStoreState = false;
		Room_01State = false;
		Room_02State = false;
		Room_03State = false;
		Room_04State = false;
		DiedState = false;
		PlayerSelectState = false;
		PlayerCreateState = false;
		RoomSelectState = false;
		TutorialState = false;
		UIRoomState = false;
		RoomStoreState = false;

        switch (UICurrentState)
		{
		case "SplashScreenState":
			SplashScreenState = true;
			break;	
		case "HomeState":
			HomeState = true;
			UIRoomState = true; // turn on UI Room
			break;
		case "InventoryStoreState":
			InventoryStoreState = true;
			//UIRoomState = true; // turn on UI Room
			RoomStoreState = true; // turn on the Store
			break;
		case "Room_01State":
			Room_01State = true;
			break;
		case "Room_02State":
			Room_02State = true;
			break;
		case "Room_03State":
			Room_03State = true;
			break;
		case "Room_04State":
			Room_04State = true;
			break;
		case "DiedState":
			DiedState = true;
			UIRoomState = true; // turn on UI Room
			break;
		case "PlayerSelectState":
			PlayerSelectState = true;
			UIRoomState = true; // turn on UI Room
			break;
		case "PlayerCreateState":
			PlayerCreateState = true;
			UIRoomState = true; // turn on UI Room
			break;
		case "RoomSelectState":
			RoomSelectState = true;
			UIRoomState = true; // turn on UI Room
			break;
		case "TutorialState":
			TutorialState = true;
			UIRoomState = true; // turn on UI Room
			break;
		case "UIRoomState":
			UIRoomState = true;
			break;
		case "RoomStoreState":
			RoomStoreState = true;
			break;
        }

		UISplashScreen.SetActive(SplashScreenState);
		UIHome.SetActive(HomeState);
		UIInventoryStore.SetActive(InventoryStoreState);
		UIRoom_01.SetActive(Room_01State);
		UIRoom_02.SetActive(Room_02State);
		UIRoom_03.SetActive(Room_03State);
		UIRoom_04.SetActive(Room_04State);
		UIDied.SetActive(DiedState);
		UIPlayerSelect.SetActive(PlayerSelectState);
		UIPlayerCreate.SetActive(PlayerCreateState);
		UIRoomSelect.SetActive(RoomSelectState);
		UITutorial.SetActive(TutorialState);
		UIRoom.SetActive(UIRoomState);
		RoomStore.SetActive(RoomStoreState);

        if (SplashScreenState)
		{
		}
		if (HomeState)
		{
			SteamVR_Fade.Start(Color.black, 0);
			SteamVR_Fade.Start(Color.clear, 6);
			UIHome.GetComponent<ControllerUI>().Init = true;
		}
		if (InventoryStoreState)
		{
			SteamVR_Fade.Start(Color.black, 0);
			SteamVR_Fade.Start(Color.clear, 6);
			UIInventoryStore.GetComponent<ControllerUI>().Init = true;
			UIInventoryStore.GetComponent<InventoryStore>().Init = true;
		}
		if (Room_01State)
		{
			SteamVR_Fade.Start(Color.black, 0);
			SteamVR_Fade.Start(Color.clear, 6);
			UIRoom_01.GetComponent<InventoryManager>().Init = true;
			UIRoom_01.GetComponent<BaddieManager>().Init = true;
			Player.GetComponent<Player>().Init = true;
		}
		if (Room_02State)
		{
			SteamVR_Fade.Start(Color.black, 0);
			SteamVR_Fade.Start(Color.clear, 6);
			UIRoom_02.GetComponent<InventoryManager>().Init = true;
			UIRoom_02.GetComponent<BaddieManager>().Init = true;
			Player.GetComponent<Player>().Init = true;
		}
		if (Room_03State)
		{
			SteamVR_Fade.Start(Color.black, 0);
			SteamVR_Fade.Start(Color.clear, 6);
			UIRoom_03.GetComponent<InventoryManager>().Init = true;
			UIRoom_03.GetComponent<BaddieManager>().Init = true;
			Player.GetComponent<Player>().Init = true;
		}
		if (Room_04State)
		{
			SteamVR_Fade.Start(Color.black, 0);
			SteamVR_Fade.Start(Color.clear, 6);
			UIRoom_04.GetComponent<InventoryManager>().Init = true;
			UIRoom_04.GetComponent<BaddieManager>().Init = true;
			Player.GetComponent<Player>().Init = true;
		}
		if (DiedState)
		{
			SteamVR_Fade.Start(Color.black, 0);
			SteamVR_Fade.Start(Color.clear, 6);
			UIDied.GetComponent<ControllerUI>().Init = true;
			UIDied.GetComponent<UI_Died>().Init = true;
		}
		if (PlayerSelectState)
		{
			UIPlayerSelect.GetComponent<ControllerUI>().Init = true;
			UIPlayerSelect.GetComponent<UI_SelectPlayer>().Init = true;
		}
		if (PlayerCreateState)
		{
			UIPlayerCreate.GetComponent<ControllerUI>().Init = true;
			UIPlayerCreate.GetComponent<UI_PlayerCreate>().Init = true;
		}
		if (RoomSelectState)
		{
			UIRoomSelect.GetComponent<ControllerUI>().Init = true;
			UIRoomSelect.GetComponent<UI_SelectRoom>().Init = true;
		}
		if (TutorialState)
		{
			UITutorial.GetComponent<ControllerUI>().Init = true;
			UITutorial.GetComponent<UI_Tutorial>().Init = true;
		}
		if (UIRoomState)
		{
			//UIRoom.GetComponent<ControllerUI>().Init = true;
		}
		if (RoomStoreState)
		{
			//RoomStore.GetComponent<ControllerUI>().Init = true;
		}

        UIPreviousState = UICurrentState;
	}
	

	void Update()
	{
		if (UIPreviousState != UICurrentState)
		{
			SetUIState();
		}

		// framerate
		FrameRateInterval = Time.deltaTime;
		if(FrameRateInterval > 0.0167f ) // less than 60 fps
		{
			//Debug.Log("FPS = " + ((int)(1/FrameRateInterval)).ToString());
			Debug.Log("FPS Below 60 ");
		}
		if(FrameRateInterval > 0.0112f ) // less than 90 fps
		{
			//Debug.Log("FPS = " + ((int)(1/FrameRateInterval)).ToString());
			Debug.Log("FPS Below 90 ");
		}

	}
}
