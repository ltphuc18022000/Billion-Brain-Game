using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;

using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    //public SceneTransition sceneTransition;
    PlayerPrefs playerPrefs;
    public GameObject loading;
    private static Manager instance;

    public static Manager Instance { get { return instance; } }
    private void Awake()
    {
        Manager.instance = this;
    }
    public void ExitFromGame()
    {
        Application.Quit();
    }
    public void LoadLogin()
    {
        PlayerPrefs.SetString("scene", "Login");
    }
    public void LoadLogin2()
    {
        PlayerPrefs.SetString("scene", "Login2");
    }

    public void LoadMainMenu()
    {
        Load("MainMenu");
    }
    public void LoadPlayScene()
    {
        Load("GamePlay");
    }

    public void LoadRetrivePassword()
    {

        Load("RetrivePassword");
    }

    public void LoadModeGame()
    {
        Load("Lobby");
    }

    public void LoadClaim()
    {
        Load("Claim");
    }

    public void LoadRankingQuarter()
    {
        Load("RankingQuarter");
    }

    public void LoadRankingMonth()
    {
        Load("RankingMonth");
    }

    public void LoadMail()
    {
        Load("Mail");
    }

    public void SignUp()
    {
        PlayerPrefs.SetString("scene", "SignUp");
    }
    public void Load(string scene)
    {
        PlayerPrefs.SetString("scene", scene);
        loading.SetActive(true);
    }
    public Manager() { }
}