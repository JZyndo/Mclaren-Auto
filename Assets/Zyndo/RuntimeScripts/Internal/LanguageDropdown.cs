using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LanguageDropdown : MonoBehaviour {

	public void ChangeLanguage(Text lang)
    {
        Localization.instance.selectedLang = lang.text;
        Localization.instance.SetLocalText();
    }
}
