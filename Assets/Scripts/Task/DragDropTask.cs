using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class DragDropTask : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform parentOnDrag;

    Transform previousParent;

    // Reference to the RectTransform of this task
    private RectTransform rectTransform;

    // Reference to the CanvasGroup for controlling the task's visibility during drag
    private CanvasGroup canvasGroup;

    void Start()
    {
        // Get references to the RectTransform and CanvasGroup components
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        previousParent = transform.parent;

        parentOnDrag = GameObject.FindGameObjectWithTag("TaskDragContainer").transform;

        parentOnDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();

        // Set the task as the dragged object and make it transparent
        canvasGroup.alpha = 0.5f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Update the position of the task to follow the mouse cursor
        rectTransform.anchoredPosition += eventData.delta / GetComponentInParent<Canvas>().scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Restore the task's visibility and raycast blocking
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        transform.SetParent(parentOnDrag);

        Debug.Log(eventData.pointerEnter.gameObject.name);

        // Check if the task was dropped onto a valid drop target
        if (eventData.pointerEnter != null && eventData.pointerEnter.GetComponent<HourSlot>() != null)
        {
            // Execute logic for dropping the task onto the hour slot
            HourSlot hourSlot = eventData.pointerEnter.GetComponent<HourSlot>();

            CheckSlot(hourSlot);
        }
        else if(eventData.pointerEnter != null && eventData.pointerEnter.GetComponentInParent<Task>() != null)
        {
            CheckTask(eventData.pointerEnter.GetComponentInParent<Task>());
        }
        else
        {
            previousParent.GetComponent<HourSlot>().HandleDrop(this.gameObject, true);
        }
    }

    public void CheckSlot(HourSlot hourSlot)
    {
        if (hourSlot.canHold)
        {
            if (hourSlot.holdingTask == null)
            {
                hourSlot.HandleDrop(this.gameObject);
            }
            else
            {
                CheckTask(hourSlot.holdingTask);
            }
        }
        else
        {
            previousParent.GetComponent<HourSlot>().HandleDrop(this.gameObject, true);
        }
    }

    public void CheckTask(Task originTask)
    {
        Task newTask = this.GetComponent<Task>();
        if(newTask)
        {
            if(newTask.CheckIfMergeable(originTask))
            {
                originTask.MergeTask(newTask);
            }
            else
            {
                previousParent.GetComponent<HourSlot>().HandleDrop(this.gameObject, true);
            }
        }
    }
}