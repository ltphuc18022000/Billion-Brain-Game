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
using UnityEditor;
using Sfs2X;
using static GetScore;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;
using TMPro;

public class GetScore : BaseSceneController
{
    public readonly string basePath = "https://tuatoi.xyz/wp/api";
    private RequestHelper requestHelper;
    private int game_id;
    public TextMeshProUGUI Score;
    private SmartFox sfs;
    

    public void Start()
    {

        
        sfs = gm.GetSfsClient();
        getScore();

    }

    public void getScore()
    {
        DateTime currentTime = DateTime.UtcNow;
        long unixTime = ((DateTimeOffset)currentTime).ToUnixTimeSeconds();

        requestHelper = new RequestHelper
        {
            Uri = basePath + "/user/getUserScore/",

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

                Status sta = JsonConvert.DeserializeObject<Status>(res.Text);
                Score.text = sta.user_score.ElementAt(0).score ;

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

    [System.Serializable]
    public class Status
    {
        public string status;
        public List<UserScore> user_score;
    }

    [System.Serializable]
    public class UserScore
    {
        public string game_id;
        public string player;
        public string score;
        public string scoredate;

    }
}
