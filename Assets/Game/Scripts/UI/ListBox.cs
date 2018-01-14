/*
 * Author: Shon Verch
 * File Name: ListBox.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 01/14/2018
 * Modified Date: 01/14/2018
 * Description: The control for a scrollable list box.
 */

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The control for a scrollable list box.
/// </summary>
[ExecuteInEditMode]
[RequireComponent(typeof(ScrollRect))]
public class ListBox : MonoBehaviour
{
    [SerializeField]
    private ListBoxItem listboxItemPrefab;
    private ScrollRect scrollRect;

    private void Start()
    {
        scrollRect = GetComponent<ScrollRect>();

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        scrollRect.scrollSensitivity = 25;
#endif
    }

    public void AddItem(string value) => AddItem(value, null);

    public void AddItem(string value, Sprite icon)
    {
        GameObject itemGameObject = Instantiate(listboxItemPrefab.gameObject);
        itemGameObject.GetComponent<ListBoxItem>().Initialize(value, icon);
        itemGameObject.transform.SetParent(scrollRect.content, false);
    }

    public void Clear()
    {
        for (int i = scrollRect.content.childCount - 1; i >= 0; i--)
        {
            Destroy(scrollRect.content.GetChild(i).gameObject);
        }
    }
}