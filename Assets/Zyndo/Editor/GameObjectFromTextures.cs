using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using TMPro;

public class GameObjectFromTextures
{
    public static Mesh canvasMesh;

    public static GameObject CreateGameObject(List<Texture> textures, PsdLayerExtractor extractor)
    {
        var page_root = new GameObject("page");
        page_root.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
        var layers = extractor.Root.children;
        var o_w = extractor.resolution.x;
        var o_h = extractor.resolution.y;

        //canvasMesh = CanvasMesh();

        Debug.Log("H res: " + o_w + " V Res: " + o_h);

        for (int i = 0; i < textures.Count; i++)
        {
            var panel = PanelFromTexture(textures[i]);
            panel.transform.parent = page_root.transform;
            panel.name = textures[i].name;

            var l = extractor.dims[i].x;// - 0.5f * o_w;
            var b = -extractor.dims[i].y;// + 0.5f * o_h;
            var w = extractor.dims[i].z;
            var h = extractor.dims[i].w;

            Debug.Log("current panel dims: " + extractor.dims[i]);

            //panel.transform.Rotate(panel.transform.forward, 180f);
            panel.transform.localPosition = new Vector3(l, b, -i * 100f);
            panel.transform.localScale = new Vector3(w, h, 1);

        }

        return page_root;
    }

    public static GameObject CreateUIObject(List<PSDLayerData> layerData, PsdLayerExtractor extractor)
    {
        //initialize prefabs
        GameObject panel_root_prefab = AssetDatabase.LoadAssetAtPath("Assets/Zyndo/Prefabs/PanelRoot.prefab", typeof(GameObject)) as GameObject;
        GameObject panel_layer_prefab = AssetDatabase.LoadAssetAtPath("Assets/Zyndo/Prefabs/PanelLayer.prefab", typeof(GameObject)) as GameObject;
        //create the root
        GameObject page_root = GameObject.Instantiate(panel_root_prefab);
        GameObject container = GameObject.Find("Pages");
        page_root.name = extractor.PsdFileName.Split('.')[0];
        if (container != null)
			page_root.transform.SetParent(container.transform);

        //grab the fade components
        //var fadeComps = page_root.GetComponents<PanelFade>();

        //attach the canvas group to fade by default
//        var cGroup = page_root.GetComponent<CanvasGroup>();
//        var fadeIn = new ObjectFadeData(page_root,
//            SupportedFadeTypes.CanvasGroup,
//            1.0f, 1.0f);
//
//        var fadeOut = new ObjectFadeData(page_root,
//            SupportedFadeTypes.CanvasGroup,
//            0.0f, 1.0f);
//
//        fadeComps[0].objectsToFade.Add(fadeIn);
//        fadeComps[1].objectsToFade.Add(fadeOut);

        //proceed with texture import
        var o_w = extractor.resolution.x;
        var o_h = extractor.resolution.y;
        Debug.Log("H res: " + o_w + " V Res: " + o_h);

        //initialize the top level rect for mask
        var top_rect = page_root.GetComponent<RectTransform>();
        var new_top_pos = new Vector3(o_w, o_h, 0.0f);
        new_top_pos.x -= 0.5f * o_w;
        new_top_pos.y += 05f * o_h;
        top_rect.localPosition = new_top_pos;
        top_rect.sizeDelta = new Vector2(o_w, o_h);

        for (int i = 0; i < layerData.Count; i++)
        {
            //instantiate a new sub panel
            GameObject panel = GameObject.Instantiate(panel_layer_prefab) as GameObject;
			panel.transform.SetParent(page_root.transform);
            panel.name = layerData[i].name;

            //parse the dimensions
            var w = layerData[i].dimensions.z;
            var h = layerData[i].dimensions.w;
            var l = layerData[i].dimensions.x;
            var b = -layerData[i].dimensions.y;

            //adjust the dimensions for unity
            var new_pos = new Vector3(l, b, (layerData.Count - i) * 25f);
            new_pos.x -= 0.5f * (o_w - w);
            new_pos.y += 0.5f * (o_h - h);
            //Debug.Log("current panel dims: " + layerData[i].dimensions);

            //adjust the rect transform
            var rect_transform = panel.GetComponent<RectTransform>();
            rect_transform.localPosition = new_pos;
            rect_transform.localScale = Vector3.one;
            rect_transform.sizeDelta = new Vector2(w, h);

            //setup parallax
            var par = panel.GetComponent<Parallax>();
            if (par == null)
                par = panel.AddComponent<Parallax>();
            if (layerData.Count > 1)
                par.parallaxFactor = (1.0f / (layerData.Count - 1)) * i;
            else
                par.parallaxFactor = 0.1f;

            //add the specific content
            switch(layerData[i].type)
            {
                case 0:
                    {
                        var raw_image = panel.GetComponent<Image>();

                        //need to load sprite rather than create it
                        var res = AssetDatabase.FindAssets(panel.name + "@", new string[1] { "Assets/Images/" + page_root.name });
                        if (res.Length < 1)
                        {
                            Debug.Log("could not find asset!");
                            continue;
                        }
                        var path = AssetDatabase.GUIDToAssetPath(res[0]);
						var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
						raw_image.sprite = sprite;
                        break;
                    }

                case 1:
                    {
   
                        //add speech bubble
                        var raw_image = panel.GetComponent<Image>();
                        //raw_image.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Zyndo/Textures/SpeechBubble_1.png");
                        //raw_image.preserveAspect = true;
                        raw_image.color = 0f * Color.white;
						raw_image.enabled = false;

                        //add canvas group
                        panel.AddComponent<CanvasGroup>();
                        panel.AddComponent<DialogueText>();

                        //add text as child object
                        var text_obj = new GameObject("Text");
                        text_obj.transform.parent = panel.transform;
                        text_obj.transform.localPosition = Vector3.zero;
                        text_obj.transform.localScale = Vector3.one;
                        var rect = text_obj.AddComponent<RectTransform>();
                        rect.sizeDelta = new Vector2(w, h);
                        text_obj.AddComponent<CanvasRenderer>();
						var text = text_obj.AddComponent<TextMeshProUGUI>();
						text.enableWordWrapping = true;
                        text.color = Color.black;
                        text.fontSize = 0.75f * ((int)layerData[i].dimensions.w);
                        text.text = layerData[i].text;               

                        break;
                    }
            }
        }

        //add page component and details
        var page = page_root.AddComponent<Page>();
        //page.psdAsset = extractor.PsdFileAsset;
        page.md5 = extractor.CalcMd5();

        //calc unique id
        page.uniquePageID = hash6432shift(System.DateTime.Now.Ticks);

        //place at origin
        page_root.transform.position = Vector3.zero;

        return page_root;
    }

