using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using UnityEngine;

[System.Serializable]
public class Team
{
    [HorizontalGroup("Team")]
    public Color col;
    [HorizontalGroup("Team")]
    public string name;
    [ReadOnly] public int TeamNumber;
}


#region Editable Image
[System.Serializable]
public class Editable_Image
{
    [TabGroup("Object")]
    public Image imgOBJ_Target;
    [TabGroup("Settings")]
    public bool changeColour;
    [TabGroup("Settings")]
    [ReadOnly] public TeamEdits TE;

    public List<Image_Edit> image_Edits;

    //[Button("Set Colours",ButtonSizes.Small)]
    public void SetColours()
    {
        int i = 0; foreach (Image_Edit iE in image_Edits)
        {
            if(TE.teams[i] != null) iE.teamColour = TE.teams[i].col; i++;
        }
    }
}
[System.Serializable]
public class Image_Edit
{
    [TabGroup("Colour")]
    [ReadOnly] public Color teamColour;
    [TabGroup("Details")]
    public Sprite imageToChangeTo;
}
#endregion

#region Editable Text
[System.Serializable]
public class Editable_Text
{
    [TabGroup("Object")]
    public Text txtOBJ_Target;
    [TabGroup("Settings")]
    public bool changeColour;
    [TabGroup("Settings")]
    [ReadOnly] public TeamEdits TE;

    public List<Text_Edit> text_Edits;

    //[Button("Set Colours", ButtonSizes.Small)]
    public void SetColours()
    {
        int i = 0; foreach (Text_Edit t in text_Edits)
        {
            if (TE.teams[i] != null) t.teamColour = TE.teams[i].col; i++;
        }
    }
}
[System.Serializable]
public class Text_Edit
{
    [TabGroup("Colour")]
    [ReadOnly] public Color teamColour;
    [TabGroup("Details")]
    [TextArea(0, 10)]
    public string stringToChangeTo;
}
#endregion

#region Editable Textbox
[System.Serializable]
public class Editable_Textbox
{
    [TabGroup("Object")]
    public TextBox txtboxOBJ_Target;
    [TabGroup("Settings")]
    [ReadOnly] public TeamEdits TE;
    [TabGroup("Settings")]
    public bool changeQuestion;

    public List<Textbox_Edit> txtbox_edits;

    public void SetColours()
    {
        int i = 0; foreach (Textbox_Edit t in txtbox_edits)
        {
            if (TE.teams[i] != null) t.teamColour = TE.teams[i].col; i++;
        }
    }
}
[System.Serializable]
public class Textbox_Edit
{
    [TabGroup("Colour")]
    [ReadOnly] public Color teamColour;
    [TabGroup("Details")]
    [TextArea(0, 10)]
    public string NewTarget;
    [TabGroup("Details")]
    [TextArea(0, 10)]
    public string NewQuestion;
}
#endregion


[System.Serializable]
public class TeamEdits : MonoBehaviour
{
    #region Variables
    public SpyteamEditor ste;
    public int currentTeam;
    public Image Background;
    public Text teamNoDisplay;

    [TitleGroup("Team Editor")]
    [Button("Add New Team" , ButtonSizes.Large)]
    public void AddNewTeam()
    {
        Team newTeam = new Team();
        teams.Add(newTeam);
        newTeam.TeamNumber = teams.Count;
    }
    [TitleGroup("Team Editor")]
    public List<Team> teams;
    [TitleGroup("Team Editor")]
    [Button("Set Colours" , ButtonSizes.Large)]
    public void SetColours()
    {
        foreach (Editable_Image ei in imgEdits) ei.SetColours();
        foreach (Editable_Text et in txtEdits) et.SetColours();
        foreach (Editable_Textbox edt in txtboxEdits) edt.SetColours();
    }


    [TitleGroup("Image Edits")]
    [Button("Add New Image To Edit",ButtonSizes.Large)]
    public void AddNewImageToEdit()
    {
        Editable_Image EI = new Editable_Image();
        imgEdits.Add(EI);
        EI.TE = this;
    }
    [TitleGroup("Image Edits")]
    public List<Editable_Image> imgEdits;


    [TitleGroup("Text Edits")]
    [Button("Add New Text To Edit", ButtonSizes.Large)]
    public void AddNewTextToEdit()
    {
        Editable_Text ET = new Editable_Text();
        txtEdits.Add(ET);
        ET.TE = this;
    }
    [TitleGroup("Text Edits")]
    public List<Editable_Text> txtEdits;

    [TitleGroup("Textbox Edits")]
    [Button("Add New Textvox To Edit" , ButtonSizes.Large)]
    public void AddNewTextboxToEdit()
    {
        Editable_Textbox EdT = new Editable_Textbox();
        txtboxEdits.Add(EdT);
        EdT.TE = this;
    }
    [TitleGroup("Textbox Edits")]
    public List<Editable_Textbox> txtboxEdits;
    #endregion

    //Editable Speech

    // Start is called before the first frame update
    private void Awake()
    {
        currentTeam = 0;
        DisplayTeams();
    }

    #region Team Navigation
    public void NextTeam()
    {
        currentTeam++; if (currentTeam >= teams.Count) currentTeam = 0;
        PlayerPrefs.SetInt("Version" + ste.SpyteamNumber, currentTeam);
        DisplayTeams();
    }
    public void LastTeam()
    {
        currentTeam--; if (currentTeam < 0) currentTeam = teams.Count-1;
        PlayerPrefs.SetInt("Version" + ste.SpyteamNumber, currentTeam);
        DisplayTeams();
    }
    public void DisplayTeams()
    {
        currentTeam = PlayerPrefs.GetInt("Version" + ste.SpyteamNumber);
        ste.Version = currentTeam; ste.VersionColor = teams[currentTeam].col;
        teamNoDisplay.text = "" + (currentTeam + 1);
        //PlayerPrefs.SetInt("Version" + ste.SpyteamNumber, currentTeam);

        if(ste.SV != null)
        ste.SV.TargetColour = teams[currentTeam].col;

        EditByVersion();
    }
    #endregion

    #region Main Function _ EditByVersion
    //Edits each setting by the Team / version chosen
    public void EditByVersion()
    {
        foreach(Editable_Image ei in imgEdits)
        {
            ei.imgOBJ_Target.sprite = ei.image_Edits[currentTeam].imageToChangeTo;
            if (ei.changeColour) ei.imgOBJ_Target.color = teams[currentTeam].col;
        }
        foreach(Editable_Text et in txtEdits)
        {
            et.txtOBJ_Target.text = et.text_Edits[currentTeam].stringToChangeTo;
            if (et.changeColour) et.txtOBJ_Target.color = teams[currentTeam].col;
        }
        foreach(Editable_Textbox edt in txtboxEdits)
        {
            edt.txtboxOBJ_Target.TargetValue = edt.txtbox_edits[currentTeam].NewTarget;
            if(edt.changeQuestion) edt.txtboxOBJ_Target.question = edt.txtbox_edits[currentTeam].NewQuestion;
        }
    }
    #endregion

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Background.color != teams[currentTeam].col)
        {
            Background.color = Color.Lerp(Background.color, teams[currentTeam].col, 4.5f * Time.deltaTime);
        }
    }
}
