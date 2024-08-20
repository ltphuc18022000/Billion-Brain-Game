using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Variables;
using Sfs2X.Requests.Buddylist;
using Sfs2X.Requests;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameBilliantController : BaseSceneController
{
    public BilliantGameManager gameManager;
    //----------------------------------------------------------
    // Private properties
    //----------------------------------------------------------
    public TextMeshProUGUI player;
    public TextMeshProUGUI score;


    public SmartFox sfs;
    public RecyclableScrollerDemo recyclableScroller;

    protected override void HideModals()
    {
       
    }

    protected override void RemoveSmartFoxListeners()
    {
        sfs.RemoveEventListener(SFSEvent.USER_ENTER_ROOM, OnUserEnterRoom);
        sfs.RemoveEventListener(SFSEvent.USER_EXIT_ROOM, OnUserExitRoom);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Set a reference to the SmartFox client instance
        sfs = gm.GetSfsClient();
        ;
        // Hide modal panels
        HideModals();
        // Add event listeners
        AddSmartFoxListeners();

        // Initialize game manager
        // The SmartFox class instance is passed, so that the game manager can add its own listeners
        // and interact with the server-side Room Extension containing the game logic
        gameManager.Init(sfs);

        player.text = sfs.LastJoinedRoom.UserCount.ToString();
        score.text = ((int)sfs.LastJoinedRoom.UserCount - 1).ToString() ;

        recyclableScroller.InitData();
        Debug.Log(" Số người chơi" + sfs.LastJoinedRoom.UserCount);
    }

    // Update is called once per frame
    void Update()
    {
        player.text = sfs.LastJoinedRoom.UserCount.ToString();
        score.text = (((int)sfs.LastJoinedRoom.UserCount - 1).ToString());
    }
    private void AddSmartFoxListeners()
    {
        sfs.AddEventListener(SFSEvent.USER_ENTER_ROOM, OnUserEnterRoom);
        sfs.AddEventListener(SFSEvent.USER_EXIT_ROOM, OnUserExitRoom);
    }
    private void OnUserEnterRoom(BaseEvent evt)
    {
        User user = (User)evt.Params["user"];
        Room room = (Room)evt.Params["room"];

        // Display system message
        Debug.Log("User " + user.Name + " joined this game as " + (user.IsPlayerInRoom(room) ? "player" : "spectator"));

        player.text = room.UserList.Count.ToString();
        score.text = (((int)sfs.LastJoinedRoom.UserCount - 1).ToString());
        recyclableScroller.InitData();
        // Stop timeout
        //  if (user.IsPlayerInRoom(room))
        //      StopTimeout(false);
    }

    private void OnUserExitRoom(BaseEvent evt)
    {
        User user = (User)evt.Params["user"];
        Room room = (Room)evt.Params["room"];

        player.text = room.UserList.Count.ToString();
        score.text = (((int)sfs.LastJoinedRoom.UserCount - 1).ToString());
        recyclableScroller.InitData();
        // Display system message
        if (user != sfs.MySelf)
            Debug.Log("User " + user.Name + " left the game");

    }
    public void OnLeaveButtonClick()
    {
        // Destroy game manager
        gameManager.Destroy();

        // Leave current game room
        sfs.Send(new LeaveRoomRequest());
        // Return to lobby scene
        SceneManager.LoadScene("MainMenu");
    }
}
