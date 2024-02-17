using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropArea : MonoBehaviour, IDropHandler
{
    [SerializeField] public int MyNumber;
    [SerializeField] InventorySystem inventorySystem;

    [SerializeField] ItemObject itemObject;
    [SerializeField] int Item_Count;

    public bool IsSlot = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDrop(PointerEventData eventData)
    {
        ItemObject dragObj = eventData.pointerDrag.GetComponent<ItemObject>();
        if (dragObj != null)
        {
            if(inventorySystem.Set_Slot(MyNumber, dragObj, this))
            {
                dragObj.parentTransform = this.transform;
            }

            IsSlot = true;
        }
    }

    public void BeginDrag()
    {
        IsSlot = false;
    }
}