    public static GameObject PanelFromTexture(Texture texture)
    {
        var obj = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/Prefabs/CanvasPanel.prefab");
        var panel = GameObject.Instantiate(obj) as GameObject;
        //panel.GetComponent<MeshFilter>().sharedMesh = canvasMesh;
        panel.name = "Panel";
        var material = Resources.Load("Materials/BasicPanel") as Material;
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        block.Clear();
        block.SetTexture("_MainTex", texture);
        // Set other values
        panel.GetComponent<MeshRenderer>().material = material;
        panel.GetComponent<MeshRenderer>().SetPropertyBlock(block);

        return panel;
    }

    public static Mesh CanvasMesh()
    {
        var m = new Mesh();
        var verts = new Vector3[4];
        verts[0] = new Vector3(0, 0, 0);
        verts[1] = new Vector3(1, 0, 0);
        verts[2] = new Vector3(1, -1, 0);
        verts[3] = new Vector3(0, -1, 0);

        var tris = new int[6];
        tris[0] = 0;
        tris[1] = 2;
        tris[2] = 1;

        tris[3] = 0;
        tris[4] = 3;
        tris[5] = 2;

        var uvs = new Vector2[4];
        uvs[0] = new Vector2(0, 1);
        uvs[1] = new Vector2(1, 1);
        uvs[2] = new Vector2(1, 0);
        uvs[3] = new Vector2(0, 0);

        m.vertices = verts;
        m.triangles = tris;
        m.uv = uvs;

        var obj = GameObject.CreatePrimitive(PrimitiveType.Quad);
        obj.GetComponent<MeshFilter>().sharedMesh = m;

        AssetDatabase.CreateAsset(m, "Assets/Resources/Prefabs/CanvasMesh.asset");
        //AssetDatabase.CreateAsset(obj, "Assets/Resources/Prefabs/PanelCanvas.prefab");

        return m;
    }

    public static int hash32shift(int key)
    {
        key = ~key + (key << 15); // key = (key << 15) - key - 1;
        key = key ^ (key >> 12);
        key = key + (key << 2);
        key = key ^ (key >> 4);
        key = key * 2057; // key = (key + (key << 3)) + (key << 11);
        key = key ^ (key >> 16);
        return key;
    }

    public static int hash6432shift(long key)
    {
        key = (~key) + (key << 18); // key = (key << 18) - key - 1;
        key = key ^ (key >> 31);
        key = key * 21; // key = (key + (key << 2)) + (key << 4);
        key = key ^ (key >> 11);
        key = key + (key << 6);
        key = key ^ (key >> 22);
        return (int)key;
    }



}
