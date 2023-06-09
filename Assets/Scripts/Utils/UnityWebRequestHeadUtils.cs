using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UnityWebRequestHeadUtils
{
    public static long GetResponseHeaderLength(UnityWebRequest request)
    {
        long fileSize;
        long.TryParse(request.GetResponseHeader("Content-Length"), out fileSize);
        return fileSize;
    }
}
