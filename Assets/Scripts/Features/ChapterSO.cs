using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChapterData", menuName = "Features/New Chapter", order = 1)]
public class ChapterSO : ScriptableObject {
    public int id;
    public string chapter_name;
    public List<Chapter> levels;
}

[System.Serializable]
public class Chapter {
    public int chapter_number;
    public GameObject level_prefab;
    public bool isComplete;
}
