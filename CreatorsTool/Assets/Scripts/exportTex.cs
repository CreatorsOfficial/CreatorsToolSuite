using AssetsTools.NET;
using AssetsTools.NET.Extra;
using SimpleFileBrowser;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class exportTex : MonoBehaviour
{
    public LoadBundle lb;
    public GameObject selectedassetinfo;
    public GameObject console;
    public GameObject consolebutton;
    public Text consoletext;
    public void exportTex2D()
    {
        try
        {
            Text type = selectedassetinfo.GetComponent<SelectionObject>().selectedAssetsFileInfo.transform.Find("TYPEText").GetComponent<Text>();
            Text name = selectedassetinfo.GetComponent<SelectionObject>().selectedAssetsFileInfo.transform.Find("NAMEText").GetComponent<Text>();
            var texid = AssetClassID.Texture2D;
            Debug.Log(name.text);
            Debug.Log(texid.ToString());
            Debug.Log(type.text);
            if (texid.ToString() == type.text.ToString())
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
                        Texture2D simpletex = new Texture2D(tex.width, tex.height);
                        simpletex.SetPixels(tex.GetPixels());
                        simpletex.Apply();
                        StartCoroutine(savetexturefile(simpletex));
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
    IEnumerator savetexturefile(Texture2D tex)
    {
        yield return FileBrowser.WaitForSaveDialog(FileBrowser.PickMode.FilesAndFolders, true, null, null, "Save Files and Folders", "Save");
        try
        {
            string path = FileBrowser.browserPath + "/" + FileBrowser.fileName + ".png";
            File.WriteAllBytes(path, tex.EncodeToPNG());

            consolebutton.GetComponent<Animator>().Play("not");
            consoletext.text = "Console::" + Environment.NewLine + "O :-  " + "Exported requested texture file successfully!";
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
            consolebutton.GetComponent<Animator>().Play("not");
            consoletext.text = "Console::" + Environment.NewLine + "O :-  " + ex.ToString();
        }
    }
}
