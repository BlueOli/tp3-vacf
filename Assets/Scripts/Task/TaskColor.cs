using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskColor : MonoBehaviour
{

    public Color[] possibleColors;

    public void AssingColor(int colorPosition, Image targetImage)
    {
        Debug.Log(possibleColors[colorPosition]);
        targetImage.color = possibleColors[colorPosition];
    }
}
