using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject agendaBox;
    public GameObject[] dayGrid;
    public GameObject[] tempDayGrid;
    public string[] dayList;
    public GameObject daySheet;

    public PlayerStats player;

    public bool isGameOver = false;

    public GameObject weekDayPrefab;
    public GameObject taskPrefab;

    public int actualDayIndex;
    public int actualTimeIndex;

    public bool dayHasChanged = false;

    private GameObject actualDayAndTimeBox;

    private Coroutine startGameCoroutine;

    public int initialDifficulty = 3;
    public int playerStressGain = 1;
    public int playerStressLoss = 1;
    public int playerProductivityLoss = 1;
    public int difficultyMultiplier = 3;

    public GameObject endScreen;
    public TextMeshProUGUI endText;

    public float speedFactor = 10f;

    public void LoadNewGame()
    {
        tempDayGrid = new GameObject[16];
        dayGrid = new GameObject[16];
        actualDayIndex = 0;
        actualTimeIndex = 0;

        PopulateAgendaBox();


        LoadGrid();
        LoadDayAndTimeBox();
        player.UpdateDayText(actualDayIndex + 1);
    }
    
    public void Update()
    {
        if (dayHasChanged)
        {
            MoveToNextDay();
            dayHasChanged = false;
            LoadGrid();
            LoadDayAndTimeBox();
            daySheet.GetComponent<DaySheetRotation>().UpdateText();
            player.UpdateDayText(actualDayIndex + 1);

            Time.timeScale = 1f + actualDayIndex / speedFactor;
        }

        if (player.productivity <= 0 || player.stress >= 100)
        {
            isGameOver = true;
        }

        if (isGameOver)
        {
            Time.timeScale = 0f;

            endScreen.SetActive(true);
            

            if(player.productivity <= 0)
            {
                endText.text = "You got fired because your productivity is below industry standards";   
            }
            if(player.stress >= 100)
            {
                endText.text = "You had a mental breakdown because you couldn't handle your workload";
            }
        }
    }

    public void LoadGrid()
    {
        int indexHalfOur = 0;

        foreach (Transform hourBox in agendaBox.transform.GetChild(0))
        {
            foreach (Transform halfBox in hourBox)
            {
                dayGrid[indexHalfOur] = halfBox.gameObject;
                indexHalfOur++;
            }
        }
    }

    public void LoadTempGrid(GameObject weekDay)
    {
        int indexHalfOur = 0;
        foreach (Transform hourBox in weekDay.transform)
        {
            foreach (Transform halfBox in hourBox)
            {
                tempDayGrid[indexHalfOur] = halfBox.gameObject;
                indexHalfOur++;
            }
        }
    }

    public void LoadDayAndTimeBox()
    {
        actualDayAndTimeBox = dayGrid[actualTimeIndex];
    }

    public void AdvanceGridBox()
    {
        actualDayAndTimeBox.GetComponent<HourSlot>().HandleActivity();

        UpdatePlayerStats(actualDayAndTimeBox.GetComponent<HourSlot>());

        actualTimeIndex++;

        if (actualTimeIndex >= dayGrid.GetLength(0))
        {
            actualTimeIndex = 0;
            actualDayIndex++;
            dayHasChanged = true;
        }
    }

    public void UpdatePlayerStats(HourSlot hourSlot)
    {
        Task task = hourSlot.holdingTask;

        if (task != null)
        {
            player.stress+=playerStressGain;
            player.productivity += (float)task.participantCount;
        }
        else
        {
            player.stress-=playerStressLoss;
            player.productivity -=playerProductivityLoss;
        }

        player.UpdatePlayerStats();
    }

    public void LoadNextTimeBox()
    {
        AdvanceGridBox();
        LoadDayAndTimeBox();
    }

    public void MoveToNextDay()
    {
        GenerateNewDay(dayList[(actualDayIndex - 1) % 5]);

        GameObject dayPassed = agendaBox.transform.GetChild(0).gameObject;
        dayPassed.transform.SetParent(null);

        foreach (Transform t in dayPassed.transform)
        {
            foreach (Transform half in t)
            {
                foreach (Transform task in half)
                {
                    Destroy(task.gameObject);
                }
                Destroy(half.gameObject);
            }
            Destroy(t.gameObject);
        }
        Destroy(dayPassed);
    }

    public void StartGameCoroutine()
    {
        if (startGameCoroutine != null)
        {
            StopCoroutine(startGameCoroutine);
        }

        startGameCoroutine = StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(1f);
        while (true)
        {
            LoadNextTimeBox();
            yield return new WaitForSeconds(1f); // Wait for 1 second
        }
    }

    public void PopulateAgendaBox()
    {
        for (int i = 0; i < 5; i++){
            GenerateNewDay(dayList[(i) % 5]);
        }
    }

    public void GenerateNewDay(string name)
    {
        GameObject weekDay = Instantiate(weekDayPrefab, agendaBox.transform);
        weekDay.name = name;

        int difficulty = initialDifficulty + actualDayIndex / difficultyMultiplier;
        if (difficulty > dayGrid.Length) difficulty = dayGrid.Length - 1;

        FillDayWithRandomTasks(weekDay, difficulty);
    }

    public void FillDayWithRandomTasks(GameObject weekDay, int randomTasks)
    {
        LoadTempGrid(weekDay);

        for(int i= 0; i < randomTasks; i++)
        {
            bool taskPlaced = false;
            do
            {
                int randomIndex = Random.Range(0, tempDayGrid.Length);
                if (!tempDayGrid[randomIndex].transform.GetComponentInChildren<Task>())
                {
                    GameObject taskObject = Instantiate(taskPrefab, tempDayGrid[randomIndex].transform);
                    tempDayGrid[randomIndex].GetComponent<HourSlot>();
                    tempDayGrid[randomIndex].GetComponent<HourSlot>().holdingTask = taskObject.GetComponentInChildren<Task>();
                    tempDayGrid[randomIndex].GetComponent<HourSlot>().UpdateHourSlotValues();
                    tempDayGrid[randomIndex].GetComponent<HourSlot>().holdingTask.SaveDayAndTime(tempDayGrid[randomIndex].GetComponent<HourSlot>().day, tempDayGrid[randomIndex].GetComponent<HourSlot>().hour);
                    tempDayGrid[randomIndex].GetComponent<HourSlot>().holdingTask.canMerge = true;
                    taskPlaced = true;                   
                }
            } while (!taskPlaced);
        }
    }
}
