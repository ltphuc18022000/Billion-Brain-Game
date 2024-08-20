using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class CheckValid : MonoBehaviour
{
    public InputField userName, email, displayName, passWord;
    public Text errorText1, errorText2, errorText3, errorText4;

    void Start()
    {
        
        // Gán sự kiện OnEndEdit cho các InputField
        userName.onEndEdit.AddListener(ValidateUserName);
        email.onEndEdit.AddListener(ValidateEmail);
        displayName.onEndEdit.AddListener(ValidateDisplayName);
        passWord.onEndEdit.AddListener(ValidatePassWord);
    }

    // Hàm kiểm tra InputField userName
    private void ValidateUserName(string inputText)
    {
        if (string.IsNullOrEmpty(inputText))
        {
            errorText1.text = "Bạn nhập thiếu tên đăng nhập";
        }
        else if (inputText.Contains(" "))
        {
            errorText1.text = "Tên đăng nhập không được chứa khoảng trắng";
        }
        else if (inputText.Length < 2)
        {
            errorText1.text = "Tên đăng nhập phải chứa ít nhất 2 ký tự";
        }
        else if (Regex.IsMatch(userName.text, @"[\W_]"))
        {
            errorText1.text = "Tên đăng nhập không được viết có dấu!";
        }
        else
        {
            errorText1.text = "";
        }
    }

    // Hàm kiểm tra InputField email
    private void ValidateEmail(string inputText)
    {
        if (string.IsNullOrEmpty(inputText))
        {
            errorText2.text = "Bạn nhập thiếu email";
        }
        else if (!inputText.Contains("@") || !inputText.Contains("."))
        {
            errorText2.text = "Địa chỉ email không hợp lệ";
        }
        else if (!Regex.IsMatch(inputText, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            errorText2.text = "Địa chỉ email không hợp lệ";
        }
        else
        {
            errorText2.text = "";
        }
    }

    // Hàm kiểm tra InputField displayName
    private void ValidateDisplayName(string inputText)
    {
        if (string.IsNullOrEmpty(inputText))
        {
            errorText3.text = "Bạn nhập thiếu Họ Tên";
        }
        else if (inputText.Length < 5)
        {
            errorText3.text = "Bạn phải nhập đầy đủ Họ Tên";
        }
        else
            errorText3.text = "";
    }

    // Hàm kiểm tra InputField passWord
    private void ValidatePassWord(string inputText)
    {
        if (string.IsNullOrEmpty(inputText))
        {
            errorText4.text = "Bạn nhập thiếu mật khẩu";
        }
        else if (inputText.Length < 8)
        {
            errorText4.text = "Mật khẩu phải chứa ít nhất 8 ký tự";
        }
        else if (!Regex.IsMatch(inputText, @"^(?=.*[a-zA-Z])(?=.*\d)[a-zA-Z\d]{8,}$"))
        {
            errorText4.text = "Mật khẩu phải chứa ít nhất 1 chữ và 1 số";
        }
        else
        {
            errorText4.text = "";
        }
    }
}
