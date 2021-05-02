using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System.IO;
using System;
using System.Linq;

public class ChapterDataManager : IInitializable, IDisposable {
    private readonly string path = Application.persistentDataPath + "/chapters";
    private readonly string file_name = "/chapters.cyber";

    private readonly ChapterSettingsInstaller.DefaultChapterSettings _defaultData;
    private readonly OfflineDataManager _offlineDataManager;

    private LocalChapterData localChapterData;

    public void Initialize() {
    
    }

    public void Dispose() {
        Debug.Log("Saved Local Chapter Data");
        _offlineDataManager.SaveData(path + file_name, localChapterData);
    }

    public ChapterDataManager(ChapterSettingsInstaller.DefaultChapterSettings defaultData, OfflineDataManager offlineDataManager) {
        _defaultData = defaultData;
        _offlineDataManager = offlineDataManager;

        if(!Directory.Exists(path)) {
            Directory.CreateDirectory(path);
            CreateDefaultData();
            Debug.Log("Chapter Data path created : " + path + file_name);
        } else {
            FetchChaptersData();
        }
    }

    private void CreateDefaultData() {
        if(_defaultData != null && _defaultData.chapters != null) {
            LocalChapterData local_data = new LocalChapterData() {
                chapters = new List<ChapterData>()
            };

            foreach(var data in _defaultData.chapters) {
                local_data.chapters.Add(data.chapter_data);
            }

            localChapterData = local_data;

            _offlineDataManager.SaveData(path + file_name, local_data);
        }
    }

    private void FetchChaptersData() {
        try {
            if(!File.Exists(path + file_name)) {
                Debug.LogError("Chapters File not found, creating a default file");
                CreateDefaultData();
            } else {
                var data = _offlineDataManager.FetchData(path + file_name);
                localChapterData = data as LocalChapterData;
                CompareAndUpdateChapterData();
            }
        } catch(Exception e) {
            Debug.LogError("Fetching Chapter failed \n"+ e.Message);
        }
     }

    private void CompareAndUpdateChapterData() {
        if(_defaultData != null && _defaultData.chapters != null && localChapterData != null && localChapterData.chapters != null) {
            foreach(var data in _defaultData.chapters) {
                if(data.chapter_data != null) {
                    int index = localChapterData.chapters.FindIndex(x => x.chaper_id == data.chapter_data.chaper_id);
                    if(index >= 0) {
                        //chapter found locally
                        foreach(var level in data.chapter_data.levels) {
                            int levelIndex = localChapterData.chapters[index].levels.FindIndex(x => x.level_number == level.level_number);
                            if(levelIndex >= 0) {
                                //updating prefab name
                                localChapterData.chapters[index].levels[levelIndex].prefab_name = level.prefab_name;
                            } else {
                                //new level found
                                Debug.Log("New level found, adding to chapter " + data.chapter_data.chaper_id);
                                localChapterData.chapters[index].levels.Add(level);
                            }
                        }
                        if(localChapterData.chapters[index].levels.Count != data.chapter_data.levels.Count) {
                            //probably an extra entry in local which needs to be removed
                            Debug.Log("Found extra level in chapter when compared to local, removing them now..");
                            localChapterData.chapters[index].levels.RemoveAll(x => !data.chapter_data.levels.Exists(y => x.level_number == y.level_number));
                        }
                    } else {
                        //new chapter found
                        Debug.Log("Adding new chapter " + data.chapter_data.chaper_id);
                        localChapterData.chapters.Add(data.chapter_data);
                    }
                } else {
                    Debug.LogError("Found a new entry of chapter in default data but the list is empty!");
                }
            }
        }
    }

    public ChapterData GetCurrentChapter() {
        if(localChapterData != null && localChapterData.chapters!=null) {
            for(int i = 0; i < localChapterData.chapters.Count; i++) {
                if(!localChapterData.chapters[i].is_complete) {
                    return localChapterData.chapters[i]; //returning the first incomplete chapter
                }
            }
        }
        return null;
    }

    public ChapterData GetChapterByID(int chapter_id) {
        if(localChapterData != null && localChapterData.chapters != null) {
            int index = localChapterData.chapters.FindIndex(x => x.chaper_id == chapter_id);
            if(index >= 0) {
                return localChapterData.chapters[index];
            }
        }
        return null;
    }

    public void SetChapterLevelAsCompelte(int chapter_id, int level_number) {
        if(localChapterData != null && localChapterData.chapters != null) {
            int index = localChapterData.chapters.FindIndex(x => x.chaper_id == chapter_id);
            if(index >= 0) {
                int levelIndex = localChapterData.chapters[index].levels.FindIndex(x => x.level_number == level_number);
                if(level_number >= 0) {
                    localChapterData.chapters[index].levels[levelIndex].is_complete = true;

                    bool allLevelsCompelete = localChapterData.chapters[index].levels.All(x => x.is_complete);
                    if(allLevelsCompelete) {
                        Debug.Log("All levels in this chapter are compelete, marking chapter as complete");
                        localChapterData.chapters[index].is_complete = true;
                    }
                } else {
                    Debug.LogError("Level not found with level number :" + level_number + " , in chapter ID " + chapter_id);
                }
            } else {
                Debug.LogError("Chapter not found with ID : " + chapter_id);
            }
        }
    }

    [System.Serializable]
    public class LocalChapterData {
        public List<ChapterData> chapters;
    }
}

[System.Serializable]
public class ChapterData {
    public int chaper_id;
    public string chapter_name;
    public bool is_complete;
    public List<LevelData> levels;
}

[System.Serializable]
public class LevelData {
    public int level_number;
    public bool is_complete;
    public string prefab_name;
}