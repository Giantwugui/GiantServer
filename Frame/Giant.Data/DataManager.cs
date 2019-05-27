using Giant.Log;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Giant.Data
{
    public class DataManager
    {
        private readonly string xmlPath;
        private readonly Dictionary<string, Dictionary<int, Data>> DataList;

        public static DataManager Instance { get; } = new DataManager();

        private DataManager()
        {
            xmlPath = $"{Directory.GetCurrentDirectory()}\\Xml";
            DataList = new Dictionary<string, Dictionary<int, Data>>();
        }

        public void LoadData()
        {
            string[] files = Directory.GetFiles(this.xmlPath, "*.xml", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                this.LoadFile(file);
            }
        }

        public Data GetData(string name, int id)
        {
            if (this.DataList.TryGetValue(name, out var data))
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
            if (this.DataList.TryGetValue(name, out var data))
            {
            }
            return data;
        }

        private void LoadFile(string path)
        {
            if (!File.Exists(path))
            {
                Logger.Error($"Xml must have not exist please check, xml : {path}");
                return;
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            //获取根节点
            XmlElement root = doc.DocumentElement;

            string tableName = root.Attributes["Config"].Value;
            if (string.IsNullOrEmpty(tableName))
            {
                Logger.Error($"Xml must have correct name (attribute 'Config'), xml : {path}");
                return;
            }

            Dictionary<string, string> param;
            XmlNodeList nodes = root.ChildNodes;
            foreach (XmlNode node in nodes)
            {
                string idStr = node.Attributes["id"].Value;
                if (!int.TryParse(idStr, out int id))
                {
                    Logger.Error($"Xml must have id (attribute 'id'), xml : {path}");
                    continue;
                }

                param = this.ParesData(node.Attributes);
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
            if (!this.DataList.TryGetValue(name, out var dataList))
            {
                dataList = new Dictionary<int, Data>();
                this.DataList.Add(name, dataList);
            }

            if (dataList.ContainsKey(data.Id))
            {
                Logger.Warn($"Repeated id in xml :{name} id {data.Id}");
                return;
            }

            this.DataList[name].Add(data.Id, data);
        }
    }
}
