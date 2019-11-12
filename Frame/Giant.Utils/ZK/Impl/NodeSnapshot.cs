using org.apache.zookeeper;
using org.apache.zookeeper.data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Giant.Utils.ZK
{
    public struct NodeSnapshot
    {
        public bool IsExist { get; set; }
        public CreateMode Mode { get; set; }
        public IEnumerable<byte> Data { get; set; }
        public int? Version { get; set; }
        public List<ACL> Acls { get; set; }
        public IEnumerable<string> Childrens { get; set; }

        public void Create(CreateMode mode, byte[] data, List<ACL> acls)
        {
            IsExist = true;
            Mode = mode;
            Data = data;
            Version = -1;
            Acls = acls;
            Childrens = null;
        }

        public void Update(IEnumerable<byte> data, int version)
        {
            IsExist = true;
            Data = data;
            Version = version;
        }

        public void Delete()
        {
            IsExist = false;
            Mode = null;
            Data = null;
            Version = null;
            Acls = null;
            Childrens = null;
        }

        public void SetData(IEnumerable<byte> data)
        {
            IsExist = true;
            Data = data;
        }

        public void SetChildrens(IEnumerable<string> childrens)
        {
            IsExist = true;
            Childrens = childrens;
        }

        public void SetExists(bool exists)
        {
            if (!exists)
            {
                Delete();
                return;
            }
            IsExist = true;
        }
    }
}
