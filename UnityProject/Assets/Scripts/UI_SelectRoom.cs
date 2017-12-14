using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;

public class UI_SelectRoom : MonoBehaviour {

	public bool Init = false;
	public GameObject UIManager;
	public GameObject UIInventoryStore;

	public GameObject[] RoomList = new GameObject[2];
	public GameObject[] RoomButtonArray = new GameObject[2];

	public GameObject RoomButton;
	public int RoomButtonIndex = 0;
	public Transform SelectRoomCanvas;

	// Use this for initialization
	void Start () 
	{
		
	}

	public void Initialize()
	{
		Init = false;
		RoomButtonIndex = 0;
		ClearRoomButtons();
		PopulateRoomButtons();

	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Init) Initialize();

	}

	public void UpdatePlayers()
	{
		ClearRoomButtons();
		PopulateRoomButtons();
	}

	public void ClearRoomButtons()
	{
		// clear store buttons
		for(int i = 0; i < RoomButtonArray.Length; i++)
		{
			DestroyImmediate(RoomButtonArray[i]);
		}
		RoomButtonArray = new GameObject[RoomButtonArray.Length+1]; // keep the same length array
	}

	public void PopulateRoomButtons()
	{
		RoomButtonArray = new GameObject[RoomList.Length];

		if(RoomList.Length < RoomButtonIndex + RoomButtonArray.Length)
		{
			RoomButtonIndex = RoomList.Length - RoomButtonArray.Length;
		}
		if(RoomButtonIndex < 0)
		{
			RoomButtonIndex = 0;
		}

		// populate buttons
		for (int i = 0; i < RoomButtonArray.Length; i++)
		{
			if ((RoomButtonIndex + i) < (RoomList.Length)) // make sure there are enough sroe items.
			{
				GameObject NewButton = Instantiate(RoomButton) as GameObject;
				RoomButtonArray[i] = NewButton;

				NewButton.name = "Button_" + RoomList[RoomButtonIndex+i].name;
				NewButton.GetComponent<MenuButton>().Data = (RoomButtonIndex+i).ToString();
				NewButton.transform.Find("Text_RoomName").GetComponent<Text>().text = RoomList[RoomButtonIndex+i].GetComponent<RoomData>().RoomName;
				NewButton.transform.Find("Image_Icon").GetComponent<Image>().sprite = RoomList[RoomButtonIndex+i].GetComponent<RoomData>().RoomIcon;
				//NewButton.transform.Find("Text_PlayerDollars").GetComponent<Text>().text = RoomList[RoomButtonIndex+i].PlayerDollars.ToString();
				//NewButton.transform.Find("Text_PlayerTotalTime").GetComponent<Text>().text = ((int)(RoomList[RoomButtonIndex+i].PlayerStatsTotalNights)).ToString();

				NewButton.transform.SetParent(SelectRoomCanvas);

				NewButton.transform.localScale = Vector3.one;
				float Xpos = (-0f / 5f); //(float)((i* NewButton.GetComponent<RectTransform>().sizeDelta.x) - (200f * 4f) );
				//float Ypos = (35 - (float)((i* NewButton.GetComponent<RectTransform>().sizeDelta.y) - (100f * 7f)) / 5f);
				float Ypos = (35 - (float)((i* 200) - (100f * 7f)) / 5f);
				Vector3 NewPosition = new Vector3(Xpos/100f, Ypos/100f, 3f);
				NewButton.transform.position= NewPosition;
				NewButton.SetActive(true);
			}
		}
	}

	public void SelectRoom(string SelectRoomIndex)
	{
		int Index = int.Parse(SelectRoomIndex);
		//Debug.Log("Room Index = " + Index);
		UIInventoryStore.GetComponent<InventoryStore>().Room = RoomList[Index];
		UIInventoryStore.GetComponent<InventoryStore>().RoomStateString = RoomList[Index].GetComponent<RoomData>().RoomState;
		this.GetComponent<ControllerUI>().DestroyControllers();
		UIManager.GetComponent<UI_Manager>().SwitchStates("InventoryStoreState");
	}

	public void Back()
	{
		this.GetComponent<ControllerUI>().DestroyControllers();
		UIManager.GetComponent<UI_Manager>().SwitchStates("PlayerSelectState");
	}

	public void RandomRoom()
	{
		int RandomIndex = Random.Range(0, (RoomList.Length));
		Debug.Log("Random Room Index = " + RandomIndex);
		UIInventoryStore.GetComponent<InventoryStore>().Room = RoomList[RandomIndex];
		UIInventoryStore.GetComponent<InventoryStore>().RoomStateString = RoomList[RandomIndex].GetComponent<RoomData>().RoomState;
		this.GetComponent<ControllerUI>().DestroyControllers();
		UIManager.GetComponent<UI_Manager>().SwitchStates("InventoryStoreState");
	}
}
