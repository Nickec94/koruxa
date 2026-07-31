using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;
using NetCord.Services.ComponentInteractions;

namespace SRC.DiscordBot;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)]
internal class SlashCommandModule(KoruxaBossService bossService) : ApplicationCommandModule<ApplicationCommandContext>
{
    private static readonly TimeZoneInfo LocalTimeZone = OperatingSystem.IsWindows()
        ? TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time")
        : TimeZoneInfo.FindSystemTimeZoneById("America/Chicago");

    private static string FormatEta(DateTimeOffset endTime)
    {
        var localTime = TimeZoneInfo.ConvertTime(endTime, LocalTimeZone);
        var unix = endTime.ToUnixTimeSeconds();
        return $"**{localTime:h:mm tt}** (<t:{unix}:R>)";
    }

    [SlashCommand("attack", "Register your boss attack")]
    public async Task AttackAsync()
    {
        await bossService.AttackAsync(Context.User.Id, CancellationToken.None);
        
        await Context.Interaction.SendResponseAsync(InteractionCallback.Message("Time to slap your bosses ass"));
    }

    [SlashCommand("killed", "Register the boss death")]
    public async Task KillAsync()
    {
        await bossService.KillBossAsync(CancellationToken.None);

        await Context.Interaction.SendResponseAsync(InteractionCallback.Message("Got it, boss is dead"));
    }

    [SlashCommand("timer", "Start a boss timer")]
    public async Task TimerAsync(int hours = 0, int minutes = 0, int seconds = 0)
    {
        if (hours == 0 && minutes == 0 && seconds == 0)
        {
            await Context.Interaction.SendResponseAsync(InteractionCallback.Message("Please specify a duration!"));
            return;
        }

        var endTime = DateTimeOffset.UtcNow
            .AddHours(hours)
            .AddMinutes(minutes)
            .AddSeconds(seconds);

        await Context.Interaction.SendResponseAsync(InteractionCallback.Message(new InteractionMessageProperties
        {
            Content = $"Boss Respawn ETA: {FormatEta(endTime)}",
            Components = [
                new ActionRowProperties([
                    new ButtonProperties("reset_12h", "Boss Defeated (12h)", ButtonStyle.Danger),
                    new ButtonProperties("open_custom_timer_modal", "Set Custom Time", ButtonStyle.Secondary)
                ])
            ]
        }));

        _ = Task.Run(async () => await RunBossTimerLoopAsync(Context, endTime));
    }

    [SlashCommand("say", "Send a message as the bot")]
    public async Task SayAsync(string message)
    {
        await Context.Channel.SendMessageAsync(new MessageProperties
        {
            Content = message
        });

        await Context.Interaction.SendResponseAsync(InteractionCallback.Message("Message sent!"));
    }

    private static async Task RunBossTimerLoopAsync(ApplicationCommandContext context, DateTimeOffset targetEndTime)
    {
        var currentEndTime = targetEndTime;

        while (true)
        {
            var delay = currentEndTime - DateTimeOffset.UtcNow;

            if (delay > TimeSpan.Zero)
            {
                await Task.Delay(delay);
            }

            currentEndTime = DateTimeOffset.UtcNow.AddHours(3).AddMinutes(59);

            try
            {
                await context.Interaction.ModifyResponseAsync(msg =>
                {
                    msg.Content = $"⚔️ Boss timer expired! Auto-resetting... Boss Respawn ETA: {FormatEta(currentEndTime)}";
                    msg.Components = [
                        new ActionRowProperties([
                            new ButtonProperties("reset_12h", "Boss Defeated (12h)", ButtonStyle.Danger),
                            new ButtonProperties("open_custom_timer_modal", "Set Custom Time", ButtonStyle.Secondary)
                        ])
                    ];
                });
            }
            catch
            {
                break;
            }
        }
    }
}

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)]
internal class BossTimerComponentModule : ComponentInteractionModule<ComponentInteractionContext>
{
    private static readonly TimeZoneInfo LocalTimeZone = OperatingSystem.IsWindows()
        ? TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time")
        : TimeZoneInfo.FindSystemTimeZoneById("America/Chicago");

    private static string FormatEta(DateTimeOffset endTime)
    {
        var localTime = TimeZoneInfo.ConvertTime(endTime, LocalTimeZone);
        var unix = endTime.ToUnixTimeSeconds();
        return $"**{localTime:h:mm tt}** (<t:{unix}:R>)";
    }

    [ComponentInteraction("reset_12h")]
    public async Task Reset12hAsync()
    {
        var newEndTime = DateTimeOffset.UtcNow.AddHours(11).AddMinutes(59);

        await Context.Interaction.SendResponseAsync(InteractionCallback.ModifyMessage(msg =>
        {
            msg.Content = $"💀 Boss defeated by {Context.User.Username}! Next Respawn ETA: {FormatEta(newEndTime)}";
            msg.Components = [
                new ActionRowProperties([
                    new ButtonProperties("reset_12h", "Boss Defeated (12h)", ButtonStyle.Danger),
                    new ButtonProperties("open_custom_timer_modal", "Set Custom Time", ButtonStyle.Secondary)
                ])
            ];
        }));
    }

    [ComponentInteraction("open_custom_timer_modal")]
    public async Task OpenCustomModalAsync()
    {
        await Context.Interaction.SendResponseAsync(InteractionCallback.Modal(new ModalProperties("custom_timer_modal", "Set Custom Boss Timer")
        {
            Components = [
                new LabelProperties("Enter remaining respawn duration:", new TextInputProperties("hours_input", TextInputStyle.Short)
                {
                    Placeholder = "Hours (e.g. 2)",
                    Required = false,
                    Value = "0"
                }),
                new LabelProperties("Minutes:", new TextInputProperties("minutes_input", TextInputStyle.Short)
                {
                    Placeholder = "Minutes (e.g. 30)",
                    Required = false,
                    Value = "0"
                })
            ]
        }));
    }

    [ComponentInteraction("custom_timer_modal")]
    public async Task SubmitCustomTimerAsync()
    {
        var modalInteraction = (ModalInteraction)Context.Interaction;
        
        var textInputs = modalInteraction.Data.Components
            .OfType<ActionRow>()
            .SelectMany(row => row.Components)
            .OfType<TextInput>();

        int.TryParse(textInputs.FirstOrDefault(c => c.CustomId == "hours_input")?.Value, out int hours);
        int.TryParse(textInputs.FirstOrDefault(c => c.CustomId == "minutes_input")?.Value, out int minutes);

        if (hours == 0 && minutes == 0)
        {
            await Context.Interaction.SendResponseAsync(InteractionCallback.Message(new InteractionMessageProperties
            {
                Content = "Invalid duration entered!",
                Flags = MessageFlags.Ephemeral
            }));
            return;
        }

        var newEndTime = DateTimeOffset.UtcNow.AddHours(hours).AddMinutes(minutes);

        await Context.Interaction.SendResponseAsync(InteractionCallback.ModifyMessage(msg =>
        {
            msg.Content = $"⏳ Timer updated by {Context.User.Username}! Next Respawn ETA: {FormatEta(newEndTime)}";
            msg.Components = [
                new ActionRowProperties([
                    new ButtonProperties("reset_12h", "Boss Defeated (12h)", ButtonStyle.Danger),
                    new ButtonProperties("open_custom_timer_modal", "Set Custom Time", ButtonStyle.Secondary)
                ])
            ];
        }));
    }
}