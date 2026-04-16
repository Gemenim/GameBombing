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
        public bool IsBeginner = true;
        public int LevelGame = 1;
        public float Coins = 0;
        public float Experience = 0;
        public int LevelUpgradeDamage = 1;
        public int LevelUpgradeRicochet = 1;
        public int LevelUpgradeSpeedAttack = 1;
        public int LevelUpgradeSpeedExplosion = 1;
        public int LevelUpgradeDamageExplosion = 1;
        public int LevelUpgradeRadiusExplosion = 1;
        public int CountDastroyBomb = 0;
        public int[] Levels = new int[6] { 1, 1, 1, 1, 1, 1 };
        // ...

        // Поля (сохранения) можно удалять и создавать новые. При обновлении игры сохранения ломаться не должны


        // Вы можете выполнить какие то действия при загрузке сохранений        
    }
}
