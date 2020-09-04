using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class FileHandler
{
    public static string LoadProperties() //This function loads the properties from a file.
    {
        string path = Application.persistentDataPath + "/properties.smyal"; //This finds the predefined path in LocalLow and tries to find the JSON file in binary.
        if (File.Exists(path)) //If the file exists..
        {
            BinaryFormatter formatter = new BinaryFormatter(); //Gets the binary formatter to convert it into plain text
            FileStream stream = new FileStream(path, FileMode.Open); //Opens the properties.smyal file

            string data = formatter.Deserialize(stream) as string; //It decodes the binary to a string
            stream.Close(); //The file is now closed.

            return data; //The data is returned.
        }
        else //If the file didn't exist...
        {
            Debug.LogError("Save file not found in " + path); //Sends an error message to the console.
            return null;
        }
    }

    public static string LoadCards()
    {
        string path = Application.persistentDataPath + "/cards.smyal"; //This finds the predefined path in LocalLow and tries to find the JSON file in binary.
        if (File.Exists(path)) //If the file exists...
        {
            BinaryFormatter formatter = new BinaryFormatter(); //Gets the binary formatter to convert it into plain text
            FileStream stream = new FileStream(path, FileMode.Open); //Opens the cards.smyal file

            string data = formatter.Deserialize(stream) as string; //It decodes the binary to a string
            stream.Close(); //The file is now closed.

            return data; //The data is returned.

        }
        else
        {
            Debug.LogError("Save file not found in " + path); //Sends an error message to the console.
            return null;
        }
    }
}
