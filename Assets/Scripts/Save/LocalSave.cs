using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class LocalSave {
    public static void SaveData(object data, string saveFilePath) {
        try {
            FileStream stream = new FileStream(saveFilePath, FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, data);
            stream.Close();
        } catch(Exception ex) {
            Debug.LogError(ex.Message);
        }
    }

    public static object FetchData(string saveFilePath) {
        if(File.Exists(saveFilePath)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(saveFilePath, FileMode.Open);
            object data = formatter.Deserialize(stream);
            stream.Close();
            return data;
        } else {
            Debug.LogError("File doesnt exist to fetch");
            return null;
        }
    }
}
