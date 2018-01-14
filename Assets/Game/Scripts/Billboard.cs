/*
 * Author: Shon Verch
 * File Name: Billboard.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 01/14/2018
 * Modified Date: 01/14/2018
 * Description: Makes an object always face the camera.
 */

using UnityEngine;

/// <summary>
/// Makes an object always face the camera.
/// </summary>
public class Billboard : MonoBehaviour
{
    /// <summary>
    /// Called every frame.
    /// </summary>
    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
    }
}
