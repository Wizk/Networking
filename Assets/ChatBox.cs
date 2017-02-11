using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 618

public class ChatBox : MonoBehaviour
{
	[SerializeField] private GUIStyle style;

    private string msgs = "";
	private string msgToSend = "";

    private Rect windowChatBox = new Rect(Screen.width - 250, Screen.height - 300, 250, 300);
	private NetworkView netView;

	private void Awake()
	{
		netView = GetComponent<NetworkView> ();
	}

    private void OnGUI()
    {
        windowChatBox = GUI.Window(2, windowChatBox, WindowChatBox, "ChatBox");
    }

    private void WindowChatBox(int id)
    {
		GUILayout.Box(msgs, style, GUILayout.Height(250));

        GUILayout.BeginHorizontal();
		msgToSend = GUILayout.TextField(msgToSend, GUILayout.MaxWidth(170));

        if (GUILayout.Button("Send"))
        {
			if (msgToSend != "") 
			{
				netView.RPC ("SendMsg", RPCMode.All, "user : " + msgToSend + "\n");
				msgToSend = "";
			}
        }

        GUILayout.EndHorizontal();

		GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
    }

	[RPC]
	private void SendMsg(string message)
	{
		msgs += message;
	}
}

