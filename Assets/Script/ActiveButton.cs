using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ActiveButton : MonoBehaviour
{
 
    public Button[] buttons;

    public void isActive()
    {

        foreach (Button button in buttons)
        {
            button.interactable = true;
        }
    }

    public void notActive()
    {
        foreach (Button button in buttons)
        {
            button.interactable = false;
        }
    }
}
