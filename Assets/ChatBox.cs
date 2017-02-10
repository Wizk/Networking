using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatBox : MonoBehaviour
{
    private string messages;
    private string messageToSend;

    private Rect windowChatBox = new Rect(Screen.width - 200, Screen.height - 400, 200, 400);

    private void OnGUI()
    {
        windowChatBox = GUI.Window(0, windowChatBox, WindowChatBox, "ChatBox");
    }

    private void WindowChatBox(int id)
    {
        GUILayout.Box(messages, GUILayout.Height(350));

        GUILayout.BeginHorizontal();
        messageToSend = GUILayout.TextField(messageToSend);

        if (GUILayout.Button("Send"))
        {
            
        }

        GUILayout.EndHorizontal();

        GUI.DragWindow(new Rect(0 , 0, Screen.width, Screen.height));
    }
}

