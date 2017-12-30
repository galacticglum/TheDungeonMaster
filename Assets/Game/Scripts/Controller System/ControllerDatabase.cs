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
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Stores all controllers.
/// </summary>
[ExecuteInEditMode]
public class ControllerDatabase : MonoBehaviour
{
    /// <summary>
    /// All the types which derive from <see cref="ControllerBehaviour"/>.
    /// </summary>
    private static HashSet<Type> controllerBehavioursType;

    /// <summary>
    /// All the controllers stored in this <see cref="ControllerDatabase"/>.
    /// </summary>
    private static Dictionary<Type, ControllerBehaviour> controllers;

    [RuntimeInitializeOnLoadMethod]
    private static void RuntimeInitialize()
    {
        if (FindObjectOfType<ControllerDatabase>() == null)
        {
            // Instantiate an empty gameobject with a ControllerDatabase object on it.
            new GameObject("_CONTROLLER_DATABASE_", typeof(ControllerDatabase));
        }
    }

    [InitializeOnLoadMethod]
    private static void EditorInitialize()
    {
        EditorApplication.update += HandleUpdate;
    }

    /// <summary>
    /// Scans all assemblies in the current <see cref="AppDomain"/> and registers all types that derive from <see cref="ControllerBehaviour"/>..
    /// </summary>
    private static void ScanForControllers()
    {
        // Build our list of types
        if (controllerBehavioursType == null)
        {
            controllerBehavioursType = new HashSet<Type>();
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (!type.IsSubclassOf(typeof(ControllerBehaviour))) continue;
                    controllerBehavioursType.Add(type);
                }
            }
        }

        // Initialize the controllers dictionary if it is NULL.
        if (controllers == null)
        {
            controllers = new Dictionary<Type, ControllerBehaviour>();
        }

        // Go through each ControllerBehaviour type and try to find it in the scene-graph, if it is exists, then we add it to our database.
        foreach (Type type in controllerBehavioursType)
        {
            AddController(type);
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
    /// The <see cref="MonoBehaviour"/> update method.
    /// </summary>
    private void Update() => HandleUpdate();

    /// <summary>
    /// The actual logic for the update method.
    /// </summary>
    private static void HandleUpdate()
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
        List<ControllerBehaviour> behavioursToRemove = GetEnumerable().Where(controllerBehaviour => FindObjectOfType(controllerBehaviour.GetType()) == null).ToList();
        foreach (ControllerBehaviour controllerBehaviour in behavioursToRemove)
        {
            controllers.Remove(controllerBehaviour.GetType());
        }
    }

    /// <summary>
    /// Add a <see cref="ControllerBehaviour"/> to this <see cref="ControllerDatabase"/> if it exists in the scene; that is, there is a gameobject of the type.
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
    public static IEnumerable<ControllerBehaviour> GetEnumerable() => controllers.Values;
}