using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;
using UnityEngine.Rendering.PostProcessing;

public class EffectMenu : MonoBehaviour
{
    public PostProcessVolume volume;

    private Bloom bloom;
    private float initialIntensity;

    public GameObject mail, play, task, claim, monthTop, quyTop, logo, glow;
    // Start is called before the first frame update
    void Start()
    {
        LeanTween.moveLocalY(mail, 0, 1.5f).setEase(LeanTweenType.easeOutBounce);
        //LeanTween.move(mail, mail.transform.position, 1f).setDelay(1.7f);

        LeanTween.moveLocalY(play, 0, 1.5f).setEase(LeanTweenType.easeOutBounce).setOnComplete(glowStart);
        LeanTween.moveLocalY(glow, 0, 1.5f).setEase(LeanTweenType.easeOutBounce);
        //LeanTween.move(play, play.transform.position, 1f).setDelay(1.7f);
        //LeanTween.scale(play, new Vector3(1.05f, 1.05f, 1f), 0.5f).setLoopType(LeanTweenType.pingPong).setDelay(2f);


        LeanTween.moveLocalY(task, 0f, 1.5f).setEase(LeanTweenType.easeOutBounce);
        //LeanTween.move(task, task.transform.position, 1f).setDelay(1.7f);

        LeanTween.moveLocalX(claim, 0, 1.5f).setEase(LeanTweenType.easeOutBounce);

        LeanTween.scale(monthTop, new Vector3(1, 1, 1), 1f).setEase(LeanTweenType.easeInExpo);
        LeanTween.scale(quyTop, new Vector3(1, 1, 1), 1f).setEase(LeanTweenType.easeInExpo);

        LeanTween.alpha(logo.GetComponent<RectTransform>(), 1.0f, 1f).setEase(LeanTweenType.easeInOutSine);
        LeanTween.scale(logo, new Vector3(1f, 1f, 1f), 1f).setEase(LeanTweenType.easeInExpo);
        //LeanTween.scale(logo, new Vector3(1.1f, 1.1f, 1.1f), 2f).setLoopType(LeanTweenType.pingPong).setDelay(1.1f);



    }

    void glowStart()
    {
        LeanTween.alpha(glow.GetComponent<RectTransform>(), 1.0f, 1f).setEase(LeanTweenType.easeInOutSine);
        LeanTween.scale(glow, new Vector3(0.9f, 0.9f, 0.9f), 0.5f).setEase(LeanTweenType.easeInExpo);
        LeanTween.scale(glow, new Vector3(1f, 1f, 1f), 1f).setLoopType(LeanTweenType.pingPong).setDelay(1f);
    }

    private void Awake()
    {
        if (!volume.profile.TryGetSettings(out bloom))
        {
            enabled = false;
            return;
        }
        initialIntensity = bloom.intensity.value;
        bloom.intensity.Override(0f);
        StartCoroutine(SetIntensity());
    }



    private IEnumerator SetIntensity()
    {
        yield return new WaitForSeconds(0.25f); // chờ 0.5 giây

        bloom.intensity.Override(20f);
        yield return new WaitForSeconds(0.1f); // chờ 0.5 giây

        float elapsedTime = 0f;
        float duration = 2f; // thời gian để chuyển từ giá trị ban đầu sang giá trị mới

        float startIntensity = bloom.intensity.value;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            bloom.intensity.Override(Mathf.Lerp(startIntensity, initialIntensity, t));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        bloom.intensity.Override(initialIntensity); ;
    }

}
