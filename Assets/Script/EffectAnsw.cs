using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectAnsw : MonoBehaviour
{

    public GameObject answer;
    // Start is called before the first frame update

    public void moveAnswer()
    {
        LeanTween.moveLocalX(answer, 0, 2f).setEase(LeanTweenType.easeOutBounce);
    }



}
