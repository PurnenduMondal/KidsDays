using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;

public class SaveLoad : MonoBehaviour
{
    public static void Save<T>(T objectToSave, string key)
    {
        string path = Application.persistentDataPath + "/saves/";
        Directory.CreateDirectory(path);
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(path + key + ".txt", FileMode.Create);
        
            //formatter.Serialize(fileStream, ObjectToSave);
            try
            {
                formatter.Serialize(fileStream, objectToSave);
            }
            catch (SerializationException exception)
            {
                Debug.Log("Save failed. Error: " + exception.Message);
            }
            finally
            {
                fileStream.Close();
            }
        
    }
    public static T Load<T>(string key)
    {
        string path = Application.persistentDataPath + "/saves/";
        BinaryFormatter formatter = new BinaryFormatter();
        T returnValue = default(T);
        FileStream fileStream = new FileStream(path + key + ".txt", FileMode.Open);
            try
            {
                returnValue = (T)formatter.Deserialize(fileStream);
            }
            catch (SerializationException exception)
            {
                Debug.Log("Load failed. Error: " + exception.Message);
            }
            finally
            {
                fileStream.Close();
            }
        
        return returnValue;
    }

    public static bool SaveExists(string key)
    {
        string path= Application.persistentDataPath + "/saves/" + key + ".txt";
        return File.Exists(path);
    }
    public static void DeleteAllSaveFiles()
    {
        string path = Application.persistentDataPath + "/saves/";
        DirectoryInfo directory = new DirectoryInfo(path);
        directory.Delete();
        Directory.CreateDirectory(path);
    }
    public static Sprite LoadSprite(string path)
    {
        if (string.IsNullOrEmpty(path)) return null;
        if (System.IO.File.Exists(path))
        {
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(bytes);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            return sprite;
        }
        return null;
    }
}
