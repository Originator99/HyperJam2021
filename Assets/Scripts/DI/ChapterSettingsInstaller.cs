using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[CreateAssetMenu(menuName = "Roulette/Chapter Settings")]
public class ChapterSettingsInstaller :ScriptableObjectInstaller<ChapterSettingsInstaller> {
    public override void InstallBindings() {
        Container.BindInstance(chapterSettings);
    }

    public DefaultChapterSettings chapterSettings;

    [System.Serializable]
    public class DefaultChapterSettings {
        public List<ChapterSO> chapters;
    }
}
