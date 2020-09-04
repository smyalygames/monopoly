using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class PropertiesHandler
{

    public static void SaveProperties(string data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/properties.smyal";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }
    
    public static void SaveCards(string data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/cards.smyal";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static string LoadProperties()
    {
        string path = Application.persistentDataPath + "/properties.smyal";
        if (File.Exists(path))
        {
            BinaryFormatter  formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            string data = formatter.Deserialize(stream) as string;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
    
    public static string LoadCards()
    {
        string path = Application.persistentDataPath + "/cards.smyal";
        if (File.Exists(path))
        {
            BinaryFormatter  formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            string data = formatter.Deserialize(stream) as string;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static bool CheckPropertyExists()
    {
        string path = Application.persistentDataPath + "/properties.smyal";
        return File.Exists(path);
    }

    public static bool CheckCardExists()
    {
        string path = Application.persistentDataPath + "/cards.smyal";
        return File.Exists(path);
    }

}
