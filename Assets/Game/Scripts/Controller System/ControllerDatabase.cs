/*
 * Author: Shon Verch
 * File Name: ControllerDatabase.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 12/29/2017
 * Modified Date: 12/29/2017
 * Description: Stores all controllers.
 */

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// Stores all controllers.
/// </summary>
[ExecuteInEditMode]
public class ControllerDatabase : MonoBehaviour
{
    /// <summary>
    /// All the controllers stored in this <see cref="ControllerDatabase"/>.
    /// </summary>
    private static Dictionary<Type, ControllerBehaviour> controllers;

    /// <summary>
    /// Scans all assemblies in the current <see cref="AppDomain"/> and adds all controllers with the <see cref="RegisterControllerAttribute"/> into the database.
    /// </summary>
    private static void ScanForControllers()
    {
        // If we have already instantiated our dictionary then we only need to clear it before scanning.
        // OTHERWISE: we need to instantiate our dictionary.
        if (controllers != null)
        {
            controllers.Clear();
        }
        else
        {
            controllers = new Dictionary<Type, ControllerBehaviour>();
        }

        // Scan each type in all assemblies in our current app domain to find controller behaviours.
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (!type.IsSubclassOf(typeof(ControllerBehaviour))) continue;
                AddController(type);
            }
        }
    }

    /// <summary>
    /// Called before Start.
    /// </summary>
    private void Awake()
    {
        ScanForControllers();
    }

    /// <summary>
    /// Called every frame.
    /// </summary>
    private void Update()
    {
        // If our controllers dictionary is uninitialized, let's initialize it by scanning through the scene for controllers.
        if (controllers == null)
        {
            ScanForControllers();
        }

        // If our controllers dictionary is STILL NULL at this point, or we're NOT in the editor, let's bail from this function as we have nothing else to do!
        if (Application.isPlaying || controllers == null) return;

        // Update our controllers dictionary by rescanning for controller behaviours.
        ScanForControllers();

        // Make sure that all controller behaviours in the database are associated with a gameobject.
        foreach (ControllerBehaviour controllerBehaviour in GetEnumerable())
        {
            if (FindObjectOfType(controllerBehaviour.GetType()) == null)
            {
                controllers.Remove(controllerBehaviour.GetType());
            }
        }
    }

    /// <summary>
    /// Add a <see cref="ControllerBehaviour"/> to this <see cref="ControllerDatabase"/>.
    /// </summary>
    /// <param name="type"></param>
    private static void AddController(Type type)
    {
        // If the type is NOT derived from ControllerBehaviour or it already exists in the database then bail!
        if (!type.IsSubclassOf(typeof(ControllerBehaviour)) || controllers.ContainsKey(type)) return;

        // Find our controller in the scene graph.
        ControllerBehaviour controller = (ControllerBehaviour)FindObjectOfType(type);
        if (controller == null) return;

        controllers.Add(type, controller);
    }

    /// <summary>
    /// Retrieve a <see cref="ControllerBehaviour"/> from type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="ControllerBehaviour"/>.</typeparam>
    public static T Get<T>() where T : ControllerBehaviour
    {
        // If our controllers dictionary is uninitialized or the type doesn't exist in the database, we bail and return null!
        if (controllers == null || !controllers.ContainsKey(typeof(T)))
        {
            return default(T);
        }

        // Retrieve the controller from the dictionary.
        return (T)controllers[typeof(T)];
    }

    /// <summary>
    /// Checks whether a <see cref="ControllerBehaviour"/> of <paramref name="type"/> exists in the database.
    /// If the <see cref="ControllerBehaviour"/> does not exist then it is added into the database but the returning value is <value>false</value>.
    /// </summary>
    /// <param name="type">The type of the <see cref="ControllerBehaviour"/>.</param>
    /// <returns>A boolean which indicates whether the <see cref="ControllerBehaviour"/> exists in the database.</returns>
    public static bool Exists(Type type)
    {
        // If the specfiied type does NOT derive from ControllerBehaviour, it can't exist in the database; return FALSE.
        if (!type.IsSubclassOf(typeof(ControllerBehaviour))) return false;

        // If the controller exists, we return TRUE.
        if (controllers.ContainsKey(type)) return true;
        
        // Our last resort: it does not exist in the database, let's add it to the database and return FALSE.
        AddController(type);
        return false;
    }

    /// <summary>
    /// Retrieve the <see cref="IEnumerable{T}"/> for this <see cref="ControllerDatabase"/> which iterates over <see cref="ControllerBehaviour"/>.
    /// </summary>
    public IEnumerable<ControllerBehaviour> GetEnumerable() => controllers.Values;
}