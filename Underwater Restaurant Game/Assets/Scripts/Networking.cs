using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using System.Net;
using System.Net.Sockets;


public class Networking : MonoBehaviour
{

    public TextMeshProUGUI ServerInfoText;

    public Object playerprefabtemplate;


    string hostname;
    string username;
    string ServerName;
    public string playerID = "0";
    public List<Player> players = new List<Player>();
    int port = 1007;
    bool receivingnow = false;
    public UdpConnection connection;
    public string buffer = "C";

    

    // Start is called before the first frame update
    void Start()
    {
        hostname = PlayerPrefs.GetString("hostname");
        username = PlayerPrefs.GetString("username");
        Debug.Log("Connecting to " + hostname);
        Debug.Log(IPAddress.Parse(hostname));

        connection = new UdpConnection();
        connection.StartConnection(hostname, 1008, 1007);

        connectPlayer(username);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //late update
    void LateUpdate()
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
        if (buffer == "C") {
            buffer += ID + "," + message;
        }
        else {
            buffer += ";" + ID + "," + message;
        }
        
    }

    public void flushBuffer() {
        if (buffer != "C") {
            sendUDP(buffer);
            buffer = "C";
        }
    }

    
    public void sendUDP(string message) {
        connection.Send(message);
    }



    public void receiveMessage(){
        foreach (string message in connection.getMessages())
        {
            
            // if message begins with S, remove it
            if (message.StartsWith("S")) {
                string mess = message.Substring(1); 
            
                

                string[] globalSplit = mess.Split(';');
                foreach ( string s in globalSplit)
                {
                    //Debug.Log("Received: " + s);
                    string[] split = s.Split(',');
                    int ID = int.Parse(split[0]);
                    switch (ID) {
                        case 1: //SendPlayerStatus(PlayerID, X Position Value, Player ID, X Position Value, Player ID, X Position Value, etc.)
                            for (int i = 1; i < split.Length; i += 2) {
                                if (split[i] == playerID) {

                                    //do nothing
                                }
                                else {
                                    foreach (Player p in players) {
                                        if (p.id == split[i]) {
                                            p.posX = int.Parse(split[i + 1]);
                                            p.move();
                                        }
                                    }
                                }
                            }
                            break;
                        case 11: //serverStatus (ServerName, PlayerID, PlayerName)

                            Debug.Log("Detected Player with ID: " + split[2] + " and name: " + split[3]);
                            if(split[3] == username){
                                ServerName = split[1];
                                playerID = split[2];
                                ServerInfoText.text = "Connected to " + ServerName + " with ID: " + playerID;
                            }
                            else{
                                
                                Player newPlayer = new Player();
                                newPlayer.id = split[2];
                                newPlayer.username = split[3];
                                newPlayer.alive();
                                players.Add(newPlayer);
                                
                            }
                            break;
                    }
                }
            }
            
        }
    }
}


public class Player : MonoBehaviour
{
    public static Object playerprefab = Resources.Load("PlayerPrefab");
    public string id;
    public string username;
    public int posX = 0;
    public GameObject playerObject = Instantiate(playerprefab, new Vector3(0, 0, -1), Quaternion.identity) as GameObject;
    public TextMeshProUGUI playerText;
    public void alive(){
        playerText = playerObject.GetComponentInChildren<TextMeshProUGUI>();
        playerText.text = username;
    }
    public void move()
    {
        playerObject.transform.position = new Vector3(posX, 0, -1);
    }
}