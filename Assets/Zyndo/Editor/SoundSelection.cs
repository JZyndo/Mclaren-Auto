using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
//public class SoundSelection : EditorWindow
//{
//    public static SoundSelection instance;
//    private Texture2D icon;
//    public static Dictionary<string, AudioClip> soundDictionary = new Dictionary<string, AudioClip>();
//    SoundSelection()
//    {
//        instance = this;
//        icon = (Texture2D)Resources.Load("CoreImages/SoundIcon", typeof(Texture2D));
//		#if UNITY_EDITOR_WIN
//		DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "\\Resources\\Sounds");
//		#endif
//		#if UNITY_EDITOR_OSX
//		DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "//Resources//Sounds");
//		#endif
//        FileInfo[] info = dir.GetFiles("*.*");
//        foreach (FileInfo f in info)
//        {
//            if ((f.Extension.ToLower() == ".mp3" || f.Extension.ToLower() == ".wav" || f.Extension.ToLower() == ".aif" || f.Extension.ToLower() == ".aiff") && !soundDictionary.ContainsKey(f.Name))
//            {
//                string filePath = Path.GetFileNameWithoutExtension(f.Name);
//                AudioClip clip = (AudioClip)Resources.Load("Sounds/" + filePath, typeof(AudioClip));
//                soundDictionary.Add(f.Name, clip);
//            }
//        }
//    }

//    void OnGUI()
//    {
//        GUILayout.BeginHorizontal(GUI.skin.customStyles[0]);
//        int width = Screen.width;
//        int pictureSize = 0;
//        int counter = 0;
//        int index = 0;
//        foreach (KeyValuePair<string, AudioClip> clip in soundDictionary)
//        {
//            pictureSize += 100;
//            if (pictureSize > width)
//            {
//                pictureSize = 100;
//                GUILayout.EndHorizontal();
//                GUILayout.BeginHorizontal(GUI.skin.customStyles[0]);
//                int counter2 = 0;
//                foreach (KeyValuePair<string, AudioClip> clip2 in soundDictionary)
//                {
//                    if (counter2 == index)
//                    {
//                        GUILayout.TextArea(clip2.Key, GUILayout.Width(100), GUILayout.Height(15));
//                        counter--;
//                        index++;
//                    }
//                    counter2++;
//                    if (counter == 0)
//                        break;
//                }
//                GUILayout.EndHorizontal();
//                GUILayout.BeginHorizontal(GUI.skin.customStyles[0]);
//            }
//            if (GUILayout.Button(icon, GUILayout.Width(100), GUILayout.Height(100)))
//            {
//                ComicEditor.instance.soundSelection = clip.Key;
//                ComicEditor.instance.Focus();
//                this.Close();
//            }
//            counter++;
//        }
//        int counter3 = 0;
//        GUILayout.EndHorizontal();
//        GUILayout.BeginHorizontal(GUI.skin.customStyles[0]);
//        foreach (KeyValuePair<string, AudioClip> clip2 in soundDictionary)
//        {
//            if (counter3 == index)
//            {
//                GUILayout.TextArea(clip2.Key, GUILayout.Width(100), GUILayout.Height(15));
//                counter--;
//                index++;
//            }
//            counter3++;
//            if (counter == 0)
//                break;
//        }
//    }
//}
