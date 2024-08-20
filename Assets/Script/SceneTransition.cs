using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneTransition : MonoBehaviour
{
    Animator animator;
    public string sceneTrasitionName = "";
    // Start is called before the first frame update
    void Start()
    {
        animator= GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void changeSceneAnimation(string sceneName)
    {
        this.sceneTrasitionName = sceneName;
        animator.Play("[LS6] Reveal");
    }
    public void changeScene()
    {
        SceneManager.LoadScene(this.sceneTrasitionName);
    }
}
