/*
 * Author: Shon Verch
 * File Name: EditorGUIHelper.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 12/28/2017
 * Modified Date: 12/28/2017
 * Description: Extension functionality related to the Editor GUI.
 */

using System;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Extension functionality related to the Editor GUI.
/// </summary>
public static class EditorGUIHelper
{
    /// <summary>
    /// The uid for any control which is focused out.
    /// </summary>
    private const string FocusOutUid = "__FOCUS_OUT_CONTROL__";

    /// <summary>
    /// The <see cref="GUIStyle"/> for a splitter.
    /// </summary>
    private static readonly GUIStyle splitter;
    
    /// <summary>
    /// The <see cref="Color"/> for a splitter.
    /// </summary>
    /// Handles colour for both dark and light skins.
    private static readonly Color splitterColor = EditorGUIUtility.isProSkin ? new Color(0.157f, 0.157f, 0.157f) : new Color(0.5f, 0.5f, 0.5f);

    /// <summary>
    /// The <see cref="Color"/> for the upper-portion of a section box.
    /// </summary>
    /// Handles colour for both dark and light skins.
    private static readonly Color upperSectionBoxColour = EditorGUIUtility.isProSkin ? new Color(0.278f, 0.278f, 0.278f) : new Color(0.855f, 0.855f, 0.855f);

    /// <summary>
    /// The <see cref="Color"/> for the lower-portion of a section box.
    /// </summary>
    /// Handles colour for both dark and light skins.
    private static readonly Color lowerSectionBoxColour = EditorGUIUtility.isProSkin ? new Color(0.235f, 0.235f, 0.235f) : new Color(0.753f, 0.753f, 0.753f);

    /// <summary>
    /// The <see cref="Color"/> for when the section box is not selected.
    /// </summary>
    /// Handles colour for both dark and light skins.
    private static readonly Color sectionBoxInactiveColour = EditorGUIUtility.isProSkin ? new Color(0.22f, 0.22f, 0.22f) : new Color(0.816f, 0.816f, 0.816f);

    /// <summary>
    /// The <see cref="GUIStyle"/> of the upper-portion of a section box.
    /// </summary>
    private static readonly GUIStyle upperSectionBoxStyle;

    /// <summary>
    /// The <see cref="GUIStyle"/> of the lower-portion of a section box.
    /// </summary>
    private static readonly GUIStyle lowerSectionBoxStyle;

    /// <summary>
    /// Initialize the <see cref="EditorGUIHelper"/>.
    /// </summary>
    static EditorGUIHelper()
    {
        splitter = new GUIStyle
        {
            normal = { background = EditorGUIUtility.whiteTexture },
            stretchWidth = true,
            margin = new RectOffset(0, 0, 7, 7)
        };

        upperSectionBoxStyle = new GUIStyle
        {
            padding = new RectOffset(0, 0, 3, 0),
            margin = new RectOffset(0, 0, 4, 0),
            border = new RectOffset(4, 4, 4, 4),
            normal = new GUIStyleState
            {
                background = Resources.Load<Texture2D>("Images/GUI/upper_section_box_background")
            }
        };

        lowerSectionBoxStyle = new GUIStyle
        {
            padding = new RectOffset(0, 0, 0, 3),
            margin = new RectOffset(0, 0, 0, 4),
            border = new RectOffset(4, 4, 4, 4),
            normal = new GUIStyleState
            {
                background = Resources.Load<Texture2D>("Images/GUI/lower_section_box_background")
            }
        };

    }

    /// <summary>
    /// Draws a splitter.
    /// </summary>
    /// <param name="rgb">The <see cref="Color"/> of the splitter.</param>
    /// <param name="thickness">The thickness of the splitter.</param>
    public static void Splitter(Color rgb, float thickness = 1)
    {
        Rect position = GUILayoutUtility.GetRect(GUIContent.none, splitter, GUILayout.Height(thickness));

        if (Event.current.type != EventType.Repaint) return;

        Color restoreColor = GUI.color;
        GUI.color = rgb;
        splitter.Draw(position, false, false, false, false);
        GUI.color = restoreColor;
    }

    /// <summary>
    /// Draws a splitter.
    /// </summary>
    /// <param name="thickness">The thickness of the splitter.</param>
    /// <param name="splitterStyle">The <see cref="GUIStyle"/> to draw the splitter with.</param>
    public static void Splitter(float thickness, GUIStyle splitterStyle)
    {
        Rect position = GUILayoutUtility.GetRect(GUIContent.none, splitterStyle, GUILayout.Height(thickness));

        if (Event.current.type != EventType.Repaint) return;

        Color restoreColor = GUI.color;
        GUI.color = splitterColor;
        splitterStyle.Draw(position, false, false, false, false);
        GUI.color = restoreColor;
    }

