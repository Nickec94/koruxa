using System;

namespace SRC.DiscordBot;

internal static class DiscordUtil
{
    public static string MentionUser(ulong id) => $"<@{id}>";
    
    public static string Bold(string text) => $"**{text}**";

    public static string RelativeTime(DateTimeOffset datetime) => $"<t:{datetime.ToUnixTimeSeconds()}:R>";
}