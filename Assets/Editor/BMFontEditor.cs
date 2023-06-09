using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using System.IO;

namespace ETEditor
{
    public class BMFontEditor : EditorWindow
    {
        public TextAsset fontPosTbl;
        public Texture fontTexture; 
        public Vector2 scrollPos; 
        struct ChrRect 
        { 
            public int id; 
            public int x; 
            public int y; 
            public int w; 
            public int h; 
            public int xofs; 
            public int yofs; 
            public int index; 
            public float uvX;
            public float uvY; 
            public float uvW; 
            public float uvH; 
            public float vertX; 
            public float vertY; 
            public float vertW; 
            public float vertH; 
            public float width; 
        }
        // add menu
        [MenuItem("Tools/自定义字体生成工具")]
        static void Init()
        {
            EditorWindow.GetWindow(typeof (BMFontEditor));
        }
        // layout window
        void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("图集导出文件部分（如果字体相关资源已经准备完毕，请忽略这一部分）", EditorStyles.label);
            if (GUILayout.Button("在Project视图选中要导出文件的图集，然后点击此按钮"))
            {
                //this.ProcessToSprite();
                //this.ProcessToSpriteToGuDing();
                this.ProcessToSpritePdDian();
            }
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("正式生成艺术字部分", EditorStyles.label);
            fontTexture = (Texture) EditorGUILayout.ObjectField("选择BMFont生成的png文件", fontTexture, typeof (Texture), false);
            EditorGUILayout.Space();EditorGUILayout.LabelField("选择BMFont生成的fnt文件", EditorStyles.label);
            fontPosTbl = (TextAsset) EditorGUILayout.ObjectField("    ", fontPosTbl, typeof (TextAsset), false);
            if (GUILayout.Button("开始生成字体"))
            {
                if (fontTexture == null) this.ShowNotification(new GUIContent("No Font Texture selected"));
                else if (fontPosTbl == null) this.ShowNotification(new GUIContent("No Font Position Table file selected"));
                else
                {
                    CalcChrRect(fontPosTbl, fontTexture);
                }
            }
            EditorGUILayout.EndVertical();
        }

