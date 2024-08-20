using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LanguageController : MonoBehaviour
{
    public Sprite image1; // hình ảnh 1
    public Sprite image2; // hình ảnh 2
    private Image buttonImage;
    public static bool isImage1 = true;

    void Start()
    {
        buttonImage = GetComponent<Image>();
        if (PlayerPrefs.GetInt("ButtonImageState") == 0)
        {
            buttonImage.sprite = image1;
            isImage1 = false;
        }
        else
        {
            buttonImage.sprite = image2;
            isImage1 = true;
        }
    }


    public void ChangeImageOnClick()
    {
        if (isImage1)
        {
            buttonImage.sprite = image1;
            isImage1 = false;
            PlayerPrefs.SetInt("ButtonImageState", 0); // Save the state of the button image
        }
        else
        {
            buttonImage.sprite = image2;
            isImage1 = true;
            PlayerPrefs.SetInt("ButtonImageState", 1); // Save the state of the button image
        }
        PlayerPrefs.Save(); // Save the changes to PlayerPrefs
    }

}
