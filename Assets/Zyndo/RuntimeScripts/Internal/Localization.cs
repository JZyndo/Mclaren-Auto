using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using System.Xml;
using System;
using System.Text;
using TMPro;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class Localization : MonoBehaviour
{

    public static Localization instance;

	//define the cultures
	public static string[] BaseLanguages = new string[]
	{
		"English",
		"Spanish"
	};

    public static TextAsset localizationFile;
	[HideInInspector]
	public string selectedLang = "English";


    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
		#if !UNITY_EDITOR
		SetToSystemLang();
		#endif
        SetLocalText();
    }

    // Update is called once per frame
    void Update()
    {

    }

	public void SetToSystemLang()
	{
		List<string> langs = GetLanguages ();
		if (langs.Contains(selectedLang))
		{
			selectedLang = Application.systemLanguage.ToString();
		}
	}

	public List<string> GetLanguages(XmlDocument doc = null)
	{
		if (doc == null)
		{
			var textObjs = GameObject.Find("Main").GetComponentsInChildren<DialogueText>();
			//load xml
			localizationFile = Resources.Load<TextAsset>("LocalizationFile");
			if (localizationFile == null)
			{
				return new List<string>(BaseLanguages);
			}
			doc = new XmlDocument();
			doc.LoadXml(localizationFile.text);
		}
		List<string> langList = new List<string> ();
		string query = String.Format("/body/Languages");            
		XmlNode tmp = doc.SelectSingleNode(query);  
		if (tmp != null)
		{
			for(int i = 0; i < tmp.ChildNodes.Count; i++)
			{
				var localText = tmp.ChildNodes[i];
				if (localText != null)
				{
					var textToSet = ((XmlElement)localText).GetAttribute("Name");
					langList.Add(textToSet);
				}
			}
		}
		return langList;
	}

    public void SetLocalText()
    {
        //grab all text components
        var textObjs = GameObject.Find("Main").GetComponentsInChildren<DialogueText>();
        //load xml
        localizationFile = Resources.Load<TextAsset>("LocalizationFile");
        if (localizationFile == null)
        {
            Debug.Log("no localization file found...");
            return;
        }
        var doc = new XmlDocument();
        doc.LoadXml(localizationFile.text);
		GetLanguages (doc);

		foreach (DialogueText t in textObjs)
        {
            var id = t.internal_id;
            if (id == "")
                continue;
            string query = String.Format("/body/ui_text[@internal_id='{0}']", t.internal_id);
            XmlNode tmp = doc.SelectSingleNode(query);
            if (tmp != null)
            {
                var localText = tmp.SelectSingleNode(selectedLang);
                if (localText != null)
                {
                    var textToSet = ((XmlElement)localText).GetAttribute("text");
                    t.SetText(textToSet);
                }
            }
        }
		#if UNITY_EDITOR
		if (!Application.isPlaying)
		{
			SceneView.RepaintAll();
		}
		#endif
    }

	#if UNITY_EDITOR    
	public static void CreateLocalizationFile(string culture)
	{
		var doc = new XmlDocument();
		WriteLanguageFile(ref doc, culture);
		doc.Save("Assets/Resources/LocalizationFile.xml");
		AssetDatabase.Refresh();

		var asset = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Resources/LocalizationFile.xml") as TextAsset;
		localizationFile = asset;
	}

    public static void CreateLocalizationFile()
    {
		Localization loc = GameObject.Find ("Main").GetComponent<Localization> ();
		CreateLocalizationFile ( loc.selectedLang);
    }

	static void WriteLanguageFile(ref XmlDocument doc, string currentLang)
    {
        //by default, create a new doc
        doc = new XmlDocument();
        XmlElement root = doc.DocumentElement;
		XmlElement body = doc.CreateElement(string.Empty, "body", string.Empty);
		XmlElement Langs = doc.CreateElement(string.Empty, "Languages", string.Empty);
		for(int i = 0; i < BaseLanguages.Length; i++)
		{
			XmlElement textElement = doc.CreateElement("Language");
			textElement.SetAttribute("Name", BaseLanguages[i]);
			Langs.AppendChild (textElement);
		}

		body.AppendChild (Langs);
        //check if a language file exists. If it does, load it.
        localizationFile = Resources.Load<TextAsset>("LocalizationFile");
        if (localizationFile != null)
        {
            doc.LoadXml(localizationFile.text);
            body = (XmlElement)doc.SelectSingleNode("body");
        }
		bool CheckForRedundancies = true;
		bool sceneDirty = false;
        //grab all text components
		DialogueText[] textObjs = GameObject.Find("Main").GetComponentsInChildren<DialogueText>(true);

        foreach (var t in textObjs)
        {
            //check if an element of this id already exists
			t.GetText();
            var id = t.internal_id;
			if (t.internal_id == null ||  t.internal_id == "")
			{
				List<XmlNode> nodes = FindRedundentNodes (doc: doc, pageName: PageEventBase.FindPage(t.gameObject).name, text:  t.GetText());
				if (nodes.Count == 0)
				{
					id = t.SetInternalID ();
					sceneDirty = true;
					Debug.Log ("Set the internal_id");
				}
				else if (nodes.Count == 1)
				{
					string pageName = PageEventBase.FindPage(t.gameObject).name;
					string objectName = t.name;
					string request = string.Format ("DialogueText from page \"{0}\" on GameObject \"{1}\" has NO INTERNAL ID, but has the same PSD and english text as an element in the localization file.", pageName, objectName);
					request +=  Environment.NewLine + "This means:" + Environment.NewLine;
					request += "-It's a coincidence, so create another ID." + Environment.NewLine;
					request += "-The ID was lost, but the localization data still exists, so use the same ID" + Environment.NewLine;
					request += "-There is another version of this panel in another scene, so use the same ID." + Environment.NewLine + Environment.NewLine;
					request += "Do you want to use the same preexisting intenal_id or create a new one?";
					if (EditorUtility.DisplayDialog ("Write Language File", request, "Use The Same ID", "Create New ID"))
					{
						string redundentId = ((XmlElement) nodes[0]).GetAttribute("internal_id");

						sceneDirty = true;
						id = t.SetInternalID (redundentId);
					}			
					else
					{
						id = t.SetInternalID ();
						sceneDirty = true;
					}
				}
				else
				{					
					string pageName = PageEventBase.FindPage(t.gameObject).name;
					string objectName = t.name;
					string request = string.Format ("DialogueText from page \"{0}\" on GameObject\"{1}\" has NO INTERNAL ID, but there are multiple DialogueText's with the same PSD and english text. Do you want to CREATE AN NEW ID?", pageName, objectName);

					if (EditorUtility.DisplayDialog ("Write Language File", request, "Create New ID", "Set Internal ID Manually"))
					{
						sceneDirty = true;
						id = t.SetInternalID ();
						Debug.Log ("Set the internal_id");
					}			
					else
					{					
						request = "As this DialogueText component doesn't have an internal_id, it will be skipped. Please see the console for details on the redundent objects and set it on the DialogueText manually.";
						EditorUtility.DisplayDialog ("Write Language File", request, "Okay");
						string logMessage = "";
						logMessage += string.Format("DialogueText from page \"{0}\" on GameObject\"{1}\" has NO INTERNAL ID" + Environment.NewLine, pageName, objectName);
						foreach (XmlNode node in nodes)
						{			
							sceneDirty = true;
							string redundentId = ((XmlElement) node).GetAttribute("internal_id");
							logMessage += string.Format("Other internal_id: \"{0}\"" + Environment.NewLine, redundentId);
						}
						Debug.Log (logMessage);
					}
				}
			}
			else if(CheckForRedundancies)
			{
				/*
				 * scene based redundancy check, really needs to be cross scene. I left this in case we want this fleshed out later.
				List<XmlNode> nodes = FindRedundentNodes (doc: doc, pageName: PageEventBase.FindPage(t.gameObject).name, text:  t.GetText());
				if (nodes.Count > 1)
				{ 
					string request = "There are two elements in the localization file that may be redundent, as they are associated with the same PSD and have the same english text. Do you want to set {0}->{1}'s DialogueText's internal_id to the other one? This will NOT delete any info in the localization file, as it may be associated with other components.";
					if (EditorUtility.DisplayDialog("Write Language File", request, "Yes", "No") )
					{
						XmlNode node = GetNodeWithoutID (nodes, t.internal_id);
						t.SetInternalID(GetInternalID(node));

						//Delete the node associated with this ID
						break;
					}
				}
				*/
			}
            string query = String.Format("/body/ui_text[@internal_id='{0}']", t.internal_id);
            XmlNode tmp = doc.SelectSingleNode(query);
            if (tmp != null)
            {
                //update text corresponding to the current culture setting
				XmlElement cultureText = (XmlElement)tmp.SelectSingleNode(currentLang);
				if (cultureText == null) {
					cultureText = CreateLocalizationOption (doc, currentLang);
					cultureText.SetAttribute ("text", currentLang);

					//append to this option
					tmp.AppendChild(cultureText);

					//append to body
					//tmp.AppendChild(textElement);
				} else {
					cultureText.SetAttribute ("text", t.GetText ());
				}
            }
            else
            {
                //set the attributes
				XmlElement textElement = doc.CreateElement("ui_text");
                textElement.SetAttribute("internal_id", t.internal_id);
                textElement.SetAttribute("page_name", PageEventBase.FindPage(t.gameObject).name);
                textElement.SetAttribute("obj_name", t.gameObject.transform.parent.name);
                textElement.SetAttribute("obj_index", t.gameObject.transform.GetSiblingIndex().ToString());

				//create the language options
				foreach (var c in BaseLanguages)
                {
                    var cultureOption = CreateLocalizationOption(doc, c);
					if (c == currentLang) {
						cultureOption.SetAttribute ("text", t.GetText ());
					}
                    //append to this option
                    textElement.AppendChild(cultureOption);
                }
                //append to body
                body.AppendChild(textElement);
            }
		}

        //append body to doc
        doc.AppendChild(body);
		if (sceneDirty)
		{						
			EditorUtility.DisplayDialog ("Write Language File", "Internal IDs in the scene have been changed/updated. Please save the scene.", "Okay");
		}
    }

	public static List<XmlNode> FindRedundentNodes(XmlDocument doc, string text, string pageName)
	{
		string query = String.Format("/body/ui_text[@page_name='{0}']", pageName);
		XmlNodeList listFromPage = doc.SelectNodes(query);
		List<XmlNode> retNodes = new List<XmlNode> ();
		foreach (XmlNode node in listFromPage)
		{
			XmlElement cultureText = (XmlElement)node.SelectSingleNode("USA");
			if (cultureText.GetAttribute("text") == text)
			{
				retNodes.Add (node);
			}
		}
		return retNodes;
	}

	public static XmlNode GetNodeWithoutID(List<XmlNode> nodes, string currID)
	{
		foreach (XmlNode node in nodes)
		{
			if (((XmlElement) node).GetAttribute("internal_id") != currID)
			{
				return node;
			}
		}
		return null;
	}
	public static string GetInternalID(XmlNode node)
	{
		return ((XmlElement)node).GetAttribute ("internal_id");	
	}

	static XmlElement CreateLocalizationOption(XmlDocument doc, string culture)
    {
        XmlElement localizationOption = doc.CreateElement(culture);

        return localizationOption;
    }
#endif

}

