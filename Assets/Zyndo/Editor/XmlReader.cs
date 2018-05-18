using UnityEngine;
using System.Collections;
using System.Xml;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class XmlReader {
    XmlDocument doc;
    Dictionary<string, Transform> gameObjectDictionary = new Dictionary<string, Transform>();
    public static string CURRENT_LANGUAGE = "English";
	// Use this for initialization
    void Start()
    { 
    }

    public void saveXML(GameObject obj)
    {
        Component[] textObjects = obj.GetComponentsInChildren(typeof(Transform), true);

        foreach (Transform gameObject in textObjects)
        {
            if (gameObject.tag == "TransparentFX") //Not sure what this is supposed to do
                gameObjectDictionary.Add(gameObject.name, gameObject);
        }

        if (Application.isEditor)
        {
            doc = new XmlDocument();
            //(1) the xml declaration is recommended, but not mandatory
            XmlElement root = doc.DocumentElement;
            //(2) string.Empty makes cleaner code
            XmlElement body = doc.CreateElement(string.Empty, "body", string.Empty);
            doc.AppendChild(body);
            foreach (KeyValuePair<string, Transform> pair in gameObjectDictionary)
            {
                XmlElement element = doc.CreateElement(string.Empty, "element", string.Empty);
                body.AppendChild(element);

                XmlElement objectNameElement = doc.CreateElement(string.Empty, "ObjectName", string.Empty);
                XmlText objectNameText = doc.CreateTextNode(pair.Value.name);
                objectNameElement.AppendChild(objectNameText);
                element.AppendChild(objectNameElement);

                //XmlElement englishElement = doc.CreateElement(string.Empty, "English", string.Empty);
                //XmlText labelText = doc.CreateTextNode(pair.Value.GetComponent<UILabel>().text);
                //englishElement.AppendChild(labelText);
                //element.AppendChild(englishElement);
            }
            var utf8WithoutBom = new System.Text.UTF8Encoding(false);
            string[] datas = EditorApplication.currentScene.Split('/');
            string sceneName = datas[datas.Length - 1].Replace(".unity", "");
            using (TextWriter sw = new StreamWriter("Assets/Build/" + sceneName + ".xml", false, utf8WithoutBom)) //Set encoding
            {
                doc.Save(sw);
            }
        }

    }

    public void loadXML(GameObject obj)
    {
        Component[] textObjects = obj.GetComponentsInChildren(typeof(Transform), true);

        foreach (Transform gameObject in textObjects)
        {
            if (gameObject.tag == "TransparentFX")
                gameObjectDictionary.Add(gameObject.name, gameObject);
        }

        if (doc == null)
        {
            string[] datas = EditorApplication.currentScene.Split('/');
            string sceneName = datas[datas.Length - 1].Replace(".unity", "");
            TextAsset textXML = (TextAsset)Resources.Load("Build/" + sceneName, typeof(TextAsset));
            doc = new XmlDocument();
            doc.LoadXml(textXML.text);
        }

        XmlNode root2 = doc.LastChild;
        GameObject currentObject = null;
        foreach (XmlNode node in root2.ChildNodes)
        {
            foreach (XmlNode innerNode in node.ChildNodes)
            {
                if (innerNode.Name == "ObjectName")
                {
                    currentObject = gameObjectDictionary[innerNode.InnerText].gameObject;
                }
                else if (innerNode.Name == CURRENT_LANGUAGE)
                {
                    //currentObject.GetComponent<UILabel>().text = innerNode.InnerText;
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
	}
}
