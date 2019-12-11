using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public enum PanelType
{
    standard,
    variable,
    blended,
}

public class SendEnum : MonoBehaviour
{
    public PanelRequirement panelReq;
    public PanelType panelType;
    public GameManagement GM;
   
    [Button("Manual Send Enum" , ButtonSizes.Medium)]
    public void Send()
    {
        if (panelType == PanelType.standard)
        {
            GM.CompleteRequirememt(panelReq);
        }
        else if (panelType == PanelType.variable)
        {
            GM.CompleteVariablePanel();
        }
        else if(panelType == PanelType.blended)
        {
            GM.CompleteRequirememt(panelReq);
            GM.CompleteVariablePanel();
        }
    }

    public void SwitchEnumType()
    {
        if (panelType == PanelType.standard)
        {
            panelType = PanelType.variable;
        }else if(panelType == PanelType.variable)
        {
            panelType = PanelType.standard;
        }
    }

    public void SetTypeToStandard()
    {
        panelType = PanelType.standard;
    }
    public void SetTypeToVariable()
    {
        panelType = PanelType.variable;
    }
    public void SwitchToBlended()
    {
        panelType = PanelType.blended;
    }
}
