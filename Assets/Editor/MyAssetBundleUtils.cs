using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class MyAssetBundleUtils : Editor
{
    [MenuItem("AssetTool/打包全部ab包")]
    static void SetAssetBundle()
    {
        string dir = "AssetBundle";
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        // 工程下所有的ab包都打包
        BuildPipeline.BuildAssetBundles(dir, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
    }
}
