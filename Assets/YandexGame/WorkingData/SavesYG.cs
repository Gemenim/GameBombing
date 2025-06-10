
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

        // Тестовые сохранения для демо сцены
        // Можно удалить этот код, но тогда удалите и демо (папка Example)
        public int LevelGame = 1;
        public double Coins = 300;
        public double Experience = 0;
        public int LevelUpgadeDamage = 1;
        public int LevelUpgadeRicochet = 1;
        public int LevelUpgadeDamageExplosion = 1;
        public int LevelUpgadeRadiusExplosion = 1;
        public int CountDastroyBomb = 0;

        // Ваши сохранения

        // ...

        // Поля (сохранения) можно удалять и создавать новые. При обновлении игры сохранения ломаться не должны


        // Вы можете выполнить какие то действия при загрузке сохранений
    }
}
