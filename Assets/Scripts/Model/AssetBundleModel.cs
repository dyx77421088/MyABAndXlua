using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    public class AssetBundleModel
    {
        private string name;
        private string path;

        public string Name { get => name; set => name = value; }
        public string Path { get => path; set => path = value; }
    }

}