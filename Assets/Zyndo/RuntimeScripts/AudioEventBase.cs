using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System;

public class AudioEventBase : PageEventBase {

    public AudioMixerSnapshot snapshot;
    public bool snapToBeat;
    public bool reset;
    public int beat;
    public AudioPartGroup partGroup;

   void Start()
    {
        FindAudioPartGroup();

    }

    public override void ForceEvent()
    {
        base.ForceEvent();
        ProcessEvent();
    }

    public override void ProcessEvent()
    {
        if (partGroup != null && snapshot != null)
        {
            if (reset)
            {
                partGroup.StartAll();
            }
            partGroup.PlaySnapshot(snapshot, beat, duration);
        }
    }

    public void FindAudioPartGroup()
    {
        var allAudioGroups = GameObject.Find("Main").GetComponentsInChildren<AudioPartGroup>();
        foreach(var ap in allAudioGroups)
        {
            if(ap.mixer == snapshot.audioMixer)
            {
                partGroup = ap;
                break;
            }
        }
    }
}
