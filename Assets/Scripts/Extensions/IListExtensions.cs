using System;
using System.Collections.Generic;

public static class IListExtensions {
    /// <summary>
    /// Shuffles the element order of the specified list.
    /// </summary>
    public static void Shuffle<T>(this IList<T> list) {
        List<T> shuffledList = new List<T>();
        while(list.Count != 0) {
            int index = UnityEngine.Random.Range(0, list.Count);
            shuffledList.Add(list[index]);
            list.RemoveAt(index);
        }
        list.Clear();
        foreach(var item in shuffledList) {
            list.Add(item);
        }
    }
}