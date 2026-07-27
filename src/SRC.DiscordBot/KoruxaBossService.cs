using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SRC.DiscordBot;

internal sealed class KoruxaBossService(AppDbContext dbContext)
{
    public async Task AttackAsync(ulong discordUserId, CancellationToken cancellationToken)
    {
        _ = await GetOrAddUserAsync(discordUserId, cancellationToken);
        
        var boss = await GetOrAddBossAsync(cancellationToken);
        
        var attack = KoruxaBossAttack.CreateNew(discordUserId);

        boss.Attacks.Add(attack);
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }
    
    public async Task KillBossAsync(CancellationToken cancellationToken)
    {
        var boss = await dbContext.Boss
            .SingleOrDefaultAsync(x => !x.KilledAt.HasValue, cancellationToken);
        
        if (boss is null) return;

        boss.MarkAsKilled(DateTimeOffset.UtcNow);
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task<KoruxaUser> GetOrAddUserAsync(ulong discordUserId, CancellationToken cancellationToken)
    {
        var user = await dbContext.User
            .SingleOrDefaultAsync(x => x.DiscordUserId == discordUserId, cancellationToken);
        
        if (user is not null) return user;
        
        user = KoruxaUser.CreateNew(discordUserId);
        
        dbContext.User.Add(user);
        
        return user;
    }

    private async Task<KoruxaBoss> GetOrAddBossAsync(CancellationToken cancellationToken)
    {
        var boss = await dbContext.Boss
            .SingleOrDefaultAsync(x => !x.KilledAt.HasValue, cancellationToken);

        if (boss is not null) return boss;

        boss = KoruxaBoss.CreateNew();
        
        dbContext.Boss.Add(boss);
        
        return boss;
    }
}