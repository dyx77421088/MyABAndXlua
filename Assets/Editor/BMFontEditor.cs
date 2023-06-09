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
        [MenuItem("Tools/�Զ����������ɹ���")]
        static void Init()
        {
            EditorWindow.GetWindow(typeof (BMFontEditor));
        }
        // layout window
        void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("ͼ�������ļ����֣�������������Դ�Ѿ�׼����ϣ��������һ���֣�", EditorStyles.label);
            if (GUILayout.Button("��Project��ͼѡ��Ҫ�����ļ���ͼ����Ȼ�����˰�ť"))
            {
                this.ProcessToSprite();
            }
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("��ʽ���������ֲ���", EditorStyles.label);
            fontTexture = (Texture) EditorGUILayout.ObjectField("ѡ��BMFont���ɵ�png�ļ�", fontTexture, typeof (Texture), false);
            EditorGUILayout.Space();EditorGUILayout.LabelField("ѡ��BMFont���ɵ�fnt�ļ�", EditorStyles.label);
            fontPosTbl = (TextAsset) EditorGUILayout.ObjectField("    ", fontPosTbl, typeof (TextAsset), false);
            if (GUILayout.Button("��ʼ��������"))
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
            Texture2D image = Selection.activeObject as Texture2D;//��ȡѡ��Ķ���
            string rootPath = System.IO.Path.GetDirectoryName(AssetDatabase.GetAssetPath(image));//��ȡ·������
            string path = rootPath + "/" + image.name + ".PNG"; //ͼƬ·������
            TextureImporter texImp = AssetImporter.GetAtPath(path) as TextureImporter;//��ȡͼƬ���
            AssetDatabase.CreateFolder(rootPath, image.name);//�����ļ���
            foreach (SpriteMetaData metaData in texImp.spritesheet)//����Сͼ��
            {
                Texture2D myimage = new Texture2D((int)metaData.rect.width, (int)metaData.rect.height);
                for (int y = (int)metaData.rect.y; y < metaData.rect.y + metaData.rect.height; y++)//Y������
                {
                    for (int x = (int)metaData.rect.x; x < metaData.rect.x + metaData.rect.width; x++)
                        myimage.SetPixel(x - (int)metaData.rect.x, y - (int)metaData.rect.y, image.GetPixel(x, y));
                }
                if(myimage.format != TextureFormat.ARGB32 && myimage.format != TextureFormat.RGB24)
                {
                    Texture2D newTexture = new Texture2D(myimage.width, myimage.height);
                    newTexture.SetPixels(myimage.GetPixels(0),0);
                    myimage = newTexture;
                }
                var pngData = myimage.EncodeToPNG();
                File.WriteAllBytes(rootPath + "/" + image.name + "/" + metaData.name + ".PNG", pngData);// ˢ����Դ���ڽ���
                AssetDatabase.Refresh();
            }
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
        }// get font table one line. �ص�����
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
