using UnityEngine;
using UnityEngine.Audio;
using UnityEditor;
using UnityEditor.Audio;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine.UI;
using System.Threading;
using System;
using System.Reflection;

public enum Platform
{
	iPhone_Android,
	iPhone,
	Android,
	Standalone,
}

public enum ResolutionOptions
{
	RES_32 = 32, 
	RES_64 = 64, 
	RES_128 = 128, 
	RES_256 = 256, 
	RES_512 = 512, 
	RES_1024 = 1024, 
	RES_2048 = 2048, 
	NEXT_SMALLEST = -1
}

public struct PSDLayerData
{
	public int type;
	public string texture_path;
	public Vector4 dimensions;
	public string text;
	public string name;
}

public class PanelEditor : MonoBehaviour
{
	public static void UpdatePanels(bool force = false)
	{
		int numImagesProcessed = 0;
		foreach (var obj in Selection.gameObjects)
		{			
			string updatedText = String.Format ("{0}/{1} Pages Complete", numImagesProcessed,  Selection.gameObjects.Length);
			if (EditorUtility.DisplayCancelableProgressBar ("Progress", updatedText, (float)numImagesProcessed / Selection.gameObjects.Length))
			{
				EditorUtility.ClearProgressBar ();
				return;				
			}
			//var old_name = obj.name;
			//obj.name = "temp";

			var page = obj.GetComponent<Page>();

			if (page.psdAsset == "" || !File.Exists(page.psdAsset))
			{
				page.psdAsset = FindPSDFromHint(obj.name);

			}

			var psd_obj = AssetDatabase.LoadAssetAtPath(page.psdAsset, typeof(UnityEngine.Object));
			string filePath = AssetDatabase.GetAssetOrScenePath(psd_obj);

			try
			{
				PsdLayerExtractor extractor = new PsdLayerExtractor(null, psd_obj, filePath, null);
				string curr_md5 = extractor.CalcMd5();
				if (curr_md5 != page.md5 || force)
				{
					Debug.Log("Checksum inequality. Updating psd asset: " + extractor.PsdFilePath);
					var psd_name = extractor.PsdFileName.Split('.')[0];
					var dataLayers = new List<PSDLayerData>();
					ExtractLayers(ref dataLayers, ref extractor);
					var new_obj = GameObjectFromTextures.CreateUIObject(dataLayers, extractor);
					new_obj.name = "temp";
					//merge in new sprits, but try to retain other settings
					MergeTwoPanels(obj, new_obj);
					//update checksum
					page.md5 = curr_md5;
				}
				else
				{
					Debug.Log("Checksum equality. Skipping psd asset update: " + extractor.PsdFilePath);
				}
				++numImagesProcessed;
			}
			catch(Exception E)
			{
				Debug.Log (E.ToString());
			}

		}		
		EditorUtility.ClearProgressBar ();
		EditorUtility.DisplayDialog("Update Panels", String.Format ("{0} Page(s) Complete", numImagesProcessed), "Okay");
	}

	public static string FindPSDFromHint(string nameHint)
	{
		string psdPath = "";

		//scan the whole Assets folder
		var allFiles = Directory.GetFiles(Application.dataPath, "*.psd", SearchOption.AllDirectories);

		foreach (var f in allFiles)
		{
			var fileName = Path.GetFileName(f);
			if (fileName.Contains(nameHint))
			{
				var localPath = f.Replace(Application.dataPath, "Assets");
				psdPath = localPath;
				break;
			}
		}

		return psdPath;
	}

