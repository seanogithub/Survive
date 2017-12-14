using UnityEngine;
using System.Collections;

public class InventoryItem : MonoBehaviour {

	public InventoryStoreData ItemData = new InventoryStoreData();

	public string ItemName = "";
	public string ItemDescription = "";
	public int ItemCost = 0;
	public string ItemType = "";
	public Sprite ItemIcon;
	public GameObject StoreButton;

	void Awake()
	{
		ItemData.ItemName = ItemName;
		ItemData.ItemDescription = ItemDescription;
		ItemData.ItemCost = ItemCost;
		ItemData.ItemType = ItemType;
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
