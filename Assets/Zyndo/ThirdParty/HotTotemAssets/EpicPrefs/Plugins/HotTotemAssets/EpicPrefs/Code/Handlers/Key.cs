using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace HotTotem
{
    public static class Key
    {
        public static string getKey()
        {
            string key = "";
            string guid2 = "[keyHolder]";
            var builder2 = new StringBuilder();
            HashAlgorithm sha = new SHA1CryptoServiceProvider();
            BinaryFormatter _bf = new BinaryFormatter();
            foreach (Byte hashed in sha.ComputeHash(Encoding.Default.GetBytes(guid2)))
                builder2.AppendFormat("{0:x2}", hashed);
            if (System.IO.Directory.Exists(Application.persistentDataPath + "/HotTotem/EpicPrefs/Settings/DataA/" + builder2.ToString()))
            {
                int count = System.IO.Directory.GetFiles(Application.persistentDataPath + "/HotTotem/EpicPrefs/Settings/DataA/" + builder2.ToString()).Length;
                for (int i = 0; i < count; i++)
                {
                    var builder3 = new StringBuilder();
                    foreach (Byte hashed in sha.ComputeHash(Encoding.Default.GetBytes(i.ToString())))
                        builder3.AppendFormat("{0:x2}", hashed);
                    var _file = File.Open(Application.persistentDataPath + "/HotTotem/EpicPrefs/Settings/DataA/" + builder2.ToString() + "/" + builder3.ToString(), FileMode.Open);
                    key += (string)_bf.Deserialize(_file);
                    _file.Close();
                }
            }
            return key;
        }
        public static void setKey(string value)
        {
            System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/HotTotem/");
            System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/HotTotem/EpicPrefs/");
            System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/HotTotem/EpicPrefs/Settings/");
            if (Directory.Exists(Application.persistentDataPath + "/HotTotem/EpicPrefs/Settings/DataA/"))
                Directory.Delete(Application.persistentDataPath + "/HotTotem/EpicPrefs/Settings/DataA/", true);
            System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/HotTotem/EpicPrefs/Settings/DataA/");
            HashAlgorithm sha = new SHA1CryptoServiceProvider();
            BinaryFormatter _bf = new BinaryFormatter();
            for (int i = 0; i < 5; i++)
            {
                string guid = "[keyHolder]" + Guid.NewGuid().ToString();
                var builder = new StringBuilder();
                foreach (Byte hashed in sha.ComputeHash(Encoding.Default.GetBytes(guid)))
                    builder.AppendFormat("{0:x2}", hashed);
                System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/HotTotem/EpicPrefs/Settings/DataA/" + builder.ToString());
                int size = UnityEngine.Random.Range(0, 25);
                for (int k = 0; k < size; k++)
                {
                    guid = "[key" + k.ToString() + "]" + Guid.NewGuid().ToString();
                    var _file = File.Create(Application.persistentDataPath + "/HotTotem/EpicPrefs/Settings/DataA/" + builder.ToString() + "/" + guid);
                    string nB = UnityEngine.Random.Range(0, 10).ToString();
                    _bf.Serialize(_file, nB);
                    _file.Close();
                }
            }
            string guid2 = "[keyHolder]";
            var builder2 = new StringBuilder();
            foreach (Byte hashed in sha.ComputeHash(Encoding.Default.GetBytes(guid2)))
                builder2.AppendFormat("{0:x2}", hashed);
            System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/HotTotem/EpicPrefs/Settings/DataA/" + builder2.ToString());
            for (int i = 0; i < value.Length; i++)
            {
                var builder3 = new StringBuilder();
                foreach (Byte hashed in sha.ComputeHash(Encoding.Default.GetBytes(i.ToString())))
                    builder3.AppendFormat("{0:x2}", hashed);
                var _file = File.Create(Application.persistentDataPath + "/HotTotem/EpicPrefs/Settings/DataA/" + builder2.ToString() + "/" + builder3.ToString());
                string nB = value[i].ToString();
                _bf.Serialize(_file, nB);
                _file.Close();
            }
        }
        public static string getVector()
        {
            string key = "";
            string guid2 = "[keyHolder]";
            var builder2 = new StringBuilder();
            HashAlgorithm sha = new SHA1CryptoServiceProvider();
            BinaryFormatter _bf = new BinaryFormatter();
            foreach (Byte hashed in sha.ComputeHash(Encoding.Default.GetBytes(guid2)))
                builder2.AppendFormat("{0:x2}", hashed);
            if (System.IO.Directory.Exists(Application.persistentDataPath + "/HotTotem/EpicPrefs/Settings/DataB/" + builder2.ToString()))
            {
                int count = System.IO.Directory.GetFiles(Application.persistentDataPath + "/HotTotem/EpicPrefs/Settings/DataB/" + builder2.ToString()).Length;
                for (int i = 0; i < count; i++)
                {
                    var builder3 = new StringBuilder();
                    foreach (Byte hashed in sha.ComputeHash(Encoding.Default.GetBytes(i.ToString())))
                        builder3.AppendFormat("{0:x2}", hashed);
                    var _file = File.Open(Application.persistentDataPath + "/HotTotem/EpicPrefs/Settings/DataB/" + builder2.ToString() + "/" + builder3.ToString(), FileMode.Open);
                    key += (string)_bf.Deserialize(_file);
                    _file.Close();
                }
            }
            return key;
        }
        public static void setVector(string value)
        {
            System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/HotTotem/");
            System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/HotTotem/EpicPrefs/");
            System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/HotTotem/EpicPrefs/Settings/");
            if (Directory.Exists(Application.persistentDataPath + "/HotTotem/EpicPrefs/Settings/DataB/"))
                Directory.Delete(Application.persistentDataPath + "/HotTotem/EpicPrefs/Settings/DataB/", true);
            System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/HotTotem/EpicPrefs/Settings/DataB/");
            HashAlgorithm sha = new SHA1CryptoServiceProvider();
            BinaryFormatter _bf = new BinaryFormatter();
            for (int i = 0; i < 5; i++)
            {
                string guid = "[keyHolder]" + Guid.NewGuid().ToString();
                var builder = new StringBuilder();
                foreach (Byte hashed in sha.ComputeHash(Encoding.Default.GetBytes(guid)))
                    builder.AppendFormat("{0:x2}", hashed);
                System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/HotTotem/EpicPrefs/Settings/DataB/" + builder.ToString());
                int size = UnityEngine.Random.Range(0, 25);
                for (int k = 0; k < size; k++)
                {
                    guid = "[key" + k.ToString() + "]" + Guid.NewGuid().ToString();
                    var _file = File.Create(Application.persistentDataPath + "/HotTotem/EpicPrefs/Settings/DataB/" + builder.ToString() + "/" + guid);
                    string nB = UnityEngine.Random.Range(0, 10).ToString();
                    _bf.Serialize(_file, nB);
                    _file.Close();
                }
            }
            string guid2 = "[keyHolder]";
            var builder2 = new StringBuilder();
            foreach (Byte hashed in sha.ComputeHash(Encoding.Default.GetBytes(guid2)))
                builder2.AppendFormat("{0:x2}", hashed);
            System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/HotTotem/EpicPrefs/Settings/DataB/" + builder2.ToString());
            for (int i = 0; i < value.Length; i++)
            {
                var builder3 = new StringBuilder();
                foreach (Byte hashed in sha.ComputeHash(Encoding.Default.GetBytes(i.ToString())))
                    builder3.AppendFormat("{0:x2}", hashed);
                var _file = File.Create(Application.persistentDataPath + "/HotTotem/EpicPrefs/Settings/DataB/" + builder2.ToString() + "/" + builder3.ToString());
                string nB = value[i].ToString();
                _bf.Serialize(_file, nB);
                _file.Close();
            }
        }
    }
}
