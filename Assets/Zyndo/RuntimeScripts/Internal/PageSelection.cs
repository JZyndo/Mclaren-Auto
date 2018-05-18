using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class PagePreview
{
    public string name;
    public Page page;
    public Button button;
}

public class PageSelection : MonoBehaviour
{

    public GameObject previewPrefab;
    public GameObject contentPanel;
    public Page[] pages;

    private GameObject rightBar;
    private GameObject leftBar;

    private bool active = false;

    // Use this for initialization
    void Start()
    {
        SetupScrollView();
        //GetComponentInChildren<CanvasGroup>().alpha = 0.0f;
        leftBar = GameObject.Find("LeftBar");
        rightBar = GameObject.Find("RightBar");
        active = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToggleActive()
    {
        if(active)
        {
            //StartCoroutine(LerpAlpha(0.0f));
            leftBar.SetActive(true);
            rightBar.SetActive(true);
            active = false;
        }
        else
        {
            //StartCoroutine(LerpAlpha(1.0f));
            leftBar.SetActive(false);
            rightBar.SetActive(false);
            active = true;
        }
    }

    public void OnClicked(Button button)
    {
        if (!active)
            return;

        var id = -1;
        for (int i = 0; i < pages.Length; i++)
        {
            if (pages[i].gameObject.name == button.name)
            {
                id = i;
                break;
            }
        }

        if (id >= 0)
            SessionManager.instance.GoToPage(id);

        //disable the selector
        ToggleActive();
    }

    public void SetupScrollView()
    {
        var pageRoot = GameObject.Find("Pages");
        if (pageRoot != null)
        {
            pages = pageRoot.GetComponentsInChildren<Page>();
            foreach (var p in pages)
            {
                if (p.pagePreview != null)
                {
                    GameObject newButton = Instantiate(previewPrefab) as GameObject;
                    newButton.name = p.gameObject.name;
                    var button = newButton.GetComponent<Button>();
                    button.onClick.AddListener(() => OnClicked(button));
                    var image = newButton.GetComponent<Image>();
                    image.sprite = p.pagePreview;
                    newButton.transform.SetParent(contentPanel.transform);
                }
            }
        }
    }

    IEnumerator LerpAlpha(float target)
    {
        var group = GetComponentInChildren<CanvasGroup>();
        var steps = Mathf.Ceil(0.2f / Time.deltaTime);
        var orgVal = group.alpha;
        if (group != null)
        {
            for (int i = 0; i <= steps; i++)
            {
                group.alpha = Mathf.Lerp(orgVal, target, i / steps);
                yield return 0;
            }              
        }
    }



}
