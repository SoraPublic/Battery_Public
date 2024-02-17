using UnityEngine;
using UnityEngine.EventSystems;

public class testdrop : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData data)
    {
        Debug.Log(gameObject.name);

        testdrag dragObj = data.pointerDrag.GetComponent<testdrag>();
        if (dragObj != null)
        {
            dragObj.parentTransform = this.transform;
            Debug.Log(gameObject.name + "‚É" + data.pointerDrag.name + "‚ğƒhƒƒbƒv");
        }
    }
}