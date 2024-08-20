using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using UnityEngine.UI;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

public class RetrivePassword : MonoBehaviour
{

    private readonly string basePath = "https://tuatoi.xyz/wp/api";
    private RequestHelper requestHelper;
    public InputField userLogin, email;
    public Text errorText;
    Manager manager;
    public ShowUi showUi;

    public void Start()
    {
        // showUi.changUIState();
        StartCoroutine(WaitLoad());
    }

    public void retrivePassword()
    {
        requestHelper = new RequestHelper
        {
            Uri = basePath + "/user/retrieve_password/",

            Params = new Dictionary<string, string>
            {
                { "user_login", userLogin.text},
                { "user_email", email.text },

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
                Login log = JsonConvert.DeserializeObject<Login>(res.Text);
                manager.LoadLogin();
                Debug.Log(log.message);
            }
            if (status.status == "error")
            {
                Error err = JsonConvert.DeserializeObject<Error>(res.Text);
                errorText.text = err.error;
            }
        }).Catch(err => {
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
    IEnumerator WaitLoad()
    {
        yield return new WaitForSeconds(1f);
        showUi.changUIState();
    }

}
