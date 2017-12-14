using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class InventoryStoreData {

	public string ItemName = "";
	public string ItemDescription = "";
	public int ItemCost = 0;
	public string ItemType = "";

	public InventoryStoreData DeepCopy (InventoryStoreData data)
	{
		InventoryStoreData NewData = new InventoryStoreData();
		NewData.ItemName = data.ItemName;
		NewData.ItemDescription = data.ItemDescription;
		NewData.ItemCost = data.ItemCost;
		NewData.ItemType = data.ItemType;

		return NewData;
	}
}
