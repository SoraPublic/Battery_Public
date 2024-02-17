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
    /// Inventory���ł��̂𓮂������Ƃ��ɌĂԁB���ׂẴA�C�e�����ړ��ł����Ƃ�true��Ԃ��B
    /// </summary>
    /// <param name="index">�u�������܂���index</param>
    /// <param name="DropItem">�A�C�e���̃I�u�W�F�N�g</param>
    /// <param name="dropArea">�u�������}�X��cs</param>
    /// <returns></returns>
    public bool Set_Slot(int index, ItemObject DropItem, DropArea dropArea)
    {
        if(Myslot[index] != null)//�u�������ꏊ�ɂ��łɃA�C�e�������鎞
        {
            if(Myslot[index].itemInfo.ItemID == DropItem.itemInfo.ItemID)//�����A�C�e���̎�
            {
                Myslot[index].ItemCount += DropItem.ItemCount;//�A�C�e�����𑫂�

                if(Myslot[index].itemInfo.MaxStack < Myslot[index].ItemCount)//�A�C�e�������ő�X�^�b�N���𒴂����Ƃ�
                {
                    DropItem.ItemCount = Myslot[index].ItemCount - Myslot[index].itemInfo.MaxStack;
                    Myslot[index].ItemCount = Myslot[index].itemInfo.MaxStack;

                    Myslot[index].Set_CountText(Myslot[index].ItemCount);
                    DropItem.Set_CountText(DropItem.ItemCount);

                    return false;
                }
                //�z���Ȃ������Ƃ�
                Myslot[index].Set_CountText(Myslot[index].ItemCount);

                Myslot[DropItem.drop.MyNumber] = null;

                Destroy(DropItem.gameObject);
            }
            else//�Ⴄ�A�C�e���̎�
            {
                //parent�̐ݒ�
                Myslot[index].parentTransform = DropItem.parentTransform;
                Myslot[index].Set_Transform();

                //slot�̐ݒ�
                Tmp_ItemObject = Myslot[index];
                Myslot[index] = DropItem;
                Myslot[DropItem.drop.MyNumber] = Tmp_ItemObject;
            }
        }
        else//�󂢂Ă�����
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

        
            //������
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