	public static void MergeTwoPanels(GameObject old_obj, GameObject new_obj)
	{
		//loop through children and replace sprites
		var old_panels = old_obj.GetComponentsInChildren<CanvasRenderer>(true).ToList();
		var new_panels = new_obj.GetComponentsInChildren<CanvasRenderer>(true).ToList();
		Transform lastSibling = null;
		foreach (var crFromNew in new_panels)
		{
			var matchFromOld = old_panels.FirstOrDefault(x => x.gameObject.name == crFromNew.gameObject.name);
			if (matchFromOld != null)
			{
				var image = matchFromOld.gameObject.GetComponent<Image>();
				var rect = matchFromOld.gameObject.GetComponent<RectTransform>();
				if (image != null)
				{
					image.sprite = crFromNew.gameObject.GetComponent<Image>().sprite;
					var new_rect = crFromNew.gameObject.GetComponent<RectTransform>();
					lastSibling = matchFromOld.transform;
					rect.sizeDelta = new_rect.sizeDelta;
					rect.position = new Vector3 (new_rect.position.x, new_rect.position.y, rect.position.z);
				}
			}
			else
			{
				var old_pos = crFromNew.transform.localPosition;
				crFromNew.transform.SetParent(old_obj.transform);
				crFromNew.transform.localPosition = old_pos;
				int lastSiblingIndex = lastSibling != null ? lastSibling.GetSiblingIndex () + 1 : 0; 
				Debug.Log (lastSiblingIndex, lastSibling);
				crFromNew.transform.SetSiblingIndex (lastSiblingIndex);
				lastSibling = crFromNew.transform;
			}
		}

		GameObject.DestroyImmediate(new_obj);

	}

	public static void CreatePanelsFromPSD()
	{
		//try to get a sense of how many pages have already been added
		//var pages = GameObject.Find("Pages");
		int counter = 0;

		//if (pages != null)
		//	counter = pages.transform.childCount;

		var move_vec = new Vector3(1, 0, 0);
		//order the selection
		var assetNames = new List<string>();
		foreach (var id in Selection.assetGUIDs)
			assetNames.Add(AssetDatabase.GUIDToAssetPath(id));
		var sorted = assetNames.OrderBy(x => x).ToList();
		//loop through the asset selection
		foreach (var id in sorted)
		{

			var psd_obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(id);
			PsdLayerExtractor extractor = new PsdLayerExtractor(null, psd_obj, id, null);
			var psd_name = extractor.PsdFileName.Split('.')[0];

            string updatedText = String.Format("{0}/{1} PSDs Complete: {2}.psd", counter, sorted.Count(), psd_name);
            if (EditorUtility.DisplayCancelableProgressBar("Progress", updatedText, (float)counter / sorted.Count()))
            {
                EditorUtility.ClearProgressBar();
                return;
            }

            var dataLayers = new List<PSDLayerData>();
			ExtractLayers(ref dataLayers, ref extractor);

			//var textures = new List<Texture>();
			//ExtractTextures(textures, extractor);
			var new_obj = GameObjectFromTextures.CreateUIObject(dataLayers, extractor);
			//now create the layers on the animator
			var page_id = new_obj.GetComponent<Page>().uniquePageID;
			new_obj.GetComponent<Page>().psdAsset = id;
			//move the panel along x-axis for simplified viz
			new_obj.transform.position += counter * 10f * move_vec;
			counter++;
		}
		EditorUtility.ClearProgressBar ();
	}

	public static string CreateObjectThumbnail(GameObject obj)
	{
		var pivot = GameObject.Find("Pivot");
		if (pivot == null)
		{
			Debug.Log("Pivot object not found. Try running Initialize.");
			return "";
		}

		//turn off the UI
		var controls = GameObject.Find("Controls");
		if (controls != null)
			controls.SetActive(false);

		//store the orignal pivot values
		var orgPos = pivot.transform.position;
		var orgRot = pivot.transform.localRotation;

		//move pivot to obj position
		pivot.transform.position = obj.transform.position;
		pivot.transform.localRotation = obj.transform.localRotation;

		//create the screenshot
		var dirPath = "Assets/Resources/Thumbnails";
		if (!AssetDatabase.IsValidFolder(dirPath))
			AssetDatabase.CreateFolder("Assets/Resources", "Thumbnails");
		var filePath = dirPath + "/" + obj.name + "_preview.png";

		Camera.main.Render();
		var camRect = Camera.main.pixelRect;
		var offset = new RectOffset(10, 10, 50, 50);
		var newRect = offset.Remove(camRect);
		Texture2D tex = new Texture2D((int)newRect.width, (int)newRect.height, TextureFormat.ARGB32, false);
		tex.ReadPixels(newRect, 0, 0);
		tex.Apply();

		var data = tex.EncodeToPNG();
		System.IO.File.WriteAllBytes(filePath, data);
		AssetDatabase.Refresh();

		//move cam back and trun UI back on
		pivot.transform.position = orgPos;
		pivot.transform.localRotation = orgRot;
		if (controls != null)
			controls.SetActive(true);

		return filePath;

	}

