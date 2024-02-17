using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu]

public class ItemInfo : ScriptableObject
{
    [SerializeField] public int ItemID;
    [SerializeField] public string ItemName;
    [TextArea][SerializeField] public string items_description;
    [SerializeField] public int MaxStack;
    [SerializeField] public GameObject itemObject;
    [SerializeField] public GameObject guiObject;
}
