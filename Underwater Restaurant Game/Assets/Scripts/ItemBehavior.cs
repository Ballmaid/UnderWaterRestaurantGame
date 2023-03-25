using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehavior : MonoBehaviour
{
    public Networking networking;
    public Item selfItem;
    Vector3 startMousePosition;
    bool isDragging = false;
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
        }
    }
    void OnMouseDown(){
        startMousePosition = Input.mousePosition;
        isDragging = true;
    }
    void OnMouseUp(){
        isDragging = false;
        MoveItem((int)transform.position.x, (int)transform.position.y);
    }
    public void MoveItem(int posX, int posY){
        networking.sendMessage(2, ItemID.ToString() + "," + (posX*1).ToString() + "," + (posY*1).ToString());
        Debug.Log("ItemID: " + ItemID + " moved to " + posX + ", " + posY);
    }
}
