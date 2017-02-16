using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
	private Rect inventoryWindowRect = new Rect();

	private void Awake()
	{
		DontDestroyOnLoad (this);	
	}

	public void Start()
	{
		DataBase.Post("http://mafialaw.alwaysdata.net/store.php?action=buyItem", new UserItem (UserManager.UserInfo.Id, 1, 100));
	}

	private void OnGUI()
	{
		
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