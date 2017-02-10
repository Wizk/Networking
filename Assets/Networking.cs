using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Networking : MonoBehaviour
{
    private string serverName = "game";
    private string maxPlayer = "16";
    private string port = "25566";

    private Rect windowHost = new Rect(Screen.width - 300, 0, 300, Screen.height);
    private Rect windowError = new Rect(Screen.width / 2 - 150, Screen.height / 2 - 75, 300, 150);
    private bool serverError = false;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
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

                /*MasterServer.ipAddress = "127.0.0.1";
                MasterServer.port = 25000;*/
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
                GUILayout.Box(data[i].gameName);
                if (GUILayout.Button("Connect"))
                {
                    Network.Connect(data[i]);
                }
            }
        }
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
}
