using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Entities.Invitation;
using Sfs2X.Entities.Match;
using Sfs2X.Entities.Variables;
using Sfs2X.Requests;
using Sfs2X.Requests.Buddylist;
using Sfs2X.Requests.Game;

/**
 * Script attached to the Controller object in the Lobby scene.
 */
public class LobbySceneController : BaseSceneController
{
    public static string BUDDYVAR_YEAR = SFSBuddyVariable.OFFLINE_PREFIX + "year";
    public static string BUDDYVAR_MOOD = "mood";

    public static string USERVAR_EXPERIENCE = "exp";
    public static string USERVAR_RANKING = "rank";

    public static string DEFAULT_ROOM = "The Lobby";
    public static string GAME_ROOMS_GROUP_NAME = "games";

    //----------------------------------------------------------
    // UI elements
    //----------------------------------------------------------

    //public Text loggedInAsLabel;
    public Text userStatusLabel;
    public StartGamePanel startGamePanel;
    public InvitationPanel invitationPanel;
    public WarningPanel warningPanel;

    public Transform gameListContent;
    public GameListItem gameListItemPrefab;

    //----------------------------------------------------------
    // Private properties
    //----------------------------------------------------------

    private SmartFox sfs;
    private Dictionary<int, GameListItem> gameListItems;
    private Dictionary<string, BuddyListItem> buddyListItems;

    // Comment EXTENSION_ID and EXTENSION_CLASS constants below and
    // uncomment the following to use the Java version of the Tic Tac Toe Extension
    private const string EXTENSION_ID = "BilliantExtension";
    private const string EXTENSION_CLASS = "Extension.BilliantGameExtension";

    // Comment above EXTENSION_ID and EXTENSION_CLASS constants and
    // uncomment the following to use the JavaScript version of the Tic Tac Toe Extension
    //private const string EXTENSION_ID = "TicTacToe-JS";
    //private const string EXTENSION_CLASS = "TicTacToeExtension.js";

    //----------------------------------------------------------
    // Unity callback methods
    //----------------------------------------------------------
    private void Start()
    {
        // Set a reference to the SmartFox client instance
        sfs = gm.GetSfsClient();

        // Hide modal panels
        HideModals();

        // Display username in footer and user profile panel
    //    loggedInAsLabel.text = "Logged in as <b>" + sfs.MySelf.Name + "</b>";
        // Add event listeners
        AddSmartFoxListeners();

        // Populate list of available games
        PopulateGamesList();

        // Join default Room
        // An initial Room where to join all users is needed to implement a public chat (not in this example)
        // or to automatically invite users to enter a private game when launched (see OnStartGameConfirm method below)
        // In this example we assume the Room already exists in the static SmartFoxServer configuration
        sfs.Send(new JoinRoomRequest(DEFAULT_ROOM));
    }

    //----------------------------------------------------------
    // UI event listeners
    //----------------------------------------------------------
    /**
	 * On Logout button click, disconnect from SmartFoxServer.
	 * This causes the SmartFox listeners added by this scene to be removed (see BaseSceneController.OnDestroy method)
	 * and the Login scene to be loaded (see SFSClientManager.OnConnectionLost method).
	 */
    public void OnLogoutButtonClick()
    {
        // Disconnect from SmartFoxServer
        sfs.Disconnect();
    }

    /**
	 * On Start game button click, show Start Game Panel prefab instance.
	 */
    public void OnStartGameButtonClick()
    {
        string roomName = "#" + sfs.MySelf.Name ;

        SFSGameSettings settings = new SFSGameSettings(roomName);
        settings.GroupId = GAME_ROOMS_GROUP_NAME;
        settings.MaxUsers = 1000;
        settings.MaxSpectators = 10;
        settings.IsPublic = true;
        settings.Extension = new RoomExtension(EXTENSION_ID, EXTENSION_CLASS);

        // Request Room creation to server
        sfs.Send(new CreateSFSGameRequest(settings));
    }

    /**
	 * On Quick join button click, send request to join a random game Room.
	 */
    public void OnQuickJoinButtonClick()
    {
        // Quick join a game in the "games" group among those matching the current user's player profile
        sfs.Send(new QuickJoinGameRequest(null, new List<string>() { GAME_ROOMS_GROUP_NAME }, sfs.LastJoinedRoom));
    }

