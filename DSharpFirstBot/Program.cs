using DSharpFirstBot;
using DSharpFirstBot.Modules;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Reflection;

namespace MyBot;

class Program
{
    static async Task Main()
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        var logFactory = new LoggerFactory().AddSerilog();
        var discord = new DiscordShardedClient(new DiscordConfiguration() 
        {
            Token = Environment.GetEnvironmentVariable("bot_token"),
            TokenType = TokenType.Bot,
            Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents | DiscordIntents.GuildPresences,
            LoggerFactory = logFactory,
        });

        await discord.UseInteractivityAsync(new DSharpPlus.Interactivity.InteractivityConfiguration()
        {
            PollBehaviour = PollBehaviour.KeepEmojis,
            Timeout = TimeSpan.FromSeconds(30)
        });

        var commands = await discord.UseCommandsNextAsync(new CommandsNextConfiguration()
        {
            StringPrefixes = new[] { "?" }
        });

        var slash = await discord.UseSlashCommandsAsync();

        discord.Ready += async (s, e) => {
            var activity = new DiscordActivity($"guilds: {s.Guilds.Count}, shard: {s.ShardCount}");

            await discord.UpdateStatusAsync(activity: activity, userStatus: UserStatus.Online);
        };

        commands.RegisterCommands(Assembly.GetExecutingAssembly());
        commands.SetHelpFormatter<HelpFormatter>();

        # if DEBUG
            slash.RegisterCommands<GuildModule>(738958548146585646);
        # else
            await slash.RegisterCommands(Assembly.GetExecutingAssembly());
        #endif


        await discord.StartAsync();
        await Task.Delay(-1);
    }
}