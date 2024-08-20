using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using UnityEngine.UI;
using Newtonsoft.Json;
using Sfs2X.Entities;
using System.Net;
using System;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine.SceneManagement;

public class LoginAPI : MonoBehaviour
{
    private readonly string basePath = "https://tuatoi.xyz/wp/api";
    private RequestHelper requestHelper;
    public InputField userLogin, userPassword;
    public Text errorText;
    public LoginSceneController loginSceneController;

    public void login()
    {
        requestHelper = new RequestHelper
        {
            Uri = basePath + "/users/login/",

            Params = new Dictionary<string, string>
            {
                { "user_login", userLogin.text},
                { "user_password", userPassword.text },

            },
           EnableDebug= true
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
                //Login log = JsonConvert.DeserializeObject<Login>(res.Text);
                Login log = Login.CreateFromJson(res.Text);
                Debug.Log(res.Text);
                
                Debug.Log(log.message);
                loginSceneController.OnLoginButtonClick();
            }
            if (status.status == "error")
            {
                Error err = JsonConvert.DeserializeObject<Error>(res.Text);
                errorText.text = err.error;
            }
        }).Catch(err=> {
            Debug.Log(err.Message);

        });
    }
    [System.Serializable]
    public class Login
    {
        public string status;
        public string message;
        public static Login CreateFromJson(string jsonString)
        {
            return JsonUtility.FromJson<Login>(jsonString);
        }
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


}

