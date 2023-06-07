using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileUtils
{
    /// <summary>
    /// 本地存放版本号的文件路径
    /// </summary>
    public static string versionPath = "version.txt";
    public static string GetTextByResources(string path)
    {
        return Resources.Load<TextAsset>(path).text;
    }

    /// <summary>
    /// 在文件下的目录，和Assets同级
    /// </summary>
    public static string GetTextByPath(string path)
    {
        using(FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read, FileShare.None))
        {
            using(StreamReader sr = new StreamReader(fs, System.Text.Encoding.Default))
            {
                if (null == sr) return null;
                string str = sr.ReadToEnd();
                return str;
            }
        }
    }

    public static void WriteTextByPath(string path, string text)
    {
        using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
        {
            using (StreamWriter sr = new StreamWriter(fs, System.Text.Encoding.Default))
            {
                if (null == sr) return;
                sr.Write(text);
            }
        }
    }
}
