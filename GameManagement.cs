using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;

#region Class_PanelRequirement
public enum PanelRequirement
{
    //create listeners to trigger these VVVVVVV

    fingerprintScan,
    sixFingerPrint,
    answerTextBox,
    speech,
    scan,
    timeTransmission,
    button,
    enable,
    other,
}
#endregion

#region Class_GamePanel
[System.Serializable]
public class GamePanel
{
    [TabGroup("Testing / Debugging")]
    public GameManagement GM;
    [TabGroup("Testing / Debugging")]
    [Button("Jump To This Panel" , ButtonSizes.Small)]
    public void JumpToThisPanel()
    {
        int i = 0;
        foreach (GamePanel gp in GM.gamePanels)
        {
            if (gp.panelName == panelName)
            {
                GM.PanelByInt(i);
                return;
            }
            else
            {
                i++;
            }
        }
    }

    [TabGroup("Panel Settings")]
    public string panelName;
    [TabGroup("Panel Settings")]
    public GameObject panelGO;
    [TabGroup("Animation Settings")]
    public Animator panelAnim;
    [TabGroup("Animation Settings")]
    public string exitAnimTrigger = "exit";
    [TabGroup("Animation Settings")]
    public bool disableOnClose = true;

    [TabGroup("Panel Settings")]
    public int tabNumber;

    [TabGroup("Requirements")]
    public List<PanelRequirement> Requirements;

    [TabGroup("Speech")]
    public List<string> speechesToPlay;
    [TabGroup("Speech")]
    public int speechInt;
}
#endregion

#region Class_VariablePanel 
[System.Serializable]
public class VariablePanel
{
    [TabGroup("Panel Settings")]
    public string panelName;
    [TabGroup("Panel Settings")]
    public GameObject panelGO;
    [TabGroup("Animation Settings")]
    public Animator panelAnim;
    [TabGroup("Animation Settings")]
    public string enterAnimTrigger = "enter";
    [TabGroup("Animation Settings")]
    public bool animateOnEntry;
    [TabGroup("Animation Settings")]
    public string exitAnimTrigger = "exit";
    [TabGroup("Animation Settings")]
    public bool disableOnClose = true;

    [TabGroup("Requirements")]
    public PanelRequirement Requirement;
}
#endregion

public class GameManagement : MonoBehaviour
{
    public TeamEdits TE;

    [FoldoutGroup("Save Progress Settings")]
    public SpyteamEditor spyteamEditor;
    [Space]
    [FoldoutGroup("Save Progress Settings")]
    [ReadOnly] public GamePanel saveProgressFromPanel;
    [FoldoutGroup("Save Progress Settings")]
    [ReadOnly] public int int_saveProgressFromPanel;
    [FoldoutGroup("Save Progress Settings")]
    public string name_saveProgressFromPanel;

    [FoldoutGroup("Main Panel Settings")]
    [ReadOnly] public GamePanel mainPanel;
    [FoldoutGroup("Main Panel Settings")]
    [ReadOnly] public int mainPanelInt;

    [FoldoutGroup("Aditional Panels")]
    public List<VariablePanel> variablePanels;

    [Space]
    [FoldoutGroup("Speech Management")]
    public SpeechManager speechMan;
    [FoldoutGroup("Speech Management")]
    [ReadOnly] public int savedProgress;

    [Space]
    [FoldoutGroup("Panel Manager")]
    public List<GamePanel> gamePanels;
    [FoldoutGroup("Panel Manager")]
    [ReadOnly] public int panelInt;

    [Space]
    [Range(0f,4f)]
    public float panelSwitchDelay;
    //public Text speechText;

    [Space]
    [FoldoutGroup("Debug")]
    [ReadOnly] public GamePanel currentPanel;
    [FoldoutGroup("Debug")]
    [ReadOnly] public VariablePanel currentVarPanel;
    public tabs tabsScript;

    #region Awake_Start_Enable_etc.
    private void Awake()
    {
        //Get saved progress from playerPrefs
        if(mainPanel == null) GetMainPanel();
        if(saveProgressFromPanel == null) GetSaveProgPanel();

        foreach(GamePanel gp in gamePanels)
        {
            gp.GM = this;
            gp.panelGO.SetActive(false);
        }

        foreach (VariablePanel vp in variablePanels)
        {
            variableStrings.Add(vp.panelName);
            vp.panelGO.SetActive(false);
        }
    }
    #endregion

    #region progress & Main Panel
    [FoldoutGroup("Main Panel Settings")]
    public string MainPanelName;
    [FoldoutGroup("Main Panel Settings")]
    [Button("Get Main Panel" , ButtonSizes.Medium)]
    public void GetMainPanel()
    {
        int i = 0;
        foreach (GamePanel gp in gamePanels)
        {
            if (gp.panelName == MainPanelName)
            {
                mainPanel = gp;
                mainPanelInt = i;
                return;
            }else i++;
        }
    }
    [FoldoutGroup("Save Progress Settings")]
    [Button("Get SaveProg Panel", ButtonSizes.Medium)]
    public void GetSaveProgPanel()
    {
        int i = 0;
        foreach (GamePanel gp in gamePanels)
        {
            if (gp.panelName == name_saveProgressFromPanel)
            {
                saveProgressFromPanel = gp;
                int_saveProgressFromPanel = i;
                return;
            }else i++;
        }
    }
    public void TurnOnMainPanel()
    {
        if(mainPanel != null)
        mainPanel.panelGO.SetActive(true);
    }
    public void SaveProgress()
    {
        if (panelInt >= int_saveProgressFromPanel)
        {
            PlayerPrefs.SetInt("Progress" + spyteamEditor.SpyteamNumber, panelInt);
            PlayerPrefs.SetFloat("timeElapsed" + spyteamEditor.SpyteamNumber, spyteamEditor.timecounter.timeTaken);
            //Debug.Log("SAVING PROGRESS!!! - " + panelInt);
        }
    }
    public void OpenAtSaved()
    {
        spyteamEditor.GetProgress();
        savedProgress = PlayerPrefs.GetInt("Progress" + spyteamEditor.SpyteamNumber);
        //TE.currentTeam = PlayerPrefs.GetInt("Version" + spyteamEditor.SpyteamNumber);
        if (savedProgress > panelInt)
        {
            PanelByInt(savedProgress);
        }else NextPanel();
    }
    #endregion

