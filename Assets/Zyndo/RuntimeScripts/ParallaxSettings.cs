using UnityEngine;
using System.Collections;

public class ParallaxSettings : MonoBehaviour {
	public string ParallaxPauseInput = "";
	public string ParallaxToZeroInput = "";

	public float ParallaxMulti = 300;
	public float ParallaxMultiVR = 1000;

	private static ParallaxSettings _instance;
	public static ParallaxSettings instance 
	{ 
		get { 
			if (_instance == null)
			{
				GameObject main = GameObject.Find ("Main");
				if (main == null)
				{
					Debug.LogError("There is no ParallaxSettings in the scene. If there was a GameObject with the name 'Main', a ParallaxSettings would be added automatically (but there isn't one). Please add ParallaxSettings somewhere.");
					return null;
				}
				return main.AddComponent<ParallaxSettings> ();
			}
			return _instance;
		}
	}

	public void Awake()
	{
		_instance = this;
		if (ParallaxSettings.instance.ParallaxPauseInput == "")
		{
			Debug.LogWarning ("ParallaxSettings's values' aren't set.");	
		}
	}
}