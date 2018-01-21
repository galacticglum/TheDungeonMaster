using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    private GameObject CameraObject;
	void Start()
    {
        CameraObject = Camera.main.gameObject;
	}
	
	void OnTriggerEnter(Collider other)
    {
        CameraObject.GetComponent<CameraMovement>().SetRotation(transform.rotation.eulerAngles.y);
	}
}