	public static void ExtractTextures(List<Texture> textures, PsdLayerExtractor extractor)
	{
		extractor.Reload();
		extractor.SaveLayersToPNGs();
		AssetDatabase.Refresh();

		foreach (var imageFilePath in extractor.ImageFilePathes)
		{
			var tex = AssetDatabase.LoadMainAssetAtPath(imageFilePath.filePath) as Texture2D;
			if (tex == null)
			{
				Debug.LogError("Cannot found texture assets. Please check " + imageFilePath.filePath + " file.");
				return;
			}

			var exist = false;
			foreach (var t in textures)
			{
				if (t.name == tex.name)
				{
					exist = true;
					break;
				}
			}
			if (!exist)
				textures.Add(tex);
		}
	}

	public static void ExtractLayers(ref List<PSDLayerData> dataLayers, ref PsdLayerExtractor extractor)
	{

		//reload the extractor
		extractor.Reload();

		//initialize the file paths and read the PSD
		var psdFilePath = extractor.PsdFilePath;
		var prePath = psdFilePath.Substring(0, psdFilePath.Length - 4) + "_layers";
		string[] tempData = prePath.Split('/');
		var name = extractor.PsdFileName.Split('.')[0];
		prePath = "Assets/Images/" + name + "/" + tempData[tempData.Length - 1];
		Debug.Log(prePath);
		var psdFileStream = new FileStream(extractor.PsdFilePath,
			FileMode.Open, FileAccess.Read, FileShare.Read);

		//start processing the layers
		foreach (var layer in extractor.Root.children)
		{
			//local extract so that we can recurse if the layer is a container
			ExtractLayersLocal(ref dataLayers,
				ref extractor,
				ref psdFileStream,
				layer,
				prePath);
		}

	}

	public static void ExtractLayersLocal(ref List<PSDLayerData> dataLayers,
		ref PsdLayerExtractor extractor,
		ref FileStream psdFileStream,
		PsdLayerExtractor.Layer layer,
		string prePath)
	{
		//exit if we can't parse the layer
		if (!layer.canLoadLayer)
			return;

		//recurse if the layer is a container
		if (layer.isContainer)
		{
			foreach (var l in layer.children)
			{
				ExtractLayersLocal(ref dataLayers,
					ref extractor,
					ref psdFileStream,
					l,
					prePath);
			}
		}
		else
		{
			//call the appropriate handler based on layer type
			var pa = new PsdLayerCommandParser.ControlParser(prePath, layer);
			switch (pa.type)
			{
			case PsdLayerCommandParser.ControlType.Label:
				HandleTextLayer(
					ref dataLayers,
					ref extractor,
					ref psdFileStream,
					layer,
					prePath,
					pa);
				break;
			case PsdLayerCommandParser.ControlType.Sprite:
				HandleImageLayer(
					ref dataLayers,
					ref extractor,
					ref psdFileStream,
					layer,
					prePath,
					pa);
				break;
			}
		}
	}

	public static void HandleImageLayer(
		ref List<PSDLayerData> dataLayers,
		ref PsdLayerExtractor extractor,
		ref FileStream psdFileStream,
		PsdLayerExtractor.Layer layer,
		string prePath,
		PsdLayerCommandParser.ControlParser pa)
	{
		var fileName = pa.fullName;

		//exit if file name is bad
		if (extractor.HasUnacceptibleChar(fileName))
		{
			Debug.LogError(fileName + " Contains wrong character '\\ / : * ? \" < > |' not allowed");
			return;
		}

		//read the image from the file stream
		var filePath = prePath + "/" + fileName + ".png";
		PsdLayerExtractor.ImageFilePath newImageFilePath = null;
		{
			try
			{
				psdFileStream.Position = 0;
				var br = new BinaryReader(psdFileStream);
				{
					layer.LoadData(br, extractor.Psd.headerInfo.bpp);
					newImageFilePath = new PsdLayerExtractor.ImageFilePath(filePath, "pass");
				}
			}
			catch (System.Exception e)
			{
				Debug.LogError(e.Message);
			}
		}

		//merge channels and check if successful
		var data = layer.psdLayer.mergeChannels();
		if (data == null)
			return;

		//create the texture
		Texture tex = null;
		if (pa.sliceArea != null)
		{
			tex = extractor.MakeSlicedSprites(ref data, layer, pa.sliceArea);
		}
		else
		{
			tex = extractor.MakeTexture(ref data, layer);
		}

		if (tex != null)
		{
			if (!System.IO.Directory.Exists(prePath))
				System.IO.Directory.CreateDirectory(prePath);

			//import the texture to the asset database
			System.IO.File.WriteAllBytes(filePath, data);
			AssetDatabase.Refresh();
			TextureImporter importer = AssetImporter.GetAtPath(filePath) as TextureImporter;
			ProcessTexture2D (importer, ResolutionOptions.NEXT_SMALLEST, true, false, Platform.iPhone_Android);

			// track the dims and imported asset filepath
			var layerData = new PSDLayerData();
			layerData.type = 0;
			layerData.dimensions = new Vector4(pa.area.left, pa.area.top, pa.area.width, pa.area.height);
			layerData.texture_path = filePath;
			layerData.name = layer.name;

			//the data to the referenced list
			dataLayers.Add(layerData);

			//destroy the temp texture
			Texture2D.DestroyImmediate(tex);
		}
	}

