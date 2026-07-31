using SRC.DiscordBot;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using NetCord.Hosting.Gateway;
using NetCord.Hosting.Services.ApplicationCommands;
using NetCord.Hosting.Services.ComponentInteractions;
using Nixon.Extensions.Hosting.Jobs;
using Nixon.Extensions.Serilog.AspNetCore;

var builder = Host.CreateApplicationBuilder(args)
    .AddSerilogConfiguration();

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

builder.Services.AddDiscordGateway(options =>
{
    options.Token = builder.Configuration["BOT_TOKEN"];
});

builder.Services.AddDbContextFactory<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite"));
    
    options.UseSnakeCaseNamingConvention();
});

builder.Services.AddCronJob<BossAttackNotifier>("0 */10 * * * ?");

builder.Services.TryAddScoped<KoruxaBossService>();

builder.Services.AddApplicationCommands();
builder.Services.AddComponentInteractions();

var host = builder.Build(); 

host.AddApplicationCommandModule<SlashCommandModule>();
host.AddComponentInteractionModule<BossTimerComponentModule>();

using (var scope = host.Services.CreateScope())
{
    await scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.MigrateAsync();
}

await host.RunAsync();