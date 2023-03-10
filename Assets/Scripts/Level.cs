using UnityEngine;

public class Level : MonoBehaviour
{
    public Transform startPoint;
    public LevelStage[] stages = new LevelStage[0];

    [ContextMenu("Fill stages")]
    public void FillStages()
    {
        stages = GetComponentsInChildren<LevelStage>();
    }
}
