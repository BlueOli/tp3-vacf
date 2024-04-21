using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject agendaBox;
    public GameObject[,] grid;

    public void Start()
    {
        grid = new GameObject[5,16];
        LoadGrid();
    }

    public void LoadGrid()
    {
        int indexDay = 0;
        int indexHour = 0;
        int indexHalfOur = 0;

        foreach (Transform dayBox in agendaBox.transform)
        {
            foreach (Transform hourBox in dayBox)
            {
                foreach (Transform halfBox in hourBox)
                {
                    grid[indexDay, indexHour + indexHalfOur] = transform.gameObject;

                    indexHalfOur++;
                }

                indexHour++;
                indexHalfOur = 0;
            }

            indexDay++;
            indexHour = 0;
        }
    }


}