    /**
	 * On:
	 * - Start game button click on the Start game panel, or
	 * - Invite buddy button click on a Buddy List Item prefab instance,
	 * create and join a new game Room.
	 */
    public void OnStartGameConfirm(bool isPublic, string buddyName)
    {
        // Configure Room

    }

    /**
	 * On Play game button click in Game List Item prefab instance, join an existing game Room as a player.
	 */
    public void OnGameItemPlayClick(int roomId)
    {
        // Join game Room as player
        sfs.Send(new Sfs2X.Requests.JoinRoomRequest(roomId, null, sfs.LastJoinedRoom.Id));
    }

    /**
	 * On Watch game button click in Game List Item prefab instance, join an existing game Room as a spectator.
	 */
    public void OnGameItemWatchClick(int roomId)
    {
        // Join game Room as spectator
        sfs.Send(new Sfs2X.Requests.JoinRoomRequest(roomId, null, sfs.LastJoinedRoom.Id, true));
    }





    /**
	 * On Invite button click in Buddy List Item prefab instance, start new game and invite buddy.
	 */
    public void OnInviteBuddyButtonClick(string buddyName)
    {
        OnStartGameConfirm(false, buddyName);
    }

    /**
	 * On custom event fired by User Profile Panel prefab instance, set user's Buddy Variables.
	 */
    public void OnBuddyDetailChange(string varName, object value)
    {
        List<BuddyVariable> buddyVars = new List<BuddyVariable>();
        buddyVars.Add(new SFSBuddyVariable(varName, value));

        // Set Buddy Variables
        sfs.Send(new SetBuddyVariablesRequest(buddyVars));
    }

    /**
	 * On custom event fired by Invitation Panel prefab instance, send reply to invitation.
	 */
    //----------------------------------------------------------
    // Helper methods
    //----------------------------------------------------------
    /**
	 * Add all SmartFoxServer-related event listeners required by the scene.
	 */
    private void AddSmartFoxListeners()
    {
        sfs.AddEventListener(SFSEvent.ROOM_ADD, OnRoomAdded);
        sfs.AddEventListener(SFSEvent.ROOM_REMOVE, OnRoomRemoved);
        sfs.AddEventListener(SFSEvent.USER_COUNT_CHANGE, OnUserCountChanged);
        sfs.AddEventListener(SFSEvent.ROOM_JOIN, OnRoomJoin);
        sfs.AddEventListener(SFSEvent.ROOM_JOIN_ERROR, OnRoomJoinError);
        sfs.AddEventListener(SFSEvent.ROOM_CREATION_ERROR, OnRoomCreationError);

    }

    /**
	 * Remove all SmartFoxServer-related event listeners added by the scene.
	 * This method is called by the parent BaseSceneController.OnDestroy method when the scene is destroyed.
	 */
    override protected void RemoveSmartFoxListeners()
    {
        sfs.RemoveEventListener(SFSEvent.ROOM_ADD, OnRoomAdded);
        sfs.RemoveEventListener(SFSEvent.ROOM_REMOVE, OnRoomRemoved);
        sfs.RemoveEventListener(SFSEvent.USER_COUNT_CHANGE, OnUserCountChanged);
        sfs.RemoveEventListener(SFSEvent.ROOM_JOIN, OnRoomJoin);
        sfs.RemoveEventListener(SFSEvent.ROOM_JOIN_ERROR, OnRoomJoinError);
        sfs.RemoveEventListener(SFSEvent.ROOM_CREATION_ERROR, OnRoomCreationError);

    }

    /**
	 * Hide all modal panels.
	 */
    override protected void HideModals()
    {
        startGamePanel.Hide();
        invitationPanel.Hide();
        warningPanel.Hide();
    }

    /**
	 * Initialize player profile.
	 * 
	 * IMPORTANT NOTE
	 * The Experience and Ranking details are custom parameters used in this example to show how to filter players through SmartFoxServer's matchmaking system
	 * when sending invitations to play a game. Of course they (or any other parameter with a similar purpose) shouldn't be set manually, but they should be
	 * determined by the game logic, saved in a database and set in User Variables by a server-side Extension upon user login.
	 * In this example, for sake of simplicity, such details are set to default values when the Lobby scene is loaded and they are lost upon user logout.
	 */

