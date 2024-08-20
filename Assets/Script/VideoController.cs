using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


public class VideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    private bool hasCalledMyFunction = false;
    public ShowUi showUi;
    public bool isStopAtEnd = false;
    void Start()
    {
      /*  int isPlayVideo = PlayerPrefs.GetInt("isPlayVideo");
         Debug.Log(isPlayVideo);
         videoPlayer = GetComponent<VideoPlayer>();

         if (isPlayVideo == 0)
         {
             videoPlayer.Play();

         }
         if (isPlayVideo == 1)
         {
             videoPlayer.loopPointReached += OnVideoPlayerEnd;
             showUi.changUIState();
         }*/
        if (isStopAtEnd)
        {
            videoPlayer.loopPointReached += OnVideoPlayerEnd;
        }     
    }
    void Update()
    {
        if (!hasCalledMyFunction && videoPlayer.frame >= 350)
        {
            showUi.changUIState();
            PlayerPrefs.SetInt("isPlayVideo", 1);
            hasCalledMyFunction = true;
        }
      

    }
    void OnVideoPlayerEnd(VideoPlayer vp)
    {
        vp.time = vp.length;
    }
   
}
