using System.Threading.Tasks;

using EmbedIO;
using EmbedIO.Routing;

namespace Serein.Core.Services.Network.WebApi.Apis;

internal partial class ApiMap
{
    [Route(HttpVerbs.Get, "/hardware/memoryStatus")]
    public async Task GetMemoryStatus()
    {
        await HttpContext.SendPacketAsync(_hardwareInfoProvider.Info?.MemoryStatus);
    }

    [Route(HttpVerbs.Get, "/hardware/batteries")]
    public async Task GetBatteryList()
    {
        await HttpContext.SendPacketAsync(_hardwareInfoProvider.Info?.BatteryList);
    }

    [Route(HttpVerbs.Get, "/hardware/bios")]
    public async Task GetBiosList()
    {
        await HttpContext.SendPacketAsync(_hardwareInfoProvider.Info?.BiosList);
    }

    [Route(HttpVerbs.Get, "/hardware/system")]
    public async Task GetComputerSystemList()
    {
        await HttpContext.SendPacketAsync(_hardwareInfoProvider.Info?.ComputerSystemList);
    }

    [Route(HttpVerbs.Get, "/hardware/cpus")]
    public async Task GetCpuList()
    {
        await HttpContext.SendPacketAsync(_hardwareInfoProvider.Info?.CpuList);
    }

    [Route(HttpVerbs.Get, "/hardware/memory")]
    public async Task GetMemoryList()
    {
        await HttpContext.SendPacketAsync(_hardwareInfoProvider.Info?.MemoryList);
    }

    [Route(HttpVerbs.Get, "/hardware/drives")]
    public async Task GetDriveList()
    {
        await HttpContext.SendPacketAsync(_hardwareInfoProvider.Info?.DriveList);
    }

    [Route(HttpVerbs.Get, "/hardware/keyboards")]
    public async Task GetKeyboardList()
    {
        await HttpContext.SendPacketAsync(_hardwareInfoProvider.Info?.KeyboardList);
    }

    [Route(HttpVerbs.Get, "/hardware/motherboards")]
    public async Task GetMotherboardList()
    {
        await HttpContext.SendPacketAsync(_hardwareInfoProvider.Info?.MotherboardList);
    }

    [Route(HttpVerbs.Get, "/hardware/monitors")]
    public async Task GetMonitorList()
    {
        await HttpContext.SendPacketAsync(_hardwareInfoProvider.Info?.MonitorList);
    }

    [Route(HttpVerbs.Get, "/hardware/mouses")]
    public async Task GetMouseList()
    {
        await HttpContext.SendPacketAsync(_hardwareInfoProvider.Info?.MouseList);
    }

    [Route(HttpVerbs.Get, "/hardware/os")]
    public async Task GetOperatingSystem()
    {
        await HttpContext.SendPacketAsync(_hardwareInfoProvider.Info?.OperatingSystem);
    }

    [Route(HttpVerbs.Get, "/hardware/printers")]
    public async Task GetPrinterList()
    {
        await HttpContext.SendPacketAsync(_hardwareInfoProvider.Info?.PrinterList);
    }

    [Route(HttpVerbs.Get, "/hardware/soundDevices")]
    public async Task GetSoundDeviceList()
    {
        await HttpContext.SendPacketAsync(_hardwareInfoProvider.Info?.SoundDeviceList);
    }
}