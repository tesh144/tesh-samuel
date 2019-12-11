using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ControlManager : MonoBehaviour
{
    [FoldoutGroup("Management",false)]
    public GameManager gameMan;
    [FoldoutGroup("Management", false)]
    public controlEvents controlEV;

    [FoldoutGroup("Scene Directions", false)]
    [InfoBox("Controls the directional indicators in the scene")]
    public GameObject directionsOBJ;
    [Space]
    [FoldoutGroup("Scene Directions", false)]
    public GameObject forwardOBJ;
    [FoldoutGroup("Scene Directions", false)]
    public GameObject backOBJ;
    [FoldoutGroup("Scene Directions", false)]
    public GameObject leftOBJ;
    [FoldoutGroup("Scene Directions", false)]
    public GameObject rightOBJ;

    [FoldoutGroup("UI Directions" , false)]
    [InfoBox("Controls the directional indicators in the canvas")]
    public GameObject forwardUI;
    [FoldoutGroup("UI Directions", false)]
    public GameObject backUI;
    [FoldoutGroup("UI Directions", false)]
    public GameObject leftUI;
    [FoldoutGroup("UI Directions", false)]
    public GameObject rightUI;

    private bool fwd;
    private bool bck;
    private bool lft;
    private bool rgt;

    // Start is called before the first frame update
    public void DoOnForward()
    {
        if (!fwd)
        {
            //controlEV.onForward.Invoke();
            gameMan.TriggerFWD();
            directionsOBJ.SetActive(true);
            forwardOBJ.SetActive(true);
            //controlEV.onAll.Invoke();
            fwd = true;
            bck = false;
            lft = false;
            rgt = false;
        }
    }

    public void DoOnBack()
    {
        if (!bck)
        {
            //controlEV.onBack.Invoke();
            gameMan.TriggerBCK();
            directionsOBJ.SetActive(true);
            backOBJ.SetActive(true);
            //controlEV.onAll.Invoke();
            fwd = false;
            bck = true;
            lft = false;
            rgt = false;
        }
    }

    public void DoOnLeft()
    {
        if (!lft)
        {
            //controlEV.onLeft.Invoke();
            gameMan.TriggerLFT();
            directionsOBJ.SetActive(true);
            leftOBJ.SetActive(true);
            //controlEV.onAll.Invoke();
            fwd = false;
            bck = false;
            lft = true;
            rgt = false;
        }
    }

    public void DoOnRight()
    {

        if (rgt)
        {
            //controlEV.onRight.Invoke();
            gameMan.TriggerRGT();
            directionsOBJ.SetActive(true);
            rightOBJ.SetActive(true);
            //controlEV.onAll.Invoke();
            fwd = false;
            bck = false;
            lft = false;
            rgt = true;
        }
    }
}
