using System.Linq;
using System.Net;
using System.Threading.Tasks;
using EmbedIO;
using EmbedIO.Routing;
using Force.DeepCloner;
using Serein.Core.Models.Automations;

namespace Serein.Core.Services.Network.Web.Apis;

internal partial class ApiMap
{
    [Route(HttpVerbs.Get, "/automations")]
    public async Task GetAutomations()
    {
        await HttpContext.SendPacketAsync(automationTaskProvider.Value);
    }

    [Route(HttpVerbs.Get, "/automations/{id}")]
    public async Task GetAutomationTask(int id)
    {
        await HttpContext.SendPacketAsync(FastGetAutomationTask(id));
    }

    [Route(HttpVerbs.Post, "/automations")]
    public async Task AddMatch()
    {
        var automationTask = await HttpContext.ConvertRequestAs<AutomationTask>();
        automationTaskProvider.Value.Add(automationTask);
        automationTaskProvider.SaveAsyncWithDebounce();

        await HttpContext.SendPacketAsync(automationTask, HttpStatusCode.Created);
    }

    [Route(HttpVerbs.Delete, "/automations/{id}")]
    public async Task DeleteAutomationTask(int id)
    {
        var automationTask = FastGetAutomationTask(id);
        automationTaskProvider.Value.Remove(automationTask);
        automationTaskProvider.SaveAsyncWithDebounce();
        await HttpContext.SendPacketWithEmptyDataAsync(HttpStatusCode.NoContent);
    }

    [Route(HttpVerbs.Put, "/automations/{id}")]
    public async Task UpdateAutomationTask(int id)
    {
        var automationTask = await HttpContext.ConvertRequestAs<AutomationTask>();
        var oldAutomationTask = FastGetAutomationTask(id);

        automationTask.DeepCloneTo(oldAutomationTask);
        automationTaskProvider.SaveAsyncWithDebounce();

        await HttpContext.SendPacketAsync(oldAutomationTask);
    }

    private AutomationTask FastGetAutomationTask(int id)
    {
        var fisrt = automationTaskProvider.Value.FirstOrDefault((m) => m.GetHashCode() == id);
        return fisrt is not null ? fisrt : throw HttpException.NotFound("未找到指定的匹配");
    }
}
