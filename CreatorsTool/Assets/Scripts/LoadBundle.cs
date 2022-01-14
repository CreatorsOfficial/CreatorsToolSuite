using AssetsTools.NET;
using AssetsTools.NET.Extra;
using SimpleFileBrowser;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadBundle : MonoBehaviour
{
    public AssetsFileTable table;
    public AssetsFileInstance inst;
    public AssetsManager am;
    public BundleFileInstance bun;
    public AssetBundleDirectoryInfo06[] dirinfo;
    public string drinfname;

    public GameObject NAMETYPECLASsIDSIZE;
    public GameObject datacontainer;
    public GameObject Endofline;
    public Text totalassetcount;
    public Text parrent;
    public GameObject consolebutton;
    public Text consoletext;

    int i = 0;

    public void Load()
    {
        try
        {
            i = 0;
            datacontainer.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
            datacontainer.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
            foreach (Transform child in datacontainer.transform)
            {
                if(child.gameObject != Endofline)
                GameObject.Destroy(child.gameObject);
            }
            StartCoroutine(loaded());
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }

    IEnumerator loaded()
    {
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders, true, null, null, "Load Files and Folders", "Load");
        try
        {
            am = new AssetsManager();
            string path = FileBrowser.browserPath + "/" + FileBrowser.fileName;
            bun = am.LoadBundleFile(path);
            dirinfo = bun.file.bundleInf6.dirInf;
            foreach (var dirInf in dirinfo)
            {
                if (!dirInf.name.EndsWith(".resS") && !dirInf.name.EndsWith(".resource"))
                {
                    inst = am.LoadAssetsFileFromBundle(bun, dirInf.name);
                    drinfname = dirInf.name.ToString();
                    if (!inst.file.typeTree.hasTypeTree)
                        am.LoadClassDatabaseFromPackage(inst.file.typeTree.unityVersion);
                    table = inst.table;
                    var count = table.assetFileInfoCount;
                    totalassetcount.text = count.ToString();
                    parrent.text = bun.name.ToString();
                    foreach (var inf in table.GetAssetsOfType((int)AssetClassID.Texture2D))
                    {
                        i = i + 15;
                        var baseField = am.GetTypeInstance(inst, inf).GetBaseField();
                        if ((AssetClassID)inf.curFileType == AssetClassID.Texture2D)
                        {
                            if (datacontainer.transform.childCount == 1)
                            {
                                var type = (AssetClassID)inf.curFileType;
                                var m_Name = baseField.Get("m_Name").GetValue().AsString();
                                Debug.Log(m_Name.ToString());
                                GameObject TextureObject = Instantiate(NAMETYPECLASsIDSIZE) as GameObject;
                                TextureObject.transform.SetParent(datacontainer.transform);
                                var atvf = am.GetTypeInstance(inst.file, inf).GetBaseField();
                                TextureFile tf = TextureFile.ReadTextureFile(atvf);
                                var classid = (AssetsTools.NET.TextureFormat)tf.m_TextureFormat;
                                var size = inf.curFileSize;
                                TextureObject.transform.Find("NAMEText").GetComponent<Text>().text = m_Name.ToString();
                                TextureObject.transform.Find("TYPEText").GetComponent<Text>().text = type.ToString();
                                TextureObject.transform.Find("CLASSIDText").GetComponent<Text>().text = classid.ToString();
                                TextureObject.transform.Find("SIZEText").GetComponent<Text>().text = size.ToString();
                                datacontainer.GetComponent<RectTransform>().offsetMin += new Vector2(0f, -150f);
                            }
                            else
                            {
                                var type = (AssetClassID)inf.curFileType;
                                var m_Name = baseField.Get("m_Name").GetValue().AsString();
                                Debug.Log(m_Name);
                                GameObject TextureObject = Instantiate(NAMETYPECLASsIDSIZE) as GameObject;
                                TextureObject.transform.position = new Vector3(TextureObject.transform.position.x , TextureObject.transform.position.y - i + 15, TextureObject.transform.position.z);
                                TextureObject.transform.SetParent(datacontainer.transform);
                                var atvf = am.GetTypeInstance(inst.file, inf).GetBaseField();
                                TextureFile tf = TextureFile.ReadTextureFile(atvf);
                                var classid = (AssetsTools.NET.TextureFormat)tf.m_TextureFormat;
                                var size = inf.curFileSize;
                                TextureObject.transform.Find("NAMEText").GetComponent<Text>().text = m_Name.ToString();
                                TextureObject.transform.Find("TYPEText").GetComponent<Text>().text = type.ToString();
                                TextureObject.transform.Find("CLASSIDText").GetComponent<Text>().text = classid.ToString();
                                TextureObject.transform.Find("SIZEText").GetComponent<Text>().text = size.ToString();
                                datacontainer.GetComponent<RectTransform>().offsetMin += new Vector2(0f, -150f);
                            }
                        }
                    }
                    foreach (var inf in table.GetAssetsOfType((int)AssetClassID.TextAsset))
                    {
                        i = i + 15;
                        var baseField = am.GetTypeInstance(inst, inf).GetBaseField();
                        if ((AssetClassID)inf.curFileType == AssetClassID.TextAsset)
                        {
                            if (datacontainer.transform.childCount == 1)
                            {
                                var type = (AssetClassID)inf.curFileType;
                                var m_Name = baseField.Get("m_Name").GetValue().AsString();
                                Debug.Log(m_Name.ToString());
                                GameObject TextObject = Instantiate(NAMETYPECLASsIDSIZE) as GameObject;
                                TextObject.transform.SetParent(datacontainer.transform);
                                var classid = inf.curFileTypeOrIndex;
                                var size = inf.curFileSize;
                                TextObject.transform.Find("NAMEText").GetComponent<Text>().text = m_Name.ToString();
                                TextObject.transform.Find("TYPEText").GetComponent<Text>().text = type.ToString();
                                TextObject.transform.Find("CLASSIDText").GetComponent<Text>().text = classid.ToString();
                                TextObject.transform.Find("SIZEText").GetComponent<Text>().text = size.ToString();
                                datacontainer.GetComponent<RectTransform>().offsetMin += new Vector2(0f, -150f);
                            }
                            else
                            {
                                var type = (AssetClassID)inf.curFileType;
                                var m_Name = baseField.Get("m_Name").GetValue().AsString();
                                Debug.Log(m_Name);
                                GameObject TextObject = Instantiate(NAMETYPECLASsIDSIZE) as GameObject;
                                TextObject.transform.position = new Vector3(TextObject.transform.position.x, TextObject.transform.position.y - i + 15, TextObject.transform.position.z);
                                TextObject.transform.SetParent(datacontainer.transform);
                                var classid = inf.curFileTypeOrIndex;
                                var size = inf.curFileSize;
                                TextObject.transform.Find("NAMEText").GetComponent<Text>().text = m_Name.ToString();
                                TextObject.transform.Find("TYPEText").GetComponent<Text>().text = type.ToString();
                                TextObject.transform.Find("CLASSIDText").GetComponent<Text>().text = classid.ToString();
                                TextObject.transform.Find("SIZEText").GetComponent<Text>().text = size.ToString();
                                datacontainer.GetComponent<RectTransform>().offsetMin += new Vector2(0f, -150f);
                            }
                        }
                    }
                    foreach (var inf in table.GetAssetsOfType((int)AssetClassID.Mesh))
                    {
                        i = i + 15;
                        var baseField = am.GetTypeInstance(inst, inf).GetBaseField();
                        if ((AssetClassID)inf.curFileType == AssetClassID.Mesh)
                        {
                            if (datacontainer.transform.childCount == 1)
                            {
                                var type = (AssetClassID)inf.curFileType;
                                var m_Name = baseField.Get("m_Name").GetValue().AsString();
                                Debug.Log(m_Name.ToString());
                                GameObject MeshObject = Instantiate(NAMETYPECLASsIDSIZE) as GameObject;
                                MeshObject.transform.SetParent(datacontainer.transform);
                                var classid = inf.curFileTypeOrIndex;
                                var size = inf.curFileSize;
                                MeshObject.transform.Find("NAMEText").GetComponent<Text>().text = m_Name.ToString();
                                MeshObject.transform.Find("TYPEText").GetComponent<Text>().text = type.ToString();
                                MeshObject.transform.Find("CLASSIDText").GetComponent<Text>().text = classid.ToString();
                                MeshObject.transform.Find("SIZEText").GetComponent<Text>().text = size.ToString();
                                datacontainer.GetComponent<RectTransform>().offsetMin += new Vector2(0f, -150f);
                            }
                            else
                            {
                                var type = (AssetClassID)inf.curFileType;
                                var m_Name = baseField.Get("m_Name").GetValue().AsString();
                                Debug.Log(m_Name);
                                GameObject MeshObject = Instantiate(NAMETYPECLASsIDSIZE) as GameObject;
                                MeshObject.transform.position = new Vector3(MeshObject.transform.position.x, MeshObject.transform.position.y - i + 15, MeshObject.transform.position.z);
                                MeshObject.transform.SetParent(datacontainer.transform);
                                var classid = inf.curFileTypeOrIndex;
                                var size = inf.curFileSize;
                                MeshObject.transform.Find("NAMEText").GetComponent<Text>().text = m_Name.ToString();
                                MeshObject.transform.Find("TYPEText").GetComponent<Text>().text = type.ToString();
                                MeshObject.transform.Find("CLASSIDText").GetComponent<Text>().text = classid.ToString();
                                MeshObject.transform.Find("SIZEText").GetComponent<Text>().text = size.ToString();
                                datacontainer.GetComponent<RectTransform>().offsetMin += new Vector2(0f, -150f);
                            }
                        }
                    }
                    foreach (var inf in table.GetAssetsOfType((int)AssetClassID.Sprite))
                    {
                        i = i + 15;
                        var baseField = am.GetTypeInstance(inst, inf).GetBaseField();
                        if ((AssetClassID)inf.curFileType == AssetClassID.Sprite)
                        {
                            if (datacontainer.transform.childCount == 1)
                            {
                                var type = (AssetClassID)inf.curFileType;
                                var m_Name = baseField.Get("m_Name").GetValue().AsString();
                                Debug.Log(m_Name.ToString());
                                GameObject SpriteObject = Instantiate(NAMETYPECLASsIDSIZE) as GameObject;
                                SpriteObject.transform.SetParent(datacontainer.transform);
                                var classid = inf.curFileTypeOrIndex;
                                var size = inf.curFileSize;
                                SpriteObject.transform.Find("NAMEText").GetComponent<Text>().text = m_Name.ToString();
                                SpriteObject.transform.Find("TYPEText").GetComponent<Text>().text = type.ToString();
                                SpriteObject.transform.Find("CLASSIDText").GetComponent<Text>().text = classid.ToString();
                                SpriteObject.transform.Find("SIZEText").GetComponent<Text>().text = size.ToString();
                                datacontainer.GetComponent<RectTransform>().offsetMin += new Vector2(0f, -150f);
                            }
                            else
                            {
                                var type = (AssetClassID)inf.curFileType;
                                var m_Name = baseField.Get("m_Name").GetValue().AsString();
                                Debug.Log(m_Name);
                                GameObject SpriteObject = Instantiate(NAMETYPECLASsIDSIZE) as GameObject;
                                SpriteObject.transform.position = new Vector3(SpriteObject.transform.position.x, SpriteObject.transform.position.y - i + 15, SpriteObject.transform.position.z);
                                SpriteObject.transform.SetParent(datacontainer.transform);
                                var classid = inf.curFileTypeOrIndex;
                                var size = inf.curFileSize;
                                SpriteObject.transform.Find("NAMEText").GetComponent<Text>().text = m_Name.ToString();
                                SpriteObject.transform.Find("TYPEText").GetComponent<Text>().text = type.ToString();
                                SpriteObject.transform.Find("CLASSIDText").GetComponent<Text>().text = classid.ToString();
                                SpriteObject.transform.Find("SIZEText").GetComponent<Text>().text = size.ToString();
                                datacontainer.GetComponent<RectTransform>().offsetMin += new Vector2(0f, -150f);
                            }
                        }
                    }
                    foreach (var inf in table.GetAssetsOfType((int)AssetClassID.AudioClip))
                    {
                        i = i + 15;
                        var baseField = am.GetTypeInstance(inst, inf).GetBaseField();
                        if ((AssetClassID)inf.curFileType == AssetClassID.AudioClip)
                        {
                            if (datacontainer.transform.childCount == 1)
                            {
                                var type = (AssetClassID)inf.curFileType;
                                var m_Name = baseField.Get("m_Name").GetValue().AsString();
                                Debug.Log(m_Name.ToString());
                                GameObject AudioObject = Instantiate(NAMETYPECLASsIDSIZE) as GameObject;
                                AudioObject.transform.SetParent(datacontainer.transform);
                                var classid = inf.curFileTypeOrIndex;
                                var size = inf.curFileSize;
                                AudioObject.transform.Find("NAMEText").GetComponent<Text>().text = m_Name.ToString();
                                AudioObject.transform.Find("TYPEText").GetComponent<Text>().text = type.ToString();
                                AudioObject.transform.Find("CLASSIDText").GetComponent<Text>().text = classid.ToString();
                                AudioObject.transform.Find("SIZEText").GetComponent<Text>().text = size.ToString();
                                datacontainer.GetComponent<RectTransform>().offsetMin += new Vector2(0f, -150f);
                            }
                            else
                            {
                                var type = (AssetClassID)inf.curFileType;
                                var m_Name = baseField.Get("m_Name").GetValue().AsString();
                                Debug.Log(m_Name);
                                GameObject AudioObject = Instantiate(NAMETYPECLASsIDSIZE) as GameObject;
                                AudioObject.transform.position = new Vector3(AudioObject.transform.position.x, AudioObject.transform.position.y - i + 15, AudioObject.transform.position.z);
                                AudioObject.transform.SetParent(datacontainer.transform);
                                var classid = inf.curFileTypeOrIndex;
                                var size = inf.curFileSize;
                                AudioObject.transform.Find("NAMEText").GetComponent<Text>().text = m_Name.ToString();
                                AudioObject.transform.Find("TYPEText").GetComponent<Text>().text = type.ToString();
                                AudioObject.transform.Find("CLASSIDText").GetComponent<Text>().text = classid.ToString();
                                AudioObject.transform.Find("SIZEText").GetComponent<Text>().text = size.ToString();
                                datacontainer.GetComponent<RectTransform>().offsetMin += new Vector2(0f, -150f);
                            }
                        }
                    }
                    foreach (var inf in table.GetAssetsOfType((int)AssetClassID.GameObject))
                    {
                        i = i + 15;
                        var baseField = am.GetTypeInstance(inst, inf).GetBaseField();
                        if ((AssetClassID)inf.curFileType == AssetClassID.GameObject)
                        {
                            if (datacontainer.transform.childCount == 1)
                            {
                                var type = (AssetClassID)inf.curFileType;
                                var m_Name = baseField.Get("m_Name").GetValue().AsString();
                                Debug.Log(m_Name.ToString());
                                GameObject go = Instantiate(NAMETYPECLASsIDSIZE) as GameObject;
                                go.transform.SetParent(datacontainer.transform);
                                var classid = inf.curFileTypeOrIndex;
                                var size = inf.curFileSize;
                                go.transform.Find("NAMEText").GetComponent<Text>().text = m_Name.ToString();
                                go.transform.Find("TYPEText").GetComponent<Text>().text = type.ToString();
                                go.transform.Find("CLASSIDText").GetComponent<Text>().text = classid.ToString();
                                go.transform.Find("SIZEText").GetComponent<Text>().text = size.ToString();
                                datacontainer.GetComponent<RectTransform>().offsetMin += new Vector2(0f, -150f);
                            }
                            else
                            {
                                var type = (AssetClassID)inf.curFileType;
                                var m_Name = baseField.Get("m_Name").GetValue().AsString();
                                Debug.Log(m_Name);
                                GameObject go = Instantiate(NAMETYPECLASsIDSIZE) as GameObject;
                                go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y - i + 15, go.transform.position.z);
                                go.transform.SetParent(datacontainer.transform);
                                var classid = inf.curFileTypeOrIndex;
                                var size = inf.curFileSize;
                                go.transform.Find("NAMEText").GetComponent<Text>().text = m_Name.ToString();
                                go.transform.Find("TYPEText").GetComponent<Text>().text = type.ToString();
                                go.transform.Find("CLASSIDText").GetComponent<Text>().text = classid.ToString();
                                go.transform.Find("SIZEText").GetComponent<Text>().text = size.ToString();
                                datacontainer.GetComponent<RectTransform>().offsetMin += new Vector2(0f, -150f);
                            }
                        }
                    }
                }
            }
            consolebutton.GetComponent<Animator>().Play("not");
            consoletext.text = "Console::" + Environment.NewLine + "O :-  " + "Done Loading Bundle!";
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
            consolebutton.GetComponent<Animator>().Play("not");
            consoletext.text = "Console::" + Environment.NewLine + "O :-  " + ex.ToString();
        }
    }
}
