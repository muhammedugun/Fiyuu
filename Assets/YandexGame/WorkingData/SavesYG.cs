﻿
namespace YG
{
    [System.Serializable]
    public class SavesYG
    {

        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;

        public int[] LevelScore, LevelStars, CompletedLevel;
        public int isMute, Level1Played, isNotFullScreen;
        public SavesYG()
        {

        }
    }
}
