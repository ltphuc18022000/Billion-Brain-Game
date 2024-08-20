using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{
    public GameObject Canvas_main;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Read()
    {
        Canvas_main.SetActive(true);
    }
    public void Close()
    {
        Canvas_main.SetActive(false);
    }
}
