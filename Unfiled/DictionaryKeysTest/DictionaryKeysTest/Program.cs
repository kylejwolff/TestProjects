using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DictionaryKeysTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var reg = new Dictionary<string, string>();
            var kc = new KVPtest();

            reg["Foo"] = "bar";
            //kc["Foo"].Value = "bar";

            kc.Add(new MyKVP { Key = "Foo", Value = "fubar" });

            kc.Add(new MyKVP { Key = "Foo", Value = "fubar" });
        }
    }
    class KVPtest : KeyedCollection<string, MyKVP>
    {
        protected override string GetKeyForItem(MyKVP item)
        {
            return item.Value;
        }
    }

    class MyKVP { public string Key { get; set; } public string Value { get; set; } }
}
