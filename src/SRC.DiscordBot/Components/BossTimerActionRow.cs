using NetCord;
using NetCord.Rest;

namespace SRC.DiscordBot.Components;

public sealed class BossTimerActionRow
{
    public const string ResetButtonCustomId = "reset_12h";
    public const string OpenTimerButtonCustomId = "open_custom_timer_modal";
    
    public static ActionRowProperties CreateNew()
    {
        return new ActionRowProperties([
            new ButtonProperties(ResetButtonCustomId, "Boss Defeated (12h)", ButtonStyle.Danger),
            new ButtonProperties(OpenTimerButtonCustomId, "Set Custom Time", ButtonStyle.Secondary)
        ]);
    } 
}