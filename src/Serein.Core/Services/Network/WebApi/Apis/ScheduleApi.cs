using System.Linq;
using System.Threading.Tasks;
using EmbedIO;
using EmbedIO.Routing;
using Force.DeepCloner;
using Serein.Core.Models.Commands;

namespace Serein.Core.Services.Network.WebApi.Apis;

internal partial class ApiMap
{
    [Route(HttpVerbs.Get, "/schedules")]
    public async Task GetSchedules()
    {
        await HttpContext.SendPacketAsync(_scheduleProvider.Value);
    }

    [Route(HttpVerbs.Post, "/schedules")]
    public async Task CreateMatch()
    {
        var schedule = await HttpContext.ConvertRequestAs<Schedule>();
        _scheduleProvider.Value.Add(schedule);
        _scheduleProvider.SaveAsyncWithDebounce();

        await HttpContext.SendPacketAsync(schedule.GetHashCode());
    }

    [Route(HttpVerbs.Delete, "/schedules/{id}")]
    public async Task DeleteSchedules(int id)
    {
        var fisrt = _scheduleProvider.Value.FirstOrDefault((m) => m.GetHashCode() == id);
        if (fisrt is not null)
        {
            _scheduleProvider.Value.Remove(fisrt);
            _scheduleProvider.SaveAsyncWithDebounce();
            await HttpContext.SendPacketAsync();
        }
        else
        {
            throw HttpException.NotFound("未找到指定的定时任务");
        }
    }

    [Route(HttpVerbs.Put, "/schedules/{id}")]
    public async Task UpdateSchedule(int id)
    {
        var schedule = await HttpContext.ConvertRequestAs<Schedule>();
        var fisrt = _scheduleProvider.Value.FirstOrDefault((m) => m.GetHashCode() == id);
        if (fisrt is not null)
        {
            schedule.DeepCloneTo(fisrt);

            _scheduleProvider.SaveAsyncWithDebounce();
            await HttpContext.SendPacketAsync(fisrt.GetHashCode());
        }
        else
        {
            throw HttpException.NotFound("未找到指定的匹配");
        }
    }
}
