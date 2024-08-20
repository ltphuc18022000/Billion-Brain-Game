using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class changeImage : MonoBehaviour
{
    public Button button1;
    public Button button2;
    public Sprite defaultImage1;
    public Sprite defaultImage2;
    public Sprite newImage1;
    public Sprite newImage2;

    private bool isChanged = false;

    // Start is called before the first frame update
    void Start()
    {
        // Kiểm tra nếu có lưu trữ tên sprite trong PlayerPrefs thì tải sprite đó lên và gán cho button
        if (PlayerPrefs.HasKey("button1Image") && PlayerPrefs.HasKey("button2Image"))
        {
            string button1ImageName = PlayerPrefs.GetString("button1Image");
            Sprite button1Sprite = Resources.Load<Sprite>(button1ImageName);
            if (button1Sprite != null)
            {
                button1.image.sprite = button1Sprite;
            }

            string button2ImageName = PlayerPrefs.GetString("button2Image");
            Sprite button2Sprite = Resources.Load<Sprite>(button2ImageName);
            if (button2Sprite != null)
            {
                button2.image.sprite = button2Sprite;
            }

            // Đánh dấu là đã đổi ảnh trước đó
            isChanged = true;
        }
        else
        {
            // Gán ảnh mặc định cho button
            button1.image.sprite = defaultImage1;
            button2.image.sprite = defaultImage2;
        }
    }

    // Update is called once per frame
    public void ChangeImage()
    {
        if (isChanged)
        {
            // Gán ảnh mặc định cho button
            button1.image.sprite = defaultImage1;
            PlayerPrefs.DeleteKey("button1Image");

            button2.image.sprite = defaultImage2;
            //button2.image.rectTransform.sizeDelta = new Vector2(230, 60);
            //button2.image.rectTransform.localScale = new Vector2(1.2f, 1.2f);

            PlayerPrefs.DeleteKey("button2Image");


            // Đánh dấu là chưa đổi ảnh
            isChanged = false;
        }
        else
        {
            // Gán ảnh mới cho button
            button1.image.sprite = newImage1;
            PlayerPrefs.SetString("button1Image", newImage1.name);

            button2.image.sprite = newImage2;
            //button2.image.rectTransform.sizeDelta = new Vector2(230, 60);
            //button2.image.rectTransform.localScale = new Vector2(1, 1.2f);
            PlayerPrefs.SetString("button2Image", newImage2.name);

            // Đánh dấu là đã đổi ảnh
            isChanged = true;
        }

        // Lưu trữ PlayerPrefs
        PlayerPrefs.Save();
    }
}
