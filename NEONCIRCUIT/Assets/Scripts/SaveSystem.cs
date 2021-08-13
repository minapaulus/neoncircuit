using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public const string filePath = "/player.NEONCIRCUIT";
    public static void SavePlayer (Playerstats playerstats)
    {
        FileStream stream = null;
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + filePath;
        try
        {
            stream = new FileStream(path, FileMode.Create);

            PlayerData data = new PlayerData(playerstats);

            formatter.Serialize(stream, data);
        }
        catch
        {

        }
        finally
        {
            stream.Close();
        }
    }

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + filePath;

        if (File.Exists(path))
        {
            FileStream stream = null;
            PlayerData data = null;
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                 stream = new FileStream(path, FileMode.Open);
                 data = formatter.Deserialize(stream) as PlayerData;
            } catch
            {

            } finally
            {
                stream.Close();
            }
            return data; 
        } else
        {
            Debug.LogError("Save File not Found in" + path);
            return null;
        }
    }
}
