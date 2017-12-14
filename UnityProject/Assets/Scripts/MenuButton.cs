using UnityEngine;
using System.Collections;

public class MenuButton : MonoBehaviour {

	public string State = "";
	public string Data = "";
	public GameObject UIManager;
	void Awake()
	{
		UIManager = GameObject.Find("UI_Manager_Prefab");
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Clicked()
	{
		//print("Clicked");
		switch(State)
		{
		case"Home":
			UIManager.GetComponent<UI_Manager>().SwitchStates("HomeState");
			break;
		case"PlayerSelect":
			GameObject Home2 = UIManager.GetComponent<UI_Manager>().UIHome;
			Home2.GetComponent<ControllerUI>().DestroyControllers();
			UIManager.GetComponent<UI_Manager>().SwitchStates("PlayerSelectState");
			break;
		case"GoToStore":
			GameObject Home1 = UIManager.GetComponent<UI_Manager>().UIHome;
			Home1.GetComponent<ControllerUI>().DestroyControllers();
			UIManager.GetComponent<UI_Manager>().SwitchStates("InventoryStoreState");
			break;
		case"StartGame":
			GameObject Home = UIManager.GetComponent<UI_Manager>().UIHome;
			Home.GetComponent<ControllerUI>().DestroyControllers();
			UIManager.GetComponent<UI_Manager>().SwitchStates("Room_01State");
			break;
		case"DiedContinue":
			GameObject Died = UIManager.GetComponent<UI_Manager>().UIDied;
			Died.GetComponent<ControllerUI>().DestroyControllers();
			UIManager.GetComponent<UI_Manager>().SwitchStates("HomeState");
			break;
		case"StoreItem":
			GameObject UIInventoryStoreItem = GameObject.Find("UI_InventoryStore_Prefab");
			UIInventoryStoreItem.GetComponent<InventoryStore>().SendMessage("AddItemToCart", GetComponent<MenuButton>().Data);
			break;
		case"CartItem":
			GameObject UIInventoryStoreCartItem = GameObject.Find("UI_InventoryStore_Prefab");
			UIInventoryStoreCartItem.GetComponent<InventoryStore>().SendMessage("DeleteItemFromCart", GetComponent<MenuButton>().Data);
			break;
		case"StoreBack":
			GameObject UIInventoryStoreBack = UIManager.GetComponent<UI_Manager>().UIInventoryStore;
			UIInventoryStoreBack.GetComponent<ControllerUI>().DestroyControllers();
			UIManager.GetComponent<UI_Manager>().SwitchStates("HomeState");
			break;
		case"StoreRepeat":
			GameObject UIInventoryStoreRepeat = UIManager.GetComponent<UI_Manager>().UIInventoryStore;
			UIInventoryStoreRepeat.GetComponent<InventoryStore>().SendMessage("RepeatLastPurchase");
			break;
		case"StoreCheckOut":
			GameObject UIInventoryStoreCheckout = UIManager.GetComponent<UI_Manager>().UIInventoryStore;
			UIInventoryStoreCheckout.GetComponent<InventoryStore>().SendMessage("CheckOut");
			UIInventoryStoreCheckout.GetComponent<ControllerUI>().DestroyControllers();
			// new room state
			string NewRoomState = UIInventoryStoreCheckout.GetComponent<InventoryStore>().RoomStateString;
			UIManager.GetComponent<UI_Manager>().SwitchStates(NewRoomState);
			break;
		case"StoreItemPageUp":
			GameObject UIInventoryStoreItemPageUp = UIManager.GetComponent<UI_Manager>().UIInventoryStore;
			UIInventoryStoreItemPageUp.GetComponent<InventoryStore>().SendMessage("ItemPageUp");
			break;
		case"StoreItemPageDown":
			GameObject UIInventoryStoreItemPageDown = UIManager.GetComponent<UI_Manager>().UIInventoryStore;
			UIInventoryStoreItemPageDown.GetComponent<InventoryStore>().SendMessage("ItemPageDown");
			break;
		case"StoreCartPageUp":
			GameObject UIInventoryStoreCartPageUp = UIManager.GetComponent<UI_Manager>().UIInventoryStore;
			UIInventoryStoreCartPageUp.GetComponent<InventoryStore>().SendMessage("CartPageUp");
			break;
		case"StoreCartPageDown":
			GameObject UIInventoryStoreCartPageDown = UIManager.GetComponent<UI_Manager>().UIInventoryStore;
			UIInventoryStoreCartPageDown.GetComponent<InventoryStore>().SendMessage("CartPageDown");
			break;
		case"SelectPlayer":
			GameObject UISelectPlayerPlayerSelect = GameObject.Find("UI_PlayerSelect_Prefab");
			UISelectPlayerPlayerSelect.GetComponent<UI_SelectPlayer>().SendMessage("SelectPlayer", GetComponent<MenuButton>().Data);
			break;
		case"SelectPlayerPageUp":
			GameObject UISelectPlayerPageUp = UIManager.GetComponent<UI_Manager>().UIPlayerSelect;
			UISelectPlayerPageUp.GetComponent<UI_SelectPlayer>().SelectPlayerPageUp();
			break;
		case"SelectPlayerPageDown":
			GameObject UISelectPlayerPageDown = UIManager.GetComponent<UI_Manager>().UIPlayerSelect;
			UISelectPlayerPageDown.GetComponent<UI_SelectPlayer>().SelectPlayerPageDown();
			break;
		case"SelectPlayerBack":
			GameObject UISelectPlayerBack = UIManager.GetComponent<UI_Manager>().UIPlayerSelect;
			UISelectPlayerBack.GetComponent<ControllerUI>().DestroyControllers();
			UIManager.GetComponent<UI_Manager>().SwitchStates("HomeState");
			break;
		case"SelectPlayerNew":
			//GameObject UISelectPlayerNew = UIManager.GetComponent<UI_Manager>().UIPlayerSelect;
			//UISelectPlayerNew.GetComponent<UI_SelectPlayer>().CreatePlayer();
			GameObject UISelectPlayerNew = UIManager.GetComponent<UI_Manager>().UIPlayerSelect;
			UISelectPlayerNew.GetComponent<ControllerUI>().DestroyControllers();
			UIManager.GetComponent<UI_Manager>().SwitchStates("PlayerCreateState");
			break;
		case"SelectPlayerDelete":
			GameObject UISelectPlayerDelete = UIManager.GetComponent<UI_Manager>().UIPlayerSelect;
			UISelectPlayerDelete.GetComponent<UI_SelectPlayer>().DeletePlayer();
			break;
		case"PlayerCreateCancel":
			GameObject UIPlayerCreateBack = UIManager.GetComponent<UI_Manager>().UIPlayerCreate;
			UIPlayerCreateBack.GetComponent<ControllerUI>().DestroyControllers();
			UIManager.GetComponent<UI_Manager>().SwitchStates("PlayerSelectState");
			break;
		case"PlayerCreateOk":
			GameObject UIPlayerCreate = UIManager.GetComponent<UI_Manager>().UIPlayerCreate;
			GameObject UIPlayerCreateOk = UIManager.GetComponent<UI_Manager>().UIPlayerSelect;
			UIPlayerCreateOk.GetComponent<UI_SelectPlayer>().SaveNewPlayer(UIPlayerCreate.GetComponent<UI_PlayerCreate>().PlayerNameText);
			UIPlayerCreate.GetComponent<ControllerUI>().DestroyControllers();
			UIManager.GetComponent<UI_Manager>().SwitchStates("PlayerSelectState");
			break;
		case"GoToTutorial":
			GameObject Tutorial = UIManager.GetComponent<UI_Manager>().UIHome;
			Tutorial.GetComponent<ControllerUI>().DestroyControllers();
			UIManager.GetComponent<UI_Manager>().SwitchStates("TutorialState");
			break;
		case"TutorialBack":
			GameObject UITutorialBack = UIManager.GetComponent<UI_Manager>().UITutorial;
			UITutorialBack.GetComponent<UI_Tutorial>().Back();
			break;
		case"TutorialNext":
			GameObject UITutorialNext = UIManager.GetComponent<UI_Manager>().UITutorial;
			UITutorialNext.GetComponent<UI_Tutorial>().Next();
			break;
		case"SelectRoomBack":
			GameObject UISelectRoomBack = UIManager.GetComponent<UI_Manager>().UIRoomSelect;
			UISelectRoomBack.GetComponent<ControllerUI>().DestroyControllers();
			UIManager.GetComponent<UI_Manager>().SwitchStates("PlayerSelectState");
			break;
		case"SelectRoomRandom":
			GameObject UISelectRoomRandom = UIManager.GetComponent<UI_Manager>().UIRoomSelect;
			UISelectRoomRandom.GetComponent<UI_SelectRoom>().RandomRoom();
			break;
		case"SelectRoom":
			GameObject UISelectRoom = UIManager.GetComponent<UI_Manager>().UIRoomSelect;
			UISelectRoom.GetComponent<UI_SelectRoom>().SendMessage("SelectRoom", GetComponent<MenuButton>().Data);
			break;
		case"VRKeyboardButton":
			GameObject UIPlayerCreateKeyboard = UIManager.GetComponent<UI_Manager>().UIPlayerCreate;
			switch(GetComponent<MenuButton>().Data)
			{
			case"SH":
				// make keyboard uppercase
				UIPlayerCreateKeyboard.GetComponent<VRKeyboard>().ShiftButtonPressed();
				break;
			case "BS":
				// backspace
				int LastCharIndex = UIPlayerCreateKeyboard.GetComponent<UI_PlayerCreate>().PlayerNameText.Length - 1;
				if(LastCharIndex >= 0)
				{
					UIPlayerCreateKeyboard.GetComponent<UI_PlayerCreate>().PlayerNameText = UIPlayerCreateKeyboard.GetComponent<UI_PlayerCreate>().PlayerNameText.Remove(LastCharIndex);
				}
				break;
			default:
				UIPlayerCreateKeyboard.GetComponent<UI_PlayerCreate>().PlayerNameText += GetComponent<MenuButton>().Data;
				break;
			}
			break;
		}
	}
}
