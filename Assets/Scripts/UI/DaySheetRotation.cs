using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DaySheetRotation : MonoBehaviour
{
    private TextMeshProUGUI stringText;

    private void Start()
    {
        stringText = GetComponent<TextMeshProUGUI>();
    }

    string MoveFirstCharacterToEnd(string input)
    {
        // Check if the input string is not empty
        if (!string.IsNullOrEmpty(input))
        {
            // Get the first character of the string
            char firstChar = input[0];

            // Remove the first character from the string
            string remainingChars = input.Substring(1);

            // Append the first character to the end of the string
            return remainingChars + firstChar;
        }

        // Return the input string as is if it's empty
        return input;
    }

    public void UpdateText()
    {
        stringText.text = MoveFirstCharacterToEnd(stringText.text);
    }
}
