using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogScript : MonoBehaviour
{
    public string[] replicas;
    private int currentReplica = 0;
    public AudioSource audio;
    public AudioClip[] clips;

    public string getNextMessage()
    {
        if (currentReplica >= replicas.Length)
        {
            currentReplica = 0;
            return null;
        }
        if (audio.isPlaying) audio.Stop();
        audio.clip = clips[currentReplica];
        audio.Play();
        return replicas[currentReplica++];
    }
    
}
