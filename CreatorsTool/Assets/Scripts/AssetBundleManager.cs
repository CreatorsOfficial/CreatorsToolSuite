using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssetsTools.NET;
using AssetsTools.NET.Extra;
using SimpleFileBrowser;
using System;

    public class AssetBundleManager : MonoBehaviour
    {
    public static AssetBundleManager Instance { get; private set; }
        public AssetsManager am;
        public BundleFileInstance bun;
        public AssetBundleDirectoryInfo06[] dirinfo;
        public AssetsFileInstance inst;
        public AssetsFileTable table;
    }