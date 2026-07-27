using System;
using System.Collections.Generic;

namespace SRC.DiscordBot;

public sealed class KoruxaBoss
{
    public int Id { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? KilledAt { get; private set; }
    public List<KoruxaBossAttack> Attacks { get; init; } = [];

    public void MarkAsKilled(DateTimeOffset newValue)
    {
        KilledAt = newValue;
    }

    public static KoruxaBoss CreateNew()
    {
        return new KoruxaBoss()
        {
            CreatedAt = DateTimeOffset.UtcNow
        };
    }
}

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