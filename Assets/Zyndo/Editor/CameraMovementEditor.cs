using UnityEngine;
using UnityEngine.Audio;
using UnityEditor;
using UnityEditor.Audio;
using UnityEditor.Animations;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class CameraMovementEditor : MonoBehaviour
{
    public static void RetargetAnimation(AnimationClip source_anim)
    {
        //var pivot = GameObject.Find("Pivot");
        //if (pivot == null)
        //{
        //    Debug.Log("Could not add camera animation: no pivot object was found.");
        //    return;
        //}
        //var anim = pivot.GetComponent<Animation>();
        //if (anim == null)
        //{
        //    Debug.Log("Could not add camera animation: no Animation component was found on Pivot.");
        //    return;
        //}

        //if (source_anim == null)
        //{
        //    Debug.Log("Could not retarget animation: please select a source animation");
        //    return;
        //}

        ////get all clips in the animator
        //var clips = AnimationUtility.GetAnimationClips(pivot);
        //var s_curves = AnimationUtility.GetAllCurves(source_anim, true);

        //var sel_clips = new List<AnimationClip>();

        //foreach (var o in Selection.gameObjects)
        //{
        //    foreach (var c in clips)
        //    {
        //        if (c.name.Contains(o.name))
        //        {
        //            sel_clips.Add(c);
        //            break;
        //        }
        //    }

        //}

        ////if nothing selected, modify all clips
        //if (sel_clips.Count == 0)
        //    sel_clips = clips.ToList();

        //foreach (var c in sel_clips)
        //{
        //    if (c == source_anim)
        //        continue;
        //    Debug.Log("retargeting clip: " + c.name);
        //    var t_curves = AnimationUtility.GetAllCurves(c, true);
        //    //anim.RemoveClip(c);
        //    var new_c = new AnimationClip();
        //    new_c.name = c.name;
        //    new_c.legacy = true;
        //    foreach (var s_c in s_curves)
        //    {
        //        //don't retarget if only end key frame are available
        //        if (s_c.curve.keys.Length <= 2)
        //            continue;

        //        Debug.Log("referencing curve: " + s_c.propertyName);

        //        var o_key = s_c.curve.keys[0];
        //        var t_curve = t_curves.FirstOrDefault(x => x.propertyName == s_c.propertyName);
        //        if (t_curve != null)
        //        {
        //            Debug.Log("found corresponding curve: " + t_curve.propertyName);
        //            //get first key frame for diff'ing
        //            var o_t_key = t_curve.curve.keys[0];
        //            var num_keys = t_curve.curve.keys.Length;
        //            var new_curve = new AnimationCurve();
        //            //copy first and last keyframes directly from target
        //            new_curve.AddKey(o_t_key);
        //            new_curve.AddKey(t_curve.curve.keys[num_keys - 1]);

        //            //add the retargeted intermediate keys from source
        //            for (int i = 1; i < s_c.curve.keys.Length - 1; i++)
        //            {
        //                var s_k = s_c.curve.keys[i];
        //                Debug.Log("processing key at: " + s_k.time);
        //                var diff = s_k.value - o_key.value;
        //                var t_val = o_t_key.value + diff;
        //                new_curve.AddKey(s_k.time, t_val);
        //            }
        //            new_c.SetCurve("", s_c.type, s_c.propertyName, new_curve);
        //            Debug.Log("target curve now has keyframes: " + t_curve.curve.keys.Length);
        //        }
        //    }

        //    //replace the target animation 
        //    anim.RemoveClip(c);
        //    var path = "Assets/Resources/Animations/" + c.name + ".anim";
        //    AssetDatabase.CreateAsset(new_c, path);
        //    anim.AddClip(new_c, new_c.name);
        //}

        //AssetDatabase.Refresh();
    }

    public static void SetAnimationLength(float length)
    {
        //var pivot = GameObject.Find("Pivot");
        //if (pivot == null)
        //{
        //    Debug.Log("Could not add camera animation: no pivot object was found.");
        //    return;
        //}
        //var anim = pivot.GetComponent<Animation>();
        //if (anim == null)
        //{
        //    Debug.Log("Could not add camera animation: no Animation component was found on Pivot.");
        //    return;
        //}


        ////if (source_anim == null)
        ////{
        ////    Debug.Log("Could not retarget animation: please select a source animation");
        ////    return;
        ////}

        ////get all clips in the animator
        //var clips = AnimationUtility.GetAnimationClips(pivot);
        ////var s_curves = AnimationUtility.GetAllCurves(source_anim, true);
        //var sel_clips = new List<AnimationClip>();

        //foreach (var o in Selection.gameObjects)
        //{
        //    foreach (var c in clips)
        //    {
        //        if (c.name.Contains(o.name))
        //        {
        //            sel_clips.Add(c);
        //            break;
        //        }
        //    }

        //}

        ////if nothing selected, modify all clips
        //if (sel_clips.Count == 0)
        //    sel_clips = clips.ToList();


        //foreach (var c in sel_clips)
        //{
        //    Debug.Log("retargeting clip: " + c.name);
        //    var t_curves = AnimationUtility.GetAllCurves(c, true);
        //    //anim.RemoveClip(c);
        //    var new_c = new AnimationClip();
        //    new_c.name = c.name;
        //    new_c.legacy = true;
        //    foreach (var t_curve in t_curves)
        //    {

        //        Debug.Log("referencing curve: " + t_curve.propertyName);
        //        var new_curve = new AnimationCurve();

        //        //add the retargeted intermediate keys from source
        //        for (int i = 0; i < t_curve.curve.keys.Length; i++)
        //        {
        //            var s_k = t_curve.curve.keys[i];
        //            var new_t = (s_k.time / c.length) * length;
        //            new_curve.AddKey(new_t, s_k.value);
        //        }
        //        new_c.SetCurve("", t_curve.type, t_curve.propertyName, new_curve);
        //        Debug.Log("target curve now has keyframes: " + t_curve.curve.keys.Length);
        //    }


        //    //replace the target animation 
        //    anim.RemoveClip(c);
        //    var path = "Assets/Resources/Animations/" + c.name + ".anim";
        //    AssetDatabase.CreateAsset(new_c, path);
        //    anim.AddClip(new_c, new_c.name);
        //}
        //AssetDatabase.Refresh();
    }


    public static void AddCameraAnimation(GameObject obj)
    {

        //var pivot = GameObject.Find("Pivot");
        //if (pivot == null)
        //{
        //    Debug.Log("Could not create camera animations: no pivot object was found.");
        //    return;
        //}
        //var anim = GameObject.Find("Main").GetComponent<Animator>();
        //if (anim == null)
        //{
        //    Debug.Log("Could not create camera animations: no Animation component was found on Main.");
        //    return;
        //}

        //var dir_path = "Assets/Resources/Animations/CameraGlobal";
        //if (!AssetDatabase.IsValidFolder(dir_path))
        //    AssetDatabase.CreateFolder("Assets/Resources/Animations", "CameraGlobal");

        //string assetPath = AssetDatabase.GetAssetPath(anim.runtimeAnimatorController);
        //AnimatorController controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(assetPath);

        //AnimatorControllerLayer nav_layer = new AnimatorControllerLayer();
        //foreach (var l in controller.layers)
        //{
        //    if (l.name == "PageNavigation")
        //        nav_layer = l;
        //}


        ////var nav_layer = AnimatorEditor.GetLayerByName(controller, "PageNavigation");
        //if (nav_layer == null)
        //    return;

        //var startObj = obj;
        //var startPage = startObj.GetComponent<Page>();
        //var name = obj.name;
        ////remove old clip
        ////find the corresponding state on the page nav layer
        //var pageState = AnimatorEditor.GetStateByName(nav_layer.stateMachine, name);
        //if (pageState == null)
        //{
        //    Debug.Log("No state with name: " + name);
        //    return;
        //}

        //foreach (var t in pageState.transitions)
        //{
        //    //check that destination state is also a page object
        //    string destName = "";
        //    if (t.destinationState != null)
        //        destName = t.destinationState.name;
        //    var targetObj = GameObject.Find(destName);
        //    if (targetObj == null)
        //    {

        //        Debug.Log("no matching page object for destination state found");
        //        continue;
        //    }

        //    if (targetObj.GetComponent<Page>() == null)
        //    {
        //        continue;
        //    }

        //    //construct the animation
        //    var curr_animation = new AnimationClip();
        //    var start = startObj.transform;
        //    var end = targetObj.transform;
        //    AddTransformKeyframe("Pivot", start, end, 0, startPage.transitionSpeed, ref curr_animation);
        //    //curr_animation.legacy = true;
        //    var anim_name = name + "_" + targetObj.name + ".anim";
        //    var path = "Assets/Resources/Animations/" + anim_name;
        //    AssetDatabase.CreateAsset(curr_animation, path);

        //    //load the animation in to the corresponding layer's intro state
        //    var destLayer = AnimatorEditor.GetLayerByName(controller, destName);
        //    if (destLayer != null)
        //    {
        //        var introState = AnimatorEditor.GetStateByName(destLayer.stateMachine, "Intro");
        //        var new_state = controller.AddMotion(curr_animation, AnimatorEditor.GetLayerIndexByName(controller, destName));
        //        if (introState != null)
        //        {
        //            introState.motion = new_state.motion;
        //        }

        //        destLayer.stateMachine.RemoveState(new_state);
        //    }

        //}
    }

    public static void IntializeAllCameraAnims()
    {
        //var pivot = GameObject.Find("Pivot");
        //if (pivot == null)
        //{
        //    Debug.Log("Could not create camera animations: no pivot object was found.");
        //    return;
        //}
        //var anim = GameObject.Find("Main").GetComponent<Animator>();
        //if (anim == null)
        //{
        //    Debug.Log("Could not create camera animations: no Animation component was found on Main.");
        //    return;
        //}

        //var dir_path = "Assets/Resources/Animations";
        //if (!AssetDatabase.IsValidFolder(dir_path))
        //    AssetDatabase.CreateFolder("Assets/Resources", "Animations");

        //string assetPath = AssetDatabase.GetAssetPath(anim.runtimeAnimatorController);
        //AnimatorController controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(assetPath);

        //AnimatorControllerLayer nav_layer = new AnimatorControllerLayer();
        //foreach (var l in controller.layers)
        //{
        //    if (l.name == "PageNavigation")
        //        nav_layer = l;
        //}


        ////var nav_layer = AnimatorEditor.GetLayerByName(controller, "PageNavigation");
        //if (nav_layer == null)
        //    return;

        //for (int i = 0; i < Selection.gameObjects.Length; i++)
        //{
        //    var startObj = Selection.gameObjects[i];
        //    var startPage = startObj.GetComponent<Page>();
        //    var name = Selection.gameObjects[i].name;
        //    //remove old clip
        //    //find the corresponding state on the page nav layer
        //    var pageState = AnimatorEditor.GetStateByName(nav_layer.stateMachine, name);
        //    if (pageState == null)
        //    {
        //        Debug.Log("No state with name: " + name);
        //        continue;
        //    }

        //    foreach (var t in pageState.transitions)
        //    {
        //        //check that destination state is also a page object
        //        string destName = "";
        //        if (t.destinationState != null)
        //            destName = t.destinationState.name;
        //        var targetObj = GameObject.Find(destName);
        //        if (targetObj == null)
        //        {

        //            Debug.Log("no matching page object for destination state found");
        //            continue;
        //        }

        //        if (targetObj.GetComponent<Page>() == null)
        //        {
        //            continue;
        //        }

        //        //construct the animation
        //        var curr_animation = new AnimationClip();
        //        var start = startObj.transform;
        //        var end = targetObj.transform;
        //        AddTransformKeyframe("Pivot", start, end, 0, startPage.transitionSpeed, ref curr_animation);
        //        //curr_animation.legacy = true;
        //        var anim_name = name + "_" + targetObj.name + ".anim";
        //        var path = "Assets/Resources/Animations/" + anim_name;
        //        AssetDatabase.CreateAsset(curr_animation, path);

        //        //load the animation in to the corresponding layer's intro state
        //        var destLayer = AnimatorEditor.GetLayerByName(controller, destName);
        //        if(destLayer != null)
        //        {
        //            var introState = AnimatorEditor.GetStateByName(destLayer.stateMachine, "Intro");
        //            var new_state = controller.AddMotion(curr_animation, AnimatorEditor.GetLayerIndexByName(controller, destName));
        //            if(introState != null)
        //            {
        //                introState.motion = new_state.motion ;                   
        //            }

        //            destLayer.stateMachine.RemoveState(new_state);
        //        }

        //    }

        //}

        //AssetDatabase.Refresh();
    }

    public static void AddTransformKeyframe(string relativePath, Transform start, Transform end, float startTime, float endTime, ref AnimationClip clip)
    {
        AnimationCurve curve = AnimationCurve.EaseInOut(startTime, start.localPosition.x, endTime, end.localPosition.x);
        clip.SetCurve(relativePath, typeof(Transform), "localPosition.x", curve);
        curve = AnimationCurve.EaseInOut(startTime, start.localPosition.y, endTime, end.localPosition.y);
        clip.SetCurve(relativePath, typeof(Transform), "localPosition.y", curve);
        curve = AnimationCurve.EaseInOut(startTime, start.localPosition.z, endTime, end.localPosition.z);
        clip.SetCurve(relativePath, typeof(Transform), "localPosition.z", curve);
        curve = AnimationCurve.EaseInOut(startTime, start.localRotation.x, endTime, end.localRotation.x);
        clip.SetCurve(relativePath, typeof(Transform), "localRotation.x", curve);
        curve = AnimationCurve.EaseInOut(startTime, start.localRotation.y, endTime, end.localRotation.y);
        clip.SetCurve(relativePath, typeof(Transform), "localRotation.y", curve);
        curve = AnimationCurve.EaseInOut(startTime, start.localRotation.z, endTime, end.localRotation.z);
        clip.SetCurve(relativePath, typeof(Transform), "localRotation.z", curve);
        curve = AnimationCurve.EaseInOut(startTime, start.localRotation.w, endTime, end.localRotation.w);
        clip.SetCurve(relativePath, typeof(Transform), "localRotation.w", curve);
    }

}
