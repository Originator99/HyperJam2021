using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialData {
    public string header;
    public string content;
    public List<Transform> highLightPanels;
    public System.Action callback;
}


public struct TutorialSignal {
    public List<TutorialData> tutorials;
}