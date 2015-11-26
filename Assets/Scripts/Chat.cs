using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using System.Collections;
using System.Collections.Generic;

public class Chat : NetworkBehaviour
{

    const short CHAT_MSG = MsgType.Highest + 1; // Unique message ID
    //public Chat chat; // Separate, non-networked script handling the chat window/interface/GUI
    NetworkClient client;
    public List<string> chatHistory = new List<string>();
    private string currentMessage = string.Empty;


    void Start()
    {
        NetworkManager netManager = GameObject.FindObjectOfType<NetworkManager>();
        client = netManager.client;
        if (client.isConnected)
            client.RegisterHandler(CHAT_MSG, ClientReceiveChatMessage);
        if (isServer)
            NetworkServer.RegisterHandler(CHAT_MSG, ServerReceiveChatMessage);

    }

    public void SendChatMessage(string msg)
    {
        StringMessage strMsg = new StringMessage(msg);
        if (isServer)
        {
            NetworkServer.SendToAll(CHAT_MSG, strMsg); // Send to all clients
        }
        else if (client.isConnected)
        {
            client.Send(CHAT_MSG, strMsg); // Sending message from client to server
        }
    }

    public void ServerReceiveChatMessage(NetworkMessage netMsg)
    {
        string str = netMsg.ReadMessage<StringMessage>().value;
        if (isServer)
        {
            SendChatMessage(str); // Send the chat message to all clients
        }
    }

    public void ClientReceiveChatMessage(NetworkMessage netMsg)
    {
        string str = netMsg.ReadMessage<StringMessage>().value;
        if (client.isConnected)
        {
            AppendMessage(str); // Add the message to the client's local chat window
        }
    }
    public void AppendMessage(string msg)
    {
        chatHistory.Add(msg);
        //Debug.Log("receive chat message " + msg);
    }

    private void OnGUI()
    {
        GUILayout.Space(125);
        GUILayout.BeginHorizontal(GUILayout.Width(250));
        currentMessage = GUILayout.TextField(currentMessage);

        if (GUILayout.Button("Send"))
        {
            if (!string.IsNullOrEmpty(currentMessage.Trim()))
            {
                SendChatMessage(currentMessage);
                currentMessage = string.Empty;
            }
        }
        GUILayout.EndHorizontal();

        foreach (string c in chatHistory)
            GUILayout.Label(c);
    }
}