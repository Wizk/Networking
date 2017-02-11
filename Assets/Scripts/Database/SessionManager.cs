using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class SessionManager : DataBase<Session> 
{
	[SerializeField] private List<Session> sessions = new List<Session>();
	[SerializeField] private GameObject sessionUI;
	[SerializeField] private NetworkManager networkManager;

	private void Start()
	{
		ListSession ();
	}

	public void CreatSession(Session session)
	{
		Post ("mafialaw.alwaysdata.net/game.php?action=creatSession", session);
	}

	public void ListSession()
	{
		Query ("mafialaw.alwaysdata.net/game.php?action=listSession", ref sessions);
		StartCoroutine (ListSessionCoroutine ());
	}

	private IEnumerator ListSessionCoroutine()
	{
		while(sessions.Count == 0)
		{
			yield return null;
		}

		for(int i = 0; sessions.Count > i; i ++)
		{
			/*Instantiate (sessionUI, transform);

			SessionDisplayer session = sessionUI.GetComponent<SessionDisplayer> ();
			session.gameObject.name = sessions [i].name;
			session.CurrentSession = sessions [i];
			session.NetManager = networkManager;
			session.CurrentSession = sessions [i];*/
		}
	}
}

[System.Serializable]
public class Session
{
	public string name;
	public string ipAddress;
	public string hostUser;
	public string password;

	public Session(string name, string hostUser, string password)
	{
		this.name = name;
		this.ipAddress = Network.player.ipAddress;
		this.hostUser = hostUser;
		this.password = password;
	}
}