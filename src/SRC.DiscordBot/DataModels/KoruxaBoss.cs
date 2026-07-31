using System;
using System.Collections.Generic;

namespace SRC.DiscordBot.DataModels;

public sealed class KoruxaBoss
{
    public int Id { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? KilledAt { get; private set; }
    public bool HasNotifiedSpawn { get; private set; }
    public List<KoruxaBossAttack> Attacks { get; init; } = [];

    public void MarkAsKilled(DateTimeOffset newValue)
    {
        KilledAt = newValue;
    }

    public void MarkSpawnNotified()
    {
        HasNotifiedSpawn = true;
    }

    public static KoruxaBoss CreateNew(DateTimeOffset createdAt)
    {
        return new KoruxaBoss()
        {
            CreatedAt = createdAt
        };
    }
}