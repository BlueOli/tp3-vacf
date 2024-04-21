using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNewTask : MonoBehaviour
{
    public NextTaskManager nextTaskManager;


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            if(nextTaskManager.transform.childCount == 0 )
            {
                nextTaskManager.GenerateNewTask();
            }
        }
    }
}
