using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Networking : MonoBehaviour
{

    [Header("UI Elements")]
    public TextMeshProUGUI ServerInfoText;

    string hostname;
    string ServerName;
    // Start is called before the first frame update
    void Start()
    {
        hostname = PlayerPrefs.GetString("hostname");
        connectPlayer("Leon");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    string connectPlayer(string playername){
        ServerInfoText.text = "Connected to " + hostname;
        return "ServerName";
    }
}
