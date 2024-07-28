using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using Dalamud.Game.Gui.Dtr;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using Dalamud.Game.Text;
using FFXIVClientStructs.FFXIV.Client.Game;
using Lumina.Excel.GeneratedSheets;

namespace InstanceInfos;

public sealed class Plugin : IDalamudPlugin
{
    [PluginService] internal static IDalamudPluginInterface PluginInterface { get; private set; } = null!;
    [PluginService] internal static IFramework Framework { get; private set; } = null!;

    private const string CommandName = "/instanceinfos";


    public readonly WindowSystem WindowSystem = new("Instance Infos");
    private IDtrBarEntry dtrEntry;
    private IDtrBarEntry currencyEntry;
    private IDtrBarEntry levelEntry;

    public Plugin(IDalamudPluginInterface pluginInterface, IDtrBar dtrBar)
    {
        this.dtrEntry = dtrBar.Get("instanceinfos");
        this.currencyEntry = dtrBar.Get("currencyinfos");
        this.levelEntry = dtrBar.Get("levelinfos");
        Framework.Update += UpdateInstanceInfos;
    }
    public unsafe void UpdateInstanceInfos(IFramework framework)
    {
        // Currency
        uint currencyAmount = InventoryManager.Instance()->GetGil();
        string formatedAmount = currencyAmount.ToString("N");
        formatedAmount = formatedAmount.Substring(0, formatedAmount.Length - 3);
        this.currencyEntry.Text = $" {formatedAmount}{SeIconChar.Gil.ToIconChar()}";
        // Instance
        uint instance = UIState.Instance()->PublicInstance.InstanceId;
        if (instance == 0)
        {
            this.dtrEntry.Shown = false;
            return;
        }
        this.dtrEntry.Shown = true;
        uint iconNumber = (int)SeIconChar.Instance1 + (instance - 1);
        SeIconChar icon = (SeIconChar)(iconNumber);
        this.dtrEntry.Text = $" {icon.ToIconChar()}";
        // Level
        short level = PlayerState.Instance()->CurrentLevel;
        // uint jobId = PlayerState.Instance()->CurrentClassJobId;
        
        // int exp = PlayerState.Instance()->ClassJobExperience[(byte)jobId];
        this.levelEntry.Text = $" {SeIconChar.LevelEn.ToIconChar()}{level}";
    }

    public void Dispose()
    {
        this.dtrEntry.Remove();
        Framework.Update -= UpdateInstanceInfos;
    }
}
