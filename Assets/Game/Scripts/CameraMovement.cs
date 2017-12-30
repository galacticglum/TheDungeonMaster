/*
 * Author: Shon Verch
 * File Name: CameraMovement.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 12/28/2017
 * Modified Date: 12/29/2017
 * Description: Manages all camera movement.
 */

using UnityEngine;

/// <summary>
/// Manages all camera movement.
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private float transitionDuration = 1f;
    [SerializeField]
    private float cameraFollowHeight = 9f;
    [SerializeField]
    private float cameraZOffset = 6.2f;

    private Vector3 CameraOffset => new Vector3(0, cameraFollowHeight, cameraZOffset);
    private LerpInformation<Vector3> cameraTransitionLerpInformation;

    /// <summary>
    /// Called when the component is created and placed into the world.
    /// </summary>
    private void Start()
    {
        transform.position = CameraOffset;
        ControllerDatabase.Get<RoomController>().CurrentRoomChanged += OnCurrentRoomChanged;
    }

    /// <summary>
    /// Called every frame.
    /// </summary>
    private void Update()
    {
        if (cameraTransitionLerpInformation == null) return;
        transform.position = cameraTransitionLerpInformation.Step(Time.deltaTime);
    }

    /// <summary>
    /// Event which is called when the current room (that the player is in) has changed.
    /// </summary>
    /// <param name="sender">The object which dispatched this event.</param>
    /// <param name="args">The arguments pertaining to the event.</param>
    private void OnCurrentRoomChanged(object sender, CurrentRoomChangedEventArgs args)
    {
        Vector3 destination = args.NewRoom.Centre + CameraOffset;
        cameraTransitionLerpInformation = new LerpInformation<Vector3>(transform.position, destination, transitionDuration, GradualCurve.Interpolate);
        cameraTransitionLerpInformation.Finished += (o, eventArgs) => cameraTransitionLerpInformation = null;
    }
}
