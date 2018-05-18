using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections;
using System.Linq;
using System;
using System.Threading;
public class CreateAssetBundles
{
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
		BuildPipeline.BuildAssetBundles("Assets/Build", BuildAssetBundleOptions.None, BuildTarget.WebGL);
    }

    [MenuItem("Assets/StreamBundle")]
    static void StreamBundle()
    {
        //create form
        var form_data = new WWWForm();
        form_data.AddField("dev_code", "testing123");
        form_data.AddField("author", "D.Hambleton");
        form_data.AddField("title", "Another Great Work from Unity");
        form_data.AddField("coupon", "DEMO2015");

        //load 
        var bundle_data = File.ReadAllBytes(Path.Combine(Application.dataPath, "Build/booktest1.data"));
        Debug.Log(bundle_data.Length);
        form_data.AddBinaryData("package_data", bundle_data);

        //upload
        WWW w = new WWW("localhost:3000/create", form_data);
        while (!w.isDone) ;
        if (!string.IsNullOrEmpty(w.error))
        {
            Debug.Log(w.error);
        }
        else
        {
            Debug.Log("Finished Uploading Screenshot");
        }
    }

    [MenuItem("Assets/DownloadBundle")]
    static void DownloadBundle()
    {
        var form_data = new WWWForm();
        form_data.AddField("code", "DEMO2015");

        //upload
        WWW w = new WWW("localhost:3000/retrieve/?code=DEMO2015");
        Debug.Log("test1");
        //if (!string.IsNullOrEmpty(w.error))
        //{
        //    Debug.Log(w.error);
        //}
        //else
        //{
        //    Debug.Log("Finished Uploading Screenshot");
        //}

        //var bundle_data = File.ReadAllBytes(Path.Combine(Application.dataPath, "Build/booktest1.data"));
        //Debug.Log(bundle_data[0]);
        //Debug.Log(w.bytes[0]);

        //File.WriteAllText(Path.Combine(Application.dataPath, "Build/booktest2_out"), w.text);
        File.WriteAllBytes(Path.Combine(Application.dataPath, "Build/booktest1_out"), w.bytes);

        ////Debug.Log(w.text);
        //AssetBundle bundle = w.assetBundle;
        //Debug.Log(bundle.name);
        //AssetDatabase.CreateAsset(bundle, "Assets/Build/import_test");
        // Unload the AssetBundles compressed contents to conserve memory
        //bundle.Unload(false);
    }

    public static byte[] StringToByteArray(string hex)
    {
        return Enumerable.Range(0, hex.Length)
                         .Where(x => x % 2 == 0)
                         .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                         .ToArray();
    }
}
