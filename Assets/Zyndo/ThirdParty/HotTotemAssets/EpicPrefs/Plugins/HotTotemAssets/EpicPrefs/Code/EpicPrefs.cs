using UnityEngine;
#if UNITY_EDITOR
        using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using System;
/// <summary>
/// EpicPrefs is a replacement for Unity’s PlayerPrefs. 
/// Everything you can do with PlayerPrefs, you can do with EpicPrefs too. 
/// Plus, much much more. 
/// </summary>
public static class EpicPrefs {
    static EpicPrefs()
    {
        #if !UNITY_EDITOR
        Operators.setupPrefs();
        #endif       
    }
    /// <summary>
    /// Initialize and setup any imported EpicPrefs.
    /// </summary>
    public static void Initialize(){
        #if !UNITY_EDITOR
        Operators.setupPrefs();
        #endif 
    }
    /// <summary>
    /// Retrieve the current passPhrase.
    /// </summary>
    /// <returns>A string containing the passPhrase.</returns>
    public static string getPassPhrase()
    {
        if (HotTotem.Key.getKey() == "")
            return "yourS3cr3tP455phr453";
        else {
            return HotTotem.Key.getKey();
        }
    }
    /// <summary>
    /// Set the new passPhrase and remove the previously encrypted
    /// values if the passPhrase has changed.
    /// </summary>
    /// <param name="value">The new passPhrase</param>
    public static void setPassPhrase(string value)
    {
        if (value != HotTotem.Key.getKey())
        {
            Debug.LogWarning("Encryption Key changed. Clearing all previously stored entries.");
            Serializer.ReInitialize();
            HotTotem.Key.setKey(value);
            #if UNITY_EDITOR
            Operators.DeleteDirectory(Application.dataPath + "/Resources/HotTotemAssets/EpicPrefs/Encrypted",true);
            Operators.DirectoryCopy("HotTotem/EpicPrefs/Settings/DataA/" ,"HotTotemAssets/EpicPrefs/Settings/DataA",true);
            AssetDatabase.Refresh();
            #endif
        }
        
    }
    /// <summary>
    /// Retrieve the current initVector.
    /// </summary>
    /// <returns>A string containing the initVector.</returns>
    public static string getInitVector()
    {
        if (HotTotem.Key.getVector() == "")
            return "bacA3diJa@aldwpW";
        else
            return HotTotem.Key.getVector();
        
    }
    /// <summary>
    /// Set the new initVector and remove the previously encrypted
    /// values if the initVector has changed.
    /// </summary>
    /// <param name="value">The new initVector. It has to be 16 byte long!</param>
    public static void setInitVector(string value)
    {
        if (System.Text.ASCIIEncoding.ASCII.GetByteCount(value) != 16)
        {
            Debug.LogError("Error while setting the initialization Vecotr : " + value + " is not 16 bytes long.");
            return;
        }
        if (value != HotTotem.Key.getVector())
        {
            Debug.LogWarning("Encryption Key changed. Clearing all previously stored entries.");
            Serializer.ReInitialize();
            HotTotem.Key.setVector(value);
            #if UNITY_EDITOR
            Operators.DeleteDirectory(Application.dataPath + "/Resources/HotTotemAssets/EpicPrefs/Encrypted",true);
            Operators.DirectoryCopy("HotTotem/EpicPrefs/Settings/DataB/" ,"HotTotemAssets/EpicPrefs/Settings/DataB",true);
            AssetDatabase.Refresh();
            #endif
        }          
    }
    // Definitions of all the access methods
    // It is highly recommanded not to modifiy this file at all.
    // Doing so can either result in unfunctional EpicPrefs, 
    // and with new updates your changes will be lost.
    // The files is seperated in regions according to the different
    // types.
    #region String
    /// <summary>
    /// Call this function to save a string to the EpicPrefs.
    /// See the parameters for more information on what to pass.
    /// </summary>
    /// <param name="name">The name under which the pref is saved, and can be retrieved with.</param>
    /// <param name="value">The value that is saved.</param>
    /// <param name="encrypted">Whether to use encryption or not. Default is false and can be left away in that case.</param>
    /// <returns>Returns a bool stating if the saving has been successfull.</returns>
    public static bool SetString(string name,string value,bool encrypted = false)
    {
        return Serializer.Serialize(name,value,Serializer.SerializationTypes.String,encrypted);
    }
    /// <summary>
    /// Call this function to retrieve a string from the EpicPrefs.
    /// If the Pref does not exist an empty string is returned.
    /// </summary>
    /// <param name="name">The name under which the EpicPref has previously been saved.</param>
    /// <param name="encrypted">Whether the EpicPref was previoulsy encrypted or not.</param>
    /// <returns>Returns the previously saved string.</returns>
    public static string GetString(string name, bool encrypted = false)
    {
        string newString = (string)Serializer.Deserialize(name, Serializer.SerializationTypes.String, encrypted);
        if (newString == null)
            return "";
        else return newString;
    }
    /// <summary>
    /// Call this function to retrieve a string from the EpicPrefs.
    /// If the Pref does not exist a default string is returned.
    /// </summary>
    /// <param name="name">The name under which the EpicPref has previously been saved.</param>
    /// <param name="defaultValue">The default returned if the EpicPref is not found.</param>
    /// <param name="encrypted">Whether the EpicPref was previoulsy encrypted or not.</param>
    /// <returns>Returns the previously saved string or the default value if not found.</returns>
    public static string GetString(string name, string defaultValue, bool encrypted = false)
    {
        string newString = (string)Serializer.Deserialize(name, Serializer.SerializationTypes.String, encrypted);
        if (newString == null)
            return defaultValue;
        else return newString;
    }
    /// <summary>
    /// Deletes an EpicPref permanently.
    /// </summary>
    /// <param name="name">The name of the pref.</param>
    /// <param name="encrypted">Whether it was encrypted or not.</param>
    public static void DeleteString(string name, bool encrypted)
    {
        Serializer.Delete(name, Serializer.SerializationTypes.String, encrypted);
    }
    #endregion
    #region Integer
    /// <summary>
    /// Call this function to save an integer to the EpicPrefs.
    /// See the parameters for more information on what to pass.
    /// </summary>
    /// <param name="name">The name under which the pref is saved, and can be retrieved with.</param>
    /// <param name="value">The value that is saved.</param>
    /// <param name="encrypted">Whether to use encryption or not. Default is false and can be left away in that case.</param>
    /// <returns>Returns a bool stating if the saving has been successfull.</returns>
    public static bool SetInt(string name, int value, bool encrypted = false)
    {
        return Serializer.Serialize(name, value, Serializer.SerializationTypes.Integer, encrypted);
    }
    /// <summary>
    /// Call this function to retrieve an integer from the EpicPrefs.
    /// If the Pref does not exist -1 is returned.
    /// </summary>
    /// <param name="name">The name under which the EpicPref has previously been saved.</param>
    /// <param name="encrypted">Whether the EpicPref was previoulsy encrypted or not.</param>
    /// <returns>Returns the previously saved integer.</returns>
    public static int GetInt(string name, bool encrypted = false)
    {
        int? newInt = (int?)Serializer.Deserialize(name, Serializer.SerializationTypes.Integer, encrypted);
        if (newInt == null)
            return -1;
        else
            return (int)newInt;
    }
    /// <summary>
    /// Call this function to retrieve an integer from the EpicPrefs.
    /// If the Pref does not exist a default integer is returned.
    /// </summary>
    /// <param name="name">The name under which the EpicPref has previously been saved.</param>
    /// <param name="defaultValue">The default returned if the EpicPref is not found.</param>
    /// <param name="encrypted">Whether the EpicPref was previoulsy encrypted or not.</param>
    /// <returns>Returns the previously saved integer or the default value if not found.</returns>
    public static int GetInt(string name, int defaultValue, bool encrypted = false)
    {
        int? newInt = (int?)Serializer.Deserialize(name, Serializer.SerializationTypes.Integer, encrypted);
        if (newInt == null)
            return defaultValue;
        else
            return (int)newInt;
    }
    /// <summary>
    /// Deletes an EpicPref permanently.
    /// </summary>
    /// <param name="name">The name of the pref.</param>
    /// <param name="encrypted">Whether it was encrypted or not.</param>
    public static void DeleteInt(string name, bool encrypted)
    {
        Serializer.Delete(name, Serializer.SerializationTypes.Integer, encrypted);
    }
    #endregion
    #region Bool
    /// <summary>
    /// Call this function to save a bool to the EpicPrefs.
    /// See the parameters for more information on what to pass.
    /// </summary>
    /// <param name="name">The name under which the pref is saved, and can be retrieved with.</param>
    /// <param name="value">The value that is saved.</param>
    /// <param name="encrypted">Whether to use encryption or not. Default is false and can be left away in that case.</param>
    /// <returns>Returns a bool stating if the saving has been successfull.</returns>
    public static bool SetBool(string name, bool value, bool encrypted = false)
    {        
        return Serializer.Serialize(name, value, Serializer.SerializationTypes.Bool,encrypted);
    }
    /// <summary>
    /// Call this function to retrieve a boolean from the EpicPrefs.
    /// If the Pref does not exist false is returned.
    /// </summary>
    /// <param name="name">The name under which the EpicPref has previously been saved.</param>
    /// <param name="encrypted">Whether the EpicPref was previoulsy encrypted or not.</param>
    /// <returns>Returns the previously saved boolean.</returns>
    public static bool GetBool(string name, bool encrypted = false)
    {
        bool? newBool = (bool?)Serializer.Deserialize(name, Serializer.SerializationTypes.Bool, encrypted);
        if (newBool == null)
            return false;
        else
            return (bool)newBool;
    }
    /// <summary>
    /// Call this function to retrieve a boolean from the EpicPrefs.
    /// If the Pref does not exist a default bool is returned.
    /// </summary>
    /// <param name="name">The name under which the EpicPref has previously been saved.</param>
    /// <param name="defaultValue">The default returned if the EpicPref is not found.</param>
    /// <param name="encrypted">Whether the EpicPref was previoulsy encrypted or not.</param>
    /// <returns>Returns the previously saved boolean or the default value if not found.</returns>
    public static bool GetBool(string name, bool defaultValue, bool encrypted)
    {
        bool? newBool = (bool?)Serializer.Deserialize(name, Serializer.SerializationTypes.Bool, encrypted);
        if (newBool == null)
            return defaultValue;
        else return (bool)newBool;
    }
    /// <summary>
    /// Deletes an EpicPref permanently.
    /// </summary>
    /// <param name="name">The name of the pref.</param>
    /// <param name="encrypted">Whether it was encrypted or not.</param>
    public static void DeleteBool(string name, bool encrypted)
    {
        Serializer.Delete(name, Serializer.SerializationTypes.Bool, encrypted);
    }
    #endregion
    #region Float
    /// <summary>
    /// Call this function to save a float to the EpicPrefs.
    /// See the parameters for more information on what to pass.
    /// </summary>
    /// <param name="name">The name under which the pref is saved, and can be retrieved with.</param>
    /// <param name="value">The value that is saved.</param>
    /// <param name="encrypted">Whether to use encryption or not. Default is false and can be left away in that case.</param>
    /// <returns>Returns a bool stating if the saving has been successfull.</returns>
    public static bool SetFloat(string name, float value, bool encrypted = false)
    {
        return Serializer.Serialize(name, value, Serializer.SerializationTypes.Float,encrypted);
    }
    /// <summary>
    /// Call this function to retrieve a float from the EpicPrefs.
    /// If the Pref does not exist -1 is returned.
    /// </summary>
    /// <param name="name">The name under which the EpicPref has previously been saved.</param>
    /// <param name="encrypted">Whether the EpicPref was previoulsy encrypted or not.</param>
    /// <returns>Returns the previously saved float.</returns>
    public static float GetFloat(string name, bool encrypted = false)
    {
        float? newFloat = (float?)Serializer.Deserialize(name, Serializer.SerializationTypes.Float, encrypted);
        if (newFloat == null)
            return -1f;
        else
            return (float)newFloat;
    }
    /// <summary>
    /// Call this function to retrieve a float from the EpicPrefs.
    /// If the Pref does not exist a default float is returned.
    /// </summary>
    /// <param name="name">The name under which the EpicPref has previously been saved.</param>
    /// <param name="defaultValue">The default returned if the EpicPref is not found.</param>
    /// <param name="encrypted">Whether the EpicPref was previoulsy encrypted or not.</param>
    /// <returns>Returns the previously saved float or the default value if not found.</returns>
    public static float GetFloat(string name, float defaultValue, bool encrypted = false)
    {
        float? newFloat = (float?)Serializer.Deserialize(name, Serializer.SerializationTypes.Float, encrypted);
        if (newFloat == null)
            return defaultValue;
        else
            return (float)newFloat;
    }
    /// <summary>
    /// Deletes an EpicPref permanently.
    /// </summary>
    /// <param name="name">The name of the pref.</param>
    /// <param name="encrypted">Whether it was encrypted or not.</param>
    public static void DeleteFloat(string name, bool encrypted)
    {
        Serializer.Delete(name, Serializer.SerializationTypes.Float, encrypted);
    }
    #endregion
    #region Long
    /// <summary>
    /// Call this function to save a long to the EpicPrefs.
    /// See the parameters for more information on what to pass.
    /// </summary>
    /// <param name="name">The name under which the pref is saved, and can be retrieved with.</param>
    /// <param name="value">The value that is saved.</param>
    /// <param name="encrypted">Whether to use encryption or not. Default is false and can be left away in that case.</param>
    /// <returns>Returns a bool stating if the saving has been successfull.</returns>
    public static bool SetLong(string name, long value, bool encrypted = false)
    {
        return Serializer.Serialize(name, value, Serializer.SerializationTypes.Long,encrypted);
    }
    /// <summary>
    /// Call this function to retrieve a long from the EpicPrefs.
    /// If the Pref does not exist -1 is returned.
    /// </summary>
    /// <param name="name">The name under which the EpicPref has previously been saved.</param>
    /// <param name="encrypted">Whether the EpicPref was previoulsy encrypted or not.</param>
    /// <returns>Returns the previously saved long.</returns>
    public static long GetLong(string name, bool encrypted = false)
    {
        long? newLong = (long?)Serializer.Deserialize(name, Serializer.SerializationTypes.Long, encrypted);
        if (newLong == null)
            return (long)-1;
        else
            return (long)newLong;
    }
    /// <summary>
    /// Call this function to retrieve a long from the EpicPrefs.
    /// If the Pref does not exist a default long is returned.
    /// </summary>
    /// <param name="name">The name under which the EpicPref has previously been saved.</param>
    /// <param name="defaultValue">The default returned if the EpicPref is not found.</param>
    /// <param name="encrypted">Whether the EpicPref was previoulsy encrypted or not.</param>
    /// <returns>Returns the previously saved long or the default value if not found.</returns>
    public static long GetLong(string name, long defaultValue, bool encrypted = false)
    {
        long? newLong = (long?)Serializer.Deserialize(name, Serializer.SerializationTypes.Long, encrypted);
        if (newLong == null)
            return defaultValue;
        else
            return (long)newLong;
    }
    /// <summary>
    /// Deletes an EpicPref permanently.
    /// </summary>
    /// <param name="name">The name of the pref.</param>
    /// <param name="encrypted">Whether it was encrypted or not.</param>
    public static void DeleteLong(string name, bool encrypted)
    {
        Serializer.Delete(name, Serializer.SerializationTypes.Long, encrypted);
    }
    #endregion
    #region Double
    /// <summary>
    /// Call this function to save a double to the EpicPrefs.
    /// See the parameters for more information on what to pass.
    /// </summary>
    /// <param name="name">The name under which the pref is saved, and can be retrieved with.</param>
    /// <param name="value">The value that is saved.</param>
    /// <param name="encrypted">Whether to use encryption or not. Default is false and can be left away in that case.</param>
    /// <returns>Returns a bool stating if the saving has been successfull.</returns>
    public static bool SetDouble(string name, double value, bool encrypted = false)
    {
        return Serializer.Serialize(name, value, Serializer.SerializationTypes.Double,encrypted);
    }
    /// <summary>
    /// Call this function to retrieve a double from the EpicPrefs.
    /// If the Pref does not exist -1 is returned.
    /// </summary>
    /// <param name="name">The name under which the EpicPref has previously been saved.</param>
    /// <param name="encrypted">Whether the EpicPref was previoulsy encrypted or not.</param>
    /// <returns>Returns the previously saved double.</returns>
    public static double GetDouble(string name, bool encrypted = false)
    {
        double? newDouble = (double?)Serializer.Deserialize(name, Serializer.SerializationTypes.Double, encrypted);
        if (newDouble == null)
            return (double)-1f;
        else
            return (double)newDouble;
    }
    /// <summary>
    /// Call this function to retrieve a double from the EpicPrefs.
    /// If the Pref does not exist a default double is returned.
    /// </summary>
    /// <param name="name">The name under which the EpicPref has previously been saved.</param>
    /// <param name="defaultValue">The default returned if the EpicPref is not found.</param>
    /// <param name="encrypted">Whether the EpicPref was previoulsy encrypted or not.</param>
    /// <returns>Returns the previously saved double or the default value if not found.</returns>
    public static double GetDouble(string name, double defaultValue, bool encrypted = false)
    {
        double? newDouble = (double?)Serializer.Deserialize(name, Serializer.SerializationTypes.Double, encrypted);
        if (newDouble == null)
            return defaultValue;
        else
            return (double)newDouble;
    }
    /// <summary>
    /// Deletes an EpicPref permanently.
    /// </summary>
    /// <param name="name">The name of the pref.</param>
    /// <param name="encrypted">Whether it was encrypted or not.</param>
    public static void DeleteDouble(string name, bool encrypted)
    {
        Serializer.Delete(name, Serializer.SerializationTypes.Double, encrypted);
    }
    #endregion
    #region Dict
    #region StringDict
    /// <summary>
    /// Call this function to save a Dictionary with its
    /// Keys being a string and
    /// Values being a string
    /// to the EpicPrefs.
    /// See the parameters for more information on what to pass.
    /// </summary>
    /// <param name="name">The name under which the pref is saved, and can be retrieved with.</param>
    /// <param name="value">The value that is saved.</param>
    /// <param name="encrypted">Whether to use encryption or not. Default is false and can be left away in that case.</param>
    /// <returns>Returns a bool stating if the saving has been successfull.</returns>
    public static bool SetDict(string name, Dictionary<string, string> value, bool encrypted = false)
    {
        if (encrypted)
        {
            Dictionary<string, string> encryptedDict = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> t in value)
            {
                encryptedDict[Cryptor.Encrypt(t.Key)] = Cryptor.Encrypt(t.Value);
            }
            return Serializer.Serialize(name, encryptedDict, Serializer.SerializationTypes.DictS, encrypted);
        }
        else
            return Serializer.Serialize(name, value, Serializer.SerializationTypes.DictS, encrypted);
    }
    /// <summary>
    /// Call this function to save a Dictionary with its
    /// Keys being a string and
    /// Values being an int
    /// to the EpicPrefs.
    /// See the parameters for more information on what to pass.
    /// </summary>
    /// <param name="name">The name under which the pref is saved, and can be retrieved with.</param>
    /// <param name="value">The value that is saved.</param>
    /// <param name="encrypted">Whether to use encryption or not. Default is false and can be left away in that case.</param>
    /// <returns>Returns a bool stating if the saving has been successfull.</returns>
    public static bool SetDict(string name, Dictionary<string, int> value, bool encrypted = false)
    {
        if (encrypted)
        {
            Dictionary<string, string> encryptedDict = new Dictionary<string, string>();
            foreach (KeyValuePair<string, int> t in value)
            {
                encryptedDict[Cryptor.Encrypt(t.Key)] = Cryptor.Encrypt(t.Value);
            }
            return Serializer.Serialize(name, encryptedDict, Serializer.SerializationTypes.DictI, encrypted);
        }
        else
            return Serializer.Serialize(name, value, Serializer.SerializationTypes.DictI, encrypted);
    }
    /// <summary>
    /// Call this function to save a Dictionary with its
    /// Keys being a string and
    /// Values being a float
    /// to the EpicPrefs.
    /// See the parameters for more information on what to pass.
    /// </summary>
    /// <param name="name">The name under which the pref is saved, and can be retrieved with.</param>
    /// <param name="value">The value that is saved.</param>
    /// <param name="encrypted">Whether to use encryption or not. Default is false and can be left away in that case.</param>
    /// <returns>Returns a bool stating if the saving has been successfull.</returns>
    public static bool SetDict(string name, Dictionary<string, float> value, bool encrypted = false)
    {
        if (encrypted)
        {
            Dictionary<string, string> encryptedDict = new Dictionary<string, string>();
            foreach (KeyValuePair<string, float> t in value)
            {
                encryptedDict[Cryptor.Encrypt(t.Key)] = Cryptor.Encrypt(t.Value);
            }
            return Serializer.Serialize(name, encryptedDict, Serializer.SerializationTypes.DictF, encrypted);
        }
        else
            return Serializer.Serialize(name, value, Serializer.SerializationTypes.DictF, encrypted);
    }
    /// <summary>
    /// Call this function to save a Dictionary with its
    /// Keys being a string and
    /// Values being a long
    /// to the EpicPrefs.
    /// See the parameters for more information on what to pass.
    /// </summary>
    /// <param name="name">The name under which the pref is saved, and can be retrieved with.</param>
    /// <param name="value">The value that is saved.</param>
    /// <param name="encrypted">Whether to use encryption or not. Default is false and can be left away in that case.</param>
    /// <returns>Returns a bool stating if the saving has been successfull.</returns>
    public static bool SetDict(string name, Dictionary<string, long> value, bool encrypted = false)
    {
        if (encrypted)
        {
            Dictionary<string, string> encryptedDict = new Dictionary<string, string>();
            foreach (KeyValuePair<string, long> t in value)
            {
                encryptedDict[Cryptor.Encrypt(t.Key)] = Cryptor.Encrypt(t.Value);
            }
            return Serializer.Serialize(name, encryptedDict, Serializer.SerializationTypes.DictL, encrypted);
        }
        else
            return Serializer.Serialize(name, value, Serializer.SerializationTypes.DictL, encrypted);
    }
    /// <summary>
    /// Call this function to save a Dictionary with its
    /// Keys being a string and
    /// Values being a double
    /// to the EpicPrefs.
    /// See the parameters for more information on what to pass.
    /// </summary>
    /// <param name="name">The name under which the pref is saved, and can be retrieved with.</param>
    /// <param name="value">The value that is saved.</param>
    /// <param name="encrypted">Whether to use encryption or not. Default is false and can be left away in that case.</param>
    /// <returns>Returns a bool stating if the saving has been successfull.</returns>
    public static bool SetDict(string name, Dictionary<string, double> value, bool encrypted = false)
    {
        if (encrypted)
        {
            Dictionary<string, string> encryptedDict = new Dictionary<string, string>();
            foreach (KeyValuePair<string, double> t in value)
            {
                encryptedDict[Cryptor.Encrypt(t.Key)] = Cryptor.Encrypt(t.Value);
            }
            return Serializer.Serialize(name, encryptedDict, Serializer.SerializationTypes.DictD, encrypted);
        }
        else
            return Serializer.Serialize(name, value, Serializer.SerializationTypes.DictD, encrypted);
    }
    /// <summary>
    /// Call this function to save a Dictionary with its
    /// Keys being a string and
    /// Values being a bool
    /// to the EpicPrefs.
    /// See the parameters for more information on what to pass.
    /// </summary>
    /// <param name="name">The name under which the pref is saved, and can be retrieved with.</param>
    /// <param name="value">The value that is saved.</param>
    /// <param name="encrypted">Whether to use encryption or not. Default is false and can be left away in that case.</param>
    /// <returns>Returns a bool stating if the saving has been successfull.</returns>
    public static bool SetDict(string name, Dictionary<string, bool> value, bool encrypted = false)
    {        
        if (encrypted) {
            Dictionary<string, string> encryptedDict = new Dictionary<string, string>();
            foreach(KeyValuePair<string,bool> t in value)
            {
                encryptedDict[Cryptor.Encrypt(t.Key)] = Cryptor.Encrypt(t.Value);
            }
            return Serializer.Serialize(name, encryptedDict, Serializer.SerializationTypes.DictB, encrypted);
        }
        else
            return Serializer.Serialize(name, value, Serializer.SerializationTypes.DictB, encrypted);
    }
    /// <summary>
    /// Call this function to retrieve a Dictionary with its 
    /// Keys being strings and its
    /// Values being strings
    /// from the EpicPrefs.
    /// If the Pref does not exist null is returned.
    /// </summary>
    /// <param name="name">The name under which the EpicPref has previously been saved.</param>
    /// <param name="encrypted">Whether the EpicPref was previoulsy encrypted or not.</param>
    /// <returns>Returns the previously saved Dictionary.</returns>
    public static Dictionary<string, string> GetDictStringString(string name, bool encrypted = false)
    {
        Dictionary<string, string> decryptedDict = new Dictionary<string, string>();
        if (encrypted)
        {
            var newDict = (Dictionary<string, string>)(Serializer.Deserialize(name, Serializer.SerializationTypes.Dict, encrypted));
            if(newDict == null)
                return null;
            if(newDict != null)
                foreach (KeyValuePair<string, string> t in newDict)
                {
                    decryptedDict[Cryptor.Decrypt(t.Key,Serializer.SerializationTypes.String).ToString()] = Cryptor.Decrypt(t.Value, Serializer.SerializationTypes.String).ToString();
                }
        }
        else
        {
            var newDict = (Dictionary<string, string>)(Serializer.Deserialize(name, Serializer.SerializationTypes.Dict, encrypted));
            if(newDict == null)
                return null;
            foreach (KeyValuePair<string, string> t in newDict)
            {
                decryptedDict[t.Key] = t.Value;
            }
        }
        return decryptedDict;
    }
    /// <summary>
    /// Call this function to retrieve a Dictionary with its 
    /// Keys being strings and its
    /// Values being strings
    /// from the EpicPrefs.
    /// If the Pref does not exist the default value is returned.
    /// </summary>
    /// <param name="name">The name under which the EpicPref has previously been saved.</param>
    /// <param name="defaultValue">The default value to be returned if the EpicPref is not found.</param>
    /// <param name="encrypted">Whether the EpicPref was previoulsy encrypted or not.</param>
    /// <returns>Returns the previously saved Dictionary.</returns>
    public static Dictionary<string, string> GetDictStringString(string name,Dictionary<string,string> defaultValue, bool encrypted = false)
    {
        Dictionary<string, string> decryptedDict = new Dictionary<string, string>();
        if (encrypted)
        {
            var newDict = (Dictionary<string, string>)(Serializer.Deserialize(name, Serializer.SerializationTypes.Dict, encrypted));
            if(newDict == null)
                return defaultValue;
            foreach (KeyValuePair<string, string> t in newDict)
            {                
                decryptedDict[Cryptor.Decrypt(t.Key, Serializer.SerializationTypes.String).ToString()] = Cryptor.Decrypt(t.Value, Serializer.SerializationTypes.String).ToString();
            }
        }
        else
        {
            var newDict = (Dictionary<string, string>)(Serializer.Deserialize(name, Serializer.SerializationTypes.Dict, encrypted));
            if(newDict == null)
                return defaultValue;
            foreach (KeyValuePair<string, string> t in newDict)
            {
                decryptedDict[t.Key] = t.Value;
            }
        }
        if (decryptedDict.Count == 0)
        {
            decryptedDict = defaultValue;
        }
        return decryptedDict;
    }
    /// <summary>
    /// Call this function to retrieve a Dictionary with its 
    /// Keys being strings and its
    /// Values being integers
    /// from the EpicPrefs.
    /// If the Pref does not exist null is returned.
    /// </summary>
    /// <param name="name">The name under which the EpicPref has previously been saved.</param>
    /// <param name="encrypted">Whether the EpicPref was previoulsy encrypted or not.</param>
    /// <returns>Returns the previously saved Dictionary.</returns>
    public static Dictionary<string, int> GetDictStringInt(string name, bool encrypted = false)
    {
        Dictionary<string, int> decryptedDict = new Dictionary<string, int>();
        if (encrypted)
        {
            var newDict = (Dictionary<string, string>)(Serializer.Deserialize(name, Serializer.SerializationTypes.Dict, encrypted));
            if(newDict == null)
                return null;
            foreach (KeyValuePair<string, string> t in newDict)
            {
                decryptedDict[Cryptor.Decrypt(t.Key, Serializer.SerializationTypes.String).ToString()] = Int32.Parse(Cryptor.Decrypt(t.Value, Serializer.SerializationTypes.String).ToString());
            }
        }
        else
        {
            var newDict = (Dictionary<string, int>)(Serializer.Deserialize(name, Serializer.SerializationTypes.Dict, encrypted));
            if(newDict == null)
                return null;
            foreach (KeyValuePair<string, int> t in newDict)
            {
                decryptedDict[t.Key] = t.Value;
            }
        }
        return decryptedDict;
    }
    /// <summary>
    /// Call this function to retrieve a Dictionary with its 
    /// Keys being strings and its
    /// Values being strings
    /// from the EpicPrefs.
    /// If the Pref does not exist the default value is returned.
    /// </summary>
    /// <param name="name">The name under which the EpicPref has previously been saved.</param>
    /// <param name="defaultValue">The default value to be returned if the EpicPref is not found.</param>
    /// <param name="encrypted">Whether the EpicPref was previoulsy encrypted or not.</param>
    /// <returns>Returns the previously saved Dictionary.</returns>
    public static Dictionary<string, int> GetDictStringInt(string name, Dictionary<string, int> defaultValue, bool encrypted = false)
    {
        Dictionary<string, int> decryptedDict = new Dictionary<string, int>();
        if (encrypted)
        {
            var newDict = (Dictionary<string, string>)(Serializer.Deserialize(name, Serializer.SerializationTypes.Dict, encrypted));
            if(newDict == null)
                return defaultValue;
            foreach (KeyValuePair<string, string> t in newDict)
            {
                decryptedDict[Cryptor.Decrypt(t.Key, Serializer.SerializationTypes.String).ToString()] = Int32.Parse(Cryptor.Decrypt(t.Value, Serializer.SerializationTypes.String).ToString());
            }
        }
        else
        {
            var newDict = (Dictionary<string, int>)(Serializer.Deserialize(name, Serializer.SerializationTypes.Dict, encrypted));
            if(newDict == null)
                return defaultValue;
            foreach (KeyValuePair<string, int> t in newDict)
            {
                decryptedDict[t.Key] = t.Value;
            }
        }
        return decryptedDict;
    }
    /// <summary>
    /// Call this function to retrieve a Dictionary with its 
    /// Keys being strings and its
    /// Values being floats
    /// from the EpicPrefs.
    /// If the Pref does not exist null is returned.
    /// </summary>
    /// <param name="name">The name under which the EpicPref has previously been saved.</param>
    /// <param name="encrypted">Whether the EpicPref was previoulsy encrypted or not.</param>
    /// <returns>Returns the previously saved Dictionary.</returns>
    public static Dictionary<string, float> GetDictStringFloat(string name, bool encrypted = false)
    {
        Dictionary<string, float> decryptedDict = new Dictionary<string, float>();
        if (encrypted)
        {
            var newDict = (Dictionary<string, string>)(Serializer.Deserialize(name, Serializer.SerializationTypes.Dict, encrypted));
            if(newDict == null)
                return null;
            foreach (KeyValuePair<string, string> t in newDict)
            {
                decryptedDict[Cryptor.Decrypt(t.Key, Serializer.SerializationTypes.String).ToString()] = (float)Convert.ToDouble(Cryptor.Decrypt(t.Value, Serializer.SerializationTypes.String).ToString());
            }
        }
        else
        {
            var newDict = (Dictionary<string, float>)(Serializer.Deserialize(name, Serializer.SerializationTypes.Dict, encrypted));
            if(newDict == null)
                return null;
            foreach (KeyValuePair<string, float> t in newDict)
            {
                decryptedDict[t.Key] = t.Value;
            }
        }
        return decryptedDict;
    }
    /// <summary>
    /// Call this function to retrieve a Dictionary with its 
    /// Keys being strings and its
    /// Values being floats
    /// from the EpicPrefs.
    /// If the Pref does not exist the default value is returned.
    /// </summary>
    /// <param name="name">The name under which the EpicPref has previously been saved.</param>
    /// <param name="defaultValue">The default value to be returned if the EpicPref is not found.</param>
    /// <param name="encrypted">Whether the EpicPref was previoulsy encrypted or not.</param>
    /// <returns>Returns the previously saved Dictionary.</returns>
    public static Dictionary<string, float> GetDictStringFloat(string name, Dictionary<string, float> defaultValue, bool encrypted = false)
    {
        Dictionary<string, float> decryptedDict = new Dictionary<string, float>();
        if (encrypted)
        {
            var newDict = (Dictionary<string, string>)(Serializer.Deserialize(name, Serializer.SerializationTypes.Dict, encrypted));
            if(newDict == null)
                return defaultValue;
            foreach (KeyValuePair<string, string> t in newDict)
            {
                decryptedDict[Cryptor.Decrypt(t.Key, Serializer.SerializationTypes.String).ToString()] = (float)Convert.ToDouble(Cryptor.Decrypt(t.Value, Serializer.SerializationTypes.String).ToString());
            }
        }
        else
        {
            var newDict = (Dictionary<string, float>)(Serializer.Deserialize(name, Serializer.SerializationTypes.Dict, encrypted));
            if(newDict == null)
                return defaultValue;
            foreach (KeyValuePair<string, float> t in newDict)
            {
                decryptedDict[t.Key] = t.Value;
            }
        }
        if (decryptedDict.Count == 0)
        {
            decryptedDict = defaultValue;
        }
        return decryptedDict;
    }
    /// <summary>
    /// Call this function to retrieve a Dictionary with its 
    /// Keys being strings and its
    /// Values being doubles
    /// from the EpicPrefs.
    /// If the Pref does not exist null is returned.
    /// </summary>
    /// <param name="name">The name under which the EpicPref has previously been saved.</param>
    /// <param name="encrypted">Whether the EpicPref was previoulsy encrypted or not.</param>
    /// <returns>Returns the previously saved Dictionary.</returns>
    public static Dictionary<string, double> GetDictStringDouble(string name, bool encrypted = false)
    {
        Dictionary<string, double> decryptedDict = new Dictionary<string, double>();
        if (encrypted)
        {
            var newDict = (Dictionary<string, string>)(Serializer.Deserialize(name, Serializer.SerializationTypes.Dict, encrypted));
            if(newDict == null)
                return null;
            foreach (KeyValuePair<string, string> t in newDict)
            {
                decryptedDict[Cryptor.Decrypt(t.Key, Serializer.SerializationTypes.String).ToString()] = Convert.ToDouble(Cryptor.Decrypt(t.Value, Serializer.SerializationTypes.String).ToString());
            }
        }
        else
        {
            var newDict = (Dictionary<string, double>)(Serializer.Deserialize(name, Serializer.SerializationTypes.Dict, encrypted));
            if(newDict == null)
                return null;
            foreach (KeyValuePair<string, double> t in newDict)
            {
                decryptedDict[t.Key] = t.Value;
            }
        }
        return decryptedDict;
    }
    /// <summary>
    /// Call this function to retrieve a Dictionary with its 
    /// Keys being strings and its
    /// Values being doubles
    /// from the EpicPrefs.
    /// If the Pref does not exist the default value is returned.
    /// </summary>
    /// <param name="name">The name under which the EpicPref has previously been saved.</param>
    /// <param name="defaultValue">The default value to be returned if the EpicPref is not found.</param>
    /// <param name="encrypted">Whether the EpicPref was previoulsy encrypted or not.</param>
    /// <returns>Returns the previously saved Dictionary.</returns>
    public static Dictionary<string, double> GetDictStringDouble(string name, Dictionary<string, double> defaultValue, bool encrypted = false)
    {
        Dictionary<string, double> decryptedDict = new Dictionary<string, double>();
        if (encrypted)
        {
            var newDict = (Dictionary<string, string>)(Serializer.Deserialize(name, Serializer.SerializationTypes.Dict, encrypted));
            if(newDict == null)
                return defaultValue;
            foreach (KeyValuePair<string, string> t in newDict)
            {
                decryptedDict[Cryptor.Decrypt(t.Key, Serializer.SerializationTypes.String).ToString()] = Convert.ToDouble(Cryptor.Decrypt(t.Value, Serializer.SerializationTypes.String).ToString());
            }
        }
        else
        {
            var newDict = (Dictionary<string, double>)(Serializer.Deserialize(name, Serializer.SerializationTypes.Dict, encrypted));
            if(newDict == null)
                return defaultValue;
            foreach (KeyValuePair<string, double> t in newDict)
            {
                decryptedDict[t.Key] = t.Value;
            }
        }
        return decryptedDict;
    }
    /// <summary>
    /// Call this function to retrieve a Dictionary with its 
    /// Keys being strings and its
    /// Values being booleans
    /// from the EpicPrefs.
    /// If the Pref does not exist null is returned.
    /// </summary>
    /// <param name="name">The name under which the EpicPref has previously been saved.</param>
    /// <param name="encrypted">Whether the EpicPref was previoulsy encrypted or not.</param>
    /// <returns>Returns the previously saved Dictionary.</returns>
    public static Dictionary<string, bool> GetDictStringBool(string name, bool encrypted = false)
    {
        Dictionary<string, bool> decryptedDict = new Dictionary<string, bool>();
        if (encrypted)
        {
            var newDict = (Dictionary<string, string>)(Serializer.Deserialize(name, Serializer.SerializationTypes.Dict, encrypted));
            if(newDict == null)
                return null;
            foreach (KeyValuePair<string, string> t in newDict)
            {
                decryptedDict[Cryptor.Decrypt(t.Key, Serializer.SerializationTypes.String).ToString()] = Convert.ToBoolean(Cryptor.Decrypt(t.Value, Serializer.SerializationTypes.String).ToString());
            }
        }
        else
        {
            var newDict = (Dictionary<string, bool>)(Serializer.Deserialize(name, Serializer.SerializationTypes.Dict, encrypted));
            if(newDict == null)
                return null;
            foreach (KeyValuePair<string, bool> t in newDict)
            {
                decryptedDict[t.Key] = t.Value;
            }
        }
        return decryptedDict;
    }
    /// <summary>
    /// Call this function to retrieve a Dictionary with its 
    /// Keys being strings and its
    /// Values being booleans
    /// from the EpicPrefs.
    /// If the Pref does not exist the default value is returned.
    /// </summary>
    /// <param name="name">The name under which the EpicPref has previously been saved.</param>
    /// <param name="defaultValue">The default value to be returned if the EpicPref is not found.</param>
    /// <param name="encrypted">Whether the EpicPref was previoulsy encrypted or not.</param>
    /// <returns>Returns the previously saved Dictionary.</returns>
    public static Dictionary<string, bool> GetDictStringBool(string name, Dictionary<string, bool> defaultValue, bool encrypted = false)
    {
        Dictionary<string, bool> decryptedDict = new Dictionary<string, bool>();
        if (encrypted)
        {
            var newDict = (Dictionary<string, string>)(Serializer.Deserialize(name, Serializer.SerializationTypes.Dict, encrypted));
            if(newDict == null)
                return defaultValue;
            foreach (KeyValuePair<string, string> t in newDict)
            {
                decryptedDict[Cryptor.Decrypt(t.Key, Serializer.SerializationTypes.String).ToString()] = Convert.ToBoolean(Cryptor.Decrypt(t.Value, Serializer.SerializationTypes.String).ToString());
            }
        }
        else
        {
            var newDict = (Dictionary<string, bool>)(Serializer.Deserialize(name, Serializer.SerializationTypes.Dict, encrypted));
            if(newDict == null)
                return defaultValue;
            foreach (KeyValuePair<string, bool> t in newDict)
            {
                decryptedDict[t.Key] = t.Value;
            }
        }
        return decryptedDict;
    }
    /// <summary>
    /// Call this function to retrieve a Dictionary with its 
    /// Keys being strings and its
    /// Values being longs
    /// from the EpicPrefs.
    /// If the Pref does not exist null is returned.
    /// </summary>
    /// <param name="name">The name under which the EpicPref has previously been saved.</param>
    /// <param name="encrypted">Whether the EpicPref was previoulsy encrypted or not.</param>
    /// <returns>Returns the previously saved Dictionary.</returns>
    public static Dictionary<string, long> GetDictStringLong(string name, bool encrypted = false)
    {
        Dictionary<string, long> decryptedDict = new Dictionary<string, long>();
        if (encrypted)
        {
            var newDict = (Dictionary<string, string>)(Serializer.Deserialize(name, Serializer.SerializationTypes.Dict, encrypted));
            if(newDict == null)
                return null;
            foreach (KeyValuePair<string, string> t in newDict)
            {
                decryptedDict[Cryptor.Decrypt(t.Key, Serializer.SerializationTypes.String).ToString()] = (long)Convert.ToDouble(Cryptor.Decrypt(t.Value, Serializer.SerializationTypes.String).ToString());
            }
        }
        else
        {
            var newDict = (Dictionary<string, long>)(Serializer.Deserialize(name, Serializer.SerializationTypes.Dict, encrypted));
            if(newDict == null)
                return null;
            foreach (KeyValuePair<string, long> t in newDict)
            {
                decryptedDict[t.Key] = t.Value;
            }
        }
        return decryptedDict;
    }
    /// <summary>
    /// Call this function to retrieve a Dictionary with its 
    /// Keys being strings and its
    /// Values being longs
    /// from the EpicPrefs.
    /// If the Pref does not exist the default value is returned.
    /// </summary>
    /// <param name="name">The name under which the EpicPref has previously been saved.</param>
    /// <param name="defaultValue">The default value to be returned if the EpicPref is not found.</param>
    /// <param name="encrypted">Whether the EpicPref was previoulsy encrypted or not.</param>
    /// <returns>Returns the previously saved Dictionary.</returns>
    public static Dictionary<string, long> GetDictStringLong(string name, Dictionary<string, long> defaultValue, bool encrypted = false)
    {
        Dictionary<string, long> decryptedDict = new Dictionary<string, long>();
        if (encrypted)
        {
            var newDict = (Dictionary<string, string>)(Serializer.Deserialize(name, Serializer.SerializationTypes.Dict, encrypted));
            if(newDict == null)
                return defaultValue;
            foreach (KeyValuePair<string, string> t in newDict)
            {
                decryptedDict[Cryptor.Decrypt(t.Key, Serializer.SerializationTypes.String).ToString()] = (long)Convert.ToDouble(Cryptor.Decrypt(t.Value, Serializer.SerializationTypes.String).ToString());
            }
        }
        else
        {
            var newDict = (Dictionary<string, long>)(Serializer.Deserialize(name, Serializer.SerializationTypes.Dict, encrypted));
            if(newDict == null)
                return defaultValue;
            foreach (KeyValuePair<string, long> t in newDict)
            {
                decryptedDict[t.Key] = t.Value;
            }
        }
        if (decryptedDict.Count == 0)
        {
            decryptedDict = defaultValue;
        }
        return decryptedDict;
    }
    /// <summary>
    /// Deletes a Dictionary permanently.
    /// </summary>
    /// <param name="name">The name of the dictionary.</param>
    /// <param name="encrypted">Whether it was encrypted or not.</param>
    public static void DeleteDict(string name, bool encrypted)
    {
        Serializer.Delete(name, Serializer.SerializationTypes.Dict, encrypted);
    }

    #endregion
    #endregion
    #region Transform
    /*private static bool SetTransform(string name, Transform value, bool encrypted = false)
    {
        return Serializer.Serialize(name, value, Serializer.SerializationTypes.Transform,encrypted);
    }
    private static Transform GetTransform(string name, bool encrypted = false)
    {
        Transform newTransform = (Transform)Serializer.Deserialize(name, Serializer.SerializationTypes.Transform, encrypted);
        return newTransform;
    }
    private static Transform GetTransform(string name, Transform defaultValue, bool encrypted = false)
    {
        Transform newTransform = (Transform)Serializer.Deserialize(name, Serializer.SerializationTypes.Transform, encrypted);
        if (newTransform == null)
            return defaultValue;
        else
            return newTransform;
    }
    private static void DeleteTransform(string name, bool encrypted)
    {
        Serializer.Delete(name, Serializer.SerializationTypes.Transform, encrypted);
    }*/
    #endregion
    #region Color
    /// <summary>
    /// Call this function to save a Color to the EpicPrefs.
    /// See the parameters for more information on what to pass.
    /// </summary>
    /// <param name="name">The name under which the pref is saved, and can be retrieved with.</param>
    /// <param name="value">The value that is saved.</param>
    /// <param name="encrypted">Whether to use encryption or not. Default is false and can be left away in that case.</param>
    /// <returns>Returns a bool stating if the saving has been successfull.</returns>
    public static bool SetColor(string name, Color value, bool encrypted = false)
    {
        return Serializer.Serialize(name, value, Serializer.SerializationTypes.Color, encrypted);
    }
    /// <summary>
    /// Call this function to retrieve a color from the EpicPrefs.
    /// If the Pref does not exist white is returned.
    /// </summary>
    /// <param name="name">The name under which the EpicPref has previously been saved.</param>
    /// <param name="encrypted">Whether the EpicPref was previoulsy encrypted or not.</param>
    /// <returns>Returns the previously saved color.</returns>
    public static Color GetColor(string name, bool encrypted = false)
    {
        Color newColor = (Color)Serializer.Deserialize(name, Serializer.SerializationTypes.Color, encrypted);
        return newColor;
    }
    /// <summary>
    /// Call this function to retrieve a color from the EpicPrefs.
    /// If the Pref does not exist a default color is returned.
    /// </summary>
    /// <param name="name">The name under which the EpicPref has previously been saved.</param>
    /// <param name="defaultValue">The default returned if the EpicPref is not found.</param>
    /// <param name="encrypted">Whether the EpicPref was previoulsy encrypted or not.</param>
    /// <returns>Returns the previously saved color or the default value if not found.</returns>
    public static Color GetColor(string name, Color defaultValue, bool encrypted = false)
    {
        Color? newColor = (Color?)Serializer.Deserialize(name, Serializer.SerializationTypes.Color, encrypted);
        if (newColor == null)
            return defaultValue;
        else
            return (Color)newColor;
    }
    /// <summary>
    /// Deletes an EpicPref permanently.
    /// </summary>
    /// <param name="name">The name of the pref.</param>
    /// <param name="encrypted">Whether it was encrypted or not.</param>
    public static void DeleteColor(string name, bool encrypted)
    {
        Serializer.Delete(name, Serializer.SerializationTypes.Color, encrypted);
    }
    #endregion
    #region Quaternion
    /// <summary>
    /// Call this function to save a Quaternion to the EpicPrefs.
    /// See the parameters for more information on what to pass.
    /// </summary>
    /// <param name="name">The name under which the pref is saved, and can be retrieved with.</param>
    /// <param name="value">The value that is saved.</param>
    /// <param name="encrypted">Whether to use encryption or not. Default is false and can be left away in that case.</param>
    /// <returns>Returns a bool stating if the saving has been successfull.</returns>
    public static bool SetQuaternion(string name,Quaternion value, bool encrypted = false)
    {
        return Serializer.Serialize(name, value, Serializer.SerializationTypes.Quaternion,encrypted);
    }
    /// <summary>
    /// Call this function to retrieve a Quaternion from the EpicPrefs.
    /// If the Pref does not exist a zero rotation is returned.
    /// </summary>
    /// <param name="name">The name under which the EpicPref has previously been saved.</param>
    /// <param name="encrypted">Whether the EpicPref was previoulsy encrypted or not.</param>
    /// <returns>Returns the previously saved Quaternion.</returns>
    public static Quaternion GetQuaternion(string name, bool encrypted = false)
    {
        Quaternion quat = (Quaternion)Serializer.Deserialize(name, Serializer.SerializationTypes.Quaternion, encrypted);
        return quat;
    }
    /// <summary>
    /// Call this function to retrieve a Quaternion from the EpicPrefs.
    /// If the Pref does not exist a default Quaternion is returned.
    /// </summary>
    /// <param name="name">The name under which the EpicPref has previously been saved.</param>
    /// <param name="defaultValue">The default returned if the EpicPref is not found.</param>
    /// <param name="encrypted">Whether the EpicPref was previoulsy encrypted or not.</param>
    /// <returns>Returns the previously saved Quaternion or the default value if not found.</returns>
    public static Quaternion GetQuaternion(string name,Quaternion defaultValue, bool encrypted = false)
    {
        Quaternion? quat = (Quaternion?)Serializer.Deserialize(name, Serializer.SerializationTypes.Quaternion, encrypted);
        if (quat == null)
            return defaultValue;
        else
            return (Quaternion)quat;
    }
    /// <summary>
    /// Deletes an EpicPref permanently.
    /// </summary>
    /// <param name="name">The name of the pref.</param>
    /// <param name="encrypted">Whether it was encrypted or not.</param>
    public static void DeleteQuaternion(string name, bool encrypted)
    {
        Serializer.Delete(name, Serializer.SerializationTypes.Quaternion, encrypted);
    }
    #endregion
    #region Vector
    /// <summary>
    /// Call this function to save a Vector2 to the EpicPrefs.
    /// See the parameters for more information on what to pass.
    /// </summary>
    /// <param name="name">The name under which the pref is saved, and can be retrieved with.</param>
    /// <param name="value">The value that is saved.</param>
    /// <param name="encrypted">Whether to use encryption or not. Default is false and can be left away in that case.</param>
    /// <returns>Returns a bool stating if the saving has been successfull.</returns>
    public static bool SetVector2(string name, Vector2 value, bool encrypted = false)
    {
        return Serializer.Serialize(name, value, Serializer.SerializationTypes.Vector2,encrypted);
    }
    /// <summary>
    /// Call this function to retrieve a Vector2 from the EpicPrefs.
    /// If the Pref does not exist a zero Vector2 is returned.
    /// </summary>
    /// <param name="name">The name under which the EpicPref has previously been saved.</param>
    /// <param name="encrypted">Whether the EpicPref was previoulsy encrypted or not.</param>
    /// <returns>Returns the previously saved Vector2.</returns>
    public static Vector2 GetVector2(string name, bool encrypted = false)
    {
        Vector2 vec = (Vector2)Serializer.Deserialize(name, Serializer.SerializationTypes.Vector2, encrypted);
        return vec;
    }
    /// <summary>
    /// Call this function to retrieve a Vector2 from the EpicPrefs.
    /// If the Pref does not exist a default Vector2 is returned.
    /// </summary>
    /// <param name="name">The name under which the EpicPref has previously been saved.</param>
    /// <param name="defaultValue">The default returned if the EpicPref is not found.</param>
    /// <param name="encrypted">Whether the EpicPref was previoulsy encrypted or not.</param>
    /// <returns>Returns the previously saved Vector2 or the default value if not found.</returns>
    public static Vector2 GetVector2(string name, Vector2 defaultValue, bool encrypted = false)
    {
        Vector2? vec = (Vector2?)Serializer.Deserialize(name, Serializer.SerializationTypes.Vector2, encrypted);
        if (vec == null)
            return defaultValue;
        else
            return (Vector2)vec;
    }
    /// <summary>
    /// Deletes an EpicPref permanently.
    /// </summary>
    /// <param name="name">The name of the pref.</param>
    /// <param name="encrypted">Whether it was encrypted or not.</param>
    public static void DeleteVector2(string name, bool encrypted)
    {
        Serializer.Delete(name, Serializer.SerializationTypes.Vector2, encrypted);
    }
    /// <summary>
    /// Call this function to save a Vector3 to the EpicPrefs.
    /// See the parameters for more information on what to pass.
    /// </summary>
    /// <param name="name">The name under which the pref is saved, and can be retrieved with.</param>
    /// <param name="value">The value that is saved.</param>
    /// <param name="encrypted">Whether to use encryption or not. Default is false and can be left away in that case.</param>
    /// <returns>Returns a bool stating if the saving has been successfull.</returns>
    public static bool SetVector3(string name, Vector3 value, bool encrypted = false)
    {
        return Serializer.Serialize(name, value, Serializer.SerializationTypes.Vector3,encrypted);
    }
    /// <summary>
    /// Call this function to retrieve a Vector3 from the EpicPrefs.
    /// If the Pref does not exist a zero Vector3 is returned.
    /// </summary>
    /// <param name="name">The name under which the EpicPref has previously been saved.</param>
    /// <param name="encrypted">Whether the EpicPref was previoulsy encrypted or not.</param>
    /// <returns>Returns the previously saved Vector3.</returns>
    public static Vector3 GetVector3(string name, bool encrypted = false)
    {
        Vector3 vec = (Vector3)Serializer.Deserialize(name, Serializer.SerializationTypes.Vector3, encrypted);
        return vec;
    }
    /// <summary>
    /// Call this function to retrieve a Vector3 from the EpicPrefs.
    /// If the Pref does not exist a default Vector3 is returned.
    /// </summary>
    /// <param name="name">The name under which the EpicPref has previously been saved.</param>
    /// <param name="defaultValue">The default returned if the EpicPref is not found.</param>
    /// <param name="encrypted">Whether the EpicPref was previoulsy encrypted or not.</param>
    /// <returns>Returns the previously saved Vector3 or the default value if not found.</returns>
    public static Vector3 GetVector3(string name, Vector3 defaultValue, bool encrypted = false)
    {
        Vector3? vec = (Vector3?)Serializer.Deserialize(name, Serializer.SerializationTypes.Vector3, encrypted);
        if (vec == null)
            return defaultValue;
        else
            return (Vector3)vec;
    }
    /// <summary>
    /// Deletes an EpicPref permanently.
    /// </summary>
    /// <param name="name">The name of the pref.</param>
    /// <param name="encrypted">Whether it was encrypted or not.</param>
    public static void DeleteVector3(string name, bool encrypted)
    {
        Serializer.Delete(name, Serializer.SerializationTypes.Vector3, encrypted);
    }
    /// <summary>
    /// Call this function to save a Vector4 to the EpicPrefs.
    /// See the parameters for more information on what to pass.
    /// </summary>
    /// <param name="name">The name under which the pref is saved, and can be retrieved with.</param>
    /// <param name="value">The value that is saved.</param>
    /// <param name="encrypted">Whether to use encryption or not. Default is false and can be left away in that case.</param>
    /// <returns>Returns a bool stating if the saving has been successfull.</returns>
    public static bool SetVector4(string name, Vector4 value, bool encrypted = false)
    {
        return Serializer.Serialize(name, value, Serializer.SerializationTypes.Vector4,encrypted);
    }
    /// <summary>
    /// Call this function to retrieve a Vector4 from the EpicPrefs.
    /// If the Pref does not exist a zero Vector4 is returned.
    /// </summary>
    /// <param name="name">The name under which the EpicPref has previously been saved.</param>
    /// <param name="encrypted">Whether the EpicPref was previoulsy encrypted or not.</param>
    /// <returns>Returns the previously saved Vector4.</returns>
    public static Vector4 GetVector4(string name, bool encrypted = false)
    {
        Vector4 vec = (Vector4)Serializer.Deserialize(name, Serializer.SerializationTypes.Vector4, encrypted);
        return vec;
    }
    /// <summary>
    /// Call this function to retrieve a Vector4 from the EpicPrefs.
    /// If the Pref does not exist a default Vector4 is returned.
    /// </summary>
    /// <param name="name">The name under which the EpicPref has previously been saved.</param>
    /// <param name="defaultValue">The default returned if the EpicPref is not found.</param>
    /// <param name="encrypted">Whether the EpicPref was previoulsy encrypted or not.</param>
    /// <returns>Returns the previously saved Vector4 or the default value if not found.</returns>
    public static Vector4 GetVector4(string name, Vector4 defaultValue, bool encrypted = false)
    {
        Vector4? vec = (Vector4?)Serializer.Deserialize(name, Serializer.SerializationTypes.Vector4, encrypted);
        if (vec == null)
            return defaultValue;
        else
            return (Vector4)vec;
    }
    /// <summary>
    /// Deletes an EpicPref permanently.
    /// </summary>
    /// <param name="name">The name of the pref.</param>
    /// <param name="encrypted">Whether it was encrypted or not.</param>
    public static void DeleteVector4(string name, bool encrypted)
    {
        Serializer.Delete(name, Serializer.SerializationTypes.Vector4, encrypted);
    }
    #endregion
    #region List
    /// <summary>
    /// Call this function to save a List of strings to the EpicPrefs.
    /// See the parameters for more information on what to pass.
    /// </summary>
    /// <param name="name">The name under which the pref is saved, and can be retrieved with.</param>
    /// <param name="value">The value that is saved.</param>
    /// <param name="encrypted">Whether to use encryption or not. Default is false and can be left away in that case.</param>
    /// <returns>Returns a bool stating if the saving has been successfull.</returns>
    public static bool SetList(string name,List<string> value, bool encrypted = false)
    {
        List<object> newList = new List<object>();
        foreach (string s in value)
        {
            if (!encrypted)
                newList.Add((object)s);  // The cast is performed implicitly even if omitted
            else
                newList.Add((object)Cryptor.Encrypt(s));
        }
        return Serializer.Serialize(name, newList, Serializer.SerializationTypes.ListS,encrypted);
    }
    /// <summary>
    /// Call this function to save a List of floats to the EpicPrefs.
    /// See the parameters for more information on what to pass.
    /// </summary>
    /// <param name="name">The name under which the pref is saved, and can be retrieved with.</param>
    /// <param name="value">The value that is saved.</param>
    /// <param name="encrypted">Whether to use encryption or not. Default is false and can be left away in that case.</param>
    /// <returns>Returns a bool stating if the saving has been successfull.</returns>
    public static bool SetList(string name, List<float> value, bool encrypted = false)
    {
        List<object> newList = new List<object>();
        foreach (float s in value)
        {
            if (!encrypted)
                newList.Add((object)s);  // The cast is performed implicitly even if omitted
            else
                newList.Add((object)Cryptor.Encrypt(s));
        }
        return Serializer.Serialize(name, newList, Serializer.SerializationTypes.ListF, encrypted);
    }
    /// <summary>
    /// Call this function to save a List of bools to the EpicPrefs.
    /// See the parameters for more information on what to pass.
    /// </summary>
    /// <param name="name">The name under which the pref is saved, and can be retrieved with.</param>
    /// <param name="value">The value that is saved.</param>
    /// <param name="encrypted">Whether to use encryption or not. Default is false and can be left away in that case.</param>
    /// <returns>Returns a bool stating if the saving has been successfull.</returns>
    public static bool SetList(string name, List<bool> value, bool encrypted = false)
    {
        List<object> newList = new List<object>();
        foreach (bool s in value)
        {
            if (!encrypted)
                newList.Add((object)s);  // The cast is performed implicitly even if omitted
            else
                newList.Add((object)Cryptor.Encrypt(s));
        }
        return Serializer.Serialize(name, newList, Serializer.SerializationTypes.ListB, encrypted);
    }
    /// <summary>
    /// Call this function to save a List of integers to the EpicPrefs.
    /// See the parameters for more information on what to pass.
    /// </summary>
    /// <param name="name">The name under which the pref is saved, and can be retrieved with.</param>
    /// <param name="value">The value that is saved.</param>
    /// <param name="encrypted">Whether to use encryption or not. Default is false and can be left away in that case.</param>
    /// <returns>Returns a bool stating if the saving has been successfull.</returns>
    public static bool SetList(string name, List<int> value, bool encrypted = false)
    {
        List<object> newList = new List<object>();
        foreach (int s in value)
        {
            if (!encrypted)
                newList.Add((object)s);  // The cast is performed implicitly even if omitted
            else
                newList.Add((object)Cryptor.Encrypt(s));
        }
        return Serializer.Serialize(name, newList, Serializer.SerializationTypes.ListI, encrypted);
    }
    /// <summary>
    /// Call this function to save a List of doubles to the EpicPrefs.
    /// See the parameters for more information on what to pass.
    /// </summary>
    /// <param name="name">The name under which the pref is saved, and can be retrieved with.</param>
    /// <param name="value">The value that is saved.</param>
    /// <param name="encrypted">Whether to use encryption or not. Default is false and can be left away in that case.</param>
    /// <returns>Returns a bool stating if the saving has been successfull.</returns>
    public static bool SetList(string name, List<double> value, bool encrypted = false)
    {
        List<object> newList = new List<object>();
        foreach (double s in value)
        {
            if (!encrypted)
                newList.Add((object)s);  // The cast is performed implicitly even if omitted
            else
                newList.Add((object)Cryptor.Encrypt(s));
        }
        return Serializer.Serialize(name, newList, Serializer.SerializationTypes.ListD, encrypted);
    }
    /// <summary>
    /// Call this function to save a List of longs to the EpicPrefs.
    /// See the parameters for more information on what to pass.
    /// </summary>
    /// <param name="name">The name under which the pref is saved, and can be retrieved with.</param>
    /// <param name="value">The value that is saved.</param>
    /// <param name="encrypted">Whether to use encryption or not. Default is false and can be left away in that case.</param>
    /// <returns>Returns a bool stating if the saving has been successfull.</returns>
    public static bool SetList(string name, List<long> value, bool encrypted = false)
    {
        List<object> newList = new List<object>();
        foreach (long s in value)
        {
            if (!encrypted)
                newList.Add((object)s);  // The cast is performed implicitly even if omitted
            else
                newList.Add((object)Cryptor.Encrypt(s));
        }
        return Serializer.Serialize(name, newList, Serializer.SerializationTypes.ListL, encrypted);
    }
    /// <summary>
    /// Call this function to retrieve a List of strings from the EpicPrefs.
    /// If the Pref does not exist a null is returned.
    /// </summary>
    /// <param name="name">The name under which the EpicPref has previously been saved.</param>
    /// <param name="encrypted">Whether the EpicPref was previoulsy encrypted or not.</param>
    /// <returns>Returns the previously saved List of strings.</returns>
    public static List<string> GetListString(string name, bool encrypted = false)
    {
        List<object> list = (List<object>)Serializer.Deserialize(name, Serializer.SerializationTypes.List, encrypted);
        if(!encrypted)
            return list.ConvertAll(obj=> obj.ToString());
        else
            return list.ConvertAll(obj => Cryptor.Decrypt(obj.ToString(),Serializer.SerializationTypes.String).ToString());
    }
    /// <summary>
    /// Call this function to retrieve a List of strings from the EpicPrefs.
    /// If the Pref does not exist the default value is returned.
    /// </summary>
    /// <param name="name">The name under which the EpicPref has previously been saved.</param>
    /// <param name="defaultValue">The default value that will be returned if the EpicPref does not exist.</param>
    /// <param name="encrypted">Whether the EpicPref was previoulsy encrypted or not.</param>
    /// <returns>Returns the previously saved List of strings.</returns>
    public static List<string> GetListString(string name,List<string> defaultValue, bool encrypted = false)
    {
        List<object> list = (List<object>)Serializer.Deserialize(name, Serializer.SerializationTypes.List, encrypted);
        if (list == null)
            return defaultValue;
        else
            if (!encrypted)
                return list.ConvertAll(obj => obj.ToString());
            else
                return list.ConvertAll(obj => Cryptor.Decrypt(obj.ToString(), Serializer.SerializationTypes.String).ToString());
    }
    /// <summary>
    /// Call this function to retrieve a List of floats from the EpicPrefs.
    /// If the Pref does not exist a null is returned.
    /// </summary>
    /// <param name="name">The name under which the EpicPref has previously been saved.</param>
    /// <param name="encrypted">Whether the EpicPref was previoulsy encrypted or not.</param>
    /// <returns>Returns the previously saved List of floats.</returns>
    public static List<float> GetListFloat(string name, bool encrypted = false)
    {
        List<object> list = (List<object>)Serializer.Deserialize(name, Serializer.SerializationTypes.List, encrypted);        
        if (!encrypted)
            return list.ConvertAll(obj => (float)Convert.ToDecimal(obj));
        else
            return list.ConvertAll(obj => (float)Convert.ToDecimal(Cryptor.Decrypt(obj.ToString(), Serializer.SerializationTypes.String)));
    }
    /// <summary>
    /// Call this function to retrieve a List of floats from the EpicPrefs.
    /// If the Pref does not exist the default value is returned.
    /// </summary>
    /// <param name="name">The name under which the EpicPref has previously been saved.</param>
    /// <param name="defaultValue">The default value that will be returned if the EpicPref does not exist.</param>
    /// <param name="encrypted">Whether the EpicPref was previoulsy encrypted or not.</param>
    /// <returns>Returns the previously saved List of floats.</returns>
    public static List<float> GetListFloat(string name, List<float> defaultValue, bool encrypted = false)
    {
        List<object> list = (List<object>)Serializer.Deserialize(name, Serializer.SerializationTypes.List, encrypted);
        if (list == null)
            return defaultValue;
        else
            if(!encrypted)
                return list.ConvertAll(obj => (float)Convert.ToDecimal(obj));
            else
                return list.ConvertAll(obj => (float)Convert.ToDecimal(Cryptor.Decrypt(obj.ToString(), Serializer.SerializationTypes.String)));
    }
    /// <summary>
    /// Call this function to retrieve a List of doubles from the EpicPrefs.
    /// If the Pref does not exist a null is returned.
    /// </summary>
    /// <param name="name">The name under which the EpicPref has previously been saved.</param>
    /// <param name="encrypted">Whether the EpicPref was previoulsy encrypted or not.</param>
    /// <returns>Returns the previously saved List of doubles.</returns>
    public static List<double> GetListDouble(string name, bool encrypted = false)
    {
        List<object> list = (List<object>)Serializer.Deserialize(name, Serializer.SerializationTypes.List, encrypted);
        if (!encrypted)
            return list.ConvertAll(obj => Convert.ToDouble(obj));
        else
            return list.ConvertAll(obj => Convert.ToDouble(Cryptor.Decrypt(obj.ToString(), Serializer.SerializationTypes.String)));
    }
    /// <summary>
    /// Call this function to retrieve a List of doubles from the EpicPrefs.
    /// If the Pref does not exist the default value is returned.
    /// </summary>
    /// <param name="name">The name under which the EpicPref has previously been saved.</param>
    /// <param name="defaultValue">The default value that will be returned if the EpicPref does not exist.</param>
    /// <param name="encrypted">Whether the EpicPref was previoulsy encrypted or not.</param>
    /// <returns>Returns the previously saved List of doubles.</returns>
    public static List<double> GetListDouble(string name, List<double> defaultValue, bool encrypted = false)
    {
        List<object> list = (List<object>)Serializer.Deserialize(name, Serializer.SerializationTypes.List, encrypted);
        if (list == null)
            return defaultValue;
        else
            if (!encrypted)
                return list.ConvertAll(obj => Convert.ToDouble(obj));
            else
                return list.ConvertAll(obj => Convert.ToDouble(Cryptor.Decrypt(obj.ToString(), Serializer.SerializationTypes.String)));
    }
    /// <summary>
    /// Call this function to retrieve a List of booleans from the EpicPrefs.
    /// If the Pref does not exist a null is returned.
    /// </summary>
    /// <param name="name">The name under which the EpicPref has previously been saved.</param>
    /// <param name="encrypted">Whether the EpicPref was previoulsy encrypted or not.</param>
    /// <returns>Returns the previously saved List of booleans.</returns>
    public static List<bool> GetListBool(string name, bool encrypted = false)
    {
        List<object> list = (List<object>)Serializer.Deserialize(name, Serializer.SerializationTypes.List, encrypted);
        if (!encrypted)
            return list.ConvertAll(obj => Convert.ToBoolean(obj));
        else
            return list.ConvertAll(obj => Convert.ToBoolean(Cryptor.Decrypt(obj.ToString(), Serializer.SerializationTypes.String)));
    }
    /// <summary>
    /// Call this function to retrieve a List of booleans from the EpicPrefs.
    /// If the Pref does not exist the default value is returned.
    /// </summary>
    /// <param name="name">The name under which the EpicPref has previously been saved.</param>
    /// <param name="defaultValue">The default value that will be returned if the EpicPref does not exist.</param>
    /// <param name="encrypted">Whether the EpicPref was previoulsy encrypted or not.</param>
    /// <returns>Returns the previously saved List of booleans.</returns>
    public static List<bool> GetListBool(string name, List<bool> defaultValue, bool encrypted = false)
    {
        List<object> list = (List<object>)Serializer.Deserialize(name, Serializer.SerializationTypes.List, encrypted);
        if (list == null)
            return defaultValue;
        else
            if (!encrypted)
                return list.ConvertAll(obj => Convert.ToBoolean(obj));
            else
                return list.ConvertAll(obj => Convert.ToBoolean(Cryptor.Decrypt(obj.ToString(), Serializer.SerializationTypes.String)));
    }
    /// <summary>
    /// Call this function to retrieve a List of integers from the EpicPrefs.
    /// If the Pref does not exist a null is returned.
    /// </summary>
    /// <param name="name">The name under which the EpicPref has previously been saved.</param>
    /// <param name="encrypted">Whether the EpicPref was previoulsy encrypted or not.</param>
    /// <returns>Returns the previously saved List of integers.</returns>
    public static List<int> GetListInt(string name, bool encrypted = false)
    {
        List<object> list = (List<object>)Serializer.Deserialize(name, Serializer.SerializationTypes.List, encrypted);
        if (!encrypted)
            return list.ConvertAll(obj => Int32.Parse(obj.ToString()));
        else
            return list.ConvertAll(obj => Int32.Parse(Cryptor.Decrypt(obj.ToString(), Serializer.SerializationTypes.String).ToString()));
    }
    /// <summary>
    /// Call this function to retrieve a List of integers from the EpicPrefs.
    /// If the Pref does not exist the default value is returned.
    /// </summary>
    /// <param name="name">The name under which the EpicPref has previously been saved.</param>
    /// <param name="defaultValue">The default value that will be returned if the EpicPref does not exist.</param>
    /// <param name="encrypted">Whether the EpicPref was previoulsy encrypted or not.</param>
    /// <returns>Returns the previously saved List of integers.</returns>
    public static List<int> GetListInt(string name, List<int> defaultValue, bool encrypted = false)
    {
        List<object> list = (List<object>)Serializer.Deserialize(name, Serializer.SerializationTypes.List, encrypted);
        if (list == null)
            return defaultValue;
        else
        {
            if (!encrypted)
                return list.ConvertAll(obj => Int32.Parse(obj.ToString()));
            else
                return list.ConvertAll(obj => Int32.Parse(Cryptor.Decrypt(obj.ToString(), Serializer.SerializationTypes.String).ToString()));
        }
    }
    /// <summary>
    /// Call this function to retrieve a List of longs from the EpicPrefs.
    /// If the Pref does not exist a null is returned.
    /// </summary>
    /// <param name="name">The name under which the EpicPref has previously been saved.</param>
    /// <param name="encrypted">Whether the EpicPref was previoulsy encrypted or not.</param>
    /// <returns>Returns the previously saved List of longs.</returns>
    public static List<long> GetListLong(string name, bool encrypted = false)
    {
        List<object> list = (List<object>)Serializer.Deserialize(name, Serializer.SerializationTypes.List, encrypted);
        if (!encrypted)
            return list.ConvertAll(obj => (long)Convert.ToDouble(obj));
        else
            return list.ConvertAll(obj => (long)Convert.ToDouble(Cryptor.Decrypt(obj.ToString(), Serializer.SerializationTypes.String)));
    }
    /// <summary>
    /// Call this function to retrieve a List of longs from the EpicPrefs.
    /// If the Pref does not exist the default value is returned.
    /// </summary>
    /// <param name="name">The name under which the EpicPref has previously been saved.</param>
    /// <param name="defaultValue">The default value that will be returned if the EpicPref does not exist.</param>
    /// <param name="encrypted">Whether the EpicPref was previoulsy encrypted or not.</param>
    /// <returns>Returns the previously saved List of longs.</returns>
    public static List<long> GetListLong(string name, List<long> defaultValue, bool encrypted = false)
    {
        List<object> list = (List<object>)Serializer.Deserialize(name, Serializer.SerializationTypes.List, encrypted);
        if (list == null)
            return defaultValue;
        else
        {
            if (!encrypted)
                return list.ConvertAll(obj => (long)Convert.ToDouble(obj));
            else
                return list.ConvertAll(obj => (long)Convert.ToDouble(Cryptor.Decrypt(obj.ToString(), Serializer.SerializationTypes.String)));
        }
    }
    /// <summary>
    /// Deletes an EpicPref permanently.
    /// </summary>
    /// <param name="name">The name of the pref.</param>
    /// <param name="encrypted">Whether it was encrypted or not.</param>
    public static void DeleteList(string name, bool encrypted)
    {
        Serializer.Delete(name, Serializer.SerializationTypes.List, encrypted);
    }
    #endregion
    #region Arrays
    /// <summary>
    /// Call this function to save an array of strings to the EpicPrefs.
    /// See the parameters for more information on what to pass.
    /// </summary>
    /// <param name="name">The name under which the pref is saved, and can be retrieved with.</param>
    /// <param name="value">The value that is saved.</param>
    /// <param name="encrypted">Whether to use encryption or not. Default is false and can be left away in that case.</param>
    /// <returns>Returns a bool stating if the saving has been successfull.</returns>
    public static bool SetArray(string name,string[] value,bool encrypted = false)
    {
        if (encrypted)
        {
            string[] encryptedValue = new string[value.Length];
            for(int i = 0; i < value.Length; i++)
            {
                encryptedValue[i] = Cryptor.Encrypt(value[i]);
            }
            return Serializer.Serialize(name, encryptedValue, Serializer.SerializationTypes.ArrayString, encrypted); 
        } else 
            return Serializer.Serialize(name, value,Serializer.SerializationTypes.ArrayString, encrypted);
    }
    /// <summary>
    /// Call this function to save an array of integers to the EpicPrefs.
    /// See the parameters for more information on what to pass.
    /// </summary>
    /// <param name="name">The name under which the pref is saved, and can be retrieved with.</param>
    /// <param name="value">The value that is saved.</param>
    /// <param name="encrypted">Whether to use encryption or not. Default is false and can be left away in that case.</param>
    /// <returns>Returns a bool stating if the saving has been successfull.</returns>
    public static bool SetArray(string name,int[] value,bool encrypted = false)
    {
        if (encrypted)
        {
            string[] encryptedValue = new string[value.Length];
            for (int i = 0; i < value.Length; i++)
            {
                encryptedValue[i] = Cryptor.Encrypt(value[i]);
            }
            return Serializer.Serialize(name, encryptedValue, Serializer.SerializationTypes.ArrayInt, encrypted);
        }
        else
            return Serializer.Serialize(name, value, Serializer.SerializationTypes.ArrayInt, encrypted);
    }
    /// <summary>
    /// Call this function to save an array of floats to the EpicPrefs.
    /// See the parameters for more information on what to pass.
    /// </summary>
    /// <param name="name">The name under which the pref is saved, and can be retrieved with.</param>
    /// <param name="value">The value that is saved.</param>
    /// <param name="encrypted">Whether to use encryption or not. Default is false and can be left away in that case.</param>
    /// <returns>Returns a bool stating if the saving has been successfull.</returns>
    public static bool SetArray(string name,float[] value, bool encrypted = false)
    {
        if (encrypted)
        {
            string[] encryptedValue = new string[value.Length];
            for (int i = 0; i < value.Length; i++)
            {
                encryptedValue[i] = Cryptor.Encrypt(value[i]);
            }
            return Serializer.Serialize(name, encryptedValue, Serializer.SerializationTypes.ArrayFloat, encrypted);
        }
        else
            return Serializer.Serialize(name, value, Serializer.SerializationTypes.ArrayFloat, encrypted);
    }
    /// <summary>
    /// Call this function to save an array of doubles to the EpicPrefs.
    /// See the parameters for more information on what to pass.
    /// </summary>
    /// <param name="name">The name under which the pref is saved, and can be retrieved with.</param>
    /// <param name="value">The value that is saved.</param>
    /// <param name="encrypted">Whether to use encryption or not. Default is false and can be left away in that case.</param>
    /// <returns>Returns a bool stating if the saving has been successfull.</returns>
    public static bool SetArray(string name, double[] value, bool encrypted = false)
    {
        if (encrypted)
        {
            string[] encryptedValue = new string[value.Length];
            for (int i = 0; i < value.Length; i++)
            {
                encryptedValue[i] = Cryptor.Encrypt(value[i]);
            }
            return Serializer.Serialize(name, encryptedValue, Serializer.SerializationTypes.ArrayDouble, encrypted);
        }
        else
            return Serializer.Serialize(name, value, Serializer.SerializationTypes.ArrayDouble, encrypted);
    }
    /// <summary>
    /// Call this function to retrieve an Array of strings from the EpicPrefs.
    /// If the Pref does not exist a null is returned.
    /// </summary>
    /// <param name="name">The name under which the EpicPref has previously been saved.</param>
    /// <param name="encrypted">Whether the EpicPref was previoulsy encrypted or not.</param>
    /// <returns>Returns the previously saved Array of strings.</returns>
    public static string[] GetArrayString(string name, bool encrypted = false)
    {
        if (!encrypted)
        {
            string[] arr = (string[])Serializer.Deserialize(name, Serializer.SerializationTypes.ArrayString, encrypted);
            return arr;
        }
        else {
            string[] arr = (string[])Serializer.Deserialize(name, Serializer.SerializationTypes.ArrayString, encrypted);
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = Cryptor.Decrypt(arr[i],Serializer.SerializationTypes.String).ToString();
            }
            return arr;
        }
    }
    /// <summary>
    /// Call this function to retrieve an Array of strings from the EpicPrefs.
    /// If the Pref does not exist the default value is returned.
    /// </summary>
    /// <param name="name">The name under which the EpicPref has previously been saved.</param>
    /// <param name="defaultValue"></param>
    /// <param name="encrypted">Whether the EpicPref was previoulsy encrypted or not.</param>
    /// <returns>Returns the previously saved Array of strings.</returns>
    public static string[] GetArrayString(string name, string[] defaultValue, bool encrypted = false)
    {
        if (!encrypted)
        {
            string[] arr = (string[])Serializer.Deserialize(name, Serializer.SerializationTypes.ArrayString, encrypted);
            if (arr.Length == 0)
                return defaultValue;
            else
                return arr;
        }
        else {
            string[] arr = (string[])Serializer.Deserialize(name, Serializer.SerializationTypes.ArrayString, encrypted);
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = Cryptor.Decrypt(arr[i], Serializer.SerializationTypes.String).ToString();
            }
            if (arr.Length == 0)
                return defaultValue;
            else
                return arr;
        }
    }
    /// <summary>
    /// Call this function to retrieve an Array of integers from the EpicPrefs.
    /// If the Pref does not exist a null is returned.
    /// </summary>
    /// <param name="name">The name under which the EpicPref has previously been saved.</param>
    /// <param name="encrypted">Whether the EpicPref was previoulsy encrypted or not.</param>
    /// <returns>Returns the previously saved Array of integers.</returns>
    public static int[] GetArrayInt(string name, bool encrypted = false)
    {
        if (!encrypted)
        {
            int[] arr = (int[])Serializer.Deserialize(name, Serializer.SerializationTypes.ArrayInt, encrypted);
            return arr;
        }
        else {
            string[] arr = (string[])Serializer.Deserialize(name, Serializer.SerializationTypes.ArrayString, encrypted);
            int[] decryptedArray = new int[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                decryptedArray[i] = Int32.Parse(Cryptor.Decrypt(arr[i], Serializer.SerializationTypes.String).ToString());
            }
            return decryptedArray;
        }
    }
    /// <summary>
    /// Call this function to retrieve an Array of integers from the EpicPrefs.
    /// If the Pref does not exist the default value is returned.
    /// </summary>
    /// <param name="name">The name under which the EpicPref has previously been saved.</param>
    /// <param name="defaultValue"></param>
    /// <param name="encrypted">Whether the EpicPref was previoulsy encrypted or not.</param>
    /// <returns>Returns the previously saved Array of integers.</returns>
    public static int[] GetArrayInt(string name, int[] defaultValue, bool encrypted = false)
    {
        if (!encrypted)
        {
            int[] arr = (int[])Serializer.Deserialize(name, Serializer.SerializationTypes.ArrayInt, encrypted);
            if (arr.Length == 0)
                return defaultValue;
            else
                return arr;
        }
        else {
            string[] arr = (string[])Serializer.Deserialize(name, Serializer.SerializationTypes.ArrayString, encrypted);
            int[] decryptedArray = new int[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                decryptedArray[i] = Int32.Parse(Cryptor.Decrypt(arr[i], Serializer.SerializationTypes.String).ToString());
            }
            if (decryptedArray.Length == 0)
                return defaultValue;
            else
                return decryptedArray;
        }
    }
    /// <summary>
    /// Call this function to retrieve an Array of floats from the EpicPrefs.
    /// If the Pref does not exist a null is returned.
    /// </summary>
    /// <param name="name">The name under which the EpicPref has previously been saved.</param>
    /// <param name="encrypted">Whether the EpicPref was previoulsy encrypted or not.</param>
    /// <returns>Returns the previously saved Array of floats.</returns>
    public static float[] GetArrayFloat(string name, bool encrypted = false)
    {
        if (!encrypted)
        {
            float[] arr = (float[])Serializer.Deserialize(name, Serializer.SerializationTypes.ArrayFloat, encrypted);
            return arr;
        }
        else {
            string[] arr = (string[])Serializer.Deserialize(name, Serializer.SerializationTypes.ArrayString, encrypted);
            float[] decryptedArray = new float[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                decryptedArray[i] = (float)Convert.ToDecimal(Cryptor.Decrypt(arr[i], Serializer.SerializationTypes.String).ToString());
            }
            return decryptedArray;
        }
    }
    /// <summary>
    /// Call this function to retrieve an Array of floats from the EpicPrefs.
    /// If the Pref does not exist the default value is returned.
    /// </summary>
    /// <param name="name">The name under which the EpicPref has previously been saved.</param>
    /// <param name="defaultValue"></param>
    /// <param name="encrypted">Whether the EpicPref was previoulsy encrypted or not.</param>
    /// <returns>Returns the previously saved Array of floats.</returns>
    public static float[] GetArrayFloat(string name, float[] defaultValue, bool encrypted = false)
    {
        if (!encrypted)
        {
            float[] arr = (float[])Serializer.Deserialize(name, Serializer.SerializationTypes.ArrayFloat, encrypted);
            if (arr.Length == 0)
                return defaultValue;
            else
                return arr;
        }
        else {
            string[] arr = (string[])Serializer.Deserialize(name, Serializer.SerializationTypes.ArrayString, encrypted);
            float[] decryptedArray = new float[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                decryptedArray[i] = (float)Convert.ToDecimal(Cryptor.Decrypt(arr[i], Serializer.SerializationTypes.String).ToString());
            }
            if (decryptedArray.Length == 0)
                return defaultValue;
            else
                return decryptedArray;
        }        
    }
    /// <summary>
    /// Call this function to retrieve an Array of doubles from the EpicPrefs.
    /// If the Pref does not exist a null is returned.
    /// </summary>
    /// <param name="name">The name under which the EpicPref has previously been saved.</param>
    /// <param name="encrypted">Whether the EpicPref was previoulsy encrypted or not.</param>
    /// <returns>Returns the previously saved Array of doubles.</returns>
    public static double[] GetArrayDouble(string name, bool encrypted = false)
    {
        if (!encrypted)
        {
            double[] arr = (double[])Serializer.Deserialize(name, Serializer.SerializationTypes.ArrayDouble, encrypted);
            return arr;
        }
        else {
            string[] arr = (string[])Serializer.Deserialize(name, Serializer.SerializationTypes.ArrayString, encrypted);
            double[] decryptedArray = new double[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                decryptedArray[i] = (double)Convert.ToDecimal(Cryptor.Decrypt(arr[i], Serializer.SerializationTypes.String).ToString());
            }
            return decryptedArray;
        }
    }
    /// <summary>
    /// Call this function to retrieve an Array of doubles from the EpicPrefs.
    /// If the Pref does not exist the default value is returned.
    /// </summary>
    /// <param name="name">The name under which the EpicPref has previously been saved.</param>
    /// <param name="defaultValue"></param>
    /// <param name="encrypted">Whether the EpicPref was previoulsy encrypted or not.</param>
    /// <returns>Returns the previously saved Array of doubles.</returns>
    public static double[] GetArrayDouble(string name, double[] defaultValue, bool encrypted = false)
    {
        if (!encrypted)
        {
            double[] arr = (double[])Serializer.Deserialize(name, Serializer.SerializationTypes.ArrayDouble, encrypted);
            if (arr.Length == 0)
                return defaultValue;
            else
                return arr;
        }
        else {
            string[] arr = (string[])Serializer.Deserialize(name, Serializer.SerializationTypes.ArrayString, encrypted);
            double[] decryptedArray = new double[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                decryptedArray[i] = (double)Convert.ToDecimal(Cryptor.Decrypt(arr[i], Serializer.SerializationTypes.String).ToString());
            }
            if (decryptedArray.Length == 0)
                return defaultValue;
            else
                return decryptedArray;
        }
    }
    /// <summary>
    /// Deletes an EpicPref permanently.
    /// </summary>
    /// <param name="name">The name of the pref.</param>
    /// <param name="encrypted">Whether it was encrypted or not.</param>
    public static void DeleteArrayFloat(string name, bool encrypted)
    {
        Serializer.Delete(name, Serializer.SerializationTypes.ArrayFloat, encrypted);
    }
    /// <summary>
    /// Deletes an EpicPref permanently.
    /// </summary>
    /// <param name="name">The name of the pref.</param>
    /// <param name="encrypted">Whether it was encrypted or not.</param>
    public static void DeleteArrayInt(string name, bool encrypted)
    {
        Serializer.Delete(name, Serializer.SerializationTypes.ArrayInt, encrypted);
    }
    /// <summary>
    /// Deletes an EpicPref permanently.
    /// </summary>
    /// <param name="name">The name of the pref.</param>
    /// <param name="encrypted">Whether it was encrypted or not.</param>
    public static void DeleteArrayString(string name, bool encrypted)
    {
        Serializer.Delete(name, Serializer.SerializationTypes.ArrayString, encrypted);
    }
    /// <summary>
    /// Deletes an EpicPref permanently.
    /// </summary>
    /// <param name="name">The name of the pref.</param>
    /// <param name="encrypted">Whether it was encrypted or not.</param>
    public static void DeleteArrayDouble(string name, bool encrypted)
    {
        Serializer.Delete(name, Serializer.SerializationTypes.ArrayDouble, encrypted);
    }
    #endregion
    #region EDITORONLY
#if UNITY_EDITOR
    /// <summary>
    /// A function designated to the EpicPrefsEditor.
    /// This should never be called in your code and it 
    /// only exists at Editortime.
    /// </summary>
    public static bool SetEditorPrefs(string name, Dictionary<string, string> value)
    {
        return Serializer.Serialize(name, value, Serializer.SerializationTypes.Editor,false);
    }
    /// <summary>
    /// A function designated to the EpicPrefsEditor.
    /// This should never be called in your code and it 
    /// only exists at Editortime.
    /// </summary>
    public static Dictionary<string, string> GetEditorPrefs(string name)
    {              
        return (Dictionary<string, string>)(Serializer.Deserialize(name, Serializer.SerializationTypes.Editor, false));
    }
    /// <summary>
    /// A function designated to the EpicPrefsEditor.
    /// This should never be called in your code and it 
    /// only exists at Editortime.
    /// </summary>
    public static void DeleteEditorPrefs(string name, Serializer.SerializationTypes type, bool encrypted)
    {
        Serializer.Delete(name, type, encrypted);
    }


#endif
    #endregion

}
