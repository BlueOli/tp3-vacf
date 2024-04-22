using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject agendaBox;
    public GameObject[] dayGrid;
    public string[] dayList;
    public GameObject daySheet;

    public bool isGameOver = false;

    public GameObject weekDayPrefab;

    public int actualDayIndex;
    public int actualTimeIndex;

    public bool dayHasChanged = false;

    private GameObject actualDayAndTimeBox;

    public void Start()
    {
        Time.timeScale = 1f;
        actualDayIndex = 0;
        actualTimeIndex = 0;
        dayGrid = new GameObject[16];

        LoadGrid();
        LoadDayAndTimeBox();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoadNextTimeBox();
        }

        if (dayHasChanged)
        {
            MoveToNextDay();
            dayHasChanged = false;
            LoadGrid();
            LoadDayAndTimeBox();
            daySheet.GetComponent<DaySheetRotation>().UpdateText();
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

    public void LoadDayAndTimeBox()
    {
        actualDayAndTimeBox = dayGrid[actualTimeIndex];
    }

    public void AdvanceGridBox()
    {
        actualDayAndTimeBox.GetComponent<HourSlot>().HandleActivity();

        actualTimeIndex++;

        if (actualTimeIndex >= dayGrid.GetLength(0))
        {
            actualTimeIndex = 0;
            actualDayIndex++;
            dayHasChanged = true;
        }
    }

    public void LoadNextTimeBox()
    {
        AdvanceGridBox();
        LoadDayAndTimeBox();
    }

    public void MoveToNextDay()
    {
        GameObject weekDay = Instantiate(weekDayPrefab, agendaBox.transform);
        weekDay.name = dayList[(actualDayIndex - 1) % 5];

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
        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        while (true)
        {
            LoadNextTimeBox();
            yield return new WaitForSeconds(1f); // Wait for 1 second
        }
    }

}
