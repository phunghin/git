using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif


public static class FileHelper
{
	/// <summary>
	/// Gets the path of the specified file name.
	/// </summary>
	public static string GetFilePath(string subPath,string fileName)
    {
#if UNITY_EDITOR

        if (!File.Exists(subPath))
        {
            System.IO.Directory.CreateDirectory(string.Format("{0}/Resources/{1}", Application.dataPath, subPath));
        }

        return string.Format("{0}/Resources/{1}/{2}", Application.dataPath, subPath, fileName);

      
        #elif UNITY_STANDALONE

                if (!File.Exists(subPath))
                {
                    System.IO.Directory.CreateDirectory(string.Format("{0}/{1}", Application.dataPath, subPath));
                }

        		return string.Format("{0}/{1}/{2}", Application.dataPath,subPath, fileName);
        #else

                if (!File.Exists(subPath))
                {
                    System.IO.Directory.CreateDirectory(string.Format("{0}/{1}", Application.persistentDataPath, subPath));
                }

        		return string.Format("{0}/{1}/{2}", Application.persistentDataPath,subPath, fileName);
#endif

    }

    /// <summary>
    /// Saves data to the specified file.
    /// </summary>
    public static bool Save<T>(T data,string subPath, string fileName)
	{
        string path = GetFilePath(subPath, fileName);

        BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(GetFilePath(subPath,fileName));
        bf.Serialize(file,data);
		file.Close();

    #if UNITY_EDITOR
            AssetDatabase.Refresh();
    #endif

        return true;
	}

	/// <summary>
	/// Loads data from the specified file.
	/// </summary>
	
    public static bool Load<T>(string subPath, string fileName, ref T data)
    {
         string path = GetFilePath(subPath,fileName);

        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            data = (T)bf.Deserialize(file);
            file.Close();

            return true;
        }

            return false;
    }
}

