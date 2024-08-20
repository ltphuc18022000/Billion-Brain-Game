using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using UnityEngine.UI;
using Newtonsoft.Json;
using Sfs2X.Entities;
using System.Net;
using System;
using Sfs2X.Requests;
using UnityEngine.SceneManagement;
using TMPro;

public class RegisterAPI : MonoBehaviour
{
    private readonly string basePath = "https://tuatoi.xyz/wp/api";
    private RequestHelper requestHelper;
    public InputField userName, email, displayName, passWord;
    public Text errorText1, errorText2, errorText3, errorText4;
    public TextMeshProUGUI errorText;
    
    Manager manager;
    public ShowUi showUi;
    public void Start()
    {
        // showUi.changUIState();
        StartCoroutine(WaitLoad());
        manager= new Manager();
    }
    public void callApi()
    {
        if (errorText1.text == "" && errorText2.text == "" && errorText3.text == "" && errorText4.text == "")
        {
            getToken();
        }
        else
        {
            StartCoroutine(ShowText());
            Debug.Log("Error check");
        }
    }
    IEnumerator ShowText()
    {
        // Hiển thị văn bản
        errorText.text = "Bạn chưa nhập đúng yêu cầu!";

        // Chờ 5 giây
        yield return new WaitForSeconds(4f);

        // Tắt văn bản
        errorText.text = "";
    }
    public void getToken()
    {
        DateTime currentTime = DateTime.UtcNow;
        long unixTime = ((DateTimeOffset)currentTime).ToUnixTimeSeconds();
        requestHelper = new RequestHelper
        {
            Uri = basePath + "/get_nonce/",
         
            Params = new Dictionary<string, string>
            {
                { "controller", "user" },
                { "method", "register" },
                { "rand",  unixTime.ToString()}
            },
            EnableDebug = true
        };
        requestHelper.Headers = new Dictionary<string, string>
        {
            { "Authorization", "Bearer aHVuZ3BxOmshZW0kTk9XODE=" }
        };
        RestClient.Post(requestHelper).Then(res =>
        {
            GetToken getToken = JsonConvert.DeserializeObject<GetToken>(res.Text);
            Debug.Log(getToken.nonce);
            if (getToken.nonce != null)
            {
                register(getToken.nonce);
            }

        });

    }

    public void register(string token)
    {

        requestHelper = new RequestHelper
        {
            Uri = basePath + "/user/register/",

            Params = new Dictionary<string, string>
            {
                { "username", userName.text },
                { "email", email.text },
                { "nonce", token},
                { "display_name", displayName.text },
                { "notify", "no"},
                { "user_pass",passWord.text}
            },
            EnableDebug = true
        };
        requestHelper.Headers = new Dictionary<string, string>
        {
            { "Authorization", "Bearer aHVuZ3BxOmshZW0kTk9XODE=" }
        };
        RestClient.Post(requestHelper).Then(res =>
        {
            Status status = JsonConvert.DeserializeObject<Status>(res.Text);
            if (status.status == "ok")
            {
                Register reg = JsonConvert.DeserializeObject<Register>(res.Text);
                StartCoroutine(Correct());
                
                Debug.Log(reg.status);
                Debug.Log("OK");
            }
            if (status.status == "error")
            {
                Error err = JsonConvert.DeserializeObject<Error>(res.Text);
                errorText.text = err.error;
                Debug.Log(err.error);
            }
            
        });
  
    }

    IEnumerator Correct()
    {
        errorText.color = Color.green;
        errorText.text = "Bạn đã đăng ký thành công";
        yield return new WaitForSeconds(3f);
        errorText.text = "";
        errorText.color = Color.red;
        manager.LoadLogin2();
        showUi.HideAndLoadScene(1);
    }


    public class GetToken
    {
        public string status;
        public string controller;
        public string method;
        public string nonce;
    }
    public class Register
    {
        public string status;
        public string cookie;
        public string cookie_admin;
        public string cookie_name;
        public int user_id;
        public string username;

    }

    public class Error
    {
        public string status;
        public string error;
    }

    public class Status
    {
        public string status;
    }

    IEnumerator WaitLoad()
    {
        yield return new WaitForSeconds(1f);
        showUi.changUIState();
    }

}