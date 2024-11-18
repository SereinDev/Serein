using System;

using Serein.Core.Models.Commands;

using Xunit;

namespace Serein.Tests.Static;

public static class ScheduleTests
{
    [Fact]
    public static void ShouldModifyCommandObjWhenSettingCommand()
    {
        var schedule = new Schedule { Command = "[g]1" };
        Assert.NotNull(schedule.CommandObj);

        schedule.Command = "111?";
        Assert.NotNull(schedule.CommandObj);
    }

    [Fact]
    public static void CronShouldBeNullIfExpressionIsEmptyOrInvalid()
    {
        var schedule = new Schedule { Expression = string.Empty };
        Assert.Null(schedule.Cron);

        schedule.Expression = "* * * * * *";
        Assert.Null(schedule.Cron);

        schedule.Expression = "1 2 3";
        Assert.Null(schedule.Cron);
    }

    [Fact]
    public static void ShouldCalculateCorrectNextTime()
    {
        var schedule = new Schedule { Expression = "* * * * *" };
        Assert.NotNull(schedule.NextTime);
        Assert.True(schedule.NextTime.Value > DateTime.Now);
    }
}