    /**
	 * Display list of existing games.
	 */
    private void PopulateGamesList()
    {
        // Initialize list
        if (gameListItems == null)
            gameListItems = new Dictionary<int, GameListItem>();

        // For the game list we use a scrollable area containing a separate prefab for each Game Room
        // The prefab contains clickable buttons to join the game
        List<Room> rooms = sfs.RoomManager.GetRoomList();
        Debug.Log(rooms.Count);

        // Display game list items
        foreach (Room room in rooms)
        {
            Debug.Log(room.Name);
            AddGameListItem(room);
        }
    }

    /**
	 * Create Game List Item prefab instance and add to games list.
	 */
    private void AddGameListItem(Room room)
    {
        // Show only game rooms
        // Also password protected Rooms are skipped, to make this example simpler
        // (protection would require an interface element to input the password)
        if (!room.IsGame || room.IsHidden || room.IsPasswordProtected)
            return;

        // Create game list item
        GameListItem gameListItem = Instantiate(gameListItemPrefab);
        gameListItems.Add(room.Id, gameListItem);

        // Init game list item
        gameListItem.Init(room);

        // Add listener to play and watch buttons
        gameListItem.playButton.onClick.AddListener(() => OnGameItemPlayClick(room.Id));
        gameListItem.watchButton.onClick.AddListener(() => OnGameItemWatchClick(room.Id));

        // Add game list item to container
        gameListItem.gameObject.transform.SetParent(gameListContent, false);
    }


    /**
	 * Initialize buddy-related entities.
	

	
	/**
	 * Process the invitation in queue, displaying the invitation accept/refuse panel.
	 */

    //----------------------------------------------------------
    // SmartFoxServer event listeners
    //----------------------------------------------------------
    private void OnRoomAdded(BaseEvent evt)
    {
        Room room = (Room)evt.Params["room"];
        Debug.Log(room);
        // Display game list item
        AddGameListItem(room);
    }

    public void OnRoomRemoved(BaseEvent evt)
    {
        Room room = (Room)evt.Params["room"];

        // Get reference to game list item corresponding to Room
        gameListItems.TryGetValue(room.Id, out GameListItem gameListItem);

        // Remove game list item
        if (gameListItem != null)
        {
            // Remove listeners
            gameListItem.playButton.onClick.RemoveAllListeners();
            gameListItem.watchButton.onClick.RemoveAllListeners();

            // Remove game list item from dictionary
            gameListItems.Remove(room.Id);

            // Destroy game object
            GameObject.Destroy(gameListItem.gameObject);
        }
    }

    public void OnUserCountChanged(BaseEvent evt)
    {
        Room room = (Room)evt.Params["room"];

        // Get reference to game list item corresponding to Room
        gameListItems.TryGetValue(room.Id, out GameListItem gameListItem);

        // Update game list item
        if (gameListItem != null)
            gameListItem.SetState(room);
    }

    private void OnRoomJoin(BaseEvent evt)
    {
        Room room = (Room)evt.Params["room"];
        Debug.Log("?ã vào room" + room);

        // If a game Room was joined, go to the Game scene, otherwise ignore this event
        if (room.IsGame)
        {
            // Set user as "Away" in Buddy List system
            if (sfs.BuddyManager.MyOnlineState)
                sfs.Send(new SetBuddyVariablesRequest(new List<BuddyVariable> { new SFSBuddyVariable(ReservedBuddyVariables.BV_STATE, "Away") }));

            // Load game scene
            Manager.Instance.LoadPlayScene();
        }
    }

    private void OnRoomJoinError(BaseEvent evt)
    {
        // Show Warning Panel prefab instance
        warningPanel.Show("Room join failed: " + (string)evt.Params["errorMessage"]);
    }

    private void OnRoomCreationError(BaseEvent evt)
    {
        // Show Warning Panel prefab instance
        warningPanel.Show("Room creation failed: " + (string)evt.Params["errorMessage"]);
    }


}