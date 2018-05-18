using UnityEngine;
using System.Collections;
using System;

public class AnimationEventBase : PageEventBase
{

    public Animation anim;
    public AnimationClip clip;
    public float crossFadeTime;
    public WrapMode wrapMode;

    // Use this for initialization
    void Start()
    {
        if(clip != null)
        {
            clip.wrapMode = wrapMode;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void ForceEvent()
    {
        base.ForceEvent();
        ProcessEvent();
    }

    public override void ProcessEvent()
    {
        Debug.Log("starting delayed event");

        
        if (clip == null || anim == null)
            return;

        if(anim.GetClip(clip.name) == null)
        {
            Debug.Log("Animation does not contain this clip");
            return;
        }
        //match speed with event duration
        anim[clip.name].speed = clip.length / duration;

        if (!anim.isPlaying)
            anim.Play(clip.name);
        else
        {
            anim.CrossFade(clip.name, crossFadeTime);
        }

        // if (mirror)
        //     StartCoroutine(MirrorTrigger());
    }

    IEnumerator MirrorTrigger()
    {
        yield return new WaitForSeconds(duration);
        anim[clip.name].speed = -clip.length / duration;
        anim.Play(clip.name);
    }
}
