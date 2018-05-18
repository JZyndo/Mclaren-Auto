using UnityEngine;
using UnityEngine.Audio;
using UnityEditor;
using UnityEditor.Audio;
using System.IO;
using System.Collections.Generic;
using System.Collections;

public class AudioEditor : MonoBehaviour
{
    public static void AddAudio()
    {
        var audioClips = new List<AudioClip>();
        //load the assets
        foreach (var id in Selection.assetGUIDs)
        {
            var path = AssetDatabase.GUIDToAssetPath(id);
            var asset = AssetDatabase.LoadAssetAtPath(path, typeof(Object));
            Debug.Log(asset.GetType());
            if (asset.GetType() == typeof(AudioClip))
            {
                audioClips.Add((AudioClip)asset);
            }
            else
            {
                Debug.Log("You must select some audio files for this import process");
                return;
            }
        }

        //copy the prefab mixer and rename
        var mixerName = "";
        var res = false;
        var targetPath = "";
        var dir_path = "Assets/Resources/Mixers";
        if (!AssetDatabase.IsValidFolder(dir_path))
            AssetDatabase.CreateFolder("Assets/Resources", "Mixers");
        if (audioClips.Count > 0)
        {
            var name_length = Mathf.Min(audioClips[0].name.Length, 10);
            mixerName = audioClips[0].name.Substring(0, name_length) + ".mixer";
            targetPath = dir_path + "/" + mixerName;
            AssetDatabase.DeleteAsset(targetPath);

            res = AssetDatabase.CopyAsset("Assets/Zyndo/Prefabs/Music.mixer", targetPath);

        }
        else
            return;

        AssetDatabase.Refresh();
        AudioMixer mixer = Resources.Load("Mixers/" + mixerName.Split('.')[0]) as AudioMixer; ;

        //add the audio sources
        if (audioClips.Count > 0)
        {
            var go = GameObject.Find("UI Root");
            if (go == null)
                go = GameObject.Find("Main");

            //see if there is an audio parent gameobject
            var audioParent = GameObject.Find("Audio");
            if (audioParent == null)
                audioParent = new GameObject("Audio");
            audioParent.transform.SetParent(go.transform);

            var new_go = new GameObject(mixerName.Split('.')[0]);
            new_go.transform.parent = audioParent.transform;

            //add audio group
            var musicGroup = new_go.AddComponent<AudioPartGroup>();
            musicGroup.mixer = mixer;
            musicGroup.audioGroupRoot = GameObject.Find(mixerName.Split('.')[0]);
            int bpm = 120;
            int beats = 4;
            musicGroup.bpm = bpm;
            musicGroup.beatsPerBar = beats;

            var outputGroups = mixer.FindMatchingGroups("Track");
            for (int i = 0; i < audioClips.Count; i++)
            {
                var c = audioClips[i];
                var new_audio_go = new GameObject(c.name);
                new_audio_go.transform.parent = new_go.transform;
                var source = new_audio_go.AddComponent<AudioSource>();
                source.clip = c;
                source.playOnAwake = false;

                if (i < (int)Mathf.Min(outputGroups.Length, audioClips.Count))
                {
                    source.outputAudioMixerGroup = outputGroups[i];
                    outputGroups[i].name = source.name;
                }
            }
        }
    }
}
