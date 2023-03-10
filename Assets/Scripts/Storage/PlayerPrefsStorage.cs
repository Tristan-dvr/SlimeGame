using UnityEngine;

public class PlayerPrefsStorage : IStorage
{
    private GameState _state;

    public PlayerPrefsStorage()
    {
        Load();
    }

    public GameState GetState() => _state;

    public void Load()
    {
        var json = PlayerPrefs.GetString("data");
        _state = JsonUtility.FromJson<GameState>(json) ?? new GameState();
    }

    public void Save()
    {
        var json = JsonUtility.ToJson(_state);
        PlayerPrefs.SetString("data", json);
    }

    public void SetState(GameState state)
    {
        _state = state;
    }
}
