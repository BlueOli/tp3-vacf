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

        // Set the task object's local scale to fit within the hour slot
        RectTransform taskRectTransform = taskObject.GetComponent<RectTransform>();

        // Calculate the scale factor to fit the task within the hour slot
        float widthScale = rectTransform.rect.width / taskRectTransform.rect.width;
        float heightScale = rectTransform.rect.height / taskRectTransform.rect.height;
        float scaleFactor = Mathf.Min(widthScale, heightScale);

        // Apply the scale factor to the task object
        taskRectTransform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);

        // Set the task object's anchored position to center it within the hour slot
        taskRectTransform.anchoredPosition = Vector2.zero;

        // Optionally, you can perform additional actions here, such as updating the task's state or triggering events.        

        if(holdingTask == null)
        {
            holdingTask = taskObject.GetComponentInChildren<Task>();
            holdingTask.SaveDayAndTime(day, hour);
            holdingTask.canMerge = true;
        }
    }

    public void HandleDrop(GameObject taskObject, bool isCancel)
    {
        if (isCancel)
        {
            // Set the task object's parent to this hour slot
            taskObject.transform.SetParent(transform, false);

            // Set the task object's local scale to fit within the hour slot
            RectTransform taskRectTransform = taskObject.GetComponent<RectTransform>();

            // Calculate the scale factor to fit the task within the hour slot
            float widthScale = rectTransform.rect.width / taskRectTransform.rect.width;
            float heightScale = rectTransform.rect.height / taskRectTransform.rect.height;
            float scaleFactor = Mathf.Min(widthScale, heightScale);

            // Apply the scale factor to the task object
            taskRectTransform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);

            // Set the task object's anchored position to center it within the hour slot
            taskRectTransform.anchoredPosition = Vector2.zero;

            // Optionally, you can perform additional actions here, such as updating the task's state or triggering events.
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
        if(transform.childCount > 0)
        {
            Color newColor = holdingTask.transform.GetComponentInChildren<Image>().color;
            newColor.a = 0.5f;
            holdingTask.gameObject.GetComponentInChildren<Image>().color = newColor;

            holdingTask.canMerge = false;
        }

        Color newColor2 = this.GetComponent<Image>().color;
        newColor2.a = 0.5f;
        this.GetComponent<Image>().color = newColor2;

        canHold = false;
    }
}