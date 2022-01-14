using AssetsTools.NET.Extra;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class TypeFieldsViewer : MonoBehaviour
{
    public LoadBundle lb;
    public GameObject selectedassetinfo;
    public GameObject console;
    public GameObject consolebutton;
    public Text consoletext;
    public GameObject typefieldviewer;
    public GameObject typefields;
    public GameObject typefieldcontainer;
    public GameObject endofline;
    public void viewTypeFields()
    {
        typefieldviewer.transform.localScale = new Vector3(1, 1, 1);
        typefieldcontainer.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
        typefieldcontainer.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
        StartCoroutine(viewtype());
    }
    public IEnumerator viewtype()
    {
        yield return new WaitForEndOfFrame();
        try
        {
            Text type = selectedassetinfo.GetComponent<SelectionObject>().selectedAssetsFileInfo.transform.Find("TYPEText").GetComponent<Text>();
            Text name = selectedassetinfo.GetComponent<SelectionObject>().selectedAssetsFileInfo.transform.Find("NAMEText").GetComponent<Text>();
            Debug.Log(name.text);
            Debug.Log(type.text);
            var inst = lb.am.LoadAssetsFileFromBundle(lb.bun, lb.drinfname);
            {
                foreach (var inf in inst.table.assetFileInfo)
                {
                    var baseField = lb.am.GetTypeInstance(inst.file, inf).GetBaseField();
                    var children = baseField.templateField.children;
                    var childrencount = baseField.templateField.childrenCount;
                    if (baseField.Get("m_Name").GetValue() != null)
                    {
                        if (baseField.Get("m_Name").GetValue().AsString() == name.text)
                        {
                            int i = 0;
                            foreach (var field in children)
                            {
                                if (field != null)
                                {
                                    i = i + 13;
                                    var nameStr = field.name;
                                    Debug.Log(nameStr.ToString());
                                    GameObject typeifeldobject = Instantiate(typefields) as GameObject;
                                    typeifeldobject.transform.Find("TypeField").GetComponent<Text>().text = nameStr.ToString();
                                    typeifeldobject.transform.position = new Vector3(typeifeldobject.transform.position.x, typeifeldobject.transform.position.y - i, typeifeldobject.transform.position.z);
                                    typeifeldobject.transform.SetParent(typefieldcontainer.transform);
                                    typefieldcontainer.GetComponent<RectTransform>().offsetMin += new Vector2(0f, -100f);
                                }
                            }
                            consolebutton.GetComponent<Animator>().Play("not");
                            consoletext.text = "Console::" + Environment.NewLine + "O :-  " + "Loaded TypeFields correctly for asset " + baseField.Get("m_Name").GetValue().AsString();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
            consolebutton.GetComponent<Animator>().Play("not");
            consoletext.text = "Console::" + Environment.NewLine + "O :-  " + ex.ToString();
        }
    }
    public void closeTypeFields()
    {
        try
        {
            typefieldcontainer.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
            typefieldcontainer.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
            typefieldviewer.transform.localScale = new Vector3(0, 0, 0);
            foreach (Transform child in typefieldcontainer.transform)
            {
                if (child.gameObject != endofline)
                    GameObject.Destroy(child.gameObject);
            }
        }
        catch(Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }
}
