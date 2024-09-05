using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private CanvasGroup canvasGroup;

    private Transform parentToReturn;

    protected bool isDraggable = true;

    private void OnEnable()
    {
        isDraggable = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isDraggable)
            return;

        parentToReturn = transform.parent;
        transform.SetParent(transform.parent.root);

        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDraggable)
            return;

        transform.position = eventData.position;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if (!isDraggable)
            return;

        canvasGroup.blocksRaycasts = true;

        if (eventData.pointerEnter == null)
        {
            SetDefaultParent();
            return;
        }

        if (eventData.pointerEnter.CompareTag("DropZone"))
        {
            isDraggable = false;

            OnDragFinished(eventData);
        }

        else
        {
            SetDefaultParent();
        }
    }

    protected void SetDefaultParent()
    {
        transform.SetParent(parentToReturn);
    }

    public virtual void OnDragFinished(PointerEventData eventData)
    {

    }
}