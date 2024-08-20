using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingManager : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator _animatorComponent;

    void Start()
    {
        _animatorComponent = transform.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void HideLoadingScreen()
    {
        // Call this function, if you want start hiding the loading screen
        _animatorComponent.SetBool(Animator.StringToHash("isHide"), true);
    }
    public void ShowLoadingScreen()
    {
        // Call this function, if you want start hiding the loading screen
        _animatorComponent.SetBool(Animator.StringToHash("isShow"), true);
    }
    public void onFinishHide()
    {
        _animatorComponent.SetBool(Animator.StringToHash("isHide"), false);

    }
    public void onFinishShow()
    {
        _animatorComponent.SetBool(Animator.StringToHash("isShow"), false);
    }
}
