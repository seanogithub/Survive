using UnityEngine;
using System.Collections;
using System.Text;
using UnityEngine.UI;
using Valve.VR;

public class UI_SelectPlayer : MonoBehaviour {

	public bool Init = false;
	public GameObject UIManager;
	public GameObject PlayerManager;
	public GameObject Player;
	public GameObject PlayerButton;
	public Transform SelectPlayerCanvas;
	public SavePlayerData PlayerList;
	public GameObject[] PlayerButtonArray = new GameObject[4];
	public int PlayerButtonIndex = 0;
	private string PlayerNameText = "";
	public GameObject DeleteButtonText;
	private Color DeleteButtonTextColor = new Color (0.75f, 0f, 0f, 1.0f);
	public bool DeleteMode = false;


	void Awake()
	{
		UIManager = GameObject.Find("UI_Manager_Prefab");
		PlayerManager = GameObject.Find("PlayerManager_Prefab");
		Player = GameObject.Find("Player_Prefab");

		//SteamVR_Utils.Event.Listen("KeyboardCharInput", OnKeyboard);
		//SteamVR_Utils.Event.Listen("KeyboardClosed", OnKeyboardClosed);

		SteamVR_Events.System(EVREventType.VREvent_KeyboardCharInput).Listen(OnKeyboard);
		SteamVR_Events.System(EVREventType.VREvent_KeyboardClosed).Listen(OnKeyboardClosed);

	}

	// Use this for initialization
	void Start () {
		DeleteButtonTextColor = DeleteButtonText.GetComponent<Text>().color;
	}

	public void Initialize()
	{
		Init = false;
		PlayerButtonIndex = 0;
		PlayerList = PlayerManager.GetComponent<PlayerManager>().UserPlayerData;
		ClearPlayerButtons();
		PopulatePlayerButtons();
	}

	// Update is called once per frame
	void Update () {
		if(Init) Initialize();
		if(DeleteMode)
		{
			DeleteButtonText.GetComponent<Text>().color = Color.white;
		}
		else
		{
			DeleteButtonText.GetComponent<Text>().color = DeleteButtonTextColor;
		}
	}

	public void UpdatePlayers()
	{
		ClearPlayerButtons();
		PopulatePlayerButtons();
	}

	public void ClearPlayerButtons()
	{
		// clear store buttons
		for(int i = 0; i < PlayerButtonArray.Length; i++)
		{
			DestroyImmediate(PlayerButtonArray[i]);
		}
		PlayerButtonArray = new GameObject[PlayerButtonArray.Length]; // keep the same length array
	}

