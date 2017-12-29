/*
 * Author: Shon Verch
 * File Name: CameraController.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 12/28/2017
 * Modified Date: 12/28/2017
 * Description: Manages all camera movement.
 */

using UnityEngine;

/// <summary>
/// Manages all camera movement.
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
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
        MasterDataController.Current.RoomController.CurrentRoomChanged += OnCurrentRoomChanged;
    }

    /// <summary>
    /// Called every frame.
    /// </summary>
    private void Update()
    {
        if (cameraTransitionLerpInformation == null) return;
        transform.position = cameraTransitionLerpInformation.Step(Time.deltaTime);
    }

    private void OnCurrentRoomChanged(object sender, CurrentRoomChangedEventArgs args)
    {
        Vector3 destination = args.NewRoom.Centre + CameraOffset;
        cameraTransitionLerpInformation = new LerpInformation<Vector3>(transform.position, destination, transitionDuration, GradualCurve.Interpolate);
        cameraTransitionLerpInformation.Finished += (o, eventArgs) => cameraTransitionLerpInformation = null;
    }
}
