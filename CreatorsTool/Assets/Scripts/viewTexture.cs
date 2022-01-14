using AssetsTools.NET;
using AssetsTools.NET.Extra;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class viewTexture : MonoBehaviour
{
    public LoadBundle lb;
    public GameObject selectedassetinfo;
    public RawImage image;
    public GameObject textureviewer;
    public GameObject console;
    public GameObject consolebutton;
    public Text consoletext;


    public void viewTex2D()
    {
        try
        {
            Text type = selectedassetinfo.GetComponent<SelectionObject>().selectedAssetsFileInfo.transform.Find("TYPEText").GetComponent<Text>();
            Text name = selectedassetinfo.GetComponent<SelectionObject>().selectedAssetsFileInfo.transform.Find("NAMEText").GetComponent<Text>();
            var texid = AssetClassID.Texture2D;
            Debug.Log(name.text);
            Debug.Log(texid.ToString());
            Debug.Log(type.text);
            if(texid.ToString() == type.text.ToString())
            {
                foreach (var inf in lb.table.GetAssetsOfType((int)AssetClassID.Texture2D))
                {
                    var baseField = lb.am.GetTypeInstance(lb.inst, inf).GetBaseField();
                    if (baseField.Get("m_Name").GetValue().AsString() == name.text)
                    {
                        var atvf = lb.am.GetTypeInstance(lb.inst.file, inf).GetBaseField();
                        TextureFile tf = TextureFile.ReadTextureFile(atvf);

                        //bundle resS
                        TextureFile.StreamingInfo streamInfo = tf.m_StreamData;
                        if (streamInfo.path != null && streamInfo.path != "" && lb.inst.parentBundle != null)
                        {
                            //some versions apparently don't use archive:/
                            string searchPath = streamInfo.path;
                            if (searchPath.StartsWith("archive:/"))
                                searchPath = searchPath.Substring(9);

                            searchPath = Path.GetFileName(searchPath);

                            AssetBundleFile bundle = lb.inst.parentBundle.file;

                            AssetsFileReader reader = bundle.reader;
                            AssetBundleDirectoryInfo06[] dirInf = bundle.bundleInf6.dirInf;
                            for (int i = 0; i < dirInf.Length; i++)
                            {
                                AssetBundleDirectoryInfo06 info = dirInf[i];
                                if (info.name == searchPath)
                                {
                                    reader.Position = bundle.bundleHeader6.GetFileDataOffset() + info.offset + (long)streamInfo.offset;
                                    tf.pictureData = reader.ReadBytes((int)streamInfo.size);
                                    tf.m_StreamData.offset = 0;
                                    tf.m_StreamData.size = 0;
                                    tf.m_StreamData.path = "";
                                }
                            }
                        }

                        ///Debug.Log(texDat.ToString());
                        UnityEngine.Texture2D tex = new UnityEngine.Texture2D(tf.m_Width, tf.m_Height, (UnityEngine.TextureFormat)tf.m_TextureFormat, false);
                        tex.LoadRawTextureData(tf.pictureData);
                        tex.Apply();
                        image.texture = tex;
                        openTextureViewer();
                        consolebutton.GetComponent<Animator>().Play("not");
                        consoletext.text = "Console::" + Environment.NewLine + "O :-  " + "Loaded Texture";
                    }
                }
            }
            else
            {
                consolebutton.GetComponent<Animator>().Play("not");
                consoletext.text = "Console::" + Environment.NewLine + "O :-  " + "Not a Texture class type/id";
            }
        }   
        catch(Exception ex)
        {
            Debug.Log(ex.ToString());
            consolebutton.GetComponent<Animator>().Play("not");
            consoletext.text = "Console::" + Environment.NewLine + "O :-  " + ex.ToString();
        }
    }
    public void openTextureViewer()
    {
        textureviewer.transform.localScale = new Vector3(1, 1, 1);
    }
    public void closeTextureViewer()
    {
        textureviewer.transform.localScale = new Vector3(0, 0, 0);
    }
    public void consoleOpen()
    {
        console.transform.localScale = new Vector3(1, 1, 1);
    }
    public void consoleClose()
    {
        console.transform.localScale = new Vector3(0, 0, 0);
    }
}
