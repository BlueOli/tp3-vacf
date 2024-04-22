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

    public void GenerateNewTask()
    {
        GameObject taskObject = Instantiate(taskPrefab, nextTaskContainer.transform);
        taskObject.GetComponent<Task>().canMerge = false;
    }
}