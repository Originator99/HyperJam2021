using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class OfflineDataManager: IInitializable {
    public void Initialize() {

    }

    public void SaveData(string path, System.Object data) {
        LocalSave.SaveData(data, path);
    }

    public System.Object FetchData(string path) {
        return LocalSave.FetchData(path);
    }
}
