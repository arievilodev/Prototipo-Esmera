/*using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum STATE
{
    DISABLED,
    WAITING,
    TYPING
}
public class dialogueSystem : MonoBehaviour
{
    public dialogueData dialogueData;
    int currentText = 0;
    bool finished = false;
    typeTextAnimation typeText;
    dialogueUI dialogueUI;
    STATE state;

    void Awake()
    {
        typeText = FindFirstObjectByType<typeTextAnimation>();
        dialogueUI = FindFirstObjectByType<dialogueUI>();
        typeText.TypeFinished = OnTypeFinished;
    }


    void Start()
    {
        state = STATE.DISABLED;

    }

    void Update()
    {
        if (state == STATE.DISABLED) return;

        switch (state) {
            case STATE.WAITING:
                Waiting();
                break;
            case STATE.TYPING:
                Typing();
                break;
        }
    }

    public void StartDialogue()
    {
        //if (state != STATE.DISABLED) return; // evita chamar de novo no meio

        currentText = 0;
        finished = false;
        Next();
    }

    public void Next() { 
        dialogueUI.SetName(dialogueData.talkScript[currentText].nameDialogue);
        typeText.fullText = dialogueData.talkScript[currentText++].textDialogue;
        if (currentText == dialogueData.talkScript.Count) finished = true;
        typeText.StartTyping();
        state = STATE.TYPING;
    }

    void OnTypeFinished() { 
        state = STATE.WAITING;
    }
    void Waiting()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!finished)
            {
                Next();
            }
            else
            {
                dialogueUI.Disable();
                state = STATE.DISABLED;
                currentText = 0;
                finished = false;
            }

        }

    }

    void Typing()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            typeText.Skip();
            state = STATE.WAITING;
        }
    }
}*/
