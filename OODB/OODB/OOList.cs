using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver.Builders;

namespace OODB
{
    public class OOList<T> : OOValueGroup
    {
        public void Add(T v)
        {
            _list.Add(v);

            SetChanged();
        }

        void SetChanged()
        {
            if(OODBInstance.__WriteRecord)
            _changed = true;
        } 

        public void Insert(int index, T v)
        {
            _list.Insert(index, v);
            SetChanged();
        }

        public void RemoveAt(int index)
        {

            _list.RemoveAt(index);
            SetChanged();
        }

        public void Remove(T v)
        {
            _list.Remove(v);
            SetChanged();
        }

        public void Clear()
        {
            _list.Clear();
            SetChanged();
        }

        public int Count { get { return _list.Count; } }

        public T this[int index] { 
            get { return _list[index]; } 
            set {   _list[index] = value; SetChanged();   } 
        }

        internal override void BuildUpdateCmd(string parentPath, UpdateBuilder updateBuilder)
        {
            if (!_changed) 
                return;

            updateBuilder = updateBuilder.Set(parentPath, ToBsonValue());
        }

        internal override BsonValue ToBsonValue()
        {
            BsonArray barray = new BsonArray();

            foreach (T curr in _list)
            {
                barray.Add(OODBValueType.ToBsonValue(curr));
            }
        
            return barray;
        }

        internal override void FromBsonValue(BsonValue v)
        {
            BsonArray barray = v as BsonArray;
            _list.Clear();

            for(int i=0;i<barray.Count;i++)
            {
                object rv = OODBValueType.FromBsonValue(typeof(T), barray[i]);
                _list.Add((T)rv);
            }
        }

        internal override void SetNoChanged()
        {
            _changed = false; 
        }

        public virtual bool Contains(T k)
        {
            return _list.Contains(k);
        }

        internal bool _changed = false;//list类型一但有变化需要全部更新
        List<T> _list = new List<T>(); 
    }
}
