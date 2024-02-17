using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySystem : MonoBehaviour
{
    [SerializeField] ItemInfo test;

    [SerializeField] GameObject[] Item_Icon = new GameObject[10];

    [SerializeField] ItemObject[] Myslot = new ItemObject[20];

    [SerializeField] GameObject[] ItemBack = new GameObject[20];

    ItemObject Tmp_ItemObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(Picup_Item(test, 3));
        }
    }

    /// <summary>
    /// Inventory内でものを動かしたときに呼ぶ。すべてのアイテムを移動できたときtrueを返す。
    /// </summary>
    /// <param name="index">置きたいますのindex</param>
    /// <param name="DropItem">アイテムのオブジェクト</param>
    /// <param name="dropArea">置きたいマスのcs</param>
    /// <returns></returns>
    public bool Set_Slot(int index, ItemObject DropItem, DropArea dropArea)
    {
        if(Myslot[index] != null)//置きたい場所にすでにアイテムがある時
        {
            if(Myslot[index].itemInfo.ItemID == DropItem.itemInfo.ItemID)//同じアイテムの時
            {
                Myslot[index].ItemCount += DropItem.ItemCount;//アイテム数を足す

                if(Myslot[index].itemInfo.MaxStack < Myslot[index].ItemCount)//アイテム数が最大スタック数を超えたとき
                {
                    DropItem.ItemCount = Myslot[index].ItemCount - Myslot[index].itemInfo.MaxStack;
                    Myslot[index].ItemCount = Myslot[index].itemInfo.MaxStack;

                    Myslot[index].Set_CountText(Myslot[index].ItemCount);
                    DropItem.Set_CountText(DropItem.ItemCount);

                    return false;
                }
                //越えなかったとき
                Myslot[index].Set_CountText(Myslot[index].ItemCount);

                Myslot[DropItem.drop.MyNumber] = null;

                Destroy(DropItem.gameObject);
            }
            else//違うアイテムの時
            {
                //parentの設定
                Myslot[index].parentTransform = DropItem.parentTransform;
                Myslot[index].Set_Transform();

                //slotの設定
                Tmp_ItemObject = Myslot[index];
                Myslot[index] = DropItem;
                Myslot[DropItem.drop.MyNumber] = Tmp_ItemObject;
            }
        }
        else//空いていた時
        {
            Myslot[index] = DropItem;
            Myslot[DropItem.drop.MyNumber] = null;
        }

        return true;
    }

    public int Picup_Item(ItemInfo Picup_item, int Pickup_Count)
    {
        GameObject Pickup_ItemObject = null;
        ItemObject itemObject = null;

        int null_Index, SameID_Index;
        bool can_Pickup;

        
            //初期化
            can_Pickup = false;
            null_Index = -1;
            SameID_Index = -1;

            for (int i = Myslot.Length - 1; i >= 0; i--)
            {
                if (Myslot[i] != null)
                {
                    if (Myslot[i].itemInfo.ItemID == Picup_item.ItemID && Myslot[i].ItemCount < Picup_item.MaxStack)
                    {
                        SameID_Index = i;
                        can_Pickup = true;
                    }
                }
                else
                {
                    null_Index = i;
                    can_Pickup = true;
                }
            }

        if (can_Pickup)
        {
            if (SameID_Index != -1)
            {
                Myslot[SameID_Index].ItemCount += Pickup_Count;

                if (Myslot[SameID_Index].ItemCount > Picup_item.MaxStack)
                {
                    Pickup_Count = Myslot[SameID_Index].ItemCount - Picup_item.MaxStack;
                    Myslot[SameID_Index].ItemCount = Picup_item.MaxStack;

                    Myslot[SameID_Index].Set_CountText(Picup_item.MaxStack);

                    return Picup_Item(Picup_item,Pickup_Count);
                }
                else
                {
                    Pickup_Count = 0;
                    Myslot[SameID_Index].Set_CountText(Myslot[SameID_Index].ItemCount);
                    return 0;
                }
            }
            else
            {
                Pickup_ItemObject = Instantiate(Picup_item.guiObject, transform.position, transform.rotation);
                itemObject = Pickup_ItemObject.GetComponent<ItemObject>();
                itemObject.ItemCount = Pickup_Count;
                itemObject.parentTransform = ItemBack[null_Index].transform;
                itemObject.Set_Transform();
                Pickup_ItemObject.transform.localScale = Vector3.one;
                Myslot[null_Index] = itemObject;
                Myslot[null_Index].Set_CountText(Pickup_Count);
                Pickup_Count = 0;

                return 0;
            }
        }
        else
        {
            return Pickup_Count;
        }
    }
}