    /// <summary>
    /// Draws a splitter.
    /// </summary>
    /// <param name="thickness">The thickness of the splitter.</param>
    /// <param name="margin">The margins of the splitter.</param>
    /// <param name="padding">The edge padding on the splitter.</param>
    public static void Splitter(float thickness, RectOffset margin, RectOffset padding)
    {
        GUIStyle style = new GUIStyle(splitter);
        splitter.margin = margin;
        splitter.padding = padding;

        Rect position = GUILayoutUtility.GetRect(GUIContent.none, style, GUILayout.Height(thickness));

        if (Event.current.type != EventType.Repaint) return;

        Color restoreColor = GUI.color;
        GUI.color = splitterColor;
        style.Draw(position, false, false, false, false);
        GUI.color = restoreColor;
    }

    /// <summary>
    /// Draws a splitter.
    /// </summary>
    /// <param name="thickness">The thickness of the splitter.</param>
    public static void Splitter(float thickness = 1) => Splitter(thickness, splitter);

    /// <summary>
    /// Draws a splitter.
    /// </summary>
    /// <param name="position">The position at which the splitter will be drawn at.</param>
    public static void Splitter(Rect position)
    {
        if (Event.current.type != EventType.Repaint) return;
        Color restoreColor = GUI.color;
        GUI.color = splitterColor;
        splitter.Draw(position, false, false, false, false);
        GUI.color = restoreColor;
    }

    /// <summary>
    /// Remove focus from the currently selected control.
    /// </summary>
    public static void RemoveFocus()
    {
        GUI.SetNextControlName(FocusOutUid);
        GUI.TextField(new Rect(-100, -100, 1, 1), "");
        GUI.FocusControl(FocusOutUid);
    }

    /// <summary>
    /// Draws a toolbar from a set of options.
    /// </summary>
    /// <param name="selected">The currently selected toolbar option.</param>
    /// <param name="options">The set of toolbar options.</param>
    /// <returns>The selected option.</returns>
    public static int Toolbar(int selected, params string[] options)
    {
        EditorGUILayout.BeginHorizontal();

        for (int i = 0; i < options.Length; i++)
        {
            if (GUILayout.Toggle(i == selected, options[i], EditorStyles.toolbarButton) != (i == selected))
            {
                selected = i;
            }
        }

        EditorGUILayout.EndHorizontal();

        return selected;
    }

    /// <summary>
    /// Draws a header in the inspector.
    /// </summary>
    /// <param name="content">The content of the header.</param>
    private static void DrawHeader(string content) => DrawHeader(new GUIContent(content));

    /// <summary>
    /// Draws a header in the inspector.
    /// </summary>
    /// <param name="content">The content of the header.</param>
    private static void DrawHeader(GUIContent content)
    {
        EditorGUILayout.Separator();
        EditorGUILayout.LabelField(content, EditorStyles.boldLabel);
    }

    /// <summary>
    /// Draw a section box with a header and content.
    /// </summary>
    /// <param name="headerContent">The <see cref="GUIContent"/> of the section header.</param>
    /// <param name="show">Indicates whether the section should be open or not.</param>
    /// <param name="drawContent">The <see cref="Action"/> for displaying the content of the section.</param>
    /// <returns>A boolean indicating whether the section is open (true) or closed (false).</returns>
    public static bool DrawSectionBox(GUIContent headerContent, bool show, Action drawContent = null)
    {
        Color previousColour = GUI.backgroundColor;
        upperSectionBoxStyle.margin.bottom = show ? 0 : 4;

        // Draw the section header
        GUI.backgroundColor = show ? upperSectionBoxColour : sectionBoxInactiveColour;
        GUILayout.BeginVertical(upperSectionBoxStyle);

        int foldoutButtonWidth = GUI.skin.FindStyle("ProfilerTimelineFoldout").normal.background.width;
        GUIStyle horizontalStyle = new GUIStyle
        {
            padding = new RectOffset(foldoutButtonWidth + 4, 0, 0, 0)
        };

        GUI.backgroundColor = lowerSectionBoxColour;

        GUILayout.BeginHorizontal(horizontalStyle);
        Rect foldoutRect = GUILayoutUtility.GetRect(headerContent, EditorStyles.foldout);
        if (GUI.Button(foldoutRect, GUIContent.none, GUIStyle.none))
        {
            show = !show;
        }

        show = EditorGUI.Foldout(foldoutRect, show, headerContent);
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();

        // Don't draw our content if we have no content drawing action.
        if (drawContent != null && show)
        {
            // Draw the section content
            GUILayout.BeginVertical(lowerSectionBoxStyle);

            // Revert background colour back to normal before we draw the section box content.
            GUI.backgroundColor = previousColour;
            drawContent();

            GUILayout.EndVertical();
        }

        // Revert background colour in the event that our lower section box was not showed.
        GUI.backgroundColor = previousColour;

        return show;
    }
}