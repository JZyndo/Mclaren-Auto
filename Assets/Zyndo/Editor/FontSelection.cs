using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
public class FontSelection : EditorWindow
{
    public static FontSelection instance;
    private Texture2D icon;
    public static Dictionary<string, Font> fontDictionary = new Dictionary<string, Font>();
    FontSelection()
    {
        instance = this;
        icon = (Texture2D)Resources.Load("CoreImages/FontIcon", typeof(Texture2D));
        DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "/Resources/Fonts");
        FileInfo[] info = dir.GetFiles("*.*");
        foreach (FileInfo f in info)
        {
            if (f.Extension.ToLower() == ".ttf" && !fontDictionary.ContainsKey(f.Name))
            {
                string filePath = Path.GetFileNameWithoutExtension(f.Name);
                Font font = (Font)Resources.Load("Fonts/" + filePath, typeof(Font));
                fontDictionary.Add(f.Name, font);
            }
        }
    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal(GUI.skin.customStyles[0]);
        int width = Screen.width;
        int pictureSize = 0;
        int counter = 0;
        int index = 0;
        foreach (KeyValuePair<string, Font> clip in fontDictionary)
        {
            pictureSize += 100;
            if (pictureSize > width)
            {
                pictureSize = 100;
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(GUI.skin.customStyles[0]);
                int counter2 = 0;
                foreach (KeyValuePair<string, Font> clip2 in fontDictionary)
                {
                    if (counter2 == index)
                    {
                        GUILayout.TextArea(clip2.Key, GUILayout.Width(100), GUILayout.Height(15));
                        counter--;
                        index++;
                    }
                    counter2++;
                    if (counter == 0)
                        break;
                }
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(GUI.skin.customStyles[0]);
            }
            if (GUILayout.Button(icon, GUILayout.Width(100), GUILayout.Height(100)))
            {
                //ComicEditor.instance.fontSelection = clip.Value;
                //ComicEditor.instance.fontSelectionText = clip.Key;
                //ComicEditor.instance.Focus();
                this.Close();
            }
            counter++;
        }
        int counter3 = 0;
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(GUI.skin.customStyles[0]);
        foreach (KeyValuePair<string, Font> clip2 in fontDictionary)
        {
            if (counter3 == index)
            {
                GUILayout.TextArea(clip2.Key, GUILayout.Width(100), GUILayout.Height(15));
                counter--;
                index++;
            }
            counter3++;
            if (counter == 0)
                break;
        }
    }
}
