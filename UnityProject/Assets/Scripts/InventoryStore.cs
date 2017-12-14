using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventoryStore : MonoBehaviour {

	public bool Init = false;
	public GameObject UIManager;
	public GameObject PlayerManager;
	public GameObject Player;
	public GameObject StoreItemButton;
	public GameObject CartItemButton;
	public GameObject StoreButton;
	public Transform StoreCanvas;
	public GameObject Room; // this is where the inventory manager is
	public string RoomStateString;
	public GameObject RoomStore; // this is the store artwork
	public GameObject Text_AvailablePoints;
	public GameObject Text_Cart;
	public GameObject Text_Cost;
	public GameObject[] InventoryStoreItems = new GameObject[7];
	public GameObject[] Cart = new GameObject[10];
	public GameObject[] StoreButtonArray = new GameObject[8];
	public GameObject[] CartButtonArray = new GameObject[5];
	public GameObject[] ThreeDButtonArray = new GameObject[8];
	public int StoreItemButtonIndex = 0;
	public int CartItemButtonIndex = 0;
	public int TotalCost = 0;

	void Awake()
	{
		UIManager = GameObject.Find("UI_Manager_Prefab");
		PlayerManager = GameObject.Find("PlayerManager_Prefab");
		Player = GameObject.Find("Player_Prefab");
		UpdateStoreButtons();
	}

	// Use this for initialization
	void Start () {
	}

	public void Initialize()
	{
		Init = false;
		StoreItemButtonIndex = 0;
		CartItemButtonIndex = 0;
		EmptyCart();
		//UpdateStore();
		UpdateCart();

	}

	// Update is called once per frame
	void Update () {
		if(Init) Initialize();
		// update ui text
		int PlayerDolarsLeft = GetPlayerDollarsLeft();
		Text_AvailablePoints.GetComponent<Text>().text = PlayerDolarsLeft.ToString();
		int NumCartItems = GetNumItemsInCart();
		Text_Cart.GetComponent<Text>().text = NumCartItems.ToString() + " of " + Cart.Length.ToString();
		TotalCost = GetTotalCost();
		Text_Cost.GetComponent<Text>().text = TotalCost.ToString();
	}

	public int GetNumItemsInCart()
	{
		int count = 0;
		for(int i = 0; i < Cart.Length; i ++)
		{
			if (Cart[i] != null)
			{
				count++;
			}
		}
		return count;
	}

	public int GetPlayerDollarsLeft()
	{
		TotalCost = GetTotalCost();
		//int PlayerDollarsLeft =  Player.GetComponent<Player>().Dollars - TotalCost;
		int PlayerDollarsLeft =  PlayerManager.GetComponent<PlayerManager>().CurrentPlayer.PlayerDollars - TotalCost;
		return PlayerDollarsLeft;
	}

	public int GetTotalCost()
	{
		int Total = 0;
		for(int i = 0; i < Cart.Length; i ++)
		{
			if (Cart[i] != null)
			{
				Total+= Cart[i].GetComponent<InventoryItem>().ItemCost;
			}
		}
		return Total;
	}

	/*
	public void UpdateStore()
	{
		ClearStoreButtons();
		PopulateStoreButtons();
	}
	*/

	public void UpdateStoreButtons()
	{
		for (int i = 0; i < InventoryStoreItems.Length; i++)
		{
			if(InventoryStoreItems[i] != null)
			{
				if(InventoryStoreItems[i].GetComponent<InventoryItem>().StoreButton != null)
				{
					StoreButton = InventoryStoreItems[i].GetComponent<InventoryItem>().StoreButton;
					GameObject NewStoreButton = Instantiate(StoreButton) as GameObject;
					NewStoreButton.transform.position = ThreeDButtonArray[i].transform.position;
					NewStoreButton.transform.rotation = ThreeDButtonArray[i].transform.rotation;

					NewStoreButton.tag = "UIButton";
					NewStoreButton.AddComponent<BoxCollider>();
					NewStoreButton.AddComponent<MenuButton>();

					NewStoreButton.name = "Button_" + InventoryStoreItems[i].GetComponent<InventoryItem>().ItemName;
					NewStoreButton.GetComponent<MenuButton>().State = "StoreItem";
					NewStoreButton.GetComponent<MenuButton>().Data = (i).ToString();
					NewStoreButton.transform.parent = RoomStore.transform;
				}
			}
		}
	}

	/*
	public void ClearStoreButtons()
	{
		// clear store buttons
		for(int i = 0; i < StoreButtonArray.Length; i++)
		{
			DestroyImmediate(StoreButtonArray[i]);
		}
		StoreButtonArray = new GameObject[StoreButtonArray.Length]; // keep the same length array
	}
	/*
	 * 
	/*
	public void PopulateStoreButtons()
	{
		if(InventoryStoreItems.Length < StoreItemButtonIndex + StoreButtonArray.Length)
		{
			StoreItemButtonIndex = InventoryStoreItems.Length - StoreButtonArray.Length;
		}
		if(StoreItemButtonIndex < 0)
		{
			StoreItemButtonIndex = 0;
		}

		// populate buttons
		for (int i = 0; i < StoreButtonArray.Length; i++)
		{
			if ((StoreItemButtonIndex + i) < (InventoryStoreItems.Length)) // make sure there are enough sroe items.
			{
				GameObject NewButton = Instantiate(StoreItemButton) as GameObject;
				StoreButtonArray[i] = NewButton;
				NewButton.name = "Button_" + InventoryStoreItems[StoreItemButtonIndex+i].GetComponent<InventoryItem>().ItemName;
				NewButton.GetComponent<MenuButton>().Data = (StoreItemButtonIndex+i).ToString();
				NewButton.transform.Find("Text_ItemName").GetComponent<Text>().text = InventoryStoreItems[StoreItemButtonIndex+i].GetComponent<InventoryItem>().ItemName;
				NewButton.transform.Find("Text_ItemCost").GetComponent<Text>().text = InventoryStoreItems[StoreItemButtonIndex+i].GetComponent<InventoryItem>().ItemCost.ToString();
				NewButton.transform.Find("Text_ItemDescription").GetComponent<Text>().text = InventoryStoreItems[StoreItemButtonIndex+i].GetComponent<InventoryItem>().ItemDescription.ToString();
				NewButton.transform.Find("Image_Icon").GetComponent<Image>().sprite = InventoryStoreItems[StoreItemButtonIndex+i].GetComponent<InventoryItem>().ItemIcon;
				NewButton.transform.SetParent(StoreCanvas);

				NewButton.transform.localScale = Vector3.one;
				float Xpos = (-500f / 5f); //(float)((i* NewButton.GetComponent<RectTransform>().sizeDelta.x) - (200f * 4f) );
				//float Ypos = (35 - (float)((i* NewButton.GetComponent<RectTransform>().sizeDelta.y) - (100f * 7f)) / 5f);
				float Ypos = (35 - (float)((i* 200) - (100f * 7f)) / 5f);
				Vector3 NewPosition = new Vector3(Xpos/100f, Ypos/100f, 3f);
				NewButton.transform.position= NewPosition;
				NewButton.SetActive(true);
			}
		}
	}
	*/

	/*
	public void ItemPageUp()
	{
		StoreItemButtonIndex -= StoreButtonArray.Length;
		UpdateStore();
	}

	public void ItemPageDown()
	{
		StoreItemButtonIndex += StoreButtonArray.Length;
		UpdateStore();
	}
	*/

	public void UpdateCart()
	{
		ClearCartButtons();
		PopulateCartButtons();
	}

	public void ClearCartButtons()
	{
		// clear cart buttons
		for(int i = 0; i < CartButtonArray.Length; i++)
		{
			DestroyImmediate(CartButtonArray[i]);
		}
		CartButtonArray = new GameObject[CartButtonArray.Length]; // keep the same length array
	}

	public void PopulateCartButtons()
	{
		if(Cart.Length < CartItemButtonIndex + CartButtonArray.Length)
		{
			CartItemButtonIndex = Cart.Length - CartButtonArray.Length;
		}
		if(CartItemButtonIndex < 0)
		{
			CartItemButtonIndex = 0;
		}

		// populate buttons left column
		for (int i = 0 ; i < CartButtonArray.Length; i++)
		{
			if(Cart[CartItemButtonIndex+i] != null)
			{
				GameObject NewButton = Instantiate(CartItemButton) as GameObject;
				CartButtonArray[i] = NewButton;
				NewButton.name = "Button_" + Cart[CartItemButtonIndex+i].GetComponent<InventoryItem>().ItemName;
				NewButton.GetComponent<MenuButton>().Data = (CartItemButtonIndex+i).ToString();

				NewButton.transform.Find("Text_ItemName").GetComponent<Text>().text = Cart[CartItemButtonIndex+i].GetComponent<InventoryItem>().ItemName;
				NewButton.transform.Find("Text_ItemCost").GetComponent<Text>().text = Cart[CartItemButtonIndex+i].GetComponent<InventoryItem>().ItemCost.ToString();
				NewButton.transform.Find("Image_Icon").GetComponent<Image>().sprite = Cart[CartItemButtonIndex+i].GetComponent<InventoryItem>().ItemIcon;
				NewButton.transform.SetParent(StoreCanvas);

				NewButton.transform.localScale = Vector3.one;
				float Xpos = (0f / 5f); //(float)((i* NewButton.GetComponent<RectTransform>().sizeDelta.x) - (200f * 4f) );
				//float Ypos = (25 - (float)((i* NewButton.GetComponent<RectTransform>().sizeDelta.y) - (100f * 7f)) / 5f);
				float Ypos = (25 - (float)((i* 100) - (100f * 9f)) / 5f);
				Vector3 NewPosition = new Vector3(Xpos/100f, Ypos/100f, 3f);
				NewButton.transform.position= NewPosition;
				NewButton.SetActive(true);
			}
		}
		/*
		// populate buttons right column
		for (int i = CartButtonArray.Length/2 ; i < CartButtonArray.Length; i++)
		{
			if(Cart[CartItemButtonIndex+i] != null)
			{
				GameObject NewButton = Instantiate(CartItemButton) as GameObject;
				CartButtonArray[i] = NewButton;
				NewButton.name = "Button_" + Cart[CartItemButtonIndex+i].GetComponent<InventoryItem>().ItemName;
				NewButton.GetComponent<MenuButton>().Data = (CartItemButtonIndex+i).ToString();

				NewButton.transform.Find("Text_ItemName").GetComponent<Text>().text = Cart[CartItemButtonIndex+i].GetComponent<InventoryItem>().ItemName;
				NewButton.transform.Find("Text_ItemCost").GetComponent<Text>().text = Cart[CartItemButtonIndex+i].GetComponent<InventoryItem>().ItemCost.ToString();
				NewButton.transform.Find("Image_Icon").GetComponent<Image>().sprite = Cart[CartItemButtonIndex+i].GetComponent<InventoryItem>().ItemIcon;
				NewButton.transform.SetParent(StoreCanvas);

				NewButton.transform.localScale = Vector3.one;
				float Xpos = (500f / 5f); //(float)((i* NewButton.GetComponent<RectTransform>().sizeDelta.x) - (200f * 4f) );
				//float Ypos = (25 - (float)((i* NewButton.GetComponent<RectTransform>().sizeDelta.y) - (100f * 7f)) / 5f);
				float Ypos = (25 - (float)(((i-CartButtonArray.Length/2)* 100) - (100f * 7f)) / 5f);
				Vector3 NewPosition = new Vector3(Xpos/100f, Ypos/100f, 3f);
				NewButton.transform.position= NewPosition;
				NewButton.SetActive(true);
			}
		}
		*/
	}

	public void CartPageUp()
	{
		CartItemButtonIndex -= CartButtonArray.Length;
		UpdateCart();
	}

	public void CartPageDown()
	{
		CartItemButtonIndex += CartButtonArray.Length;
		UpdateCart();
	}


	public void AddItemToCart(string ItemNumber)
	{
		// can the player afford it?
		int PlayerDolarsLeft = GetPlayerDollarsLeft();
		if (PlayerDolarsLeft >= InventoryStoreItems[int.Parse(ItemNumber)].GetComponent<InventoryItem>().ItemCost)
		{
			// is the cart full?
			int NextEmptyItemIndex = FindNextEmptyCartItem();
			if(NextEmptyItemIndex >= 0)
			{
				// add the item
				CartItemButtonIndex = NextEmptyItemIndex - 4;
				Cart[NextEmptyItemIndex] = InventoryStoreItems[int.Parse(ItemNumber)];
				UpdateCart();
			}
			else
			{
				Debug.Log("Cart is full");
			}
		}
	}

	public void DeleteItemFromCart(string ItemNumberString)
	{
		// remove the item
		int ItemNumber = int.Parse(ItemNumberString);
		Cart[ItemNumber] = null;

		// clean up the cart
		GameObject[] tempCart = new GameObject[Cart.Length];
		int IndexNotNull = -1;
		for(int i = 0; i < Cart.Length; i++)
		{
			if(Cart[i] != null)
			{
				IndexNotNull++;
				tempCart[IndexNotNull] = Cart[i];
			}
		}
		Cart = tempCart;

		UpdateCart();
	}

	public int FindNextEmptyCartItem()
	{
		int index = -1;
		for(int i = 0; i < Cart.Length; i++)
		{
			if (Cart[i] == null)
			{
				index = i;
				return index;
			}
		}
		return index;
	}

	public void EmptyCart()
	{
		for(int i = 0; i <= Cart.Length; i++)
		{
			if (Cart[0] != null)
			{
				DeleteItemFromCart("0".ToString());
			}
		}
	}

	public int FindItemNumberByName(string ItemName)
	{
		int ItemNumber = -1;
		for(int i = 0; i < InventoryStoreItems.Length; i++)
		{
			if(InventoryStoreItems[i] != null) // store item may be empty
			{
				if(InventoryStoreItems[i].name.ToString() == ItemName)
				{
					ItemNumber = i;
				}
			}
		}
		return ItemNumber;
	}

	public void RepeatLastPurchase()
	{
		EmptyCart();
		if(PlayerManager.GetComponent<PlayerManager>().CurrentPlayer.CartItem01 != "")
		{
			int ItemNumber = FindItemNumberByName(PlayerManager.GetComponent<PlayerManager>().CurrentPlayer.CartItem01);
			if(ItemNumber >= 0)	AddItemToCart(ItemNumber.ToString());
		}
		if(PlayerManager.GetComponent<PlayerManager>().CurrentPlayer.CartItem02 != "")
		{
			int ItemNumber = FindItemNumberByName(PlayerManager.GetComponent<PlayerManager>().CurrentPlayer.CartItem02);
			if(ItemNumber >= 0) AddItemToCart(ItemNumber.ToString());
		}
		if(PlayerManager.GetComponent<PlayerManager>().CurrentPlayer.CartItem03 != "")
		{
			int ItemNumber = FindItemNumberByName(PlayerManager.GetComponent<PlayerManager>().CurrentPlayer.CartItem03);
			if(ItemNumber >= 0) AddItemToCart(ItemNumber.ToString());
		}
		if(PlayerManager.GetComponent<PlayerManager>().CurrentPlayer.CartItem04 != "")
		{
			int ItemNumber = FindItemNumberByName(PlayerManager.GetComponent<PlayerManager>().CurrentPlayer.CartItem04);
			if(ItemNumber >= 0) AddItemToCart(ItemNumber.ToString());
		}
		if(PlayerManager.GetComponent<PlayerManager>().CurrentPlayer.CartItem05 != "")
		{
			int ItemNumber = FindItemNumberByName(PlayerManager.GetComponent<PlayerManager>().CurrentPlayer.CartItem05);
			if(ItemNumber >= 0) AddItemToCart(ItemNumber.ToString());
		}
		if(PlayerManager.GetComponent<PlayerManager>().CurrentPlayer.CartItem06 != "")
		{
			int ItemNumber = FindItemNumberByName(PlayerManager.GetComponent<PlayerManager>().CurrentPlayer.CartItem06);
			if(ItemNumber >= 0) AddItemToCart(ItemNumber.ToString());
		}
		if(PlayerManager.GetComponent<PlayerManager>().CurrentPlayer.CartItem07 != "")
		{
			int ItemNumber = FindItemNumberByName(PlayerManager.GetComponent<PlayerManager>().CurrentPlayer.CartItem07);
			if(ItemNumber >= 0) AddItemToCart(ItemNumber.ToString());
		}
		if(PlayerManager.GetComponent<PlayerManager>().CurrentPlayer.CartItem08 != "")
		{
			int ItemNumber = FindItemNumberByName(PlayerManager.GetComponent<PlayerManager>().CurrentPlayer.CartItem08);
			if(ItemNumber >= 0) AddItemToCart(ItemNumber.ToString());
		}
		if(PlayerManager.GetComponent<PlayerManager>().CurrentPlayer.CartItem09 != "")
		{
			int ItemNumber = FindItemNumberByName(PlayerManager.GetComponent<PlayerManager>().CurrentPlayer.CartItem09);
			if(ItemNumber >= 0) AddItemToCart(ItemNumber.ToString());
		}
		if(PlayerManager.GetComponent<PlayerManager>().CurrentPlayer.CartItem10 != "")
		{
			int ItemNumber = FindItemNumberByName(PlayerManager.GetComponent<PlayerManager>().CurrentPlayer.CartItem10);
			if(ItemNumber >= 0) AddItemToCart(ItemNumber.ToString());
		}
	}

	public void SavePlayerCart()
	{
		if(Cart[0] != null)
		{
			PlayerManager.GetComponent<PlayerManager>().CurrentPlayer.CartItem01 = Cart[0].name.ToString();
		}
		else
		{
			PlayerManager.GetComponent<PlayerManager>().CurrentPlayer.CartItem01 = "";
		}
		if(Cart[1] != null)
		{
			PlayerManager.GetComponent<PlayerManager>().CurrentPlayer.CartItem02 = Cart[1].name.ToString();
		}
		else
		{
			PlayerManager.GetComponent<PlayerManager>().CurrentPlayer.CartItem02 = "";
		}
		if(Cart[2] != null)
		{
			PlayerManager.GetComponent<PlayerManager>().CurrentPlayer.CartItem03 = Cart[2].name.ToString();
		}
		else
		{
			PlayerManager.GetComponent<PlayerManager>().CurrentPlayer.CartItem03 = "";
		}
		if(Cart[3] != null)
		{
			PlayerManager.GetComponent<PlayerManager>().CurrentPlayer.CartItem04 = Cart[3].name.ToString();
		}
		else
		{
			PlayerManager.GetComponent<PlayerManager>().CurrentPlayer.CartItem04 = "";
		}
		if(Cart[4] != null)
		{
			PlayerManager.GetComponent<PlayerManager>().CurrentPlayer.CartItem05 = Cart[4].name.ToString();
		}
		else
		{
			PlayerManager.GetComponent<PlayerManager>().CurrentPlayer.CartItem05 = "";
		}
		if(Cart[5] != null)
		{
			PlayerManager.GetComponent<PlayerManager>().CurrentPlayer.CartItem06 = Cart[5].name.ToString();
		}
		else
		{
			PlayerManager.GetComponent<PlayerManager>().CurrentPlayer.CartItem06 = "";
		}
		if(Cart[6] != null)
		{
			PlayerManager.GetComponent<PlayerManager>().CurrentPlayer.CartItem07 = Cart[6].name.ToString();
		}
		else
		{
			PlayerManager.GetComponent<PlayerManager>().CurrentPlayer.CartItem07 = "";
		}
		if(Cart[7] != null)
		{
			PlayerManager.GetComponent<PlayerManager>().CurrentPlayer.CartItem08 = Cart[7].name.ToString();
		}
		else
		{
			PlayerManager.GetComponent<PlayerManager>().CurrentPlayer.CartItem08 = "";
		}
		if(Cart[8] != null)
		{
			PlayerManager.GetComponent<PlayerManager>().CurrentPlayer.CartItem09 = Cart[8].name.ToString();
		}
		else
		{
			PlayerManager.GetComponent<PlayerManager>().CurrentPlayer.CartItem09 = "";
		}
		if(Cart[9] != null)
		{
			PlayerManager.GetComponent<PlayerManager>().CurrentPlayer.CartItem10 = Cart[9].name.ToString();
		}
		else
		{
			PlayerManager.GetComponent<PlayerManager>().CurrentPlayer.CartItem10 = "";
		}
	}

	public void CheckOut()
	{
		SavePlayerCart();
		TotalCost = GetTotalCost();
		//Player.GetComponent<Player>().Dollars = Player.GetComponent<Player>().Dollars - TotalCost;
		PlayerManager.GetComponent<PlayerManager>().CurrentPlayer.PlayerDollars =  PlayerManager.GetComponent<PlayerManager>().CurrentPlayer.PlayerDollars - TotalCost;
		PlayerManager.GetComponent<PlayerManager>().SavePlayer();
		Room.GetComponent<InventoryManager>().InventorySourceArray = Cart;
	}
}
