using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "Settings", menuName = "Slime/Settings")]
public class GameSettingsInstaller : ScriptableObjectInstaller
{
    public SlimeConfig config = new SlimeConfig();
    public RewardConfig reward = new RewardConfig();
    public PriceConfig price = new PriceConfig();

    public override void InstallBindings()
    {
        Container.BindInstances(config, reward, price);
    }
}
