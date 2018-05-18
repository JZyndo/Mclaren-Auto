using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.VR;

public class StartUp : MonoBehaviour
{
    public float startAlpha = 0f;
    public float vrEyePower = 0.01f;
	public float parallaxMobileMultiplier = 2.0f;
	public Color StartingAmbientColor = Color.black;
	public Material StartingSkybox;
	[SerializeField]
	public string MenuScene;

    // Use this for initialization
    void Start()
	{
		EpicPrefs.SetString ("LastSceneName", SceneManager.GetActiveScene().name, true);
		if (!VRSettings.enabled && MenuScene != "")
		{
			GameObject currControls = GameObject.Find ("Controls");
			if (currControls != null && currControls.scene == gameObject.scene)
			{
				currControls.SetActive (false);
			}
			Scene menuScene = SceneManager.GetSceneByName (MenuScene);
			if (MenuScene != "" && menuScene != null) {
				if (!menuScene.isLoaded) {
					SceneManager.LoadSceneAsync (MenuScene, LoadSceneMode.Additive);
				} else {
					Debug.Log ("StartUp is using an additive scene, but that scene is alredy loaded: " + MenuScene);
				}
			} else {

				Debug.LogError ("StartUp is using an additive scene, but that scene isn't in build settings: " + MenuScene);
			}

		}

        //do all the things needed to start correctly
        //lerp camera to first panel
        var pivot = GameObject.Find("Pivot");
		string startPageName = EpicPrefs.GetString ("StartPage");

        GameObject firstPanel = null;
		if(startPageName != "")
		{
			firstPanel = GameObject.Find (startPageName);
			EpicPrefs.DeleteString ("StartPage", false);
			if (firstPanel != null)
			{
				SetClosestSkybox (firstPanel.GetComponent<Page>());
			}
		}
		if (firstPanel == null && GameObject.Find ("Pages").transform.childCount > 0) {
			firstPanel = GameObject.Find ("Pages").transform.GetChild (0).gameObject;
		}
        if (pivot != null && firstPanel != null)
        {
            if (GameObject.FindObjectOfType<TutorialManager>())
            {
                pivot.transform.position = firstPanel.transform.position;
                pivot.transform.localRotation = firstPanel.transform.localRotation;
            }
            else if (PlayerPrefs.GetInt("LoadedBack", 0) != 0)
            {
                GameObject panel = GameObject.Find("Pages").transform.GetChild(GameObject.Find("Pages").transform.childCount-1).gameObject;
                if (panel)
                {
                    //make sure the visited pages has been initialized
                    if (SessionManager.instance.visitedPages == null)
                        SessionManager.instance.visitedPages = new System.Collections.Generic.Stack<Page>();

                    //we need to update the visitted pages
                    for (int i = 0; i < GameObject.Find("Pages").transform.childCount - 1; i++)
                    {
                        SessionManager.instance.visitedPages.Push(GameObject.Find("Pages").transform.GetChild(i).GetComponent<Page>());
                    }

                    pivot.transform.position = panel.transform.position;
                    pivot.transform.localRotation = panel.transform.localRotation;
                }
                else
                {
                    pivot.transform.position = firstPanel.transform.position;
                    pivot.transform.localRotation = firstPanel.transform.localRotation;
                }
            }

            else if(PageEventsManager.instance.rememberLastPage)
            {
                string lastScene = PlayerPrefs.GetString("LastSceneLoaded", "");

                if (lastScene.Length > 0 && lastScene != SceneManager.GetActiveScene().name)
                    SceneManager.LoadScene(lastScene);

                int lastPage = PlayerPrefs.GetInt("LastVisitedPage", 0);
                GameObject panel = GameObject.Find("Pages").transform.GetChild(lastPage).gameObject;
                if(panel)
                {
                    //make sure the visited pages has been initialized
                    if (SessionManager.instance.visitedPages == null)
                        SessionManager.instance.visitedPages = new System.Collections.Generic.Stack<Page>();

                    //we need to update the visitted pages
                    for(int i = 0; i < lastPage; i++)
                    {
                        SessionManager.instance.visitedPages.Push(GameObject.Find("Pages").transform.GetChild(i).GetComponent<Page>());
                    }

                    pivot.transform.position = panel.transform.position;
                    pivot.transform.localRotation = panel.transform.localRotation;
                }
                else
                {
                    pivot.transform.position = firstPanel.transform.position;
                    pivot.transform.localRotation = firstPanel.transform.localRotation;
                }
            }
            else
            {
                //StartCoroutine(LerpToObject(pivot, firstPanel));
                pivot.transform.position = firstPanel.transform.position;
                pivot.transform.localRotation = firstPanel.transform.localRotation;
            }

        }

		SetStartupSkyboxData ();

        //make sure controls are enabled
        var controls = GameObject.Find("Controls");
        if (controls != null)
        {
            controls.SetActive(true);
        }

        //set the start alpha of all fade controlled objects
        //SetStartAlpha();
    }