	public static void HandleTextLayer(
		ref List<PSDLayerData> dataLayers,
		ref PsdLayerExtractor extractor,
		ref FileStream psdFileStream,
		PsdLayerExtractor.Layer layer,
		string prePath,
		PsdLayerCommandParser.ControlParser pa)
	{
		// track the dims and imported asset filepath
		var layerData = new PSDLayerData();
		layerData.type = 1;
		layerData.dimensions = new Vector4(pa.area.left, pa.area.top, pa.area.width, pa.area.height);
		layerData.text = layer.text;
		layerData.name = layer.name;

		//the data to the referenced list
		dataLayers.Add(layerData);
	}

	/// <summary>
	/// Updates the panel settings.
	/// </summary>
	/// <param name="force">If set to <c>true</c> force.</param>
	public static void UpdateImportSettings(bool force, ResolutionOptions res, bool usePackingTags, bool useAssetBundles, Platform plat)
	{
		int numImagesProcessed = 0;			
		#region Get Data To Process
		if (Selection.gameObjects.Length != 0 && EditorUtility.DisplayDialog("Update Image Import Settings", 
			string.Format("{0} object(s) selected in the hierachy. Process images on selected pages?", Selection.gameObjects.Length),
			"Update", "Cancel"))
		{
			int numPagesComplete = 0;
			foreach (var obj in Selection.gameObjects)
			{
				var page = obj.GetComponent<Page>();

				//var psd_obj = AssetDatabase.LoadAssetAtPath(page.psdAsset, typeof(UnityEngine.Object));
				//string pageFilePath = AssetDatabase.GetAssetOrScenePath(psd_obj);
				//string panelName = page.name;
				Image[] spritesInPage = obj.GetComponentsInChildren<Image> ();
				foreach (Image imageToProcess in spritesInPage)
				{
					Sprite spriteToProcess = imageToProcess.sprite;
					string spritePath = AssetDatabase.GetAssetOrScenePath(spriteToProcess);
					if (spritePath.Contains("Images"))
					{
						TextureImporter importer = AssetImporter.GetAtPath(spritePath) as TextureImporter;
						if (importer != null) {
							ProcessTexture2D (importer, res, usePackingTags, useAssetBundles, plat);
							numImagesProcessed++;
							string updatedText = String.Format ("{0}/{1} Pages Complete", numPagesComplete, Selection.gameObjects.Count());
							if (EditorUtility.DisplayCancelableProgressBar ("Progress", updatedText, (float)numPagesComplete / Selection.gameObjects.Count()))
							{
								EditorUtility.ClearProgressBar ();
								return;
							}	
						}
					}
				}
				numPagesComplete++;
			}
			EditorUtility.ClearProgressBar ();
			EditorUtility.DisplayDialog("Update Import Settings", String.Format ("{0} Pages Complete\n{1} Images Complete", numPagesComplete, numImagesProcessed), "Okay");
		}		
		else
		{
			UnityEngine.Object[] selectedAsset = Selection.GetFiltered (typeof(Texture2D), SelectionMode.DeepAssets);
			if (selectedAsset.Length != 0 && EditorUtility.DisplayDialog("Update Image Import Settings", 
			string.Format("No pages were selected in the scene, but folders are selected in the project window. Process images in selected folders?"),
			"Update", "Cancel"))
			{
				selectedAsset = selectedAsset.Where((val) => val.GetType() == typeof(Texture2D)).ToArray();
				int numimagesProcessed = 0;
				foreach(UnityEngine.Object obj in selectedAsset)
				{
					TextureImporter importer = AssetImporter.GetAtPath (AssetDatabase.GetAssetPath (obj)) as TextureImporter;
					if (importer != null) {
						ProcessTexture2D (importer, res, usePackingTags, useAssetBundles, plat);
					}
					numImagesProcessed++;
					string updatedText = String.Format ("{0}/{1} Images Complete", numImagesProcessed,  selectedAsset.Length);
					if (EditorUtility.DisplayCancelableProgressBar ("Progress", updatedText, (float)numImagesProcessed / selectedAsset.Length))
					{
						EditorUtility.ClearProgressBar ();
						return;
					}
				}		
				EditorUtility.ClearProgressBar ();
				EditorUtility.DisplayDialog("Update Import Settings", String.Format ("{0} Images Complete", numImagesProcessed), "Okay");
			}
			else if(EditorUtility.DisplayDialog("Update Image Import Settings", 
				string.Format("Would you like to update ALL IMAGES?"),
				"Update", "Cancel"))
			{
				//Test All this stuff
				string[] paths = System.IO.Directory.GetFiles (Application.dataPath + "/Images", "*.png", SearchOption.AllDirectories);
				foreach(string spritePath in paths)
				{
					string processedSpritePath = "Assets" + spritePath.Replace (Application.dataPath, "");//.Replace (Path.AltDirectorySeparatorChar, '/')
					TextureImporter importer = AssetImporter.GetAtPath(processedSpritePath) as TextureImporter;
					if (importer != null) {
						ProcessTexture2D (importer, res, usePackingTags, useAssetBundles, plat);
					}
					numImagesProcessed++;
					string updatedText = String.Format ("{0}/{1} Images Complete", numImagesProcessed,  paths.Length);
					if (EditorUtility.DisplayCancelableProgressBar ("Progress", updatedText, (float)numImagesProcessed / paths.Length))
					{
						EditorUtility.ClearProgressBar ();
						return;				
					}
				}		
				EditorUtility.ClearProgressBar ();
				EditorUtility.DisplayDialog("Update Import Settings", String.Format ("{0} Images Complete", numImagesProcessed), "Okay");
			}		
		}
		#endregion
	}

