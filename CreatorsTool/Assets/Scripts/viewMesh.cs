using AssetsTools.NET.Extra;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssetsTools.NET;
using UnityEngine.UI;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class viewMesh : MonoBehaviour
{
    public LoadBundle lb;
    public GameObject selectedassetinfo;
    public GameObject console;
    public GameObject consolebutton;
    public Text consoletext;
    
    public void MeshView()
    {
        try
        {
            Text type = selectedassetinfo.GetComponent<SelectionObject>().selectedAssetsFileInfo.transform.Find("TYPEText").GetComponent<Text>();
            Text name = selectedassetinfo.GetComponent<SelectionObject>().selectedAssetsFileInfo.transform.Find("NAMEText").GetComponent<Text>();
            var texid = AssetClassID.Mesh;
            Debug.Log(name.text);
            Debug.Log(texid.ToString());
            Debug.Log(type.text);
            if (texid.ToString() == type.text.ToString())
            {
                foreach (var inf in lb.table.GetAssetsOfType((int)AssetClassID.Mesh))
                {
                    var basebytes = File.ReadAllBytes("");
                    var field = ByteArrayToObject(basebytes);
                    AssetTypeValueField myfield = field;
                    var exportbytes = myfield.WriteToByteArray();
                    AssetTypeValueField baseField = lb.am.GetTypeInstance(lb.inst, inf).GetBaseField();
                    if (baseField.Get("m_Name").GetValue().AsString() == name.text)
                    {
                        File.WriteAllBytes("G:/toextract/basefield.txt", basebytes);
                    }
                }
            }
        }
        catch(Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }
    private AssetTypeValueField ByteArrayToObject(byte[] arrBytes)
    {
        MemoryStream memStream = new MemoryStream();
        BinaryFormatter binForm = new BinaryFormatter();
        memStream.Write(arrBytes, 0, arrBytes.Length);
        memStream.Seek(0, SeekOrigin.Begin);
        AssetTypeValueField basefield = (AssetTypeValueField)binForm.Deserialize(memStream);

        return basefield;
    }
}
