using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    public List<Item> items = new List<Item>();

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
    public static Object prefab;
    public GameObject instance;
    public Transform transform;

    public Item(int itemID, int itemState){
        this.itemID = itemID;
        this.posX = 0;
        this.posY = 0;
        this.itemState = itemState;
    }

    public void MoveItemStatus(int posX, int posY){
        this.posX = posX;
        this.posY = posY;
        transform.position = new Vector3(posX, posY, 0);
    }



}

public class Cola : Item //ItemID 0
{
    public static Object prefab = Resources.Load("Prefabs/Cola");
    public GameObject instance = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
    bool opened = false;


    public Cola(int itemID, int itemState) : base(itemID, itemState)
    {
        transform = instance.GetComponent<Transform>();
    }

}