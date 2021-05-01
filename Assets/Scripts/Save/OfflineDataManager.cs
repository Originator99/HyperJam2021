﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class OfflineDataManager :IInitializable {
    private ChapterSettingsInstaller.ChapterSettings _chapterSettings;

    public OfflineDataManager(ChapterSettingsInstaller.ChapterSettings chapterSettings) {
        _chapterSettings = chapterSettings;
    }

    public void Initialize() {

    }

    public void MarkChapterLevelAsComplete(Chapter data) {
        if(_chapterSettings != null && _chapterSettings.chapters != null) {
            
        }
    }
}
