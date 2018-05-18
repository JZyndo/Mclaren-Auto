using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;

sealed class QuaternionSerializationSurrogate : ISerializationSurrogate
{

    // Serialize the Employee object to save the object�s name and address fields.
    public void GetObjectData(System.Object obj,
        SerializationInfo info, StreamingContext context)
    {
        Quaternion quat = (Quaternion)obj;
        info.AddValue("x", quat.x);
        info.AddValue("y", quat.y);
        info.AddValue("z", quat.z);
        info.AddValue("w", quat.w);
    }

    // Deserialize the Employee object to set the object�s name and address fields.
    public System.Object SetObjectData(System.Object obj,
        SerializationInfo info, StreamingContext context,
        ISurrogateSelector selector)
    {

        Quaternion quat = (Quaternion)obj;
        quat = new Quaternion((float)info.GetDecimal("x"), (float)info.GetDecimal("y"), (float)info.GetDecimal("z"), (float)info.GetDecimal("w"));
        return quat;
    }
}