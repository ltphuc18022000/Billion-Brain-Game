using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ShowUi : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isShow;
    public CanvasGroup UIGroup;
    public GameObject Group;
    void Start()
    {
        if (UIGroup == null)
        {
            UIGroup =GetComponent<CanvasGroup>();
        }
        if (isShow == false)
        {
            LeanTween.alphaCanvas(UIGroup, 0, 0);
            Group.SetActive(isShow);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void changUIState()
    {
        if(isShow)
        {
            LeanTween.alphaCanvas(UIGroup, 0, 1).setOnComplete(hide);
            isShow = !isShow;
        }
        else
        {
            LeanTween.alphaCanvas(UIGroup, 1, 1);
            isShow = !isShow;
            Group.SetActive(isShow);
        }
    }
    private void hide()
    {
        Group.SetActive(false);
    }
    public void HideAndLoadScene(int timeDelay)
    {
        changUIState();
        StartCoroutine(waitToLoad(timeDelay));
    }
    IEnumerator waitToLoad(int delay)
    {
        yield return new WaitForSeconds(delay);
        string sceneName = PlayerPrefs.GetString("scene", "");
        SceneManager.LoadScene(sceneName);
    }
    public ShowUi()
    {
    }
}
