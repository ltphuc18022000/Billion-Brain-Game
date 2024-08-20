using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using UnityEngine.UI;
using Newtonsoft.Json;
using Sfs2X.Entities;
using System.Net;
using System;
using Unity.VisualScripting.Antlr3.Runtime;

using Sfs2X.Requests;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using Sfs2X;
using static GetInfo;
using System.Linq;
using TMPro;

public class GetInfo : BaseSceneController
{
    private readonly string basePath = "https://tuatoi.xyz/wp/api";
    private RequestHelper requestHelper;
    private int game_id;
    public TextMeshProUGUI displayName;
    private SmartFox sfs;

    public void Start()
    {
        sfs = gm.GetSfsClient();
        getInfo();
    }

    public void getInfo()
    {
        DateTime currentTime = DateTime.UtcNow;
        long unixTime = ((DateTimeOffset)currentTime).ToUnixTimeSeconds();

        requestHelper = new RequestHelper
        {
            Uri = basePath + "/user/get_game_userinfo/",

            Params = new Dictionary<string, string>
            {
                { "username", sfs.MySelf.Name},
                { "game_id", "1" },
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
            Status status = JsonConvert.DeserializeObject<Status>(res.Text);
            if (status.status == "ok")
            {
                UserInfo info = JsonConvert.DeserializeObject<UserInfo>(res.Text);
                displayName.text = info.username;
                Debug.Log(info);
            }
            
        });
    }

    protected override void RemoveSmartFoxListeners()
    {
        //throw new NotImplementedException();
    }

    protected override void HideModals()
    {
        throw new NotImplementedException();
    }

    public class Status
    {
        public string status;
    }

    [System.Serializable]
    public class UserInfo
    {
        public string status;
        public string id;
        public string username;
        public string nicename;
        public string displayname;
        public string firstname;
        public string lastname;
        public string nickname;
        public string avatar;
    }
}