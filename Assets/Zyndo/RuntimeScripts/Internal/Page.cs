using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public struct PageConnection
{
	public Page page;
	public string text;
	public PageConnection(Page page)
	{
		this.page = page;
		this.text = "";
	}

	public PageConnection(Page page, string text)
	{
		this.page = page;
		this.text = text;
	}
}
public class Page : MonoBehaviour {

	// Use this for initialization
	public List<PageConnection> connectedPages= new List<PageConnection>();
	public string psdAsset;
	public Sprite pagePreview;
	public int uniquePageID;
	public string md5;
	public float parallaxFactor;

    List<Parallax> parallaxes = new List<Parallax>();

	void Start()
	{
		CanvasGroup cg = GetComponent<CanvasGroup> ();
		if (cg == null)
		{
			cg = gameObject.AddComponent<CanvasGroup> ();
		}
		cg.blocksRaycasts = false;

        GetParallaxInChildren(transform);
	}

	// Update is called once per frame
	void Update () 
	{
        //here we handle all of our childrens components
        if (this != PageEventsManager.currentPage)
            return;

        //calculate the parallaxes
        for (int i = 0; i < parallaxes.Count; i++)
        {
            parallaxes[i].CalculatePosition();
        }

    }


    //this gets the parallaxes in this page in hiearchical order just in case we have some nested parallaxes
    void GetParallaxInChildren(Transform t)
    {

        //loop through every child
        for(int i = 0; i < t.childCount; i++)
        {
            //if this child has a parallax, then add it to our list
            if (t.GetChild(i).GetComponent<Parallax>())
                parallaxes.Add(t.GetChild(i).GetComponent<Parallax>());
        }

        //calculate their childrens parallaxes with a recursive call
        for (int i = 0; i < t.childCount; i++)
        {
            GetParallaxInChildren(t.GetChild(i));
        }

    }

}
