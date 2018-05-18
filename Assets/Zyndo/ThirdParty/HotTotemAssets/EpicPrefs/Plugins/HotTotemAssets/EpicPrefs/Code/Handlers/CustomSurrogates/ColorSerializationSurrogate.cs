using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;

sealed class ColorSerializationSurrogate : ISerializationSurrogate
{

    // Serialize the Employee object to save the object�s name and address fields.
    public void GetObjectData(System.Object obj,
        SerializationInfo info, StreamingContext context)
    {
        Color col = (Color)obj;
        info.AddValue("r", col.r);
        info.AddValue("g", col.g);
        info.AddValue("b", col.b);
        info.AddValue("a", col.a);
    }

    // Deserialize the Employee object to set the object�s name and address fields.
    public System.Object SetObjectData(System.Object obj,
        SerializationInfo info, StreamingContext context,
        ISurrogateSelector selector)
    {

        Color col = (Color)obj;
        col = new Color((float)info.GetDecimal("r"), (float)info.GetDecimal("g"), (float)info.GetDecimal("b"), (float)info.GetDecimal("a"));
        return col;
    }
}