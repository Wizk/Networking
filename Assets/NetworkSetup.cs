using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;

[AddComponentMenu ("Network/NetworkSetup")]
public class NetworkSetup : MonoBehaviour
{
	[SerializeField] public string onlineScenePath;
	[SerializeField] private GameObject player;

    private string serverName = "game";
    private string maxPlayer = "16";
    private string port = "25000";

    private Rect windowHost = new Rect(Screen.width - 300, 0, 300, Screen.height);
    private Rect windowError = new Rect(Screen.width / 2 - 150, Screen.height / 2 - 75, 300, 150);
    
	private bool serverError = false;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

	private void Start()
	{
		InitializeMasterServer ();
		MasterServer.RequestHostList("PVP");
	}

    private void OnGUI()
    {
        windowHost = GUI.Window(0, windowHost, WindowHost, "Servers");

        if (serverError)
        {
            windowError = GUI.Window(1, windowError, WindowError, "Connection Error");
        }

        if (Network.peerType == NetworkPeerType.Disconnected)
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

                /*ConnectionTesterStatus status = Network.TestConnection();
                print(status.ToString());*/

                Network.InitializeSecurity();
                Network.InitializeServer(int.Parse(maxPlayer), int.Parse(port), !haveNat);
                Debug.Log("Server started at " + Network.player.externalIP + " port " + Network.player.externalPort);

                MasterServer.RegisterHost("PVP", serverName);
            }
        }
        else
        {
            if (GUILayout.Button("Disconnect"))
            {
                Network.Disconnect();
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
				if (GUILayout.Button(data[i].gameName))
                {
					string serverIp = "";
					for(int j = 0; j < data[i].ip.Length; j++)
					{
						serverIp += data [i].ip [j] + " ";
					}

					Network.Connect(serverIp, data[i].port);
					Debug.Log ("Connect to host : " + serverIp + "; port : " + data[i].port);
                }
            }
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
}

[CustomEditor(typeof(NetworkSetup), true)]
public class NetworkSetupEditor : Editor
{
	private SerializedProperty playerProp;
	private SerializedProperty onlineScenePathProp;

	void OnEnable () 
	{
		playerProp = serializedObject.FindProperty ("player");
		onlineScenePathProp = serializedObject.FindProperty ("onlineScenePath");
	}

	public override void OnInspectorGUI()
	{
		SceneAsset oldScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(onlineScenePathProp.stringValue);

		serializedObject.Update();

		EditorGUILayout.PropertyField (playerProp);
		EditorGUI.BeginChangeCheck();
		SceneAsset newScene = EditorGUILayout.ObjectField("Online Scene", oldScene, typeof(SceneAsset), false) as SceneAsset;

		if (EditorGUI.EndChangeCheck())
		{
			string newPath = AssetDatabase.GetAssetPath(newScene);
			SerializedProperty scenePathProperty = serializedObject.FindProperty("onlineScenePath");
			scenePathProperty.stringValue = newPath;
		}

		serializedObject.ApplyModifiedProperties();
	}
}