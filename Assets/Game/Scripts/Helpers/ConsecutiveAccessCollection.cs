/*
 * Author: Shon Verch
 * File Name: ConsecutiveAccessCollection.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 01/16/2018
 * Modified Date: 01/16/2018
 * Description: A generic collection which only allows access to elements in a consecutive order (which loops).
 */

using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A generic collection which only allows access to elements in a consecutive order (which loops).
/// </summary>
public class ConsecutiveAccessCollection<T> : IEnumerable<T>
{
    public int Count => data.Count;

    private int currentAccessIndex;
    private readonly List<T> data;

    public ConsecutiveAccessCollection(IEnumerable<T> data)
    {
        this.data = new List<T>(data);
    }

    public ConsecutiveAccessCollection()
    {
        data = new List<T>();
    }

    public T GetNext() => data[currentAccessIndex++ % Count];

    public void Add(T item) => data.Add(item);
    public bool Remove(T item) => data.Remove(item);
    public int IndexOf(T item) => data.IndexOf(item);
    public void Insert(int index, T item) => data.Insert(index, item);
    public void RemoveAt(int index) => data.RemoveAt(index);

    public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)data).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}