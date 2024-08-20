using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Entities.Managers;
using Sfs2X.Requests;
using System;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using System.Security.Claims;
//using UnityEngine.UIElements;

public class BilliantGameManager : MonoBehaviour
{
    private SmartFox sfs;
    private State state;
    public int index;
    public GameObject characterAnimator;
    public TextMeshProUGUI question, countdown, notifi;
    public TextMeshProUGUI answer1, answer2, answer3, answer4;
    public GameObject Table, Time, Win, Wrong, Canvas_main, Gameover, panelTable, score, WAIT;
    public Button[] btn;
    public Color correctColor;
    public Color incorrectColor;
    private Color originalColor;
    private int btnIndex;
    public AudioSource correctSound, incorrectSound;
    public AudioSource[] countDown;
    public SandClock clock;
    public GameObject timeUp;
    public GameObject Loading, listIcon, borderQuestion;
    public GetScore getScore;

    [SerializeField] GameObject pointChangePrefab;
    [SerializeField] Transform pointParent;
    [SerializeField] RectTransform endPoint;
    public GameBilliantController gameBilliantController;
    [SerializeField] Color colorGreen;
    [SerializeField] Color colorRed;
    void Start()
    {
        //Loading.SetActive(true);
        // GetComponent<Button>().onClick.AddListener(delegate { DisableButtons(); });
        
        originalColor = Color.white;
    }
    private void Awake()
    {
        //LeanTween.scale(WAIT, new Vector3(1.05f, 1.05f, 1f), 1f).setLoopType(LeanTweenType.pingPong);
        characterAnimator.GetComponent<Animator>().Play("idl");
    }

    //public void NotClick(bool isClick)
    //{
    //    Button button = GetComponent<Button>();
    //    button.interactable = !isClick; // để khoá button

    //}

