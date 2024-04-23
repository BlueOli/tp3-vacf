using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HourSlot : MonoBehaviour
{
    public Task holdingTask;

    public bool isInAgenda = true;
    public bool canHold = true;

    public string day;
    public string hour;
    public bool isFirstHalf = true;

    // Reference to the RectTransform of the hour slot
    private RectTransform rectTransform;

    void Start()
    {
        // Get reference to the RectTransform component
        rectTransform = GetComponent<RectTransform>();

        if (transform.GetSiblingIndex() == 0)
        {
            isFirstHalf = true;
        }
        else
        {
            isFirstHalf = false;
        }

        if (isInAgenda)
        {
            day = transform.parent.parent.gameObject.name;
            hour = transform.parent.gameObject.name;

            if (!isFirstHalf)
            {
                hour = hour.Replace(":00", ":30");
            }
        }
    }

    public void HandleDrop(GameObject taskObject)
    {
        // Set the task object's parent to this hour slot
        taskObject.transform.SetParent(transform, false);

        if (holdingTask == null)
        {
            HoldTask(taskObject.GetComponent<Task>());
        }
    }

    public void HandleDrop(GameObject taskObject, bool isCancel)
    {
        if (isCancel)
        {
           // Set the task object's parent to this hour slot
            taskObject.transform.SetParent(transform, false);
            

            if (isInAgenda && !canHold)
            {
                Destroy(taskObject);
            }
        }
        else
        {
            HandleDrop(taskObject);
        }
    }

    public void UpdateHourSlotValues()
    {
        if (transform.GetSiblingIndex() == 0)
        {
            isFirstHalf = true;
        }
        else
        {
            isFirstHalf = false;
        }

        if (isInAgenda)
        {
            day = transform.parent.parent.gameObject.name;
            hour = transform.parent.gameObject.name;

            if (!isFirstHalf)
            {
                hour = hour.Replace(":00", ":30");
            }
        }
    }

    public void HandleActivity()
    {
        if (holdingTask != null || this.transform.GetComponentInChildren<Task>())
        {
            HoldTask(this.transform.GetComponentInChildren<Task>());

            Debug.Log(holdingTask.taskText.text);
            Color newColor = holdingTask.transform.GetComponentInChildren<Image>().color;
            newColor.a = 0.25f;
            holdingTask.gameObject.GetComponentInChildren<Image>().color = newColor;
            Destroy(holdingTask.GetComponent<DragDropTask>());
            holdingTask.canMerge = false;
        }

        Color newColor2 = this.GetComponent<Image>().color;
        newColor2.a = 0.25f;
        this.GetComponent<Image>().color = newColor2;

        canHold = false;
    }

    public void HoldTask(Task task)
    {
        Debug.Log(task.taskText.text);
        holdingTask = task;
        holdingTask.SaveDayAndTime(day, hour);
        holdingTask.canMerge = true;
    }
}