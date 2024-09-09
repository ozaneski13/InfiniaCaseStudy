using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] protected DeckLockSO deckLockSO;

    private Transform parentToReturn;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (deckLockSO.IsLocked)
            return;

        parentToReturn = transform.parent;
        transform.SetParent(transform.parent.root);

        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (deckLockSO.IsLocked)
            return;

        transform.position = eventData.position;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if (deckLockSO.IsLocked)
            return;

        canvasGroup.blocksRaycasts = true;

        if (eventData.pointerEnter == null)
        {
            SetDefaultParent();
            return;
        }

        if (eventData.pointerEnter.CompareTag("DropZone"))
        {
            if (!OnDragFinished(eventData))
                SetDefaultParent();
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

    public virtual bool OnDragFinished(PointerEventData eventData)
    {
        return false;
    }
}