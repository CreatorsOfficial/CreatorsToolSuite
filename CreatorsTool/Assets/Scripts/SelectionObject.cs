using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionObject : MonoBehaviour
{
    public static SelectionObject Instance { get; private set; }
    public GameObject selectedAssetsFileInfo;
}
