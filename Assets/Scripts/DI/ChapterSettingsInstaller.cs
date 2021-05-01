using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[CreateAssetMenu(menuName = "Roulette/Chapter Settings")]
public class ChapterSettingsInstaller :ScriptableObjectInstaller<ChapterSettingsInstaller> {
    public override void InstallBindings() {
        Container.BindInstance(chapterSettings);
    }

    public ChapterSettings chapterSettings;

    [System.Serializable]
    public class ChapterSettings {
        public List<ChapterSO> chapters;

        public ChapterSO GetChapterByID(int id) {
            if(chapters != null && chapters.Count > 0) {
                int index = chapters.FindIndex(x => x.id == id);
                return chapters[index];
            }
            return null;
        }
    }
}
