using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;
using TMPro;

public class TimeClock : MonoBehaviour
{
    public GameObject bar;
    public GameObject timeUp;
    public int time;
    // Start is called before the first frame update
    void Start()
    {
        AnimateBar();
    }

    public void AnimateBar()
    {
        LeanTween.scaleX(bar, 0f, time).setOnComplete(ShowMessage);
    }

    void ShowMessage()
    {

        timeUp.SetActive(true);
    }

  
}
