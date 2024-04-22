using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Task : MonoBehaviour
{
    // Array of tasks
    public string[] possibleTasks;
    public int[] possibleDifficulties;
    public int[] possibleParticipants;
    public int maxParticipants;

    public string taskType;
    public int difficulty;
    public int participantCount;

    private TextMeshProUGUI taskText;
    private TaskColor taskColor;

    public string day;
    public string time;

    public bool canMerge = true;

    private void Start()
    {
        taskColor = GetComponent<TaskColor>();
        taskText = GetComponentInChildren<TextMeshProUGUI>();

        AssignRandomTask();
    }

    private void AssignRandomTask()
    {
        taskType = possibleTasks[Random.Range(0, possibleTasks.Length)];
        difficulty = possibleDifficulties[Random.Range(0, possibleDifficulties.Length)];
        participantCount = possibleParticipants[Random.Range(0, possibleParticipants.Length)];

        UpdateValues();
    }

    public bool CheckIfMergeable(Task originTask)
    {
        bool isMergeable = false;

        if(!originTask.canMerge)
        {
            isMergeable = false;
            return isMergeable;
        }

        if(originTask != null)
        {
            if(originTask.taskType == taskType)
            {
                int participantSum = participantCount + originTask.participantCount;
                if(participantSum <= maxParticipants)
                {
                    isMergeable = true;
                }
            }
        }

        if(day != "" && time!= "")
        {
            if (isMergeable)
            {
                switch (difficulty)
                {
                    case 1:
                        isMergeable = true;
                        break;

                    case 2:
                        isMergeable = false;

                        if (originTask.day == this.day)
                        {
                            isMergeable = true;
                        }
                        break;

                    case 3:
                        isMergeable = false;

                        if (originTask.time == this.time)
                        {
                            isMergeable = true;
                        }
                        break;
                }
            }
        }        

        return isMergeable;
    }

    public void MergeTask(Task newTask)
    {
        this.participantCount = participantCount + newTask.participantCount;

        if(newTask.difficulty >  difficulty)
        {
            difficulty = newTask.difficulty;
        }

        UpdateValues();

        Destroy(newTask.gameObject);
    }

    public void UpdateValues()
    {
        UpdateText();
        UpdateColor();
    }

    public void UpdateText()
    {
        taskText.text = taskType + ": " + participantCount + " / " + maxParticipants + " [" + difficulty + "]";
    }

    public void UpdateColor()
    {
        taskColor.AssingColor(FindTaskTypeIndex(), GetComponentInChildren<Image>());
    }

    public int FindTaskTypeIndex()
    {
        int index = -1;

        for(int i = 0; i<possibleTasks.Length; i++)
        {
            if (taskType == possibleTasks[i])
            {
                index = i;
            }
        }

        Debug.Log(index);
        return index;
    }

    public void SaveDayAndTime(string dayT, string timeT)
    {
        day = dayT;
        time = timeT;
    }
}
