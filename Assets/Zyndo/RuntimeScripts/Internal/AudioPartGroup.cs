using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
using System;

public enum AudioType
{
    Voice,
    FX,
    Music
}
public class AudioPartGroup : MonoBehaviour
{
    public AudioType type;
    public AudioMixer mixer;
    public float bpm = 120.0f;
    public int beatsPerBar = 4;  
    public GameObject audioGroupRoot;

    public float timeStep;
    private AudioSource[] sources;
    private float startTime;
    private bool isGroupPlaying = false;
    void Start()
    {
        timeStep = (float)(60 / (bpm));
        if (audioGroupRoot != null)
        {
            sources = audioGroupRoot.GetComponentsInChildren<AudioSource>();
            sources = Array.FindAll(sources, x => x.outputAudioMixerGroup != null);
        }

        if (sources == null)
        {
            Debug.LogError("No audio sources detected for this AudioGroup", this);
        }

        StartAll();
    }

    void Update()
    {

    }

    public void StartAll()
    {
        if (sources == null)
            return;
        //start all the loops
        foreach (var s in sources)
        {
            s.Play();
            s.loop = true;
        }
        startTime = Time.timeSinceLevelLoad;
        isGroupPlaying = true;
    }

    public void StopAll()
    {
        if (sources == null)
            return;
        foreach (var s in sources)
        {
            s.Stop();
            isGroupPlaying = false;
        }
    }
    public void PlaySnapshot(AudioMixerSnapshot snapshot, float transitionTime)
    {
        snapshot.TransitionTo(transitionTime);
    }

    public void PlaySnapshot(AudioMixerSnapshot snapshot, int targetBeat, float transitionTime)
    {
        StartCoroutine(PlayOnBeat(snapshot, targetBeat, transitionTime));
    }

    IEnumerator PlayOnBeat(AudioMixerSnapshot snapshot, int targetBeat, float transitionTime)
    {
        var deltaTime = (Time.timeSinceLevelLoad - startTime);
        var beatsSoFar = Mathf.Floor(deltaTime / timeStep);
        var beatsInBar = beatsSoFar % beatsPerBar;
        var barTime = beatsPerBar * timeStep;

        var nextDownTime = (Mathf.Floor(deltaTime / barTime) + 1) * barTime;
        var waitTime = nextDownTime - deltaTime;

        if (deltaTime < 0.001)
            waitTime = 0.0f;

        yield return new WaitForSeconds(waitTime);
        //start transition
        snapshot.TransitionTo(transitionTime);
        yield return 0;
    }
}
