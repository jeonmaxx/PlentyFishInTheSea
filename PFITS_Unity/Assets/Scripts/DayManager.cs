using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayManager : MonoBehaviour
{
    public int currentDay;
    public TextMeshProUGUI dayText;

    void Start()
    {
        dayText.text = "Day: " + currentDay;
    }

    void Update()
    {
        
    }
}
