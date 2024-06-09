using System.Threading.Tasks;

using Hardware.Info;

namespace Serein.Core.Services;

public class HardwareInfoProvider
{
    public HardwareInfoProvider()
    {
        Task.Run(() => Info = new());
    }

    public HardwareInfo? Info { get; set; }

    public void Update()
    {
        if (Info is null)
        {
            Task.Run(() => Info = new());
            return;
        }

        try
        {
            Info?.RefreshAll();
        }
        catch (System.Exception)
        {

            throw;
        }
    }
}
