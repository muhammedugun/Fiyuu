
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

        public int[] LevelScore = new int[11];
        public int[] LevelStars = new int[11];
        public int[] CompletedLevel = new int[11];
        public int isMute, Level1Played, isNotFullScreen;
        public SavesYG()
        {

        }
    }
}
