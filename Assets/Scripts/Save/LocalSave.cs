using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class LocalSave {
    private readonly string saveFilePath = Application.persistentDataPath + "/level.cyber";

    public void SaveData(object data) {
        try {
            FileStream stream = new FileStream(saveFilePath, FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, data);
            stream.Close();
        } catch(Exception ex) {
            Debug.LogError(ex.Message);
        }
    }

    public LevelLogic FetchData() {
        if(File.Exists(saveFilePath)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(saveFilePath, FileMode.Open);
            LevelLogic data = formatter.Deserialize(stream) as LevelLogic;
            stream.Close();
            return data;
        } else {
            Debug.LogError("File doesnt exist to fetch");
            return null;
        }
    }
}
