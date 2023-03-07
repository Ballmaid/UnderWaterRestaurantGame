using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Net;
using System.Net.Sockets;


public class Networking : MonoBehaviour
{

    [Header("UI Elements")]
    public TextMeshProUGUI ServerInfoText;

    [Header("Networking")]
    string hostname;
    string username;
    string ServerName;
    int port = 1008;

    IPEndPoint serverEndPoint;
    UdpClient client;

    // Start is called before the first frame update
    void Start()
    {
        hostname = PlayerPrefs.GetString("hostname");
        username = PlayerPrefs.GetString("username");
        Debug.Log(IPAddress.Parse(hostname));
        serverEndPoint = new IPEndPoint(IPAddress.Parse(hostname), port);
        client = new UdpClient();
        connectPlayer(username);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    string connectPlayer(string playername){
        sendMessage(10, playername, hostname);

        ServerInfoText.text = "Connected to " + hostname;
        return "ServerName";
    }

    public void disconnectPlayer(string playername){
        sendMessage(12, playername, hostname);
    }

    void sendMessage(int ID, string message, string addr) {
        // Send UDP message to server
            byte[] data = System.Text.Encoding.ASCII.GetBytes(ID.ToString() + "," + message);
            client.Send(data, data.Length, serverEndPoint);
    }

    
}
