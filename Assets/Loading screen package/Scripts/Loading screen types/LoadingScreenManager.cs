using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreenManager : MonoBehaviour
{
    private Animator _animatorComponent;

    private void Start()
    {
        _animatorComponent = transform.GetComponent<Animator>();

        // Remove it if you don't want to hide it in the Start function and call it elsewhere
    }

    //public void RevealLoadingScreen()
    //{
    //    _animatorComponent.SetTrigger("Reveal");
    //}

    //public void HideLoadingScreen()
    //{
    //    // Call this function, if you want start hiding the loading screen
    //    _animatorComponent.SetTrigger("Hide");
    //}

    //public void OnFinishedReveal()
    //{
    //   //transform.parent.GetComponent<DemoSceneManager>().OnLoadingScreenRevealed();
    //}

    //public void OnFinishedHide()
    //{
    //    // TODO: remove it and call your functions 
    //   // transform.parent.GetComponent<DemoSceneManager>().OnLoadingScreenHided();
    //}
    public void OnLevelLoad()
    {
        string sceneName = PlayerPrefs.GetString("scene", "");
        SceneManager.LoadScene(sceneName);
    }
}
