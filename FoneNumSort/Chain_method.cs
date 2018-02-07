using System;
using System.Collections.Generic;

namespace FoneNumSort
{
    class Chain_Item
    {
        public string Key { get; private set; }
        public string Value { get; private set; }

        public Chain_Item(String key, String val)
        {
            Key = key;
            Value = val;
        }
        public override string ToString()
        {
            string str;

            str = Value;//str = Key + " " + Value;

            return str;
        }
    }
    class Chain_method
    {
        LinkedList<Chain_Item>[] Arr;
        public int Capacity;
        int Size;
        double Completeness;
        bool resizable = false;
        public long arrsize;
       

        public Chain_method(int size, double completeness, bool resiz)
        {
            Capacity = size;
            Completeness = completeness;
            resizable = resiz;
            arrsize = GC.GetTotalMemory(true);
            Arr = new LinkedList<Chain_Item>[Capacity];
            arrsize = GC.GetTotalMemory(true) - arrsize;


        }
        double get_Completeness()
        {
            return Size / Capacity;
        }
        int Hash(string key)
        {
            return Math.Abs(HashFAQ6(key)) % Capacity;
        }
        static int HashFAQ6(string str)
        {
            int hash = 0;

            for (int i = 0; i < str.Length; i++)
            {
                hash += (char)(str[i]);
                hash += (hash << 10);
                hash ^= (hash >> 6);
            }
            hash += (hash << 3);
            hash ^= (hash >> 11);
            hash += (hash << 15);

            return hash;

        }
        void Resize()
        {
            Capacity = Capacity * 2 + 1;

            var Old_Arr = Arr;

            Size = 0;
            Arr = null;
            Arr = new LinkedList<Chain_Item>[Capacity];

            foreach (var item in Old_Arr)
            {
                if (item != null)
                {
                    foreach (var node in item)
                    {

                        if (node != null)
                            this.Add(node.Value); // this.Add(node.Key, node.Value);
                    }
                }
            }
        }
        public void Add(String val) // public void Add(String key, String val)
        {
            if ((get_Completeness() >= Completeness) && (resizable == true))
            {
                Resize();
            }
            int index = Hash(val); // int index = Hash(key);
            if (Arr[index] == null)
            {
                Arr[index] = new LinkedList<Chain_Item>();
            }
            var ch_item = new Chain_Item(HashFAQ6(val).ToString(), val); // var ch_item = new Chain_Item(key, val);

            var Node = new LinkedListNode<Chain_Item>(ch_item);

            Arr[index].AddFirst(Node);

            Size++;
        }

        public Out_Of_Search Search(string val)
        {
            Out_Of_Search Out_Data = new Out_Of_Search();

            Out_Data.Check();

            int index = Hash(val);
            string key = Convert.ToString(Hash(val));

            if (Arr[index] == null)
            {
                Out_Data.Found = false;
                Out_Data.Complete = true;
                return Out_Data;
            }
            foreach (var item in Arr[index])
            {
                Out_Data.Steps++;
                if (item.Value == val)
                {
                    Out_Data.Found = true;
                    break;
                }
                else
                    Out_Data.Found = false;
            }
            Out_Data.Complete = true;
            return Out_Data;
        }

        public string Display()
        {
            int count = 0;
            string str = "";

            foreach (var item in Arr)
            {
                
                if (item != null)
                {
                    foreach (var node in item)
                    {
                        if (node != null)
                            str += node + "\n"; ;
                    }
                }
                count++;
            }
            return str;
        }
    }
}
