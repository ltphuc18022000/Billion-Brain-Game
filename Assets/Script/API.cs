using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using UnityEngine.UI;
using Newtonsoft.Json;
using Sfs2X.Entities;

public class API : MonoBehaviour
{
    private readonly string basePath = "https://tuatoi.xyz/wp/api";
    private RequestHelper requestHelper;


    public string getToken()
    {
        requestHelper = new RequestHelper
        {
            Uri = basePath + "/get_nonce/",

            Params = new Dictionary<string, string>
            {
                { "controller", "user" },
                { "method", "register" }
            },
            EnableDebug = true
        };
        RestClient.Post(requestHelper).Then(res =>
        {
            GetToken getToken = JsonConvert.DeserializeObject<GetToken>(res.Text);
            Debug.Log(getToken.nonce);

            if(getToken != null )
            {

            }
        });
        return null;
    }

    public void register()
    {
        requestHelper = new RequestHelper
        {
            Uri = basePath + "/user/register/",

            Params = new Dictionary<string, string>
            {
                { "controller", "user" },
                { "method", "register" }
            },
            EnableDebug = true
        };
        RestClient.Post(requestHelper).Then(res =>
        {
            GetToken getToken = JsonConvert.DeserializeObject<GetToken>(res.Text);
            Debug.Log(getToken.nonce);

            if (getToken != null)
            {

            }
        });
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
        public string uss;
    }
}
