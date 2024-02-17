using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenInventory : MonoBehaviour
{
    bool IsOpen = true;

    public void Open_Inventory(RectTransform Inventory)
    {
        if (IsOpen)
        {
            Inventory.localPosition = new Vector3(0,1350,0);
            IsOpen = false;
        }
        else
        {
            Inventory.localPosition = new Vector3(0, 0, 0);
            IsOpen = true;
        }
    }
}
