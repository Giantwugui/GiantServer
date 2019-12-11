using Giant.Core;
using Giant.Logger;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Giant.Framework
{
    public class DataComponent : InitSystem, ILoad
    {
        private string xmlPath;
        private DepthMap<string, int, Data> DataList = new DepthMap<string, int, Data>();

        private static DataComponent instance;
        public static DataComponent Instance => instance;

        public DataComponent()
        {
        }

        public override void Init()
        {
            instance = this;
            xmlPath = Path.Combine(Directory.GetCurrentDirectory(), "Xml");

            Load();
        }

        public void Load()
        {
            DataList.Clear();
            string[] files = Directory.GetFiles(xmlPath, "*.xml", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                LoadFile(file);
            }
        }

        public Data GetData(string name, int id)
        {
            if (DataList.TryGetValue(name, out var data))
            {
                if (data.TryGetValue(id, out var aimData))
                {
                    return aimData;
                }
            }
            return null; ;
        }

        public Dictionary<int, Data> GetDatas(string name)
        {
            if (DataList.TryGetValue(name, out var data))
            {
            }
            return data;
        }

        private void LoadFile(string path)
        {
            if (!File.Exists(path))
            {
                Log.Error($"Xml must have not exist please check, xml : {path}");
                return;
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            //获取根节点
            XmlElement root = doc.DocumentElement;

            string tableName = root.Attributes["Config"].Value;
            if (string.IsNullOrEmpty(tableName))
            {
                Log.Error($"Xml must have correct name (attribute 'Config'), xml : {path}");
                return;
            }

            Dictionary<string, string> param;
            XmlNodeList nodes = root.ChildNodes;
            foreach (XmlNode node in nodes)
            {
                string idStr = node.Attributes["id"].Value;
                if (!int.TryParse(idStr, out int id))
                {
                    Log.Error($"Xml must have id (attribute 'id'), xml : {path}");
                    continue;
                }

                param = ParesData(node.Attributes);
                AddToDataList(tableName, new Data() { Id = id, Params = param });
            }

        }

        private Dictionary<string, string> ParesData(XmlAttributeCollection xmlAttribute)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            foreach (XmlAttribute kv in xmlAttribute)
            {
                param[kv.Name] = kv.Value;
            }

            return param;
        }

        private void AddToDataList(string name, Data data)
        {
            DataList.Add(name, data.Id, data);
        }
    }
}