    #region Variable Panels
    //turns on a variable panel - call it by name
    [ReadOnly] public List<string> variableStrings;
    public void TurnOnVariablePanel(string name)
    {
        int i = 0;  foreach (string s in variableStrings)
        {
            if (s == name)
            {
                VariablePanel vp = variablePanels[i];
                vp.panelGO.SetActive(true);
                if (vp.animateOnEntry) vp.panelAnim.SetTrigger(vp.enterAnimTrigger);
                currentVarPanel = vp;
                return;
            }
            else
            {
                i++;
            }
        }
        //foreach (VariablePanel v in variablePanels)
       // {
           // if(v.panelName == name)
           //{
               // v.panelGO.SetActive(true);
                //if (v.animateOnEntry)
                   // v.panelAnim.SetTrigger(v.enterAnimTrigger);
                //currentVarPanel = v;
                //return;
           // }
        //}
    }
    public void CompleteVariablePanel()
    {
        currentVarPanel.panelAnim.SetTrigger(currentVarPanel.exitAnimTrigger);
        if (currentVarPanel.disableOnClose)
            StartCoroutine(TurnOffVarPanel());
    }

    private IEnumerator TurnOffVarPanel()
    {
        yield return new WaitForSeconds(panelSwitchDelay);
        currentVarPanel.panelGO.SetActive(false);
    }
    #endregion

    #region Speech
    public void PlaySpeech()
    {
        if (currentPanel.speechInt < currentPanel.speechesToPlay.Count)
        {
            speechMan.playclip(currentPanel.speechesToPlay[currentPanel.speechInt]);
            currentPanel.speechInt++;
        }
    }
    #endregion

    #region Standard Panel Settings
    //Panel is complete - start switching panel
    public void CompletePanel()
    {
        currentPanel.panelAnim.SetTrigger(currentPanel.exitAnimTrigger);
        StartCoroutine(SwitchPanel());
    }
    //Completes a requirement for the panel - if all requirements are complete then complete the panel
    public void CompleteRequirememt(PanelRequirement req)
    {
        for(int i = 0; i < currentPanel.Requirements.Count; i++)
        {
            PanelRequirement pr = currentPanel.Requirements[i];
            if(pr == req)
            {
               currentPanel.Requirements.RemoveAt(i);
                if (currentPanel.Requirements.Count <= 0)
                {
                    CompletePanel();
                    //Debug.Log("Panel Complete");
                }
                return;
            }
        }
    }
    #endregion

    #region Panel Switch Function
    //Turning on the next scheduled panel once it has been selected
    public void newPanel()
    {
        //First disable the current panel
        if (currentPanel.panelGO != null && currentPanel.disableOnClose)
        {
            currentPanel.panelGO.SetActive(false);
        }

        //checks if the new panel exists
        if (panelInt < gamePanels.Count)
        {
            //Sets the current panel and turns it on.
            currentPanel = gamePanels[panelInt];
            currentPanel.panelGO.SetActive(true);

            //if the panel requirement is to be enabled, complete this requirement now
            foreach(PanelRequirement pr in currentPanel.Requirements)
            {
                if (pr == PanelRequirement.enable)
                    CompleteRequirememt(PanelRequirement.enable);
            }

            //should display correct name on tabs
            tabsScript.siblingIndex = currentPanel.tabNumber;
            if (currentPanel.panelName != mainPanel.panelName)
            {
                //tabsScript.title[tabsScript.siblingIndex] = currentPanel.panelName;
            }//else tabsScript.title[tabsScript.siblingIndex] = "Loading...";
            tabsScript.setTabs();
        }
    }

    //3 functions to schedule the next panel to be turned on
    //Change panelint to identify new panel
    //trigger newPanel() to switch it on
    //Check if the main panel should be on, if so, turn it on
    //Save current progress

    //Goes forward 1 in the hierachy
    public void NextPanel()
    {
        panelInt++;
        if (panelInt < gamePanels.Count)
        {
            newPanel(); tabsScript.TabByNumber(currentPanel.tabNumber);
            if (panelInt >= mainPanelInt && mainPanel.panelGO != null)
                TurnOnMainPanel();
            SaveProgress();
        }
    }
    //Goes back 1 in the hierachy
    public void LastPanel()
    {
        panelInt--; newPanel(); tabsScript.TabByNumber(currentPanel.tabNumber);
        if(panelInt >= mainPanelInt)
            TurnOnMainPanel();
        SaveProgress();
    }
    //Goes to a specific panel in the hierachy - identify by int
    public void PanelByInt(int paneli)
    {
        panelInt = paneli;
        if (panelInt < gamePanels.Count)
        {
            newPanel(); tabsScript.TabByNumber(currentPanel.tabNumber);
            if (panelInt >= mainPanelInt)
                TurnOnMainPanel();
            SaveProgress();
        }
    }
    //Corourtine to delay the arrival of the next panel after the previous one has completed its exit animation
    public IEnumerator SwitchPanel()
    {
        yield return new WaitForSeconds(panelSwitchDelay);
        NextPanel();
        StopCoroutine(SwitchPanel());
    }
    #endregion
}
