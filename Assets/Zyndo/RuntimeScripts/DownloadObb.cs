using UnityEngine;
using System.Collections;

public class DownloadObb : MonoBehaviour {
	public string PublicKeyFromGooglePlay;
	#if UNITY_ANDROID
	public void Start()
	{
		GooglePlayDownloader.Init (PublicKeyFromGooglePlay);
		if (GooglePlayDownloader.RunningOnAndroid ()) {
		
			string expPath = GooglePlayDownloader.GetExpansionFilePath ();
			if (expPath != null) {
				string mainPath = GooglePlayDownloader.GetMainOBBPath (expPath);

				if (mainPath == null) {
					GooglePlayDownloader.DeleteAllOldOBBs (expPath);
					GooglePlayDownloader.FetchOBB ();
				}
			}
		}
	}
	#endif
}
