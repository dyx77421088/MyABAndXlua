using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestConfig
{
    public static string url = "http://localhost:3000/";

    /// <summary>
    /// �汾�ŵ��ļ���·��
    /// </summary>
    public static string versionPath = "version";

    /// <summary>
    /// �汾�Ŷ�Ӧ��json�ļ���·��
    /// </summary>
    public static string GetVersionJsonPath(string version)
    {
        Debug.Log("version" + "_" + version.Replace(".", "_") + "_json");
        return "version" + "_" + version.Replace(".", "_") + "_json";
    }
}
