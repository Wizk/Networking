using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Networking : MonoBehaviour
{
    private string sessionName = "game";
    private string maxPlayer = "16";
    private string port = "25004";

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    private void OnGUI()
    {
        if (!Network.isServer)
        {
            GUILayout.Label("Session name : ");
            sessionName = GUILayout.TextField(sessionName);

            GUILayout.Label("Max player : ");
            maxPlayer = GUILayout.TextField(maxPlayer);

            GUILayout.Label("Port : ");
            port = GUILayout.TextField(port);

            if (GUILayout.Button("Host"))
            {
                Network.InitializeSecurity();
                Network.InitializeServer(int.Parse(maxPlayer), int.Parse(port), !Network.HavePublicAddress());
                Debug.Log("Server started at " + Network.player.externalIP + " port " + Network.player.externalPort);

                /*MasterServer.ipAddress = "127.0.0.1";
                MasterServer.port = 25000;*/
                MasterServer.RegisterHost("PVP", sessionName);
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
}
