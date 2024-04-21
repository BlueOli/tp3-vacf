using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridReader : MonoBehaviour
{
    // Reference to the AgendaBox GameObject
    public GameObject agendaBox;

    void Start()
    {
        // Check if the agendaBox reference is set
        if (agendaBox == null)
        {
            Debug.LogError("AgendaBox reference is not set!");
            return;
        }

        // Iterate through each DayBox child of the AgendaBox
        foreach (Transform dayBox in agendaBox.transform)
        {
            Debug.Log("Day: " + dayBox.name);

            // Iterate through each HourBox child of the DayBox
            foreach (Transform hourBox in dayBox)
            {
                Debug.Log("    Hour: " + hourBox.name);

                // Iterate through each FirstHalfBox and SecondHalfBox child of the HourBox
                foreach (Transform halfBox in hourBox)
                {
                    // Print the name of each HalfBox
                    Debug.Log("        Half: " + halfBox.name);

                    // You can access and print any additional information stored in the HalfBox here
                    // For example, you can access a Text component to print its text value:
                    // Debug.Log("            Text: " + halfBox.GetComponentInChildren<Text>().text);
                }
            }
        }
    }
}