	public static void ProcessTexture2D(TextureImporter importer, ResolutionOptions res, bool usePackingTags, bool useAssetBundles, Platform plat)
	{			
		string[] splitPath = importer.assetPath.Split (Path.AltDirectorySeparatorChar);
		string pageName = splitPath [splitPath.Length - 2];

		importer.textureType = TextureImporterType.Sprite;			
		importer.mipmapEnabled = false;

		if (useAssetBundles) {
			importer.assetBundleName = pageName;
			importer.assetBundleVariant = "";
		}

		string[] splitPlat = plat.ToString ().Split('_');
		foreach (string platStr in splitPlat) {

			if (usePackingTags) {
				importer.spritePackingTag = pageName;
			} else {
				importer.spritePackingTag = "";
			}

			int maxTextureSize = (int)res;
			if (res == ResolutionOptions.NEXT_SMALLEST) {
				object[] args = new object[2] { 0, 0 };
				MethodInfo mi = typeof(TextureImporter).GetMethod ("GetWidthAndHeight", BindingFlags.NonPublic | BindingFlags.Instance);
				mi.Invoke (importer, args);
 
				int width = (int)args [0];
				int height = (int)args [1];
				maxTextureSize = Mathf.ClosestPowerOfTwo (Mathf.Max (width, height));
				if (maxTextureSize >= Mathf.Max (width, height)) {
					maxTextureSize = maxTextureSize / 2;
				}
			}

			importer.SetPlatformTextureSettings (platStr, Mathf.Min (2048, maxTextureSize), TextureImporterFormat.AutomaticCompressed, 50, false);
		}
		AssetDatabase.ImportAsset (AssetDatabase.GetAssetPath (importer), ImportAssetOptions.ForceUpdate);
	}
}


