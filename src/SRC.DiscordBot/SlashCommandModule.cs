using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using NetCord.Services.ApplicationCommands;

namespace SRC.DiscordBot;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)]
internal sealed class SlashCommandModule(KoruxaBossService bossService) : ApplicationCommandModule<ApplicationCommandContext>
{
    [SlashCommand("attack", "Register your boss attack")]
    public async Task AttackAsync()
    {
        await bossService.AttackAsync(Context.User.Id, CancellationToken.None);

        await Context.Interaction.ResponseWithMessageAsync("Got it");
    }
    
    [SlashCommand("killed", "Register the boss death")]
    public async Task KillAsync()
    {
        await bossService.KillBossAsync(CancellationToken.None);

        await Context.Interaction.ResponseWithMessageAsync("Got it");
    }
}