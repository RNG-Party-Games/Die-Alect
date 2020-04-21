using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AdvanceDay() {
        GameInformation.instance.AdvanceDay();
    }

    public void Boat() {
        GameInformation.instance.Boat();
    }

    public void Night() {
        Ambience.instance.Night();
    }

    public void Day() {
        Ambience.instance.Day();
    }
}
