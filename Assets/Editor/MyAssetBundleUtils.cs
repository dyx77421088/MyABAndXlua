using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class MyAssetBundleUtils : Editor
{
    [MenuItem("AssetTool/���ȫ��ab��")]
    static void SetAssetBundle()
    {
        string dir = "AssetBundle";
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        // ���������е�ab�������
        BuildPipeline.BuildAssetBundles(dir, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
    }
}
