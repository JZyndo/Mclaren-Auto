using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class PlaySound : PageEventBase
{
    public AudioSource audioSource;
    public AudioClip clip;
    public bool loop;
    public bool useVolumeCurve;
	public bool autoFadeOut;
    private float startTime;

    // Use this for initialization
	public override void OnEnable ()
	{
		base.OnEnable ();
		PageEventsManager.PageDeparture += AutoFadeOut;
	}

	void OnDisable()
	{
		PageEventsManager.PageDeparture -= AutoFadeOut;
	}

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = clip;
    }

    public override void ForceEvent()
    {
        base.ForceEvent();
        ProcessEvent();
    }
    public override void ProcessEvent()
    {
        if (clip != null && audioSource != null)
        {
            if (!audioSource.isPlaying)
            {
				if (!loop) {
					audioSource.PlayOneShot (clip);
					startTime = Time.timeSinceLevelLoad;
				}
                else
                {
                    Debug.Log("playing audio");
                    audioSource.loop = true;
                    audioSource.clip = clip;
                    audioSource.Play();
                    startTime = Time.timeSinceLevelLoad;
                }
            }
        }
    }

	void AutoFadeOut(object sender, EventArgs e)
	{
		if (page != PageEventsManager.currentPage)
			return;

		StartCoroutine (FadeVolume ());

	}

	IEnumerator FadeVolume()
	{
		float steps = Mathf.Ceil(1.0f / Time.deltaTime);
		steps = Mathf.Max (steps, 1);
		float orgVol = audioSource.volume;
		for (int i = 0; i <= steps; i++) {

			audioSource.volume = Mathf.Lerp (orgVol, 0.0f, i / steps);
			yield return 0;
		}

		audioSource.Stop ();
        audioSource.volume = orgVol;

		yield return 0;
	}

    void Update()
    {

        if (clip != null && audioSource != null)
        {
            if (Time.timeSinceLevelLoad - startTime >= duration + delay && audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            if (useVolumeCurve && audioSource.isPlaying)
            {
                var t = (Time.timeSinceLevelLoad - startTime) / duration;
                audioSource.volume = curve.Evaluate(t);
            }
        }
    }
}
