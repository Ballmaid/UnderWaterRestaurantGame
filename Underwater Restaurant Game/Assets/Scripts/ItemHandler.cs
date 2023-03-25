using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public Networking networking;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void MoveItemStatus(int itemID, int posX, int posY){
        foreach(Item item in items){
            if(item.itemID == itemID){
                item.MoveItemStatus(posX, posY);
            }
        }
    }
    public void createItem(int ItemID, int ItemType, int State){
        switch(ItemType){
            case 0:
                items.Add(new Cola(ItemID, State));
                break;
        }
    }
    public void destroyItem(int ItemID){
        foreach(Item item in items){
            if(item.itemID == ItemID){
                Destroy(item.instance);
                items.Remove(item);
            }
        }
    }
    public void changeItemState(int ItemID, int State){
        foreach(Item item in items){
            if(item.itemID == ItemID){
                item.itemState = State;
            }
        }
    }
}


public class Item : MonoBehaviour
{
    public int itemID;
    public int posX;
    public int posY;
    public int itemState;
    public static Object prefab = Resources.Load("Cola");
    public GameObject instance = Instantiate(prefab, new Vector3(0, 0, -9), Quaternion.identity) as GameObject;
    public Transform transform;
    public Networking networking = GameObject.Find("Networking").GetComponent<Networking>();

    public Item(int itemID, int itemState){
        this.itemID = itemID;
        this.posX = 0;
        this.posY = 0;
        this.itemState = itemState;
        //this.transform = instance.GetComponent<Transform>();
    }

    public void MoveItemStatus(int posX, int posY){
        Debug.Log("ItemID: " + itemID + " moved to " + posX + ", " + posY);
        this.posX = posX;
        this.posY = posY;
        //transform.position.Set(posX, posY, 0);
        instance.transform.position = new Vector3(posX, posY, -9);
    }
    public void MoveItem(int posX, int posY){
        networking.sendMessage(2, itemID.ToString() + "," + (posX*10).ToString() + "," + (posY*10).ToString());
    }


}

public class Cola : Item //ItemID 0
{
    public static Object prefab = Resources.Load("Cola");
    //public GameObject instance = Instantiate(prefab, new Vector3(0, 0, -9), Quaternion.identity) as GameObject;
    bool opened = false;


    public Cola(int itemID, int itemState) : base(itemID, itemState)
    {
        instance.GetComponent<ItemBehavior>().ItemID = itemID;
        instance.GetComponent<ItemBehavior>().selfItem = this;
        this.transform = instance.GetComponent<Transform>();
    }

}