using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileUtils
{
    /// <summary>
    /// ���ش�Ű汾�ŵ��ļ�·��
    /// </summary>
    public static string versionPath = "version";
    public static string GetTextByResources(string path)
    {
        return Resources.Load<TextAsset>(path).text;
    }

    /// <summary>
    /// ���ļ��µ�Ŀ¼����Assetsͬ��
    /// </summary>
    public static string GetTextByPath(string path, string extension = ".txt")
    {
        using(FileStream fs = new FileStream(path + extension, FileMode.OpenOrCreate, FileAccess.Read, FileShare.None))
        {
            using(StreamReader sr = new StreamReader(fs, System.Text.Encoding.Default))
            {
                if (null == sr) return null;
                string str = sr.ReadToEnd();
                return str;
            }
        }
    }
    /// <summary>
    /// ��txtд��path·����Ӧ���ļ���
    /// </summary>
    /// <param name="path"></param>
    /// <param name="text"></param>
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

    /// <summary>
    /// ��ab��д��path·����Ӧ���ļ���
    /// </summary>
    /// <param name="path"></param>
    /// <param name="text"></param>
    public static void WriteABByPath(string path, AssetBundle ab)
    {
        //string filePath = Application.persistentDataPath + "/mybundle";
        //File.WriteAllBytes("", ab.get);
        //using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
        //{
        //    using (StreamWriter sr = new StreamWriter(fs, System.Text.Encoding.Default))
        //    {
        //        if (null == sr) return;
        //        sr.Write(text);
        //    }
        //}
    }
}
