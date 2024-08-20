using UnityEngine;
using UnityEngine.UI;

public class InputFieldController : MonoBehaviour
{
    public InputField inputField;
    public Image image;
    public Sprite sprite1;
    public Sprite sprite2;

    public void ToggleInputFieldType()
    {
        if (inputField.contentType == InputField.ContentType.Password)
        {
            inputField.contentType = InputField.ContentType.Standard;
        }
        else
        {
            inputField.contentType = InputField.ContentType.Password;
        }
        inputField.ForceLabelUpdate();
    }

    public void ToggleImage()
    {
        if (image.sprite == sprite1)
        {
            image.sprite = sprite2;
        }
        else
        {
            image.sprite = sprite1;
        }
    }
}
