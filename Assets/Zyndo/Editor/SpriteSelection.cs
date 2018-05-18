using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
//public class SpriteSelection : EditorWindow
//{
//    public static Dictionary<string, Texture2D> textureDictionary = new Dictionary<string, Texture2D>();
//    public static Dictionary<Texture2D, int> idDictionary = new Dictionary<Texture2D, int>();
//    public static SpriteSelection instance;

//    SpriteSelection()
//    {
//        instance = this;
//        readImages(Application.dataPath + "/Resources/Images", "Images");
//    }

//    void readImages(string path, string folderName)
//    {
//        DirectoryInfo dir = new DirectoryInfo(path);
//        DirectoryInfo[] info2 = dir.GetDirectories();
//        FileInfo[] info = dir.GetFiles("*.*");
//        foreach (FileInfo f in info)
//        {
//            if (f.Extension.ToLower() == ".png" && !textureDictionary.ContainsKey(Path.GetFileNameWithoutExtension(f.Name)))
//            {
//                string filePath = Path.GetFileNameWithoutExtension(f.Name);
//                Debug.Log(filePath);
//                Texture2D texture = (Texture2D)Resources.Load(folderName + "/" + filePath, typeof(Texture2D));
//                if (texture == null)
//                    Debug.Log("olmauyo haco");
//                idDictionary.Add(texture, texture.GetInstanceID());
//                //Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100);
//                textureDictionary.Add(Path.GetFileNameWithoutExtension(f.Name), texture);
//            }
//        }
//        foreach (DirectoryInfo infoTemp in info2)
//            readImages(path + "/" + infoTemp.Name, folderName + "/" + infoTemp.Name);
//    }

//    void OnGUI()
//    {
//        GUILayout.BeginHorizontal(GUI.skin.customStyles[0]);
//        int width = Screen.width;
//        int pictureSize = 0;
//        foreach (KeyValuePair<string, Texture2D> element in textureDictionary)
//        {
//            pictureSize += 100;
//            if (pictureSize > width)
//            {
//                pictureSize = 100;
//                GUILayout.EndHorizontal();
//                GUILayout.BeginHorizontal(GUI.skin.customStyles[0]);
//            }
//            if (GUILayout.Button(element.Value, GUILayout.Width(100), GUILayout.Height(100)))
//            {
//                ComicEditor.instance.spriteSelection = Path.GetFileNameWithoutExtension(element.Key);
//                ComicEditor.instance.Focus();
//                this.Close();
//            }
//        }
//    }
//}