    void OnDisable()
    {
        //remove the first set textures in the skybox
        RenderSettings.skybox.SetTexture("_FrontTex", Texture2D.whiteTexture);
        RenderSettings.skybox.SetTexture("_BackTex", Texture2D.whiteTexture);
        RenderSettings.skybox.SetTexture("_LeftTex", Texture2D.whiteTexture);
        RenderSettings.skybox.SetTexture("_RightTex", Texture2D.whiteTexture);
        RenderSettings.skybox.SetTexture("_UpTex", Texture2D.whiteTexture);
        RenderSettings.skybox.SetTexture("_DownTex", Texture2D.whiteTexture);

        RenderSettings.skybox.SetTexture("_FrontTex2", Texture2D.whiteTexture);
        RenderSettings.skybox.SetTexture("_BackTex2", Texture2D.whiteTexture);
        RenderSettings.skybox.SetTexture("_LeftTex2", Texture2D.whiteTexture);
        RenderSettings.skybox.SetTexture("_RightTex2", Texture2D.whiteTexture);
        RenderSettings.skybox.SetTexture("_UpTex2", Texture2D.whiteTexture);
        RenderSettings.skybox.SetTexture("_DownTex2", Texture2D.whiteTexture);
        RenderSettings.skybox.SetFloat("_Blend", 0.0f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SetStartAlpha()
    {
        var fades = GetComponentsInChildren<PanelFade>();
        foreach (var f in fades)
        {
            if (!f.isActiveAndEnabled)
                continue;

            foreach (var fd in f.objectsToFade)
            {
                var componentToFade = fd.objectToFade.GetComponent(fd.type.ToString());
                if (componentToFade != null)
                {
                    componentToFade.SetAlphaGeneric(startAlpha);
                }
            }
        }
    }

    IEnumerator LerpToObject(GameObject source, GameObject target)
    {
        var steps = Mathf.Ceil(2.0f / Time.deltaTime);
        var curve = AnimationCurve.EaseInOut(0.0f, 0.0f, 1.0f, 1.0f);
        var orgPos = source.transform.position;
        var orgRot = source.transform.localRotation;
        for (int i = 0; i <= steps; i++)
        {
            var lerpVal = curve.Evaluate(i / steps);
            source.transform.position = Vector3.Lerp(orgPos, target.transform.position, lerpVal);
            source.transform.localRotation = Quaternion.Lerp(orgRot, target.transform.localRotation, lerpVal);
            yield return 0;
        }
    }

	public void SetStartupSkyboxData()
	{
		//remove the first set textures in the skybox
		if (StartingSkybox == null) {
			RenderSettings.skybox.SetTexture ("_FrontTex", Texture2D.whiteTexture);
			RenderSettings.skybox.SetTexture ("_BackTex", Texture2D.whiteTexture);
			RenderSettings.skybox.SetTexture ("_LeftTex", Texture2D.whiteTexture);
			RenderSettings.skybox.SetTexture ("_RightTex", Texture2D.whiteTexture);
			RenderSettings.skybox.SetTexture ("_UpTex", Texture2D.whiteTexture);
			RenderSettings.skybox.SetTexture ("_DownTex", Texture2D.whiteTexture);
			RenderSettings.skybox.SetColor ("_Tint", new Color(128, 128, 128, 128));
			RenderSettings.skybox.SetFloat ("_Rotation", 0);
			RenderSettings.skybox.SetFloat ("_Exposure", 1);
		} 
		else {
			RenderSettings.skybox.SetTexture ("_FrontTex", StartingSkybox.GetTexture("_FrontTex"));
			RenderSettings.skybox.SetTexture ("_BackTex",  StartingSkybox.GetTexture("_BackTex"));
			RenderSettings.skybox.SetTexture ("_LeftTex", StartingSkybox.GetTexture("_LeftTex"));
			RenderSettings.skybox.SetTexture ("_RightTex", StartingSkybox.GetTexture("_RightTex"));
			RenderSettings.skybox.SetTexture ("_UpTex", StartingSkybox.GetTexture("_UpTex"));
			RenderSettings.skybox.SetTexture ("_DownTex", StartingSkybox.GetTexture("_DownTex"));

			RenderSettings.skybox.SetColor ("_Tint", StartingSkybox.GetColor("_Tint"));
			RenderSettings.skybox.SetFloat ("_Rotation", StartingSkybox.GetFloat("_Rotation"));
			RenderSettings.skybox.SetFloat ("_Exposure", StartingSkybox.GetFloat("_Exposure"));
		}

		RenderSettings.skybox.SetTexture ("_FrontTex2", Texture2D.whiteTexture);
		RenderSettings.skybox.SetTexture ("_BackTex2", Texture2D.whiteTexture);
		RenderSettings.skybox.SetTexture ("_LeftTex2", Texture2D.whiteTexture);
		RenderSettings.skybox.SetTexture ("_RightTex2", Texture2D.whiteTexture);
		RenderSettings.skybox.SetTexture ("_UpTex2", Texture2D.whiteTexture);
		RenderSettings.skybox.SetTexture ("_DownTex2", Texture2D.whiteTexture);

		RenderSettings.skybox.SetFloat("_Blend", 0.0f);

		SetStartupAmbientLightData ();
	}

	public void SetStartupAmbientLightData()
	{
		RenderSettings.ambientLight = StartingAmbientColor;
	}

	public void SetClosestSkybox(Page page)
	{
		if (page != null)
		{
			int siblingIndex = page.transform.GetSiblingIndex ();
			SkyboxFade isThereSb = page.GetComponent<SkyboxFade> ();
			while (siblingIndex >= 1 && isThereSb == null)
			{
				isThereSb = page.transform.parent.GetChild (--siblingIndex).GetComponent<SkyboxFade> ();
			}
			if (isThereSb) {
				isThereSb.SetToSkybox ();
				if (!isThereSb.UseAmbientColor)
				{
					isThereSb = null;

					//Continue until you can set the previous ambient light
					while (siblingIndex >= 1 && isThereSb == null)
					{
						Debug.Log (siblingIndex);
						isThereSb = page.transform.parent.GetChild (--siblingIndex).GetComponent<SkyboxFade> ();
						if(isThereSb != null && !isThereSb.UseAmbientColor)
						{
							//Only accept this script if it is using ambient color;
							isThereSb = null;
						}
					}
					//If there is a preivious Fade Skybox that uses an ambient light, use that light, otherwise use Startup's default light
					if (isThereSb) {
						isThereSb.SetToAmbientLight ();
					} else {
						GameObject.Find ("Main").GetComponent<StartUp> ().SetStartupAmbientLightData ();
					}
				}
			} else {
				GameObject.Find ("Main").GetComponent<StartUp> ().SetStartupSkyboxData ();
			}
		}
	}
}
