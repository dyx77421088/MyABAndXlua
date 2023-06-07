using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestConfig
{
    public static string url = "http://localhost:3000/";

    /// <summary>
    /// 版本号的文件的路径
    /// </summary>
    public static string versionPath = "version";

    /// <summary>
    /// 版本号对应的json文件的路径
    /// </summary>
    public static string GetVersionJsonPath(string version)
    {
        Debug.Log("version" + "_" + version.Replace(".", "_") + "_json");
        return "version" + "_" + version.Replace(".", "_") + "_json";
    }
}
