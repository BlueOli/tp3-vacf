using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class DragDropTask : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform parentOnDrag;
    public Transform previousParent;

    private bool comesFromHourSlot = false;

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

        transform.SetParent(transform.root);
        transform.SetAsLastSibling();

        // Set the task as the dragged object and make it transparent
        canvasGroup.alpha = 0.5f;
        canvasGroup.blocksRaycasts = false;

        if (previousParent.GetComponent<HourSlot>().isInAgenda)
        {
            comesFromHourSlot = true;
            previousParent.GetComponent<HourSlot>().holdingTask = null;
        }
        else
        {
            comesFromHourSlot = false;
        }
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
            if(hourSlot == previousParent.GetComponent<HourSlot>())
            {
                previousParent.GetComponent<HourSlot>().HandleDrop(this.gameObject);
            }
            else
            {
                if (hourSlot.holdingTask == null)
                {
                    if (comesFromHourSlot)
                    {
                        if (CheckTaskDifficultyVsSlot(this.GetComponent<Task>(), hourSlot))
                        {
                            hourSlot.HandleDrop(this.gameObject);
                            CheckIfResetNextTaskContainer();
                        }

                        else
                        {
                            previousParent.GetComponent<HourSlot>().HandleDrop(this.gameObject, true);
                        }
                    }
                    else
                    {
                        hourSlot.HandleDrop(this.gameObject);
                        CheckIfResetNextTaskContainer();
                    }
                }
                else
                {
                    CheckTask(hourSlot.holdingTask);
                }
            }            
        }
        else
        {
            previousParent.GetComponent<HourSlot>().HandleDrop(this.gameObject, true);
        }
    }

    private bool CheckTaskDifficultyVsSlot(Task newTask, HourSlot hourSlot)
    {
        bool canHoldTask = false;

        hourSlot.UpdateHourSlotValues();

        switch (newTask.difficulty)
        {
            case 1:
                canHoldTask = true;
                break;

            case 2:
                canHoldTask = false;

                if (hourSlot.day == newTask.day)
                {
                    canHoldTask = true;
                }
                break;

            case 3:
                canHoldTask = false;

                if (hourSlot.hour == newTask.time)
                {
                    canHoldTask = true;
                }
                break;
        }

        return canHoldTask;
    }

    public void CheckTask(Task originTask)
    {
        Task newTask = this.GetComponent<Task>();
        if (newTask != null)
        {
            if(originTask != newTask)
            {
                if (newTask.CheckIfMergeable(originTask))
                {
                    originTask.MergeTask(newTask);
                    CheckIfResetNextTaskContainer();
                }
                else
                {
                    previousParent.GetComponent<HourSlot>().HandleDrop(this.gameObject, true);
                }
            }
            else
            {
                previousParent.GetComponent<HourSlot>().HandleDrop(this.gameObject, true);
            }
        }
    }

    public void CheckIfResetNextTaskContainer()
    {
        if (previousParent.GetComponent<NextTaskManager>() != null)
        {
            previousParent.GetComponent<TaskTimeGenerator>().StartTaskHoldCoroutine();
            previousParent.GetComponent<FillImageOverTime>().StartFillCoroutine();
            previousParent.GetComponent<NextTaskManager>().GenerateNewTask();
        }
        else
        {
            ReleasePreviousParent();
        }
    }

    public void ReleasePreviousParent()
    {
        previousParent.GetComponent<HourSlot>().holdingTask = null;
    }
}