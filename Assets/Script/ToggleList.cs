using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleList : MonoBehaviour
{

    public GameObject Table;
    private bool objectIsVisible = false;

    public void ToggleObject()
    {
        if (objectIsVisible)
        {
            LeanTween.scale(Table, new Vector3(0, 0, 0), 0.5f);
            objectIsVisible = false;
        }
        else
        {
            LeanTween.scale(Table, new Vector3(1.4f, 0.85f, 1), 0.5f);
            objectIsVisible = true;
        }
    }

}
