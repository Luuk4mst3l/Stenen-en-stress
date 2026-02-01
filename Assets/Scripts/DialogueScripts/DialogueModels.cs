using System;
using UnityEngine;

[Serializable]
public class LevelDialogueData
{
    public int level;
    public float popupDelaySeconds;
    public DialogueEvent[] events;
}

[Serializable]
public class DialogueEvent
{
    public string id;
    public string titel;
    public int partij;
    public string description;
    public DialogueOption[] options;
}

[Serializable]
public class DialogueOption
{
    public string id;
    public string description;
    public int budget;

    public string partij1;
    public int waarde1;

    public string partij2;
    public int waarde2;
}
