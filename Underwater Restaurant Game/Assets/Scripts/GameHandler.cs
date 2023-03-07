using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameHandler : MonoBehaviour
{

    public Networking network;
    

    // Start is called before the first frame update
    void Start()
    {
        //keydownevent for escape
        

    }

    // Update is called once per frame
    void Update()
    {
        exit();
    }

    public void exit()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Disconnect from the server
            network.disconnectPlayer(PlayerPrefs.GetString("username"));
            StartCoroutine(WaitForOneSecond());
        }
    }

    IEnumerator WaitForOneSecond()
    {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene("Menu");
    }
}
