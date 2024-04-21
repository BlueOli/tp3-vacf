using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HourSlot : MonoBehaviour
{
    public Task holdingTask;

    public bool isInAgenda = true;
    public bool canHold = true;

    public string day;
    public string hour;

    // Reference to the RectTransform of the hour slot
    private RectTransform rectTransform;

    void Start()
    {
        // Get reference to the RectTransform component
        rectTransform = GetComponent<RectTransform>();

        if (isInAgenda)
        {
            day = transform.parent.parent.gameObject.name;
            hour = transform.parent.gameObject.name;
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

}