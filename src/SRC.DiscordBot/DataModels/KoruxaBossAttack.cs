using System;

namespace SRC.DiscordBot.DataModels;

public sealed class KoruxaBossAttack
{
    public int Id { get; init; }
    public int BossId { get; init; }
    public required ulong DiscordUserId { get; init; }
    public required DateTimeOffset AttackedAt { get; init; }
    
    public static KoruxaBossAttack CreateNew(ulong discordUserId)
    {
        return new KoruxaBossAttack()
        {
            AttackedAt = DateTimeOffset.UtcNow,
            DiscordUserId = discordUserId
        };
    }
}