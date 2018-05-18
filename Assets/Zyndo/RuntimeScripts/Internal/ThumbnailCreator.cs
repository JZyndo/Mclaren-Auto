#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ThumbnailCreator : MonoBehaviour {

    private bool doCapture = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

    }

    public void DoCapture()
    {
        doCapture = true;
    }

    void OnPostRender()
    {
        if(doCapture)
        {
#if UNITY_EDITOR
            //create the screenshot
            var dirPath = "Assets/Resources/Thumbnails";
            if (!AssetDatabase.IsValidFolder(dirPath))
                AssetDatabase.CreateFolder("Assets/Resources", "Thumbnails");
            var currObj = PageEventsManager.currentPage.gameObject;
            var filePath = dirPath + "/" + currObj.name + "_preview.png";

            var camRect = Camera.main.pixelRect;
            var offset = new RectOffset(10, 10, 50, 50);
            var newRect = offset.Remove(camRect);
            Texture2D tex = new Texture2D((int)newRect.width, (int)newRect.height, TextureFormat.ARGB32, false);
            tex.ReadPixels(newRect, 0, 0);
            tex.Apply();

            var data = tex.EncodeToPNG();
            System.IO.File.WriteAllBytes(filePath, data);
            AssetDatabase.Refresh();

            TextureImporter importer = AssetImporter.GetAtPath(filePath) as TextureImporter;
            if (importer != null)
            {
                importer.spriteImportMode = SpriteImportMode.Single;
                importer.textureType = TextureImporterType.Sprite;
                AssetDatabase.ImportAsset(filePath, ImportAssetOptions.ForceUpdate);
                AssetDatabase.Refresh();

                //link to page component (if available)
                Sprite preview = AssetDatabase.LoadAssetAtPath<Sprite>(filePath);
                var page = currObj.GetComponent<Page>();
                if (page != null && preview != null)
                {
                    page.pagePreview = preview;
                }
            }
#endif
            doCapture = false;
        }
    }
}
