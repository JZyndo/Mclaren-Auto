using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ToolbarNavigation : MonoBehaviour
{

    public GameObject[] toolbarMembers;
    static GameObject activeMember;

	public bool AddToolBarMember(GameObject toAdd = null, int index = 0)
	{
		index = Mathf.Clamp (index, 0, toolbarMembers.Length + 1);
		GameObject[] updatedToolbarMembers = new GameObject[toolbarMembers.Count() + 1]; 
		for(int i = 0; i < index ; i++)
		{
			updatedToolbarMembers[i] = toolbarMembers[i];
		}
		updatedToolbarMembers[index] = toAdd;
		for(int i = index; i < toolbarMembers.Count() ; i++)
		{
			updatedToolbarMembers[i + 1] = toolbarMembers[i];
		}
		toolbarMembers = updatedToolbarMembers;
		return true;
	}

    public void SetActive(GameObject obj)
    {
        if (activeMember == obj)
        {
            //turn off
            obj.GetComponent<UIPanelOperations>().ToggleActive();

            //set active to null
            activeMember = null;
        }
        else if (activeMember == null)
        {
            //set active member
            activeMember = obj;

            //turn on
            obj.GetComponent<UIPanelOperations>().ToggleActive();
        }
        else if (activeMember != obj)
        {
            //turn off active
            activeMember.GetComponent<UIPanelOperations>().ToggleActive();
            //turn on current
            obj.GetComponent<UIPanelOperations>().ToggleActive();
            //set active
            activeMember = obj;
        }
    }

    public void TurnOffActive()
    {
        if (activeMember != null)
        {
            //turn off active
            activeMember.GetComponent<UIPanelOperations>().ToggleActive();
        }
    }

    public void Help()
    {
        if(PageEventsManager.currentPage)
        {
            PlayerPrefs.SetInt("LastVisitedPage", PageEventsManager.currentPage.gameObject.transform.GetSiblingIndex());
            PlayerPrefs.SetString("LastSceneLoaded", SceneManager.GetActiveScene().name);
        }

        PlayerPrefs.SetInt("TutorialComplete", 0);

        SceneManager.LoadScene(0);
    }
}
