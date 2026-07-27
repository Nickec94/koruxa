namespace SRC.DiscordBot;

internal static class DiscordUtil
{
    public static string MentionUser(ulong id) => $"<@{id}>";
}