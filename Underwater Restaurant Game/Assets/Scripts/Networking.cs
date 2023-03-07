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
    public string playerID = "0";
    int port = 1008;

    IPEndPoint serverEndPoint;
    UdpClient client;

    [Header("Players")]
    public List<Player> players = new List<Player>();

    // Start is called before the first frame update
    void Start()
    {
        hostname = PlayerPrefs.GetString("hostname");
        username = PlayerPrefs.GetString("username");
        Debug.Log("Connecting to " + hostname);
        Debug.Log(IPAddress.Parse(hostname));
        serverEndPoint = new IPEndPoint(IPAddress.Parse(hostname), port);
        client = new UdpClient();
        connectPlayer(username);
    }

    // Update is called once per frame
    void Update()
    {
        receiveMessage();
    }

    string connectPlayer(string playername){
        sendMessage(10, playername);

        
        return "ServerName";
    }

    public void disconnectPlayer(string playername){
        sendMessage(12, playername);
    }

    public void sendMessage(int ID, string message) {
        // Send UDP message to server
            byte[] data = System.Text.Encoding.ASCII.GetBytes(ID.ToString() + "," + message);
            client.Send(data, data.Length, serverEndPoint);
    }

    public void receiveMessage() {
        // Receive UDP message from server
        IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
        byte[] data = client.Receive(ref anyIP);
        string text = System.Text.Encoding.ASCII.GetString(data);
        // Separate ID and message from format "ID,var1,var2,var3..."
        string[] split = text.Split(',');
        int ID = int.Parse(split[0]);
        switch (ID) {
            case 1:
                // All Players movement
                // Format: "1,playerID,x,PlayerID,x,PlayerID,x..."
                break;
            case 11:
                if(split[3] == username){
                    ServerName = split[1];
                    playerID = split[2];
                }
                else{
                    Player newPlayer = new Player();
                    newPlayer.id = split[2];
                    newPlayer.username = split[3];
                    players.Add(newPlayer);
                }
                ServerInfoText.text = "Connected to " + ServerName;
                break;
        }
    }
}


public class Player
{
    public string id;
    public string username;
    public int posX = 0;
}