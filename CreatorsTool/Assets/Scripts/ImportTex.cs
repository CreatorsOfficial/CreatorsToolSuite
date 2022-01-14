using AssetsTools.NET.Extra;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssetsTools;
using AssetsTools.NET;
using UnityEngine.UI;
using SimpleFileBrowser;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class ImportTex : MonoBehaviour
{
    public LoadBundle lb;
    public GameObject selectedassetinfo;
    public GameObject console;
    public GameObject consolebutton;
    public Text consoletext;
    public InputField settingsTexturetypeEnum;

    public void importTex()
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
                foreach (AssetFileInfoEx inf in lb.table.GetAssetsOfType((int)AssetClassID.Texture2D))
                {
                    var baseField = lb.am.GetTypeInstance(lb.inst, inf).GetBaseField();
                    if (baseField.Get("m_Name").GetValue().AsString() == name.text)
                    {
                        AssetTypeValueField atvf = lb.am.GetTypeInstance(lb.inst.file, inf).GetBaseField();
                        StartCoroutine(LoadAndSet(atvf, inf));
                    }
                }
            }
        }
        catch(Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }
    IEnumerator LoadAndSet(AssetTypeValueField atvf , AssetFileInfoEx inf)
    {
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders, true, null, null, "Load Files and Folders", "Load");
        try
        {
            string path = FileBrowser.browserPath + "/" + FileBrowser.fileName;

            byte[] Bytes = File.ReadAllBytes(path);

            Texture2D loadedtex = new Texture2D(2,2);
            loadedtex.LoadImage(Bytes);
            loadedtex.Apply();

            UnityEngine.TextureFormat selectedformat = UnityEngine.TextureFormat.RGBA32;
            if (settingsTexturetypeEnum.text == 0.ToString())
            {
                selectedformat = UnityEngine.TextureFormat.RGBA32;
            }
            if (settingsTexturetypeEnum.text == 1.ToString())
            {
                selectedformat = UnityEngine.TextureFormat.RG16;
            }
            if (settingsTexturetypeEnum.text == 2.ToString())
            {
                selectedformat = UnityEngine.TextureFormat.RGB24;
            }
            if (settingsTexturetypeEnum.text == 3.ToString())
            {
                selectedformat = UnityEngine.TextureFormat.ARGB32;
            }
            if (settingsTexturetypeEnum.text == 4.ToString())
            {
                selectedformat = UnityEngine.TextureFormat.RGB565;
            }
            if (settingsTexturetypeEnum.text == 5.ToString())
            {
                selectedformat = UnityEngine.TextureFormat.ASTC_HDR_10x10;
            }
            if (settingsTexturetypeEnum.text == 6.ToString())
            {
                selectedformat = UnityEngine.TextureFormat.ETC2_RGB;
            }
            if (settingsTexturetypeEnum.text == 7.ToString())
            {
                selectedformat = UnityEngine.TextureFormat.ETC_RGB4;
            }
            if (settingsTexturetypeEnum.text == 8.ToString())
            {
                selectedformat = UnityEngine.TextureFormat.ETC2_RGBA8;
            }
            if (settingsTexturetypeEnum.text == 9.ToString())
            {
                selectedformat = UnityEngine.TextureFormat.ETC2_RGBA1;
            }

            Texture2D ARGBtex = new Texture2D(loadedtex.width, loadedtex.height, selectedformat, false);
            ARGBtex.SetPixels(loadedtex.GetPixels());
            ARGBtex.Apply();

            byte[] newbytes = ARGBtex.GetRawTextureData();

            atvf.Get("m_StreamData").Get("offset").GetValue().Set(0);
            atvf.Get("m_StreamData").Get("size").GetValue().Set(0);
            atvf.Get("m_StreamData").Get("path").GetValue().Set("");

            if (!atvf.Get("m_MipCount").IsDummy())
                atvf.Get("m_MipCount").GetValue().Set(1);

            AssetsTools.NET.TextureFormat modifiedformat = (AssetsTools.NET.TextureFormat)selectedformat;

            atvf.Get("m_TextureFormat").GetValue().Set((int)modifiedformat);
            atvf.Get("m_CompleteImageSize").GetValue().Set(newbytes.Length);

            atvf.Get("m_Width").GetValue().Set(ARGBtex.width);
            atvf.Get("m_Height").GetValue().Set(ARGBtex.height);


            atvf.Get("image data").GetValue().type = EnumValueTypes.ByteArray;
            atvf.Get("image data").templateField.valueType = EnumValueTypes.ByteArray;
            AssetTypeByteArray byteArray = new AssetTypeByteArray()
            {
                size = (uint)newbytes.Length,
                data = newbytes
            };
            atvf.Get("image data").GetValue().Set(byteArray);

            var newGoBytes = atvf.WriteToByteArray();
            var repl = new AssetsReplacerFromMemory(0, inf.index, (int)inf.curFileType, 0xffff, newGoBytes);

            //write changes to memory
            byte[] newAssetData;
            using (var stream = new MemoryStream())
            using (var writer = new AssetsFileWriter(stream))
            {
                lb.inst.file.Write(writer, 0, new List<AssetsReplacer>() { repl }, 0);
                newAssetData = stream.ToArray();
                stream.Close();
            }

            StartCoroutine(savemodified(newAssetData));
        }
        catch(Exception ex)
        {
            Debug.Log(ex.ToString());
            consolebutton.GetComponent<Animator>().Play("not");
            consoletext.text = "Console::" + Environment.NewLine + "O :-  " + ex.ToString();
        }
    }
    IEnumerator savemodified(byte[] newAssetData)
    {
        yield return FileBrowser.WaitForSaveDialog(FileBrowser.PickMode.FilesAndFolders, true, null, null, "Save Files and Folders", "Save");
        try
        {
            var bunRepl = new BundleReplacerFromMemory(lb.inst.name, lb.inst.name, true, newAssetData, -1);
            FileStream fs = new FileStream(FileBrowser.browserPath+"/"+FileBrowser.fileName, FileMode.Create);
            var bunWriter = new AssetsFileWriter(fs);
            lb.bun.file.Write(bunWriter, new List<BundleReplacer>() { bunRepl });
            File.Delete("G:/extract" + "/" + "temp.bun");
            fs.Close();

            consolebutton.GetComponent<Animator>().Play("not");
            consoletext.text = "Console::" + Environment.NewLine + "O :-  " + "Export Bundle with modified texture file you added successfully!!";
        }
        catch(Exception ex)
        {
            Debug.Log(ex.ToString());
            consolebutton.GetComponent<Animator>().Play("not");
            consoletext.text = "Console::" + Environment.NewLine + "O :-  " + ex.ToString();
        }
    }
}