        private void ProcessToSprite()
        {
            int addSize = 5; // 给图片增添的外边距
            Texture2D image = Selection.activeObject as Texture2D;//获取选择的对象
            string rootPath = System.IO.Path.GetDirectoryName(AssetDatabase.GetAssetPath(image));//获取路径名称
            string path = rootPath + "/" + image.name + ".PNG"; //图片路径名称
            TextureImporter texImp = AssetImporter.GetAtPath(path) as TextureImporter;//获取图片入口
            AssetDatabase.CreateFolder(rootPath, image.name);//创建文件夹
            foreach (SpriteMetaData metaData in texImp.spritesheet)//遍历小图集
            {
                int addX = addSize, addY = addSize;
                if (metaData.name == "点") addY = 0;
                Texture2D myimage = new Texture2D((int)metaData.rect.width + addX * 2, (int)metaData.rect.height + addY * 2);
                for (int y = (int)metaData.rect.y - addY; y < metaData.rect.y + metaData.rect.height + addY; y++)//Y轴像素
                {
                    for (int x = (int)metaData.rect.x - addX; x < metaData.rect.x + metaData.rect.width + addX; x++)
                        myimage.SetPixel(x - (int)metaData.rect.x + addX, y - (int)metaData.rect.y + addY, image.GetPixel(x, y));
                }
                if(myimage.format != TextureFormat.ARGB32 && myimage.format != TextureFormat.RGB24)
                {
                    Texture2D newTexture = new Texture2D(myimage.width, myimage.height);
                    newTexture.SetPixels(myimage.GetPixels(0),0);
                    myimage = newTexture;
                }
                var pngData = myimage.EncodeToPNG();
                File.WriteAllBytes(rootPath + "/" + image.name + "/" + metaData.name + ".png", pngData);// 刷新资源窗口界面
                AssetDatabase.Refresh();
            }
        }
        /// <summary>
        /// 在 ProcessToSprite 的基础上判断是否基于最下方
        /// </summary>
        private void ProcessToSpritePdDian()
        {
            int addSize = 5; // 给图片增添的外边距
            Texture2D image = Selection.activeObject as Texture2D;//获取选择的对象
            string rootPath = System.IO.Path.GetDirectoryName(AssetDatabase.GetAssetPath(image));//获取路径名称
            string path = rootPath + "/" + image.name + ".PNG"; //图片路径名称
            TextureImporter texImp = AssetImporter.GetAtPath(path) as TextureImporter;//获取图片入口
            AssetDatabase.CreateFolder(rootPath, image.name);//创建文件夹

            float maxX = 0, maxY = 0;
            // 找最大的图片
            foreach (SpriteMetaData metaData in texImp.spritesheet)
            {
                if (maxX < metaData.rect.width) maxX = metaData.rect.width;
                if (maxY < metaData.rect.height) maxY = metaData.rect.height;
            }

            foreach (SpriteMetaData metaData in texImp.spritesheet)//遍历小图集
            {
                // 计算需要增加的宽和高
                int addX = addSize, addY = addSize;
                bool isDian = metaData.name == dian;
                if (isDian)
                {
                    addY = (int)(maxY - metaData.rect.height) / 2;
                }
                Texture2D myimage = new Texture2D((int)metaData.rect.width + addX * 2, (int)metaData.rect.height + addY * 2);
                for (int y = (int)metaData.rect.y - (isDian ? 0 : addY); y < metaData.rect.y + metaData.rect.height + (isDian ? addY * 2 : addY); y++)//Y轴像素
                {
                    for (int x = (int)metaData.rect.x - addX; x < metaData.rect.x + metaData.rect.width + addX; x++)
                        myimage.SetPixel(x - (int)metaData.rect.x + addX, y - (int)metaData.rect.y + (isDian ? 0 : addY), image.GetPixel(x, y));
                }
                if (myimage.format != TextureFormat.ARGB32 && myimage.format != TextureFormat.RGB24)
                {
                    Texture2D newTexture = new Texture2D(myimage.width, myimage.height);
                    newTexture.SetPixels(myimage.GetPixels(0), 0);
                    myimage = newTexture;
                }
                var pngData = myimage.EncodeToPNG();
                File.WriteAllBytes(rootPath + "/" + image.name + "/" + metaData.name + ".png", pngData);// 刷新资源窗口界面
                AssetDatabase.Refresh();
            }
        }
        private string dian = "点";
        /// <summary>
        /// 把导出的图片设定为固定宽高
        /// </summary>
        private void ProcessToSpriteToGuDing()
        {

            Texture2D image = Selection.activeObject as Texture2D;//获取选择的对象
            string rootPath = System.IO.Path.GetDirectoryName(AssetDatabase.GetAssetPath(image));//获取路径名称
            string path = rootPath + "/" + image.name + ".PNG"; //图片路径名称
            TextureImporter texImp = AssetImporter.GetAtPath(path) as TextureImporter;//获取图片入口
            AssetDatabase.CreateFolder(rootPath, image.name);//创建文件夹
            float maxX = 0, maxY = 0;
            // 找最大的图片
            foreach (SpriteMetaData metaData in texImp.spritesheet)
            {
                if (maxX < metaData.rect.width) maxX = metaData.rect.width;
                if (maxY < metaData.rect.height) maxY = metaData.rect.height;
            }
            //int addXTemp = GetXY((int)maxX), addYTemp = GetXY((int)maxY);
            int addXTemp = (int)maxX, addYTemp = (int)maxY;
            foreach (SpriteMetaData metaData in texImp.spritesheet)//遍历小图集
            {
                // 计算需要增加的宽和高
                int addX = (addXTemp - (int)metaData.rect.width) / 2, addY = (addYTemp - (int)metaData.rect.height) / 2;
                bool isDian = metaData.name == dian;
                Debug.Log("是否是点" + isDian);
                Texture2D myimage = new Texture2D((int)metaData.rect.width + addX * 2, (int)metaData.rect.height + addY * 2);
                for (int y = (int)metaData.rect.y - (isDian ? 0 : addY); y < metaData.rect.y + metaData.rect.height + (isDian ? addY * 2 : addY); y++)//Y轴像素
                {
                    for (int x = (int)metaData.rect.x - addX; x < metaData.rect.x + metaData.rect.width + addX; x++)
                        myimage.SetPixel(x - (int)metaData.rect.x + addX, y - (int)metaData.rect.y + (isDian ? 0 : addY), image.GetPixel(x, y));
                }
                if (myimage.format != TextureFormat.ARGB32 && myimage.format != TextureFormat.RGB24)
                {
                    Texture2D newTexture = new Texture2D(myimage.width, myimage.height);
                    newTexture.SetPixels(myimage.GetPixels(0), 0);
                    myimage = newTexture;
                }
                var pngData = myimage.EncodeToPNG();
                File.WriteAllBytes(rootPath + "/" + image.name + "/" + metaData.name + ".png", pngData);// 刷新资源窗口界面
                AssetDatabase.Refresh();
            }
        }

