/*
 * Author: Shon Verch
 * File Name: DoorTrigger.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 01/21/2018
 * Modified Date: 01/21/2018
 * Description: DESCRIPTION
 */

using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    private GameObject cameraObject;

	private void Start()
    {
        cameraObject = Camera.main.gameObject;
	}
	
	private void OnTriggerEnter(Collider other)
    {
        cameraObject.GetComponent<CameraMovement>().SetRotation(transform.rotation.eulerAngles.y);
	}
}
