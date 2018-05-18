using UnityEngine;
using System.Collections;

public class Metronome : MonoBehaviour {

    public float BPM = 30;
    public AudioSource source;
    public float timeStarted;
    public float timeStep;
    public bool isPlaying = false;

	// Use this for initialization
	void Start () {
        timeStep = (float)60 / BPM;
	}
	
	// Update is called once per frame
	void Update () {

        timeStep = (float)60 / BPM;
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (!isPlaying)
                StartMetronome();
            else
                StopMetronome();
        }
	}

    public void StartMetronome()
    {
        timeStarted = Time.time;
        isPlaying = true;
        StartCoroutine("MetroLoop");
    }

    public void StopMetronome()
    {
        source.Stop();
        isPlaying = false;
    }

    IEnumerator MetroLoop()
    {
        while (isPlaying)
        {
            source.PlayOneShot(source.clip);
            yield return new WaitForSeconds(timeStep);
        }
    }
}
