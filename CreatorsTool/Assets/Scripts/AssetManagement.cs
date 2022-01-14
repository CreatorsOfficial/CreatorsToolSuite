using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SimpleFileBrowser;
using AssetsTools;
using AssetsTools.NET.Extra;
using System;
using System.Text;
using AssetsTools.NET;
using System.Drawing;
using UnityEngine.UI;

public class AssetManagement : MonoBehaviour
{
    public RawImage image;
    public void loadFile()
    {
        try
        {
            StartCoroutine(Loaded());
        }
        catch(Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }
    IEnumerator Loaded()
    {
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders, true, null, null, "Load Files and Folders", "Load");
        try
        {
            var am = new AssetsManager();
            string path = FileBrowser.browserPath + "/" + FileBrowser.fileName;
            var bun = am.LoadBundleFile(path);
            var DirInfo = bun.file.bundleInf6.dirInf;
            foreach (var dirInf in DirInfo)
            {
                if (!dirInf.name.EndsWith(".resS") && !dirInf.name.EndsWith(".resource"))
                {
                    Debug.Log(dirInf.name);
                    var inst = am.LoadAssetsFileFromBundle(bun, dirInf.name);
                    if (!inst.file.typeTree.hasTypeTree)
                        am.LoadClassDatabaseFromPackage(inst.file.typeTree.unityVersion);
                    var table = inst.table;
                    Debug.Log(inst.parentBundle.name);
                    foreach (var info in table.assetFileInfo)
                    {
                        var baseField = am.GetTypeInstance(inst, info).GetBaseField();
                        if ((AssetClassID)info.curFileType == AssetClassID.Texture2D)
                        {
                            var m_Name = baseField.Get("m_Name").GetValue().AsString();
                            var m_type = info.curFileType;
                            var type_tex = (int)AssetClassID.Texture2D;
                            Debug.Log(m_Name);
                            Debug.Log((AssetClassID)m_type);
                            if (m_type == type_tex)
                            {
                                var atvf = am.GetTypeInstance(inst.file, info).GetBaseField();
                                TextureFile tf = TextureFile.ReadTextureFile(atvf);

                                //bundle resS
                                TextureFile.StreamingInfo streamInfo = tf.m_StreamData;
                                if (streamInfo.path != null && streamInfo.path.StartsWith("archive:/") && bun != null)
                                {
                                    Debug.Log("true");
                                    string searchPath = streamInfo.path.Substring(9);
                                    searchPath = Path.GetFileName(searchPath);

                                    AssetBundleFile bundle = bun.file;

                                    AssetsFileReader reader = bundle.reader;
                                    AssetBundleDirectoryInfo06[] dirInf2 = bundle.bundleInf6.dirInf;
                                    bool foundFile = false;
                                    for (int i = 0; i < dirInf2.Length; i++)
                                    {
                                        AssetBundleDirectoryInfo06 info2 = dirInf2[i];
                                        if (info2.name == searchPath)
                                        {
                                            reader.Position = bundle.bundleHeader6.GetFileDataOffset() + info2.offset + (long)streamInfo.offset;
                                            tf.pictureData = reader.ReadBytes((int)streamInfo.size);
                                            tf.m_StreamData.offset = 0;
                                            tf.m_StreamData.size = 0;
                                            tf.m_StreamData.path = "";
                                            foundFile = true;
                                            break;
                                        }
                                    }
                                    if (!foundFile)
                                    {
                                        Debug.Log("resS was detected but no file was found in bundle");
                                    }
                                }
                                Debug.Log((AssetsTools.NET.TextureFormat)tf.m_TextureFormat);
                                {
                                    byte[] bytearray = tf.GetTextureData(inst);
                                    Texture2D tex = new Texture2D(tf.m_Width, tf.m_Height, UnityEngine.TextureFormat.RGBA32, false);
                                    tex.LoadRawTextureData(bytearray);
                                    tex.Apply();
                                    image.texture = tex;
                                    File.WriteAllBytes("G:/toextract/hi.png", tex.EncodeToPNG());
                                }
                            }

                        }
                    }
                }
            }
            Debug.Log("done");
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }    
}
