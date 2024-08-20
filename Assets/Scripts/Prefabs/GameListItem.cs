using System;
using UnityEngine;
using UnityEngine.UI;
using Sfs2X.Entities;
using TMPro;

/**
 * Script attached to Game List Item prefab.
 */
public class GameListItem : MonoBehaviour
{
	public Button playButton;
	public Button watchButton;
	public TextMeshProUGUI nameText;
	public TextMeshProUGUI detailsText;
	public GameObject background;
	public Sprite[] images;
	public int roomId;

	/**
	 * Initialize the prefab instance.
	 */
	public void Init(Room room)
	{
		background.GetComponent<Image>().sprite = images[UnityEngine.Random.Range(0,images.Length)];
		nameText.text = "Room" + " " + room.Name;
		roomId = room.Id;

		SetState(room);
	}

	/**
	 * Update prefab instance based on the corresponding Room state.
	 */
	public void SetState(Room room)
	{
		int playerSlots =/* room.MaxUsers -*/ room.UserCount;
		int spectatorSlots = room.MaxSpectators - room.SpectatorCount;

		// Set player count and spectator count in game list item
		detailsText.text = playerSlots.ToString();

		// Enable/disable game play button
		playButton.interactable = playerSlots >= 0;

		// Enable/disable game watch button
		watchButton.interactable = spectatorSlots > 0;
	}
}
