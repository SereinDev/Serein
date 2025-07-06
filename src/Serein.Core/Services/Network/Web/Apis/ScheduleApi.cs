using System.Linq;
using System.Net;
using System.Threading.Tasks;
using EmbedIO;
using EmbedIO.Routing;
using Force.DeepCloner;
using Serein.Core.Models.Commands;

namespace Serein.Core.Services.Network.Web.Apis;

internal partial class ApiMap
{
    [Route(HttpVerbs.Get, "/schedules")]
    public async Task GetSchedules()
    {
        await HttpContext.SendPacketAsync(scheduleProvider.Value);
    }

    [Route(HttpVerbs.Get, "/schedules/{id}")]
    public async Task GetSchedule(int id)
    {
        await HttpContext.SendPacketAsync(FastGetSchedule(id));
    }

    [Route(HttpVerbs.Post, "/schedules")]
    public async Task AddSchedule()
    {
        var schedule = await HttpContext.ConvertRequestAs<Schedule>();
        scheduleProvider.Value.Add(schedule);
        scheduleProvider.SaveAsyncWithDebounce();

        await HttpContext.SendPacketAsync(schedule, HttpStatusCode.Created);
    }

    [Route(HttpVerbs.Delete, "/schedules/{id}")]
    public async Task DeleteSchedules(int id)
    {
        var schedule = FastGetSchedule(id);
        scheduleProvider.Value.Remove(schedule);
        scheduleProvider.SaveAsyncWithDebounce();
        await HttpContext.SendPacketWithEmptyDataAsync(HttpStatusCode.NoContent);
    }

    [Route(HttpVerbs.Put, "/schedules/{id}")]
    public async Task UpdateSchedule(int id)
    {
        var schedule = await HttpContext.ConvertRequestAs<Schedule>();
        var old = FastGetSchedule(id);
        schedule.DeepCloneTo(old);
        old.ForceUpdate();

        scheduleProvider.SaveAsyncWithDebounce();
        await HttpContext.SendPacketAsync(old);
    }

    private Schedule FastGetSchedule(int id)
    {
        var fisrt = scheduleProvider.Value.FirstOrDefault((m) => m.GetHashCode() == id);
        return fisrt is not null ? fisrt : throw HttpException.NotFound("未找到指定的匹配");
    }
}