	public void PopulatePlayerButtons()
	{
		if(PlayerList.Player.Length < PlayerButtonIndex + PlayerButtonArray.Length)
		{
			PlayerButtonIndex = PlayerList.Player.Length - PlayerButtonArray.Length;
		}
		if(PlayerButtonIndex < 0)
		{
			PlayerButtonIndex = 0;
		}

		// populate buttons
		for (int i = 0; i < PlayerButtonArray.Length; i++)
		{
			if ((PlayerButtonIndex + i) < (PlayerList.Player.Length)) // make sure there are enough sroe items.
			{
				GameObject NewButton = Instantiate(PlayerButton) as GameObject;
				PlayerButtonArray[i] = NewButton;
				NewButton.name = "Button_" + PlayerList.Player[PlayerButtonIndex+i].PlayerName;
				NewButton.GetComponent<MenuButton>().Data = (PlayerButtonIndex+i).ToString();
				NewButton.transform.Find("Text_PlayerName").GetComponent<Text>().text = PlayerList.Player[PlayerButtonIndex+i].PlayerName;
				NewButton.transform.Find("Text_PlayerDollars").GetComponent<Text>().text = PlayerList.Player[PlayerButtonIndex+i].PlayerDollars.ToString();
				NewButton.transform.Find("Text_PlayerTotalTime").GetComponent<Text>().text = ((int)(PlayerList.Player[PlayerButtonIndex+i].PlayerStatsTotalNights)).ToString();
				//NewButton.transform.Find("Image_Icon").GetComponent<Image>().sprite = PlayerList[PlayerButtonIndex+i].GetComponent<InventoryItem>().ItemIcon;
				NewButton.transform.SetParent(SelectPlayerCanvas);

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

	public void SelectPlayer(string SelectPlayerIndex)
	{
		//Debug.Log(int.Parse(SelectPlayerIndex));
		int Index = int.Parse(SelectPlayerIndex);

		// set player data if its valid
		if(PlayerManager.GetComponent<PlayerManager>().UserPlayerData.Player[Index] != null)
		{
			if (DeleteMode)
			{
				// this should check to make sure the player wants to delete the user data
				PlayerManager.GetComponent<PlayerManager>().UserPlayerData.RemoveAt(Index);
				PlayerManager.GetComponent<PlayerManager>().SavePlayer();
				UpdatePlayers();
				DeleteMode = false;
			}
			else
			{
				PlayerManager.GetComponent<PlayerManager>().CurrentPlayerIndex = Index;
				PlayerManager.GetComponent<PlayerManager>().CurrentPlayer = PlayerManager.GetComponent<PlayerManager>().UserPlayerData.Player[Index];
				GetComponent<ControllerUI>().SendMessage("DestroyControllers");
				//UIManager.GetComponent<UI_Manager>().SwitchStates("InventoryStoreState");
				UIManager.GetComponent<UI_Manager>().SwitchStates("RoomSelectState");
			}
		}
	}

	public void SelectPlayerPageUp()
	{
		PlayerButtonIndex -= PlayerButtonArray.Length;
		UpdatePlayers();
	}

	public void SelectPlayerPageDown()
	{
		PlayerButtonIndex += PlayerButtonArray.Length;
		UpdatePlayers();
	}

	public void CreatePlayer()
	{
		int NumPlayers = PlayerManager.GetComponent<PlayerManager>().UserPlayerData.Length();
		int MaxNumberOfPlayers = PlayerManager.GetComponent<PlayerManager>().MaxNumberOfPlayers;
		if(NumPlayers < MaxNumberOfPlayers)
		{
			SteamVR.instance.overlay.ShowKeyboard(0,0,"Test",256,"",false, 0);
			GetComponent<ControllerUI>().DestroyControllers();
			PlayerNameText = "";
		}
	}

	public void DeletePlayer()
	{
		DeleteMode = !DeleteMode;
	}

	private void OnKeyboard(Valve.VR.VREvent_t args) //(object[] args)
	{
		StringBuilder stringBuilder = new StringBuilder(256);
		SteamVR.instance.overlay.GetKeyboardText(stringBuilder, 256);
		PlayerNameText = stringBuilder.ToString(); 
	}

	private void OnKeyboardClosed(Valve.VR.VREvent_t args) //(object[] args)
	{
		//Debug.Log("Keyboard Closed");
		if(PlayerNameText != "")
		{
			PlayerData NewPlayer = new PlayerData();
			NewPlayer.PlayerName = PlayerNameText;
			PlayerManager.GetComponent<PlayerManager>().UserPlayerData.Add(NewPlayer);
			PlayerManager.GetComponent<PlayerManager>().SavePlayer();
			UpdatePlayers();
		}
		else
		{
			//Debug.Log("Player name text is null");
		}
		GetComponent<ControllerUI>().Init = true;
	}

	public void SaveNewPlayer (string PlayerNameString)
	{
		//Debug.Log("Keyboard Closed");
		if(PlayerNameString != "")
		{
			PlayerData NewPlayer = new PlayerData();
			NewPlayer.PlayerName = PlayerNameString;
			PlayerManager.GetComponent<PlayerManager>().UserPlayerData.Add(NewPlayer);
			PlayerManager.GetComponent<PlayerManager>().SavePlayer();
			UpdatePlayers();
		}
		else
		{
			//Debug.Log("Player name text is null");
		}
		GetComponent<ControllerUI>().Init = true;
	}

}
