using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public float productivity;
    public float stress;
    public int dayCount;

    public Image productivityBar;
    public Image stressBar;
    public TextMeshProUGUI dayCountText;

    public void Start()
    {
        productivity = 75f;
        stress = 10f;
        UpdatePlayerStats();
    }

    public void UpdatePlayerStats()
    {
        productivity = Mathf.Clamp(productivity, 0f, 100f);
        stress = Mathf.Clamp(stress, 0f, 100f);

        productivityBar.fillAmount = productivity / 100f;
        stressBar.fillAmount = stress / 100f;
    }

    public void UpdateDayText(int day)
    {
        dayCountText.text = "Day: " + day;
    }
}
