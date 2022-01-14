using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Selection : MonoBehaviour
{
    public SelectionObject selectedBundleINFO;
    public void Update()
    {
        if (GetComponent<Toggle>().isOn)
        {
            GetComponent<Image>().color = GetComponent<Toggle>().colors.disabledColor;
            selectedBundleINFO.selectedAssetsFileInfo = gameObject;
        }
        else
        {
            GetComponent<Image>().color = GetComponent<Toggle>().colors.highlightedColor;
        }
    }
}
