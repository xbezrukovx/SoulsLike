using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Component = System.ComponentModel.Component;

public class TalkScript : MonoBehaviour
{
    private GameObject _talkPlane;
    private GameObject _dialogueBox;
    private CanvasGroup _talkCanvasGroup;
    private CanvasGroup _dialogueCanvasGroup;
    private NPCDialogScript _npcDialogScript;
    private TextMeshProUGUI _textBox;
    private CharacterMovement _characterMovement;
    // Start is called before the first frame update
    void Start()
    {
        _talkPlane = GameObject.Find("DialogPlane");
        _dialogueBox = GameObject.Find("DialogPhrase");
        _textBox = _dialogueBox.GetComponentInChildren<TextMeshProUGUI>(true);
        _talkCanvasGroup = _talkPlane.GetComponent<CanvasGroup>();
        _dialogueCanvasGroup = _dialogueBox.GetComponent<CanvasGroup>();
        _characterMovement = GetComponent<CharacterMovement>();

        _dialogueCanvasGroup.alpha = 0;
        _talkCanvasGroup.alpha = 0;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            _npcDialogScript = (NPCDialogScript) other.GetComponent("NPCDialogScript");
            _talkCanvasGroup.alpha = 1;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            var npcDialogScript = (NPCDialogScript) other.GetComponent("NPCDialogScript");
            if (npcDialogScript.Equals(_npcDialogScript))
            {
                _npcDialogScript = null; 
                _talkCanvasGroup.alpha = 0;
                _dialogueCanvasGroup.alpha = 0;   
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            var message = _npcDialogScript.getNextMessage();
            if (message != null)
            {
                _characterMovement.DisableMovement();
                _talkCanvasGroup.alpha = 0;
                _dialogueCanvasGroup.alpha = 1;
                _textBox.SetText(message);   
            }
            else
            {
                _characterMovement.EnableMovement();
                _talkCanvasGroup.alpha = 1;
                _dialogueCanvasGroup.alpha = 0;
            }
        }
    }
}
