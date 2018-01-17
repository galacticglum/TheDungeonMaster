/*
 * Author: Shon Verch
 * File Name: ConsecutiveAccessCollection.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 01/16/2018
 * Modified Date: 01/16/2018
 * Description: A generic collection which only allows access to elements in a consecutive order (which loops).
 */

using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A generic collection which only allows access to elements in a consecutive order (which loops).
/// </summary>
public class ConsecutiveAccessCollection<T> : IEnumerable<T>
{
    /// <summary>
    /// Gets the number of elements contained in the <see cref="ConsecutiveAccessCollection{T}"/>.
    /// </summary>
    public int Count => data.Count;

    /// <summary>
    /// The index of the element last accessed.
    /// </summary>
    private int accessIndex;

    /// <summary>
    /// The backing data buffer.
    /// </summary>
    private readonly List<T> data;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConsecutiveAccessCollection{T}"/> class from a pre-existing collection of data.
    /// </summary>
    /// <param name="data"></param>
    public ConsecutiveAccessCollection(IEnumerable<T> data)
    {
        this.data = new List<T>(data);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConsecutiveAccessCollection{T}"/> class that is empty.
    /// </summary>
    public ConsecutiveAccessCollection()
    {
        data = new List<T>();
    }

    /// <summary>
    /// Get the next element in the collection.
    /// This loops back to the first element once it has reached the last.
    /// </summary>
    public T GetNext() => data[accessIndex++ % Count];

    /// <summary>
    /// Adds an object to the end of the <see cref="ConsecutiveAccessCollection{T}"/>.
    /// </summary>
    /// <param name="item">
    /// The object to be added to the end of the <see cref="ConsecutiveAccessCollection{T}"/>.
    /// The value can be null for reference types.
    /// </param>
    public void Add(T item) => data.Add(item);

    /// <summary>
    /// Removes all elements from the <see cref="ConsecutiveAccessCollection{T}"/>.
    /// </summary>
    public void Clear()
    {
        data.Clear();
        accessIndex = 0;
    }

    /// <summary>
    /// Determines whether an element is in the <see cref="ConsecutiveAccessCollection{T}"/>.
    /// </summary>
    /// <param name="item">
    /// The object to locate in the <see cref="ConsecutiveAccessCollection{T}"/>. 
    /// The value can be null for reference types.
    /// </param>
    public bool Contains(T item) => data.Contains(item);

    /// <summary>
    /// Copies the entire <see cref="FixedList{T}"/> to a compatible one-dimensional array, starting at the specified index of the target array.
    /// </summary>
    /// <param name="array">The one-dimensional <see cref="Array"/> that is the destination of the elements from <see cref="ConsecutiveAccessCollection{T}"/>. 
    /// The <see cref="Array"/> must have zero-based indexing.</param>
    /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> which copying begins.</param>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="ArgumentOutOfRangeException"/>
    /// <exception cref="ArgumentException"/>
    public void CopyTo(T[] array, int arrayIndex) => data.CopyTo(array, arrayIndex);

    /// <summary>
    /// Removes the first occurence of a specific object from the <see cref="ConsecutiveAccessCollection{T}"/>.
    /// </summary>
    /// <param name="item">
    /// The object to remove from the <see cref="ConsecutiveAccessCollection{T}"/>. 
    /// The value can be null for reference types.
    /// </param>
    public bool Remove(T item)
    {
        accessIndex--;
        return data.Remove(item);
    }

    /// <summary>
    /// Searches for the specified object and returns the zero-based index of the first occurence within the entire <see cref="ConsecutiveAccessCollection{T}"/>.
    /// </summary>
    /// <param name="item">
    /// The object to locate in the <see cref="ConsecutiveAccessCollection{T}"/>. 
    /// The value can be null for reference types.
    /// </param>
    public int IndexOf(T item) => data.IndexOf(item);

    /// <summary>
    /// Inserts an element into the <see cref="ConsecutiveAccessCollection{T}"/> at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
    /// <param name="item">
    /// The object to insert. 
    /// The value can be null for reference types.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException"/>
    public void Insert(int index, T item) => data.Insert(index, item);

    /// <summary>
    /// Removes the element at the specified index of the <see cref="ConsecutiveAccessCollection{T}"/>.
    /// </summary>
    /// <param name="index">The zero-based index of the element to remove.</param>
    /// <exception cref="ArgumentOutOfRangeException"/>
    public void RemoveAt(int index)
    {
        data.RemoveAt(index);
        accessIndex--;
    }

    /// <summary>
    /// Returns an enumerator that iterates through the <see cref="ConsecutiveAccessCollection{T}"/>.
    /// </summary>
    public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)data).GetEnumerator();

    /// <summary>
    /// Returns an enumerator that iterates through the <see cref="ConsecutiveAccessCollection{T}"/>.
    /// </summary>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}