using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserManager : DataBase<User> 
{
	private static int id;
	private static string email = "";
	private static string username = "";
	private static string password = "";

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
		username = GUILayout.TextField (username);

		GUILayout.Label ("Password :");
		password = GUILayout.PasswordField (password, '*');

		if (GUILayout.Button ("Connect")) 
		{
			StartCoroutine(Connect ());
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
		email = GUILayout.TextField (email);

		GUILayout.Label ("*Username :");
		username = GUILayout.TextField (username);

		GUILayout.Label ("*Password :");
		password = GUILayout.PasswordField (password, '*');

		GUILayout.Space (5);
		if (GUILayout.Button ("Sign in")) 
		{
			StartCoroutine (Signin ());
		}

		GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
	}

	private void WindowUserPanel(int id)
	{
		GUILayout.Label ("Username : " + Username);
		//GUILayout.Label("Gold : " + gold);
		if (GUILayout.Button ("Log out")) 
		{
			StartCoroutine (Logout ());
		}
	}

	private IEnumerator Connect()
	{
		User user = new User (email, username, password);
		yield return StartCoroutine(CheckQuery("http://mafialaw.alwaysdata.net/user.php?action=login", user, value => Connected = value, value => connectionError = value));
		//TODO
			
		if (!connected) { registered = true; }
	}

	private IEnumerator Signin()
	{
		bool isValid = false;

		User user = new User (email, username, password);

		yield return StartCoroutine(CheckQuery ("http://mafialaw.alwaysdata.net/user.php?action=signin", user, value => isValid = value, value => signinError = value));

		if (isValid) 
		{
			StartCoroutine (Connect ());
		}
	}

	private IEnumerator Logout()
	{
		//TODO

		email = "";
		username = "";
		password = "";		
		connectionError = "";

		
		Connected = false;

		yield return null;
	}

	public static string Username
	{
		get
		{
			return username;
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

	public static int Id
	{
		get
		{ 
			return id;
		}
	}
}

[System.Serializable]
public class User
{
	public int id;
	public string email;
	public string username;
	public string password;

	public User(string email, string username, string password)
	{
		this.email = email;
		this.username = username;
		this.password = password;
	}
}