using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserManager : DataBase<User> 
{
	private string email = "";
	private string username = "";
	private string password = "";

	private Rect loginWindowRect = new Rect (Screen.width / 2 - 90, Screen.height / 2 - 100, 180, 200);

	private void OnGUI()
	{
		GUI.Window (3, loginWindowRect, WindowLogin, "Login");
	}

	private void WindowLogin(int id)
	{
		GUILayout.Space (10);

		GUILayout.Label ("Username :");
		username = GUILayout.TextField (username);

		GUILayout.Label ("Password :");
		password = GUILayout.PasswordField (password, '*');

		GUILayout.Space (40);

		if (GUILayout.Button ("Connect")) 
		{
			User user = new User (email, username, password);
			Post ("mafialaw.alwaysdata.net/signin.php?action=signin", user);
		}

		GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
	}
}

[System.Serializable]
public class User
{
	private string email = "";
	private string username = "";
	private string password = "";

	public User(string email, string username, string password)
	{
		this.email = email;
		this.username = username;
		this.password = password;
	}
}
