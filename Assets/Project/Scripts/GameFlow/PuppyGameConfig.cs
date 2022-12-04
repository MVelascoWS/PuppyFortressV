using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WasdStudio.GameConfig;
using WasdStudio.GameConfig.Settings;

[CreateAssetMenu(menuName = "Game Settings/Puppy")]
public class PuppyGameConfig : GameConfig
{
    [SerializeField]
    public string PlayerName { get; set; }

    private const string VRSelectedKey = "VRSelected";
    public BoolSetting VRSelected = new BoolSetting(VRSelectedKey);

    protected override void Init()
    {
        base.Init();
        PlayerName = "";
    }
    public void ManualStart()
    {
        Init();
    }
}
