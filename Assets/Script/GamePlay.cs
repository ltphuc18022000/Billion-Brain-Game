using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePlay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GameStart());
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator GameStart()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(3);
    }
}
