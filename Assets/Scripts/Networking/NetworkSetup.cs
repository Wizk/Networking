using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

#pragma warning disable 618

[AddComponentMenu ("Network/NetworkSetup")]
public class NetworkSetup : NetworkBehaviour
{
	[SerializeField] private string offlineScenePath;
	[SerializeField] private string onlineScenePath;
	[SerializeField] private GameObject player;

	private NetworkManager mana;
    private string serverName = "game";
    private string maxPlayer = "16";
    private string port = "25000";

	private Rect windowHost = new Rect(Screen.width - 300, 100, 300, Screen.height - 100);
	private Rect windowServer = new Rect(300, 100, 300, Screen.height - 100);
    private Rect windowError = new Rect(Screen.width / 2 - 150, Screen.height / 2 - 75, 300, 150);
    
	private bool serverError = false;

    private void Awake()
    {
		DontDestroyOnLoad(this);
    }

	private void Start()
	{
		InitializeMasterServer ();
		MasterServer.RequestHostList("PVP");
	}

    private void OnGUI()
    {
		//if (!UserManager.Connected) { return; }

        if (serverError)
        {
            windowError = GUI.Window(1, windowError, WindowError, "Connection Error");
        }

        if (Network.peerType == NetworkPeerType.Disconnected)
        {
			windowHost = GUI.Window(0, windowHost, WindowHost, "Join Server");
			windowServer = GUI.Window(9, windowServer, WindowServer, "Creat Server");
        }
        else
        {
            if (GUILayout.Button("Disconnect"))
            {
                Network.Disconnect();
				StartCoroutine(LoadOfflineSceneAsync());
            }
        }
    }

    private void WindowHost(int id)
    {
        if (GUILayout.Button("Refresh"))
        {
            MasterServer.RequestHostList("PVP");
        }

        GUILayout.BeginHorizontal();
        GUILayout.Box("Server name");
        GUILayout.EndHorizontal();

        if (MasterServer.PollHostList().Length != 0)
        {
            HostData[] data = MasterServer.PollHostList();

            for (int i = 0; i < data.Length; i++)
            {
				if (GUILayout.Button(data[i].gameType + " " + data[i].gameName + " " + data[i].connectedPlayers + " / " + data[i].playerLimit + " " + (data[i].passwordProtected ? "locked" : "open")))
                {
					string serverIp = "";
					for(int j = 0; j < data[i].ip.Length; j++)
					{
						serverIp += data [i].ip [j] + " ";
					}

					StartCoroutine (Connect (serverIp, data [i].port));
					Debug.Log ("Connect to host : " + serverIp + "; port : " + data[i].port);
                }
            }
        }

		GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
    }

	private void WindowServer(int id)
	{
		GUILayout.Label("Server name : ");
		serverName = GUILayout.TextField(serverName);

		GUILayout.Label("Max player : ");
		maxPlayer = GUILayout.TextField(maxPlayer);

		GUILayout.Label("Port : ");
		port = GUILayout.TextField(port);

		if (GUILayout.Button("Creat server"))
		{
			bool haveNat = Network.HavePublicAddress();

			Network.InitializeSecurity();
			Network.InitializeServer (int.Parse (maxPlayer), int.Parse (port), !haveNat);

			MasterServer.RegisterHost("PVP", serverName);

			StartCoroutine (LoadOnlineSceneAsync ());
			Debug.Log("Server started at " + Network.player.externalIP + " port " + Network.player.externalPort);
		}

		GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
	}

    private void WindowError(int id)
    {
        GUILayout.Label("No network connection");

        if (GUILayout.Button("ok"))
        {
            Network.Disconnect();
            serverError = false;
        }

        GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
    }

	private void InitializeMasterServer()
	{
		MasterServer.ipAddress = "88.173.176.162"; 
		MasterServer.port = 23466;
	}

	private IEnumerator LoadOfflineSceneAsync()
	{
		AsyncOperation loadAsync = SceneManager.LoadSceneAsync(offlineScenePath);

		while (!loadAsync.isDone) 
		{
			yield return null;
		}
	}

	[RPC]
	private IEnumerator LoadOnlineSceneAsync()
	{
		Network.SetSendingEnabled (0, false);
		Network.isMessageQueueRunning = false;
		Network.SetLevelPrefix (1);

		AsyncOperation loadAsync = SceneManager.LoadSceneAsync(onlineScenePath);

		while (!loadAsync.isDone) 
		{
			yield return null;
		}

		Network.isMessageQueueRunning = true;
		Network.SetSendingEnabled (0, true);

		CmdSpawn (player, new Vector3 (Random.Range (-5, 5), Random.Range (-5, 5), Random.Range (-5, 5)));
		//Network.Instantiate(player, new Vector3(Random.Range(-5,5), Random.Range(-5,5), Random.Range(-5,5)), Quaternion.identity, 0);
	}

	[Command]
	private void CmdSpawn(GameObject go, Vector3 position)
	{
		go.transform.position = position;
		NetworkServer.Spawn (go);
	}

	private IEnumerator Connect(string ip, int port)
	{
		Network.Connect(ip, port);

		while (Network.peerType != NetworkPeerType.Client) 
		{
			yield return null;
		}

		StartCoroutine (LoadOnlineSceneAsync ());
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(NetworkSetup), true)]
public class NetworkSetupEditor : Editor
{
	private SerializedProperty offlineScenePathProp;
	private SerializedProperty onlineScenePathProp;
	private SerializedProperty playerProp;

	void OnEnable () 
	{
		offlineScenePathProp = serializedObject.FindProperty ("offlineScenePath");
		onlineScenePathProp = serializedObject.FindProperty ("onlineScenePath");
		playerProp = serializedObject.FindProperty ("player");
	}

	public override void OnInspectorGUI()
	{
		SceneAsset oldOfflineScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(offlineScenePathProp.stringValue);
		SceneAsset oldOnlineScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(onlineScenePathProp.stringValue);

		serializedObject.Update();

		EditorGUILayout.PropertyField (playerProp);

		EditorGUI.BeginChangeCheck();
		SceneAsset newOfflineScene = EditorGUILayout.ObjectField("Offline Scene", oldOfflineScene, typeof(SceneAsset), false) as SceneAsset;
		SceneAsset newOnlineScene = EditorGUILayout.ObjectField("Online Scene", oldOnlineScene, typeof(SceneAsset), false) as SceneAsset;

		if (EditorGUI.EndChangeCheck())
		{
			string newOfflinePath = AssetDatabase.GetAssetPath(newOfflineScene);
			offlineScenePathProp.stringValue = newOfflinePath;

			string newOnlinePath = AssetDatabase.GetAssetPath(newOnlineScene);
			onlineScenePathProp.stringValue = newOnlinePath;
		}

		serializedObject.ApplyModifiedProperties();
	}
}
#endif