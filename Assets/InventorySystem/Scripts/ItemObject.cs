using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //ItemÇÃèÓïÒ
    [SerializeField] public ItemInfo itemInfo;
    [SerializeField] public int ItemCount;

    //UI
    [SerializeField] Text Count_text;

    //êe
    public Transform parentTransform;

    //
    private Transform Parentobject;
    private CanvasGroup canvasGroup;
    public DropArea drop;

    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        drop = transform.parent.GetComponent<DropArea>();
        parentTransform = transform.parent;
        Parentobject = GameObject.Find("DragObject").transform;

        Count_text.text = ItemCount.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;

        transform.SetParent(Parentobject);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Set_Transform();

        canvasGroup.blocksRaycasts = true;
    }

    public void Set_CountText(int Count)
    {
        Count_text.text = Count.ToString("F0");
    }

    public void Set_Transform()
    {
        transform.SetParent(parentTransform);
        transform.position = parentTransform.position;

        drop = parentTransform.GetComponent<DropArea>();
    }
}