        /// <summary>
        /// 返回2的x倍数
        /// </summary>
        public int GetXY(int size)
        {
            int sum = 1;
            for (int i = 1; i < 20; ++i)
            {
                sum *= 2;
                if (sum >= size) return sum;
            }
            return 0;
        }

        void OnInspectorUpdate()
        {
            this.Repaint();
        }
        void CalcChrRect(TextAsset posTbl, Texture tex)
        {
            string fileName = AssetDatabase.GetAssetPath(fontPosTbl);
            string texName = AssetDatabase.GetAssetPath(tex);
            string fontName = System.IO.Path.GetFileNameWithoutExtension(fileName);
            string fontPath = fileName.Replace(".fnt", ".fontsettings");
            string matPath = fileName.Replace(".fnt", ".mat");
            float imgw = tex.width;
            float imgh = tex.height;
            string txt = posTbl.text;
            List<ChrRect> tblList = new List<ChrRect>();
            foreach (string line in txt.Split('\n'))
            {
                if (line.IndexOf("char id=") == 0)
                {
                    ChrRect d = GetChrRect(line, imgw, imgh);
                    tblList.Add(d);
                }
            }
            if (tblList.Count == 0)
            {
                new GUIContent("Failed");
                return;
            }
            ChrRect[] tbls = tblList.ToArray();
            Font font = new Font();
            font.name = fontName;
            SetCharacterInfo(tbls, font);
            Material mat = new Material(Shader.Find("UI/Default"));
            mat.mainTexture = tex;
            mat.name = fontName;
            font.material = mat;
            Debug.Log(System.IO.Path.GetFileNameWithoutExtension(fileName));
            Debug.Log(fileName);
            AssetDatabase.CreateAsset(mat, matPath);
            AssetDatabase.CreateAsset(font, fontPath);
            AssetDatabase.SaveAssets();
            this.ShowNotification(new GUIContent("Complete"));
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        }// over write custom font by new CharacterInfo
         void SetCharacterInfo(ChrRect[] tbls, Font fontObj)
        {
            CharacterInfo[] nci = new CharacterInfo[tbls.Length];
            for (int i = 0; i < tbls.Length; i++)
            {
                nci[i].index = tbls[i].index;
                nci[i].advance = (int) tbls[i].width;
                nci[i].uv.x = tbls[i].uvX;
                nci[i].uv.y = tbls[i].uvY;
                nci[i].uv.width = tbls[i].uvW;
                nci[i].uv.height = tbls[i].uvH;
                nci[i].vert.x = tbls[i].vertX;
                nci[i].vert.y = tbls[i].vertY;
                nci[i].vert.width = tbls[i].vertW;
                nci[i].vert.height = tbls[i].vertH;
            }
            fontObj.characterInfo = nci;
        }// get font table one line. 重点在这
        ChrRect GetChrRect(string line, float imgw, float imgh)
        {
            ChrRect d = new ChrRect();
            foreach (string s in line.Split(' '))
            {
                if (s.IndexOf("id=") >= 0) d.id = GetParamInt(s, "id=");
                else if (s.IndexOf("x=") >= 0) d.x = GetParamInt(s, "x=");
                else if (s.IndexOf("y=") >= 0) d.y = GetParamInt(s, "y=");
                else if (s.IndexOf("width=") >= 0) d.w = GetParamInt(s, "width=");
                else if (s.IndexOf("height=") >= 0) d.h = GetParamInt(s, "height=");
                else if (s.IndexOf("xoffset=") >= 0) d.xofs = GetParamInt(s, "xoffset=");
                else if (s.IndexOf("yoffset=") >= 0) d.yofs = GetParamInt(s, "yoffset=");
                else if (s.IndexOf("xadvance=") >= 0) d.width = GetParamInt(s, "xadvance=");
            }
            d.index = d.id;
            d.uvX = d.x / imgw;
            d.uvY = (imgh - (d.y)) / imgh;
            d.uvW = d.w / imgw;
            d.uvH = -d.h / imgh;
            d.uvH = d.h / imgh;
            d.vertX = d.xofs;
            d.vertY = -d.yofs;
            d.vertW = d.w;
            d.vertH = d.h;
            return d;
        }
        int GetParamInt(string s, string wd)
        {
            if (s.IndexOf(wd) >= 0)
            {
                int v;
                if (int.TryParse(s.Substring(wd.Length), out v)) return v;
            }
            return int.MaxValue;
        }
    //}
    }
}
