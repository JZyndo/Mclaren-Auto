using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Text;
using System;

public static class Serializer {
    #region Editor Only
#if UNITY_EDITOR
    public class PrefEventArgs
    {
        public PrefEventArgs(string s, SerializationTypes t, bool e)
        {
            Name = s;
            Type = t;
            Encrpyted = e;
            Deleted = false;
        }
        public PrefEventArgs(string s, SerializationTypes t, bool e,bool d)
        {
            Name = s;
            Type = t;
            Encrpyted = e;
            Deleted = d;
        }
        public String Name { get; private set; }
        public SerializationTypes Type { get; private set; }
        public bool Encrpyted { get; private set; }
        public bool Deleted { get; private set; }
    }
    public delegate void PrefEventHandler(PrefEventArgs e);
    public static event PrefEventHandler PrefEvent;
#endif
    #endregion
    static Serializer()
    {
        Initialize();
    }
    public static void ReInitialize()
    {
        System.IO.Directory.Delete(Application.persistentDataPath + "/HotTotem/EpicPrefs/Encrypted/", true);
        Initialize();
    }
    private static void Initialize()
    {
        System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/HotTotem/");
        System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/HotTotem/EpicPrefs/");
        System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/HotTotem/EpicPrefs/String/");
        System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/HotTotem/EpicPrefs/Bool/");
        System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/HotTotem/EpicPrefs/Int/");
        System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/HotTotem/EpicPrefs/Float/");
        System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/HotTotem/EpicPrefs/Double/");
        System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/HotTotem/EpicPrefs/Long/");
        System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/HotTotem/EpicPrefs/Dict/");
        System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/HotTotem/EpicPrefs/List/");
        System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/HotTotem/EpicPrefs/Vector2/");
        System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/HotTotem/EpicPrefs/Vector3/");
        System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/HotTotem/EpicPrefs/Vector4/");
        System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/HotTotem/EpicPrefs/Transform/");
        System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/HotTotem/EpicPrefs/Color/");
        System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/HotTotem/EpicPrefs/Quaternion/");
        System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/HotTotem/EpicPrefs/Array/");

        System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/HotTotem/EpicPrefs/Encrypted/");
        System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/HotTotem/EpicPrefs/Encrypted/String/");
        System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/HotTotem/EpicPrefs/Encrypted/Bool/");
        System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/HotTotem/EpicPrefs/Encrypted/Int/");
        System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/HotTotem/EpicPrefs/Encrypted/Float/");
        System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/HotTotem/EpicPrefs/Encrypted/Long/");
        System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/HotTotem/EpicPrefs/Encrypted/Double/");
        System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/HotTotem/EpicPrefs/Encrypted/Dict/");
        System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/HotTotem/EpicPrefs/Encrypted/List/");
        System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/HotTotem/EpicPrefs/Encrypted/Array/");
#if UNITY_EDITOR
        System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/HotTotem/EpicPrefs/Editor/Collection/");
#endif
    }
    public static bool Serialize(string name,object value,SerializationTypes type,bool encrypted)
    {
        
#if UNITY_EDITOR
SerializationTypes storeType = type;   
#endif
        if (type == SerializationTypes.DictS || type == SerializationTypes.DictI || type == SerializationTypes.DictB || type == SerializationTypes.DictF || type == SerializationTypes.DictD || type == SerializationTypes.DictL)
            type = SerializationTypes.Dict;
        if (type == SerializationTypes.ListS || type == SerializationTypes.ListI || type == SerializationTypes.ListB || type == SerializationTypes.ListF || type == SerializationTypes.ListD || type == SerializationTypes.ListL)
            type = SerializationTypes.List;
        if (encrypted && (type == SerializationTypes.ArrayDouble || type == SerializationTypes.ArrayFloat || type == SerializationTypes.ArrayInt))
            type = SerializationTypes.ArrayString;
        HashAlgorithm sha = new SHA1CryptoServiceProvider();
        var result = sha.ComputeHash(Encoding.Default.GetBytes(name));
        var builder = new StringBuilder();
        foreach (Byte hashed in result)
            builder.AppendFormat("{0:x2}", hashed);
        string filename = builder.ToString() + ".epic";
        FileStream _file;
        BinaryFormatter _bf = new BinaryFormatter();
        SurrogateSelector ss = new SurrogateSelector();
        bool success = false;
        switch (type)
        {
            case SerializationTypes.String:
                if (encrypted)
                {
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Encrypted/String/" + filename;
                    _file = File.Create(filename);
                    _bf.Serialize(_file, Cryptor.Encrypt((string)value));
                } else
                {
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/String/" + filename;
                    _file = File.Create(filename);
                    _bf.Serialize(_file, (string)value);
                }
                _file.Close();
                success = true;
                break;
            case SerializationTypes.Integer:
                if (encrypted)
                {
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Encrypted/Int/" + filename;
                    _file = File.Create(filename);
                    _bf.Serialize(_file, Cryptor.Encrypt((int)value));
                }
                else
                {
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Int/" + filename;
                    _file = File.Create(filename);
                    _bf.Serialize(_file, (int)value);
                }
                _file.Close();
                success = true;
                break;
            case SerializationTypes.Float:
                if (encrypted)
                {
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Encrypted/Float/" + filename;
                    _file = File.Create(filename);
                    _bf.Serialize(_file, Cryptor.Encrypt((float)value));
                }
                else
                {
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Float/" + filename;
                    _file = File.Create(filename);
                    _bf.Serialize(_file, (float)value);
                }
                _file.Close();
                success = true;
                break;
            case SerializationTypes.Long:
                if (encrypted)
                {
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Encrypted/Long/" + filename;
                    _file = File.Create(filename);
                    _bf.Serialize(_file, Cryptor.Encrypt((long)value));
                }
                else
                {
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Long/" + filename;
                    _file = File.Create(filename);
                    _bf.Serialize(_file, (long)value);
                }
                _file.Close();
                success = true;
                break;
            case SerializationTypes.Double:
                if (encrypted)
                {
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Encrypted/Double/" + filename;
                    _file = File.Create(filename);
                    _bf.Serialize(_file, Cryptor.Encrypt((double)value));
                }
                else
                {
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Double/" + filename;
                    _file = File.Create(filename);
                    _bf.Serialize(_file, (double)value);
                }
                _file.Close();
                success = true;
                break;
            case SerializationTypes.Bool:
                if (encrypted)
                {
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Encrypted/Bool/" + filename;
                    _file = File.Create(filename);
                    _bf.Serialize(_file, Cryptor.Encrypt((bool)value));
                }
                else
                {
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Bool/" + filename;
                    _file = File.Create(filename);
                    _bf.Serialize(_file, (bool)value);
                }
                _file.Close();
                success = true;
                break;
            case SerializationTypes.Vector2:
                filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Vector2/" + filename;
                _file = File.Create(filename);
                ss = new SurrogateSelector();
                ss.AddSurrogate(typeof(Vector2),
                new StreamingContext(StreamingContextStates.All),
                    new Vector2SerializationSurrogate());
                // Associate the SurrogateSelector with the BinaryFormatter.
                _bf.SurrogateSelector = ss;
                try
                {
                    // Serialize an Employee object into the memory stream.
                    _bf.Serialize(_file, (Vector2)value);
                    success = true;
                }
                catch (SerializationException e)
                {
                    Debug.LogError("Serialization failed - eC11 : " + e.Message);
                    throw;
                }
                _file.Close();
                break;
            case SerializationTypes.Vector3:
                filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Vector3/" + filename;
                _file = File.Create(filename);
                ss = new SurrogateSelector();
                ss.AddSurrogate(typeof(Vector3),
                new StreamingContext(StreamingContextStates.All),
                    new Vector3SerializationSurrogate());
                // Associate the SurrogateSelector with the BinaryFormatter.
                _bf.SurrogateSelector = ss;
                try
                {
                    // Serialize an Employee object into the memory stream.
                    _bf.Serialize(_file, (Vector3)value);
                    success = true;
                }
                catch (SerializationException e)
                {
                    Debug.LogError("Serialization failed - eC00 : " + e.Message);
                    throw;
                }
                _file.Close();
                break;
            case SerializationTypes.Vector4:
                filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Vector4/" + filename;
                _file = File.Create(filename);
                ss = new SurrogateSelector();
                ss.AddSurrogate(typeof(Vector4),
                new StreamingContext(StreamingContextStates.All),
                    new Vector4SerializationSurrogate());
                // Associate the SurrogateSelector with the BinaryFormatter.
                _bf.SurrogateSelector = ss;
                try
                {
                    // Serialize an Employee object into the memory stream.
                    _bf.Serialize(_file, (Vector4)value);
                    success = true;
                }
                catch (SerializationException e)
                {
                    Debug.LogError("Serialization failed - eC01 : " + e.Message);
                    throw;
                }
                _file.Close();
                break;
            case SerializationTypes.Dict:
                if (!encrypted)
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Dict/" + filename;
                else
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Encrypted/Dict/" + filename;
                _file = File.Create(filename);
                _bf.Serialize(_file, value);
                _file.Close();
                success = true;
                break;
            case SerializationTypes.List:
                if(!encrypted)
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/List/" + filename;
                else
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Encrypted/List/" + filename;
                _file = File.Create(filename);
                _bf.Serialize(_file, (List<object>)value);
                _file.Close();
                success = true;
                break;
            case SerializationTypes.ArrayString:
                if (!encrypted)
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Array/" + filename;
                else
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Encrypted/Array/" + filename;
                _file = File.Create(filename);
                _bf.Serialize(_file, (string[])value);
                _file.Close();
                success = true;
                break;
            case SerializationTypes.ArrayInt:
                if (!encrypted)
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Array/" + filename;
                else
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Encrypted/Array/" + filename;
                _file = File.Create(filename);
                _bf.Serialize(_file, (int[])value);
                _file.Close();
                success = true;
                break;
            case SerializationTypes.ArrayFloat:
                if (!encrypted)
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Array/" + filename;
                else
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Encrypted/Array/" + filename;
                _file = File.Create(filename);
                _bf.Serialize(_file, (float[])value);
                _file.Close();
                success = true;
                break;
            case SerializationTypes.ArrayDouble:
                if (!encrypted)
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Array/" + filename;
                else
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Encrypted/Array/" + filename;
                _file = File.Create(filename);
                _bf.Serialize(_file, (double[])value);
                _file.Close();
                success = true;
                break;
            case SerializationTypes.Transform:
                filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Transform/" + filename;
                _file = File.Create(filename);
                ss = new SurrogateSelector();
                ss.AddSurrogate(typeof(Transform),
                new StreamingContext(StreamingContextStates.All),
                    new TransformSerializationSurrogate());
                // Associate the SurrogateSelector with the BinaryFormatter.
                _bf.SurrogateSelector = ss;
                try
                {
                    // Serialize an Employee object into the memory stream.
                    _bf.Serialize(_file, (Transform)value);
                    success = true;
                }
                catch (SerializationException e)
                {
                    Debug.LogError("Serialization failed - eC02 : " + e.Message);
                    throw;
                }
                _file.Close();
                break;
            case SerializationTypes.Color:
                filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Color/" + filename;
                _file = File.Create(filename);
                ss = new SurrogateSelector();
                ss.AddSurrogate(typeof(Color),
                new StreamingContext(StreamingContextStates.All),
                    new ColorSerializationSurrogate());
                // Associate the SurrogateSelector with the BinaryFormatter.
                _bf.SurrogateSelector = ss;
                try
                {
                    // Serialize an Employee object into the memory stream.
                    _bf.Serialize(_file, (Color)value);
                    success = true;
                }
                catch (SerializationException e)
                {
                    Debug.LogError("Serialization failed - eC09 : " + e.Message);
                    throw;
                }
                _file.Close();
                break;
            case SerializationTypes.Quaternion:
                filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Quaternion/" + filename;
                _file = File.Create(filename);
                ss = new SurrogateSelector();
                ss.AddSurrogate(typeof(Quaternion),
                new StreamingContext(StreamingContextStates.All),
                    new QuaternionSerializationSurrogate());
                // Associate the SurrogateSelector with the BinaryFormatter.
                _bf.SurrogateSelector = ss;
                try
                {
                    // Serialize an Employee object into the memory stream.
                    _bf.Serialize(_file, (Quaternion)value);
                    success = true;
                }
                catch (SerializationException e)
                {
                    Debug.LogError("Serialization failed - eC03 : " + e.Message);
                    throw;
                }
                _file.Close();
                break;
            #region Editor Only
#if UNITY_EDITOR
            case SerializationTypes.Editor:
                filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Editor/Collection/" + filename;
                _file = File.Create(filename);
                _bf.Serialize(_file, value);
                _file.Close();
                success = true;
                break;
#endif
            #endregion
            default:                
                break;
        }
        #region Editor Only
#if UNITY_EDITOR
        if (PrefEvent != null && storeType != SerializationTypes.Editor)
        {
            PrefEvent(new PrefEventArgs(name, storeType, encrypted));
        }
#endif
        #endregion
        return success;
    }
    public static object Deserialize(string name, SerializationTypes type, bool encrypted)
    {
        HashAlgorithm sha = new SHA1CryptoServiceProvider();
        var result = sha.ComputeHash(Encoding.Default.GetBytes(name));
        var builder = new StringBuilder();
        foreach (Byte hashed in result)
            builder.AppendFormat("{0:x2}", hashed);
        string filename = builder.ToString() + ".epic";
        FileStream _file;
        BinaryFormatter _bf = new BinaryFormatter();
        SurrogateSelector ss = new SurrogateSelector();
        object newObject = null;
        switch (type)
        {
            case SerializationTypes.String:
                if (encrypted)
                {
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Encrypted/String/" + filename;
                    if (File.Exists(filename))
                    {
                        _file = File.Open(filename, FileMode.Open);
                        newObject = (string)Cryptor.Decrypt((string)_bf.Deserialize(_file), SerializationTypes.String);
                        _file.Close();
                    }
                }
                else
                {
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/String/" + filename;
                    if (File.Exists(filename))
                    {
                        _file = File.Open(filename, FileMode.Open);
                        newObject = (string)_bf.Deserialize(_file);
                        _file.Close();
                    }
                }
                break;
            case SerializationTypes.Integer:
                if (encrypted)
                {
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Encrypted/Int/" + filename;
                    if (File.Exists(filename))
                    {
                        _file = File.Open(filename, FileMode.Open);
                        newObject = (int)Cryptor.Decrypt((string)_bf.Deserialize(_file), SerializationTypes.Integer);
                        _file.Close();
                    }
                }
                else
                {
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Int/" + filename;
                    if (File.Exists(filename))
                    {
                        _file = File.Open(filename, FileMode.Open);
                        newObject = (int)_bf.Deserialize(_file);
                        _file.Close();
                    }
                }
                break;
            case SerializationTypes.Float:
                if (encrypted)
                {
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Encrypted/Float/" + filename;
                    if (File.Exists(filename))
                    {
                        _file = File.Open(filename, FileMode.Open);
                        newObject = (float)Cryptor.Decrypt((string)_bf.Deserialize(_file), SerializationTypes.Float);
                        _file.Close();
                    }
                }
                else
                {
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Float/" + filename;
                    if (File.Exists(filename))
                    {
                        _file = File.Open(filename, FileMode.Open);
                        newObject = (float)_bf.Deserialize(_file);
                        _file.Close();
                    }
                }
                break;
            case SerializationTypes.Long:
                if (encrypted)
                {
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Encrypted/Long/" + filename;
                    if (File.Exists(filename))
                    {
                        _file = File.Open(filename, FileMode.Open);
                        newObject = (long)Cryptor.Decrypt((string)_bf.Deserialize(_file), SerializationTypes.Long);
                        _file.Close();
                    }
                }
                else
                {
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Long/" + filename;
                    if (File.Exists(filename))
                    {
                        _file = File.Open(filename, FileMode.Open);
                        newObject = (long)_bf.Deserialize(_file);
                        _file.Close();
                    }
                }
                break;
            case SerializationTypes.Double:
                if (encrypted)
                {
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Encrypted/Double/" + filename;
                    if (File.Exists(filename))
                    {
                        _file = File.Open(filename, FileMode.Open);
                        newObject = (double)Cryptor.Decrypt((string)_bf.Deserialize(_file), SerializationTypes.Double);
                        _file.Close();
                    }
                }
                else
                {
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Double/" + filename;
                    if (File.Exists(filename))
                    {
                        _file = File.Open(filename, FileMode.Open);
                        newObject = (double)_bf.Deserialize(_file);
                        _file.Close();
                    }
                }
                break;
            case SerializationTypes.Bool:
                if (encrypted)
                {
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Encrypted/Bool/" + filename;
                    if (File.Exists(filename))
                    {
                        _file = File.Open(filename, FileMode.Open);
                        newObject = (bool)Cryptor.Decrypt((string)_bf.Deserialize(_file), SerializationTypes.Bool);
                        _file.Close();
                    }
                }
                else
                {
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Bool/" + filename;
                    if (File.Exists(filename))
                    {
                        _file = File.Open(filename, FileMode.Open);
                        newObject = (bool)_bf.Deserialize(_file);
                        _file.Close();
                    }
                }
                break;
            case SerializationTypes.Vector2:
                filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Vector2/" + filename;
                if (File.Exists(filename))
                {
                    _file = File.Open(filename, FileMode.Open);
                    ss = new SurrogateSelector();
                    ss.AddSurrogate(typeof(Vector2),
                    new StreamingContext(StreamingContextStates.All),
                        new Vector2SerializationSurrogate());
                    // Associate the SurrogateSelector with the BinaryFormatter.
                    _bf.SurrogateSelector = ss;
                    try
                    {
                        // Deserialize the Employee object from the memory stream.
                        newObject = (Vector2)_bf.Deserialize(_file);
                    }
                    catch (SerializationException e)
                    {
                        Debug.LogError("Deserialization failed - eC12 : " + e.Message);
                        throw;
                    }
                    _file.Close();
                }
                break;
            case SerializationTypes.Vector3:
                filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Vector3/" + filename;
                if (File.Exists(filename))
                {
                    _file = File.Open(filename, FileMode.Open);
                    ss = new SurrogateSelector();
                    ss.AddSurrogate(typeof(Vector3),
                    new StreamingContext(StreamingContextStates.All),
                        new Vector3SerializationSurrogate());
                    // Associate the SurrogateSelector with the BinaryFormatter.
                    _bf.SurrogateSelector = ss;
                    try
                    {
                        // Deserialize the Employee object from the memory stream.
                        newObject = (Vector3)_bf.Deserialize(_file);
                    }
                    catch (SerializationException e)
                    {
                        Debug.LogError("Deserialization failed - eC04 : " + e.Message);
                        throw;
                    }
                    _file.Close();
                }
                break;
            case SerializationTypes.Vector4:
                filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Vector4/" + filename;
                if (File.Exists(filename))
                {
                    _file = File.Open(filename, FileMode.Open);
                    ss = new SurrogateSelector();
                    ss.AddSurrogate(typeof(Vector4),
                    new StreamingContext(StreamingContextStates.All),
                        new Vector4SerializationSurrogate());
                    // Associate the SurrogateSelector with the BinaryFormatter.
                    _bf.SurrogateSelector = ss;
                    try
                    {
                        // Deserialize the Employee object from the memory stream.
                        newObject = (Vector4)_bf.Deserialize(_file);
                    }
                    catch (SerializationException e)
                    {
                        Debug.LogError("Deserialization failed - eC05 : " + e.Message);
                        throw;
                    }
                    _file.Close();
                }
                break;
            case SerializationTypes.Dict:
                if (!encrypted)
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Dict/" + filename;
                else
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Encrypted/Dict/" + filename;
                if (File.Exists(filename))
                {
                    _file = File.Open(filename, FileMode.Open);
                    newObject = _bf.Deserialize(_file);
                    _file.Close();
                }
                break;
            case SerializationTypes.List:
                if (!encrypted)
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/List/" + filename;
                else
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Encrypted/List/" + filename;
                if (File.Exists(filename))
                {
                    _file = File.Open(filename, FileMode.Open);
                    newObject = (List<object>)_bf.Deserialize(_file);
                    _file.Close();
                }
                break;
            case SerializationTypes.ArrayString:
                if (!encrypted)
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Array/" + filename;
                else
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Encrypted/Array/" + filename;
                if (File.Exists(filename))
                {
                    _file = File.Open(filename, FileMode.Open);
                    newObject = (string[])_bf.Deserialize(_file);
                    _file.Close();
                }
                break;
            case SerializationTypes.ArrayInt:
                if (!encrypted)
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Array/" + filename;
                else
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Encrypted/Array/" + filename;
                if (File.Exists(filename))
                {
                    _file = File.Open(filename, FileMode.Open);
                    newObject = (int[])_bf.Deserialize(_file);
                    _file.Close();
                }
                break;
            case SerializationTypes.ArrayFloat:
                if (!encrypted)
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Array/" + filename;
                else
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Encrypted/Array/" + filename;
                if (File.Exists(filename))
                {
                    _file = File.Open(filename, FileMode.Open);
                    newObject = (float[])_bf.Deserialize(_file);
                    _file.Close();
                }
                break;
            case SerializationTypes.ArrayDouble:
                if (!encrypted)
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Array/" + filename;
                else
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Encrypted/Array/" + filename;
                if (File.Exists(filename))
                {
                    _file = File.Open(filename, FileMode.Open);
                    newObject = (double[])_bf.Deserialize(_file);
                    _file.Close();
                }
                break;
            case SerializationTypes.Transform:
                filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Transform/" + filename;
                if (File.Exists(filename))
                {
                    _file = File.Open(filename, FileMode.Open);
                    ss = new SurrogateSelector();
                    ss.AddSurrogate(typeof(Transform),
                    new StreamingContext(StreamingContextStates.All),
                        new TransformSerializationSurrogate());
                    // Associate the SurrogateSelector with the BinaryFormatter.
                    _bf.SurrogateSelector = ss;
                    try
                    {
                        // Deserialize the Employee object from the memory stream.
                        GameObject _obj = new GameObject();
                        _obj.transform.position = ((Transform)_bf.Deserialize(_file)).position;
                        _obj.transform.localScale = ((Transform)_bf.Deserialize(_file)).localScale;
                        _obj.transform.rotation = ((Transform)_bf.Deserialize(_file)).rotation;
                        _obj.transform.name = ((Transform)_bf.Deserialize(_file)).name;
                        newObject = _obj.transform;
                    }
                    catch (SerializationException e)
                    {
                        Debug.LogError("Deserialization failed - eC06 : " + e.Message);
                        throw;
                    }
                    _file.Close();
                }
                break;
            case SerializationTypes.Color:
                filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Color/" + filename;
                if (File.Exists(filename))
                {
                    _file = File.Open(filename, FileMode.Open);
                    ss = new SurrogateSelector();
                    ss.AddSurrogate(typeof(Color),
                    new StreamingContext(StreamingContextStates.All),
                        new ColorSerializationSurrogate());
                    // Associate the SurrogateSelector with the BinaryFormatter.
                    _bf.SurrogateSelector = ss;
                    try
                    {
                        // Deserialize the Employee object from the memory stream.
                        newObject = (Color)_bf.Deserialize(_file);
                    }
                    catch (SerializationException e)
                    {
                        Debug.LogError("Deserialization failed - eC06 : " + e.Message);
                        throw;
                    }
                    _file.Close();
                }
                break;
            case SerializationTypes.Quaternion:
                filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Quaternion/" + filename;
                if (File.Exists(filename))
                {
                    _file = File.Open(filename, FileMode.Open);
                    ss = new SurrogateSelector();
                    ss.AddSurrogate(typeof(Quaternion),
                    new StreamingContext(StreamingContextStates.All),
                        new QuaternionSerializationSurrogate());
                    // Associate the SurrogateSelector with the BinaryFormatter.
                    _bf.SurrogateSelector = ss;
                    try
                    {
                        // Deserialize the Employee object from the memory stream.
                        newObject = (Quaternion)_bf.Deserialize(_file);
                    }
                    catch (SerializationException e)
                    {
                        Debug.LogError("Deserialization failed - eC07 : " + e.Message);
                        throw;
                    }
                    _file.Close();
                }
                break;
            #region Editor Only
#if UNITY_EDITOR
            case SerializationTypes.Editor:
                filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Editor/Collection/" + filename;
                if (File.Exists(filename))
                {
                    _file = File.Open(filename, FileMode.Open);
                    newObject = _bf.Deserialize(_file);
                    _file.Close();
                }
                break;
#endif
#endregion
            default:
                Debug.LogError("Deserialization failed - eC08 : Not a valid type.");
                break;
        }
        return newObject;
    }
    public static bool Delete(string name, SerializationTypes type, bool encrypted)
    {
        #if UNITY_EDITOR
        SerializationTypes storeType = type;
        #endif
        if (type == SerializationTypes.DictS || type == SerializationTypes.DictI || type == SerializationTypes.DictB || type == SerializationTypes.DictF || type == SerializationTypes.DictD || type == SerializationTypes.DictL)
            type = SerializationTypes.Dict;
        if (type == SerializationTypes.ListS || type == SerializationTypes.ListI || type == SerializationTypes.ListB || type == SerializationTypes.ListF || type == SerializationTypes.ListD || type == SerializationTypes.ListL)
            type = SerializationTypes.List;
        HashAlgorithm sha = new SHA1CryptoServiceProvider();
        var result = sha.ComputeHash(Encoding.Default.GetBytes(name));
        var builder = new StringBuilder();
        foreach (Byte hashed in result)
            builder.AppendFormat("{0:x2}", hashed);
        string filename = builder.ToString() + ".epic";
        bool success = false;
        switch (type)
        {
            case SerializationTypes.String:
                if (encrypted)
                {
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Encrypted/String/" + filename;
                    File.Delete(filename);
                }
                else
                {
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/String/" + filename;
                    File.Delete(filename);
                }
                success = true;
                break;
            case SerializationTypes.Integer:
                if (encrypted)
                {
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Encrypted/Int/" + filename;
                }
                else
                {
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Int/" + filename;
                }
                File.Delete(filename);
                success = true;
                break;
            case SerializationTypes.Float:
                if (encrypted)
                {
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Encrypted/Float/" + filename;
                }
                else
                {
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Float/" + filename;
                }
                File.Delete(filename);
                success = true;
                break;
            case SerializationTypes.Long:
                if (encrypted)
                {
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Encrypted/Long/" + filename;
                }
                else
                {
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Long/" + filename;
                }
                File.Delete(filename);
                success = true;
                break;
            case SerializationTypes.Double:
                if (encrypted)
                {
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Encrypted/Double/" + filename;
                }
                else
                {
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Double/" + filename;
                }
                File.Delete(filename);
                success = true;
                break;
            case SerializationTypes.Bool:
                if (encrypted)
                {
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Encrypted/Bool/" + filename;
                }
                else
                {
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Bool/" + filename;
                }
                File.Delete(filename);
                success = true;
                break;
            case SerializationTypes.Vector2:
                filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Vector2/" + filename;
                File.Delete(filename);
                success = true;
                break;
            case SerializationTypes.Vector3:
                filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Vector3/" + filename;
                File.Delete(filename);
                break;
            case SerializationTypes.Vector4:
                filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Vector4/" + filename;
                File.Delete(filename);
                break;
            case SerializationTypes.Dict:
                if (!encrypted)
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Dict/" + filename;
                else
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Encrypted/Dict/" + filename;
                File.Delete(filename);
                success = true;
                break;
            case SerializationTypes.List:
                if (!encrypted)
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/List/" + filename;
                else
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Encrypted/List/" + filename;
                File.Delete(filename);
                success = true;
                break;
            case SerializationTypes.ArrayString:
                if (!encrypted)
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Array/" + filename;
                else
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Encrypted/Array/" + filename;
                File.Delete(filename);
                success = true;
                break;
            case SerializationTypes.ArrayInt:
                if (!encrypted)
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Array/" + filename;
                else
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Encrypted/Array/" + filename;
                File.Delete(filename);
                success = true;
                break;
            case SerializationTypes.ArrayFloat:
                if (!encrypted)
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Array/" + filename;
                else
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Encrypted/Array/" + filename;
                File.Delete(filename);
                success = true;
                break;
            case SerializationTypes.ArrayDouble:
                if (!encrypted)
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Array/" + filename;
                else
                    filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Encrypted/Array/" + filename;
                File.Delete(filename);
                success = true;
                break;
            case SerializationTypes.Transform:
                filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Transform/" + filename;
                File.Delete(filename);
                success = true;
                break;
            case SerializationTypes.Color:
                filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Color/" + filename;
                File.Delete(filename);
                success = true;
                break;
            case SerializationTypes.Quaternion:
                filename = Application.persistentDataPath + "/HotTotem/EpicPrefs/Quaternion/" + filename;
                File.Delete(filename);
                success = true;
                break;
            default:
                break;
        }
        #region Editor Only
#if UNITY_EDITOR
        if (PrefEvent != null && type != SerializationTypes.Editor)
        {
            PrefEvent(new PrefEventArgs(name, storeType, encrypted,true));
        }
#endif
        #endregion
        return success;
    }
    public enum SerializationTypes
    {
        Integer,
        String,
        Float,
        Long,
        Double,
        Bool,
        Vector2,
        Vector3,
        Vector4,
        List,
        Dict,
        Transform,
        Quaternion,
        Color,
        ArrayString,
        ArrayInt,
        ArrayFloat,
        ArrayDouble,
        Editor,
        DictS,
        DictI,
        DictB,
        DictL,
        DictD,
        DictF,
        ListS,
        ListI,
        ListB,
        ListL,
        ListD,
        ListF
    }
}
