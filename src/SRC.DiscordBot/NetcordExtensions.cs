using System.Threading.Tasks;
using NetCord;
using NetCord.Rest;

namespace SRC.DiscordBot;

public static class NetcordExtensions
{
    public static async Task ResponseWithMessageAsync(
        this ApplicationCommandInteraction interaction,
        string content)
    {
        await interaction.SendResponseAsync(
            InteractionCallback.Message(
                new InteractionMessageProperties()
                {
                    Content = content
                }
            )
        );
    }
}