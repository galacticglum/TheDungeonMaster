/*
 * Author: Shon Verch
 * File Name: DoorTrigger.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 01/21/2018
 * Modified Date: 01/21/2018
 * Description: Handles the logic for when the player crosses through a door.
 */

using System.Collections;
using UnityEngine;

/// <summary>
/// Handles the logic for when the player crosses through a door.
/// </summary>
public class DoorTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject doorGameObject;
    [Tooltip("The door trigger that is on the other side of this one.")]
    [SerializeField]
    private DoorTrigger otherDoorTrigger;

    private CameraMovement cameraMovement;
    private PlayerController playerController;
    private RoomController roomController;

    private bool autoMovePlayer;

	private void Start()
    {
        cameraMovement = Camera.main.gameObject.GetComponent<CameraMovement>();
        playerController = ControllerDatabase.Get<PlayerController>();
        roomController = ControllerDatabase.Get<RoomController>();

        roomController.CurrentRoomChanged += (sender, args) => StartCoroutine(nameof(EndAutoMove));
    }

	private void OnTriggerEnter(Collider other)
	{
	    if (doorGameObject.activeInHierarchy) return;
        cameraMovement.SetRotation(transform.rotation.eulerAngles.y);
        playerController.CanMove = false;

	    StartCoroutine(nameof(BeginAutoMove));
	}

    private IEnumerator BeginAutoMove()
    {
        yield return new WaitForSeconds(cameraMovement.RotationDuration);

        autoMovePlayer = true;
        playerController.ThirdPersonController.input = Vector2.up;
        otherDoorTrigger.GetComponent<Collider>().enabled = false;
    }

    private IEnumerator EndAutoMove()
    {
        if (!autoMovePlayer) yield break;
        yield return new WaitForSeconds(0.5f);

        playerController.CanMove = true;
        autoMovePlayer = false;

        playerController.ThirdPersonController.transform.rotation = transform.rotation;
        playerController.ThirdPersonController.input = Vector2.zero;

        GetComponent<Collider>().enabled = false;
        otherDoorTrigger.GetComponent<Collider>().enabled = true;
    }
}
