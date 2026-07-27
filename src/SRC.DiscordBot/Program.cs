using SRC.DiscordBot;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NetCord.Hosting.Gateway;
using NetCord.Hosting.Services.ApplicationCommands;
using Nixon.Extensions.Hosting.Jobs;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddDiscordGateway(options =>
{
    options.Token = builder.Configuration["BOT_TOKEN"];
});

builder.Services.AddDbContextFactory<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite"));
    
    options.UseSnakeCaseNamingConvention();
});

builder.Services.AddCronJob<BossAttackNotifier>("* * * * * *");

builder.Services.TryAddScoped<KoruxaBossService>();

builder.Services.AddApplicationCommands();
    
var host = builder.Build(); 

host.AddApplicationCommandModule<SlashCommandModule>();

using (var scope = host.Services.CreateScope())
{
    await scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.MigrateAsync();
}

await host.RunAsync();