using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using Dalamud.Game.Gui.Dtr;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using Dalamud.Game.Text;

namespace InstanceInfos;

public sealed class Plugin : IDalamudPlugin
{
    [PluginService] internal static IDalamudPluginInterface PluginInterface { get; private set; } = null!;
    [PluginService] internal static IFramework Framework { get; private set; } = null!;

    private const string CommandName = "/instanceinfos";


    public readonly WindowSystem WindowSystem = new("Instance Infos");
    private IDtrBarEntry dtrEntry;

    public Plugin(IDalamudPluginInterface pluginInterface, IDtrBar dtrBar)
    {
        this.dtrEntry = dtrBar.Get("instanceinfos");
        Framework.Update += UpdateInstanceInfos;
    }
    public unsafe void UpdateInstanceInfos(IFramework framework)
    {
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
    }

    public void Dispose()
    {
        this.dtrEntry.Remove();
        Framework.Update -= UpdateInstanceInfos;
    }
}
