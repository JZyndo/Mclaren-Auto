using UnityEngine;
using System.Collections;
using System.IO;

public class PageLoader : MonoBehaviour
{

    public string[] pagesToLoad;

    // Use this for initialization
    void Start()
    {

        string dirPath = Application.streamingAssetsPath;

        for (int i = 0; i < pagesToLoad.Length; i++)
        {
            string path = Path.Combine(dirPath, pagesToLoad[i]);
            path = "file://" + path;
            StartCoroutine(LoadBundleLocal(path, pagesToLoad[i]));
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator LoadBundleLocal(string bundleURL, string assetToLoad)
    {
        WWW www = new WWW(bundleURL);
        yield return www;

        if (www.error == null)
        {

            // Get the designated main asset and instantiate it.
            var firstObjName = www.assetBundle.GetAllAssetNames()[0];
            var pagesObj = Instantiate(www.assetBundle.LoadAsset(firstObjName, typeof(GameObject))) as GameObject;
            pagesObj.name = assetToLoad;
            pagesObj.transform.parent = this.transform;
            PageEventsManager.pages = pagesObj.GetComponentsInChildren<Page>();
        }
    }
}
