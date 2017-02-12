using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserManager : DataBase<User> 
{
	private static string email = "";
	private static string username = "";
	private static string password = "";

	private Rect loginWindowRect = new Rect (Screen.width / 2 - 90, Screen.height / 2 - 100, 180, 200);
	private Rect signinWindowRect = new Rect (Screen.width / 2 - 90, Screen.height / 2 - 100, 180, 200);
	private Rect userPanelRect = new Rect (0, Screen.height - 180, 180, 200);

	private bool registered = true;
	private static bool connected = false;
	private string connectionError;

	private void Awake()
	{
		DontDestroyOnLoad(transform.gameObject);
	}

	private void OnGUI()
	{
		if (Connected) 
		{
			GUI.Window (3, userPanelRect, WindowUserPanel, "User Panel");
		}
		else
		{
			if (registered) 
			{
				GUI.Window (3, loginWindowRect, WindowLogin, "Login");
			}
			else 
			{
				GUI.Window (4, signinWindowRect, WindowSignin, "Sign in");
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
		GUILayout.Label ("*Email :");
		email = GUILayout.TextField (email);

		GUILayout.Label ("*Username :");
		username = GUILayout.TextField (username);

		GUILayout.Label ("*Password :");
		password = GUILayout.PasswordField (password, '*');

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
		GUILayout.Space (20);
	}

	private IEnumerator Connect()
	{
		User user = new User (email, username, password);
		yield return StartCoroutine(CheckQuery("http://mafialaw.alwaysdata.net/user.php?action=login", user, value => Connected = value));

		if (!connected) 
		{
			connectionError = "Invalid credential";
			yield break;
		}

		registered = true;
	}

	private IEnumerator Signin()
	{
		User user = new User (email, username, password);
		yield return StartCoroutine(Post ("http://mafialaw.alwaysdata.net/user.php?action=signin", user));
		StartCoroutine(Connect ());
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
}

[System.Serializable]
public class User
{
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