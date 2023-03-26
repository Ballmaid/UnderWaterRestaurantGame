using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehavior : MonoBehaviour
{
    public Networking networking;
    public Item selfItem;
    Vector3 startMousePosition;
    public bool isDragging = false;
    public int ItemID = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake(){
        networking = GameObject.Find("Networking").GetComponent<Networking>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isDragging){
            Vector3 mousePosition;
            mousePosition = Input.mousePosition;
            Vector3 mouseDelta = mousePosition - startMousePosition;
            mouseDelta.x = mouseDelta.x / Screen.width * 18;
            mouseDelta.y = mouseDelta.y / Screen.height * 10;
            transform.position += mouseDelta;
            startMousePosition = mousePosition;
            MoveItem((float)transform.position.x, (float)transform.position.y);
        }
    }
    void OnMouseDown(){
        startMousePosition = Input.mousePosition;
        isDragging = true;
    }
    void OnMouseUp(){
        isDragging = false;
        MoveItem((float)transform.position.x, (float)transform.position.y);
    }
    public void MoveItem(float posX, float posY){
        //delete older message with same itemID
        bool found = false;
        string buffer = networking.buffer;
        string[] bufferArray = buffer.Split(';');
        for(int i = 0; i < bufferArray.Length; i++){
            string[] message = bufferArray[i].Split(',');
            if(message[0] == "2" && message[1] == ItemID.ToString()){
                message[2] = ((int)(posX*10)).ToString();
                message[3] = ((int)(posY*10)).ToString();
                bufferArray[i] = message[0] + "," + message[1] + "," + message[2] + "," + message[3];
                found = true;
            }
        }
        networking.buffer = bufferArray[0];
        for(int i = 1; i < bufferArray.Length; i++){
            networking.buffer += ";" + bufferArray[i];
        }
        if(!found){
            networking.sendMessage(2, ItemID.ToString() + "," + ((int)(posX*10)).ToString() + "," +  ((int)(posY*10)).ToString());
        }
        
        //Debug.Log("ItemID: " + ItemID + " moved to " + posX + ", " + posY);
    }
}
