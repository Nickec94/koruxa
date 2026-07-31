using System;
using System.Linq;
using NetCord;
using NetCord.Rest;

namespace SRC.DiscordBot.Components;

public sealed record BossTimerModal(int Hours, int Minutes)
{
    public const string CustomId = "custom_timer_modal";
    public const string HoursInputCustomId = "hours_input";
    public const string MinutesInputCustomId = "minutes_input";

    public TimeSpan TimeSpan => TimeSpan.FromMinutes(Minutes + (Hours * 60));

    public static BossTimerModal Parse(ModalInteraction interaction)
    {
        var textInputs = interaction.Data.Components
            .OfType<Label>()
            .Select(x => x.Component)
            .OfType<TextInput>()
            .ToList();

        var hourInput = textInputs.SingleOrDefault(c => c.CustomId == HoursInputCustomId);
        var minuteInput = textInputs.SingleOrDefault(c => c.CustomId == MinutesInputCustomId);

        if (!int.TryParse(hourInput?.Value, out var hourValue))
        {
            hourValue = 0;
        }
        
        if (!int.TryParse(minuteInput?.Value, out var minuteValue))
        {
            minuteValue = 0;
        }

        return new BossTimerModal(hourValue, minuteValue);
    }
    
    public static ModalProperties CreateModal()
    {
        return new ModalProperties(CustomId, "Set Custom Boss Timer")
        {
            Components =
            [
                new LabelProperties(
                    "Remaining Respawn Duration (Minutes)",
                    new TextInputProperties(HoursInputCustomId, TextInputStyle.Short)
                        .WithPlaceholder("Hours (e.g. 2)")
                        .WithRequired(false)
                        .WithValue("0")
                ),
                new LabelProperties(
                    "Remaining Respawn Duration (Hours)",
                    new TextInputProperties(MinutesInputCustomId, TextInputStyle.Short)
                        .WithPlaceholder("Minutes (e.g. 2)")
                        .WithRequired(false)
                        .WithValue("0")
                )
            ]
        };
    }
}