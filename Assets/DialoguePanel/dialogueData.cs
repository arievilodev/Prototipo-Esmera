using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Dialogue{

    public string nameDialogue;
    [TextArea(5, 10)]
    public string textDialogue;

}

[CreateAssetMenu(fileName = "dialogueData", menuName = "ScriptableObject/TalkScript", order = 1)]
public class dialogueData : ScriptableObject
{
    public List<Dialogue> talkScript;
}