    public void DisableButtons(bool isActives)
    {
        foreach (Button button in btn)
        {
            button.image.color = Color.gray;
            button.interactable = !isActives;

        }

    }
    private void ShowPointChange(int change)
    {
        var inst = Instantiate(pointChangePrefab, Vector3.zero, Quaternion.identity);
        inst.transform.SetParent(pointParent, false);


        RectTransform rect = inst.GetComponent<RectTransform>();
        TextMeshProUGUI text = inst.GetComponent<TextMeshProUGUI>(); 

        text.text = "+ " + change.ToString();
        text.color = Color.green;
   
        LeanTween.moveY(rect, endPoint.anchoredPosition.y, 1.5f).setOnComplete(() => {
            LeanTween.scale(score, new Vector3(1.2f, 1.2f, 1.2f), 0.5f);
            LeanTween.scale(score, new Vector3(1f, 1f, 1f), 0.5f).setDelay(0.5f);
            getScore.getScore();
            Destroy(inst);
           
        });
        LeanTween.alphaText(rect, 0.5f, 1.5f);
        
    }
    private enum State
    {
        READY,
        CORRECT,
        WRONG,
        COUNTDOWN,
        END
    };
    public bool isStart=false;
    // Start is called before the first frame update
    public void Init(SmartFox sfs)
    {
        this.sfs = sfs;
        // Add SmartFoxServer-related event listeners required by this game
        sfs.AddEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionResponse);
        // Tell Room Extension that user is ready
        sfs.Send(new ExtensionRequest("ready", new SFSObject(), sfs.LastJoinedRoom));
    }
    public void Update()
    {
        if (state != null&&isStart==false)
        {
            Loading.GetComponent<LoadingManager>().HideLoadingScreen();
            isStart=true;
        }
        
    }
    public void Destroy()
    {
        // Remove SmartFoxServer-related event listeners added by this game
        sfs.RemoveEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionResponse);
        sfs = null;
    }

    public string ConvertStringDecimal(string cmd)
    {
        string pattern = "&#(\\d+);";
        Regex regex = new Regex(pattern);
        MatchCollection matches = regex.Matches(cmd);
        foreach (Match match in matches)
        {
            int decimalCode = int.Parse(match.Groups[1].Value);
            char character = (char)decimalCode;
            string plainText = character.ToString();
            cmd = cmd.Replace(match.Value, plainText);
        }
        return cmd;
    }

    private void OnExtensionResponse(BaseEvent evt)
    {
        string cmd = (string)evt.Params["cmd"];
        ISFSObject data = (SFSObject)evt.Params["params"];


        switch (cmd)
        {
            case "ready":
                WAIT.SetActive(false);
                state = State.READY;
                LeanTween.scale(borderQuestion, new Vector3(1, 1, 1), 0.1f);
                characterAnimator.GetComponent<Animator>().Play("gree");
                //LeanTween.moveLocalX(listIcon, 0, 2f).setEase(LeanTweenType.easeOutBounce);
                SandClock sandClock = FindObjectOfType<SandClock>();
                if (sandClock != null)
                {
                    sandClock.ResetAll();
                }


                clock.roundDuration = (float)data.GetInt("timeRemain");
     
                clock.Begin();
                // Hiển thị text 2
                question.text = ConvertStringDecimal( data.GetUtfString("question"));
                Debug.Log(data.GetUtfString("state") + ":" + data.GetUtfString("question"));
                index = data.GetInt("stt");
                //STT.text = "Câu " + ((int)index + 1).ToString();
                String[] answer = data.GetUtfStringArray("answers");

                answer1.text = ConvertStringDecimal("A. " + answer[0]);
                answer2.text = ConvertStringDecimal("B. " + answer[1]);
                answer3.text = ConvertStringDecimal("C. " + answer[2]);
                answer4.text = ConvertStringDecimal("D. " + answer[3]);    
                
                Loading.GetComponent<LoadingManager>().HideLoadingScreen();
            
               
                break;

            case "correct":
                Debug.Log("CORRECT");
                
                User user = sfs.LastJoinedRoom.GetUserById(data.GetInt("userid"));
                if (sfs.MySelf.Id == data.GetInt("userid"))
                {
                    Debug.Log("Bạn là người trả lời đúng");
                    Win.SetActive(true);
                    Wrong.SetActive(false);
                    notifi.text = "Chúc mừng bạn là người trả lời đúng và nhanh nhất!!!";
                    characterAnimator.GetComponent<Animator>().Play("Happy");
                    ShowPointChange(gameBilliantController.sfs.LastJoinedRoom.UserCount - 1);
                    
                }
                else
                {
                    Debug.Log(user.Name+ " là người trả lời đúng");
                    Win.SetActive(true);
                    Wrong.SetActive(false);
                    //score.SetActive(false);
                    notifi.text = user.Name + " là người trả lời đúng";
                    characterAnimator.GetComponent<Animator>().Play("Angry");
                }
                panelTable.SetActive(true);
                LeanTween.scale(Table, new Vector3(1, 1, 1), 0.8f);

                btn[data.GetInt("correctId")].image.color = Color.white;

                correctSound.Play();

                break;

            case "wrong":
                Debug.Log("WRONG");

                panelTable.SetActive(true);
                LeanTween.scale(Table, new Vector3(1, 1, 1), 1.2f).setOnComplete(HideWrong);
                btn[btnIndex].image.color = Color.red;
                characterAnimator.GetComponent<Animator>().Play("Sad");
                Wrong.SetActive(true);
                Win.SetActive(false);
                incorrectSound.Play();
                getScore.getScore();
                break;

            case "wait":
                Debug.Log("WAIT");
                WAIT.SetActive(true);
                LeanTween.scale(borderQuestion, new Vector3(0, 0, 0), 0.1f);
                LeanTween.scale(listIcon, new Vector3(0, 0, 0), 1f).setEase(LeanTweenType.easeOutBounce);
                LeanTween.moveLocalX(listIcon, 765f, 1f).setEase(LeanTweenType.easeOutBounce).setDelay(1.5f);

                break;  

            case "countdown":
                timeUp.SetActive(false);

                panelTable.SetActive(true);
                Win.SetActive(false);
                Wrong.SetActive(false);
                LeanTween.scale(borderQuestion, new Vector3(0, 0, 0), 1f);
                LeanTween.scale(Table, new Vector3(1, 1, 1), 1f);

                Time.SetActive(true);
                DisableButtons(true);
                state = State.COUNTDOWN;
                //DisableButtons(true);
                LeanTween.scale(listIcon, new Vector3(0, 0, 0), 1f).setEase(LeanTweenType.easeOutBounce);
                LeanTween.moveLocalX(listIcon, 765f, 1f).setEase(LeanTweenType.easeOutBounce).setDelay(1.5f);
    
                //LeanTween.scale(listIcon, new Vector3(0, 0, 0), 1f).setEase(LeanTweenType.easeInExpo);
                int cd = data.GetInt("cd");
                countdown.text = cd.ToString() ;
                if (cd <= 3)
                {
                    characterAnimator.GetComponent<Animator>().Play("idl2");
                  
                
                    //LeanTween.moveX(listIcon, listIcon.transform.position.x, 1f).setEase(LeanTweenType.easeOutBounce);
                    soundCountDown(cd);
         
                }

                if(cd <= 1)
                {
                    LeanTween.scale(Table, new Vector3(0, 0, 0), 1f);
                 
                }

                if (cd == 0) 
                {
                    DisableButtons(false);
                    Time.SetActive(false);
                    panelTable.SetActive(false);
                    ResetColor();
                    Loading.GetComponent<LoadingManager>().ShowLoadingScreen();
                    
                    //LeanTween.scale(listIcon, new Vector3(1, 1, 1), 0.2f).setEase(LeanTweenType.easeInExpo);

                }
                Debug.Log(cd);

                break;

            case "not":
                characterAnimator.GetComponent<Animator>().Play("Sad");
                timeUp.SetActive(true);

                
                break;
            case "end":
                Debug.Log("End game");
                StartCoroutine(WinGame());
                
                break;
        }

    }
    public void moveAnswer()
    {
        LeanTween.scale(listIcon, new Vector3(1, 1, 1), 0.1f).setEase(LeanTweenType.easeOutBounce);
        LeanTween.moveLocalX(listIcon, 0, 2f).setEase(LeanTweenType.easeOutBounce).setDelay(0.2f);
    }

    //public void resetAnswer()
    //{
    //    LeanTween.moveLocalX(listIcon, 765f, 1f).setEase(LeanTweenType.easeOutBounce);
    //}

    IEnumerator WinGame()
    {
        yield return new WaitForSeconds(3f);
        Canvas_main.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        LeanTween.scale(Gameover, new Vector3(1, 1, 1), 0.5f);
    }

    public void answer(int indexAnswer)
    {
        btnIndex = indexAnswer;
        if (state == State.READY)
        {
            ISFSObject ans = new SFSObject();
            ans.PutInt("uid", 1);
            ans.PutInt("index", index);
            ans.PutInt("a", indexAnswer);
            DisableButtons(true);
            // Send move to Extension
            sfs.Send(new ExtensionRequest("answer", ans, sfs.LastJoinedRoom));
        }
    }
    public void ResetColor()
    {
        for(int i=0;i<btn.Length;i++)
        {
            btn[i].image.color = originalColor;
        }
    }
    public void HideWrong()
    {
        StartCoroutine(DelayTable());
    }


    IEnumerator DelayTable()
    {
        yield return new WaitForSeconds(3f);
        LeanTween.scale(Table, new Vector3(0, 0, 0), 0.5f);
        panelTable.SetActive(false);
        DisableButtons(false);
        ResetColor();

    }
    void soundCountDown(int cd)
    {
        switch (cd)
        {
            case 0:
                countDown[0].Play();
                break;
            case 1:
                countDown[1].Play();
                break;
            case 2:
                countDown[2].Play();
                break;
            case 3:
                countDown[3].Play(); 
                break;
        }
    }
}
