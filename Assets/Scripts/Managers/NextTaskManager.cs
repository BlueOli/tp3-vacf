using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NextTaskManager : MonoBehaviour
{
    // Reference to the nextTaskContainer GameObject
    public GameObject nextTaskContainer;

    // Prefab representing a task UI element
    public GameObject taskPrefab;

    void Start()
    {
        // Check if the nextTaskContainer reference is set
        if (nextTaskContainer == null)
        {
            Debug.LogError("nextTaskContainer reference is not set!");
            return;
        }

        // Instantiate the task UI element
        GameObject taskObject = Instantiate(taskPrefab, nextTaskContainer.transform);
    }

    public void GenerateNewTask()
    {
        GameObject taskObject = Instantiate(taskPrefab, nextTaskContainer.transform);
        taskObject.GetComponent<Task>().canMerge = false;
    }
}