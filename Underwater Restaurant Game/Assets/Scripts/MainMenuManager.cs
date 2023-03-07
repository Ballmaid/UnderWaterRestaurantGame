using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour
{

    
    //Textmeshpro input field
    public TMP_InputField HostnameInput;
    public TMP_InputField UsernameInput;
    //Textmeshpro text 
    public TextMeshProUGUI StartText;
    public Button StartButton;
    // Start is called before the first frame update
    void Start()
    {
        UsernameInput.text = PlayerPrefs.GetString("username");
        HostnameInput.text = PlayerPrefs.GetString("hostname");
    }

    // Update is called once per frame
    void Update()
    {
        if (HostnameInput.text.Length > 0)
        {
            StartText.color = Color.white;
            StartButton.interactable = true;
        }
        else
        {
            StartText.color = Color.gray;
            StartButton.interactable = false;
        }
    }

    public void start()
    {
        //Get the text from the input field
        string username = UsernameInput.text;
        string hostname = HostnameInput.text;
        //Connect to the server
        PlayerPrefs.SetString("username", username);
        PlayerPrefs.SetString("hostname", hostname);
        //Load the game scene
        SceneManager.LoadScene("Game");
    }

    public void exit()
    {
        Application.Quit();
    }
}
