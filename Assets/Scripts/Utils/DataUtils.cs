using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 数据的工具类
/// </summary>
public class DataUtils
{
    public static string GetDataSizeToMB(long bytes)
    {
        return (1.0 * bytes / 1024 / 1024).ToString("0.00") + "MB";
    }

    public static string GetDataSizeToKB(long bytes)
    {
        return (1.0 * bytes / 1024).ToString("0.00") + "KB";
    }

    public static string GetDataSize(long bytes)
    {
        if (bytes / 1024 / 1024 > 0)
        {
            return GetDataSizeToMB(bytes);
        }
        return GetDataSizeToKB(bytes);
    }
}
