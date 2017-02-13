using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : DataBase<Item>
{
	private Rect storeWindowRect = new Rect(0, 0, 500, 400);

	private void OnGUI()
	{
		storeWindowRect = GUI.Window (8, storeWindowRect, WindowStore, "Store");
	}

	private string[] str = new string[] { "All", "Weapon", "Character", "Suit" };
	private int toolBar = 0;

	private void WindowStore(int id)
	{
		GUILayout.BeginHorizontal();
		GUILayout.BeginVertical();

		GUILayout.Label ("Sort by :");
		if (GUILayout.Button ("Cost")) {}

		GUILayout.EndVertical();
		GUILayout.BeginVertical();

		toolBar = GUILayout.Toolbar (toolBar, str);

		switch (toolBar) 
		{
		case 0 :
			break;
		case 1:
			
			string[] str2 = new string[] { "Weapon Skin", "Weapon Accessory" };

			switch (GUILayout.Toolbar (0, str2)) 
			{
			case 1 :
				break;
			case 2:
				break;
			}

			break;
		case 2 :
			break;
		case 3 :
			break;
		}

		GUILayout.EndVertical();
		GUILayout.EndHorizontal();

		GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
	}
}

[System.Serializable]
public class Item
{
	public ItemCategory category;
	public int cost;
	public int sale;
	public bool payable;
	public int stock;

	public Item(ItemCategory category, int cost, int sale, bool payable, int stock)
	{
		this.category = category;
		this.cost = cost;
		this.sale = sale;
		this.payable = payable;
		this.stock = stock;
	}
}

[System.Serializable]
public enum ItemCategory
{
	Weapon = 0,
	WeaponSkin = 1,
	WeaponAccessory = 2,
	Character = 3,
	Suit = 4
}