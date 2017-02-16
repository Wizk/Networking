using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserManager : MonoBehaviour
{
	private static User user = new User ();

	private Rect loginWindowRect = new Rect (Screen.width / 2 - 90, Screen.height / 2 - 100, 180, 200);
	private Rect signinWindowRect = new Rect (Screen.width / 2 - 90, Screen.height / 2 - 115, 180, 230);
	private Rect userPanelRect = new Rect (0, Screen.height - 100, 180, 100);

	private bool registered = true;
	private static bool connected = false;

	private string connectionError = "";
	private string signinError = "";

	private void Awake()
	{
		DontDestroyOnLoad(this);
	}

	private void OnGUI()
	{
		if (Connected) 
		{
			userPanelRect = GUI.Window (5, userPanelRect, WindowUserPanel, "User Panel");
		}
		else
		{
			if (registered) 
			{
				loginWindowRect = GUI.Window (3, loginWindowRect, WindowLogin, "Login");
			}
			else 
			{
				signinWindowRect = GUI.Window (4, signinWindowRect, WindowSignin, "Sign in");
			}
		}
	}

	private void WindowLogin(int id)
	{
		GUI.color = Color.red;
		GUILayout.Label (connectionError);
		GUI.color = Color.white;

		GUILayout.Label ("Username :");
		UserInfo.username = GUILayout.TextField (UserInfo.username);

		GUILayout.Label ("Password :");
		UserInfo.password = GUILayout.PasswordField (UserInfo.password, '*');

		if (GUILayout.Button ("Connect")) 
		{
			Login ();
		}

		if (GUILayout.Button ("Creat account")) 
		{
			registered = false;
		}

		GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
	}

	private void WindowSignin(int id)
	{
		GUI.color = Color.red;
		GUILayout.Label (signinError);
		GUI.color = Color.white;

		GUILayout.Label ("*Email :");
		UserInfo.email = GUILayout.TextField (UserInfo.email);

		GUILayout.Label ("*Username :");
		UserInfo.username = GUILayout.TextField (UserInfo.username);

		GUILayout.Label ("*Password :");
		UserInfo.password = GUILayout.PasswordField (UserInfo.password, '*');

		GUILayout.Space (5);
		if (GUILayout.Button ("Sign in")) 
		{
			Signin ();
		}

		GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
	}

	private void WindowUserPanel(int id)
	{
		GUILayout.Label ("Username : " + UserInfo.username);
		if (GUILayout.Button ("Log out")) 
		{
			Logout ();
		}
	}

	private void Login()
	{
		if (!DataBase.CheckQuery ("http://mafialaw.alwaysdata.net/user.php?action=login", user)) { connectionError = "Invalid credential"; }
		else 
		{ 
			Connected = true;
			registered = true;
		}
	}

	private void Signin()
	{
		if (DataBase.CheckQuery ("http://mafialaw.alwaysdata.net/user.php?action=signin", user, value => signinError = value)) { Login (); }
	}

	private void Logout()
	{
		user = new User ();	
		connectionError = "";
		Connected = false;
	}

	public static User UserInfo
	{
		get
		{
			return user;
		}
		private set
		{ 
			user = value;
		}
	}

	public static bool Connected
	{
		get
		{ 
			return connected;
		}
		private set
		{ 
			connected = value;
		}
	}
}

[System.Serializable]
public class User
{
	private int id = 0;
	public string email;
	public string username;
	public string password;

	public User(string email, string username, string password)
	{
		this.email = email;
		this.username = username;
		this.password = password;
	}

	public User()
	{
		email = "";
		username = "";
		password = "";
	}

	public int Id
	{
		get
		{ 
			return id;
		}
		set
		{
			id = value;
		}
	}
}