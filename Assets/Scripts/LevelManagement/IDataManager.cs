public interface IDataManager
{
    int GetHighestUnlockedLevel();
    void SetHighestUnlockedLevel(int level);
    int GetHighScore(int level);
}
