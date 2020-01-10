using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Giant.Core
{
    public class DataManager : Single<DataManager>
    {
        private string xmlPath;
        private DepthMap<string, int, DataModel> DataList = new DepthMap<string, int, DataModel>();

        public void Init()
        {
            xmlPath = Path.Combine(Directory.GetCurrentDirectory(), "Xml");

            DataList.Clear();
            string[] files = Directory.GetFiles(xmlPath, "*.xml", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                LoadFile(file);
            }
        }

        public DataModel GetData(string name, int id)
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

        public Dictionary<int, DataModel> GetDatas(string name)
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
                throw new Exception($"Xml must have not exist please check, xml : {path}");
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            //获取根节点
            XmlElement root = doc.DocumentElement;

            string tableName = root.Attributes["Config"].Value;
            if (string.IsNullOrEmpty(tableName))
            {
                throw new Exception($"Xml must have correct name (attribute 'Config'), xml : {path}");
            }

            Dictionary<string, string> param;
            XmlNodeList nodes = root.ChildNodes;
            foreach (XmlNode node in nodes)
            {
                string idStr = node.Attributes["id"].Value;
                if (!int.TryParse(idStr, out int id))
                {
                    throw new Exception($"Xml must have id (attribute 'id'), xml : {path}");
                }

                param = ParesData(node.Attributes);
                AddToDataList(tableName, new DataModel() { Id = id, Params = param });
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

        private void AddToDataList(string name, DataModel data)
        {
            DataList.Add(name, data.Id, data);
        }
    }
}
