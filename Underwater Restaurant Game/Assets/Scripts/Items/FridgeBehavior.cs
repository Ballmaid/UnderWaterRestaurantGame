using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FridgeBehavior : MonoBehaviour
{
    public Networking networking;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown(){
        networking.sendMessage(101, "");
    }
}
