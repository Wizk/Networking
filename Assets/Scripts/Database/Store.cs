using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : DataBase<Item>
{
	[SerializeField] private List<Item> items;

	private Rect storeWindowRect = new Rect(0, 0, 500, 400);

	private void Start()
	{
		LoadItems ();
	}

	private void LoadItems()
	{
		StartCoroutine (Query ("http://mafialaw.alwaysdata.net/store.php?action=listItems", items));	
	}

	private void OnGUI()
	{
		storeWindowRect = GUI.Window (8, storeWindowRect, WindowStore, "Store");
	}

	private string[] mainCategory = new string[] { "All", "Weapon", "Character", "Suit" };
	string[] weaponCategory = new string[] { "Weapon", "Weapon Skin", "Weapon Accessory" };

	private int mainToolBar = 0;
	private int weaponToolBar = 0;

	private void WindowStore(int id)
	{
		GUILayout.BeginHorizontal();
		GUILayout.BeginVertical();

		GUILayout.Label ("Sort by :");
		if (GUILayout.Button ("Cost")) {}
		if (GUILayout.Button ("Promotion")) {}

		GUILayout.EndVertical();
		GUILayout.BeginVertical();

		mainToolBar = GUILayout.Toolbar (mainToolBar, mainCategory);

		switch (mainToolBar) 
		{
		case 0:
			for (int i = 0; i < items.Count; i++) 
			{
				if (GUILayout.Button (ItemInfo(items[i])))
				{
					UserItem userItem = new UserItem (UserManager.Id, items[i].id, 0);
					StartCoroutine (Post ("http://mafialaw.alwaysdata.net/store.php?action=buyItem", items[i]));
				}
			}
			break;
		case 1:
			weaponToolBar = GUILayout.Toolbar (weaponToolBar, weaponCategory);
			switch (weaponToolBar) 
			{
			case 0:
				for (int i = 0; i < items.Count; i++) 
				{
					if (items[i].category == ItemCategory.Weapon && GUILayout.Button (ItemInfo(items[i])))
					{

					}
				}
				break;
			case 1:
				for (int i = 0; i < items.Count; i++) 
				{
					if (items[i].category == ItemCategory.WeaponSkin && GUILayout.Button (ItemInfo(items[i])))
					{

					}
				}
				break;
			case 2:
				for (int i = 0; i < items.Count; i++) 
				{
					if (items[i].category == ItemCategory.WeaponAccessory && GUILayout.Button (ItemInfo(items[i])))
					{

					}
				}
				break;
			}

			break;
		case 2 :
			for (int i = 0; i < items.Count; i++) 
			{
				if (items[i].category == ItemCategory.Character && GUILayout.Button (ItemInfo(items[i])))
				{

				}
			}
			break;
		case 3 :
			for (int i = 0; i < items.Count; i++) 
			{
				if (items[i].category == ItemCategory.Suit && GUILayout.Button (ItemInfo(items[i])))
				{

				}
			}
			break;
		}

		GUILayout.EndVertical();
		GUILayout.EndHorizontal();

		GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
	}

	private string ItemInfo(Item item)
	{
		if (item.stock > 0) 
		{
			return item.name + " cost : " + item.cost + (!item.payable ? " $" : " gem") + ((item.stock > 50) ? "" : (" Only " + item.stock + " left in stock !"));
		}
		else 
		{
			return item.name +" Sold out";
		}
	}
}

[System.Serializable]
public class Item
{
	public int id;
	public string name;
	public ItemCategory category;
	public int cost;
	public int sale;
	public bool payable;
	public int stock;

	public Item(string name, ItemCategory category, int cost, int sale, bool payable, int stock)
	{
		this.name = name;
		this.category = category;
		this.cost = cost;
		this.sale = sale;
		this.payable = payable;
		this.stock = stock;
	}
}

[System.Serializable]
public class UserItem
{
	public int idUser;
	public int idItem;
	public int stock;

	public UserItem(int idUser, int idItem, int stock)
	{
		this.idUser = idUser;
		this.idItem = idItem;
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