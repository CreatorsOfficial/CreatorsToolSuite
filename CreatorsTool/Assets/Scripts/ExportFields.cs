using AssetsTools.NET;
using AssetsTools.NET.Extra;
using SimpleFileBrowser;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class ExportFields : MonoBehaviour
{
    public LoadBundle lb;
    public GameObject selectedassetinfo;
    public GameObject console;
    public GameObject consolebutton;
    public Text consoletext;

    public void ExportTypeFields()
    {
        try
        {
            Text type = selectedassetinfo.GetComponent<SelectionObject>().selectedAssetsFileInfo.transform.Find("TYPEText").GetComponent<Text>();
            Text name = selectedassetinfo.GetComponent<SelectionObject>().selectedAssetsFileInfo.transform.Find("NAMEText").GetComponent<Text>();
            var spriteid = AssetClassID.Sprite;
            var meshid = AssetClassID.Mesh;
            var texid = AssetClassID.Texture2D;
            var audioid = AssetClassID.AudioClip;
            var goid = AssetClassID.GameObject;
            var textid = AssetClassID.TextAsset;
            Debug.Log(name.text);
            Debug.Log(type.text);
            if (spriteid.ToString() == type.text.ToString())
            {
                foreach (var inf in lb.table.GetAssetsOfType((int)AssetClassID.Sprite))
                {
                    AssetTypeValueField exportField = lb.am.GetTypeInstance(lb.inst, inf).GetBaseField();
                    byte[] exportedBytes = exportField.WriteToByteArray();
                    if (exportField.Get("m_Name").GetValue().AsString() == name.text)
                    {
                        StartCoroutine(writetobytes(exportedBytes));
                    }
                }
            }
            if (meshid.ToString() == type.text.ToString())
            {
                foreach (var inf in lb.table.GetAssetsOfType((int)AssetClassID.Mesh))
                {
                    AssetTypeValueField exportField = lb.am.GetTypeInstance(lb.inst, inf).GetBaseField();
                    byte[] exportedBytes = exportField.WriteToByteArray();
                    if (exportField.Get("m_Name").GetValue().AsString() == name.text)
                    {
                        StartCoroutine(writetobytes(exportedBytes));
                    }
                }
            }
            if (goid.ToString() == type.text.ToString())
            {
                foreach (var inf in lb.table.GetAssetsOfType((int)AssetClassID.GameObject))
                {
                    AssetTypeValueField exportField = lb.am.GetTypeInstance(lb.inst, inf).GetBaseField();
                    byte[] exportedBytes = exportField.WriteToByteArray();
                    if (exportField.Get("m_Name").GetValue().AsString() == name.text)
                    {
                        StartCoroutine(writetobytes(exportedBytes));
                    }
                }
            }
            if (texid.ToString() == type.text.ToString() || audioid.ToString() == type.text.ToString() || textid.ToString() == type.text.ToString())
            {
                consolebutton.GetComponent<Animator>().Play("not");
                consoletext.text = "Console::" + Environment.NewLine + "O :-  " + name.text.ToString() + " is not supported as the file is a " + type.text.ToString()+ " file..!";
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
            consolebutton.GetComponent<Animator>().Play("not");
            consoletext.text = "Console::" + Environment.NewLine + "O :-  " + ex.ToString();
        }
    }
    IEnumerator writetobytes(byte[] bytestoload)
    {
        yield return FileBrowser.WaitForSaveDialog(FileBrowser.PickMode.FilesAndFolders, true, null, "Untitled", "Save your file to specific Folders", "Save");
        try
        {
            string path = FileBrowser.browserPath + "/" + FileBrowser.fileName + ".CREATORSBIN";
            File.WriteAllBytes(path, bytestoload);

            consolebutton.GetComponent<Animator>().Play("not");
            consoletext.text = "Console::" + Environment.NewLine + "O :-  " + "Done extracting typefields for asset " + name.ToString();
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
            consolebutton.GetComponent<Animator>().Play("not");
            consoletext.text = "Console::" + Environment.NewLine + "O :-  " + ex.ToString();
        }
    }
}
