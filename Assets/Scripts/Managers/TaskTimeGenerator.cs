using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskTimeGenerator : MonoBehaviour
{
    public HourSlot hourSlot;
    public NextTaskManager nextTaskManager;

    public float failedPlacedTask;

    private Coroutine taskHoldCoroutine;

    public void StartTaskHoldCoroutine()
    {
        if(taskHoldCoroutine != null)
        {
            StopCoroutine(taskHoldCoroutine);
        }
        
        taskHoldCoroutine = StartCoroutine(CheckTaskHoldCoroutine());
    }

    IEnumerator CheckTaskHoldCoroutine()
    {
        while (true)
        {
            // Wait for 5 seconds
            yield return new WaitForSeconds(8f);

            // Check if the box is still holding a task
            if (hourSlot.holdingTask != null)
            {
                Destroy(hourSlot.holdingTask);

                // Generate a new task
                nextTaskManager.GenerateNewTask();

                // Increment the accumulator
                failedPlacedTask++;
            }
            else
            {
                nextTaskManager.GenerateNewTask();
            }
        }
    }
}
