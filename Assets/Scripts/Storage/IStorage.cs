public interface IStorage
{
    void Load();
    void Save();
    GameState GetState();
    void SetState(GameState state);
}
