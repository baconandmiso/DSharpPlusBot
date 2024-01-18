using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.CommandsNext.Entities;
using DSharpPlus.Entities;

namespace DSharpFirstBot;

public class HelpFormatter(CommandContext ctx) : DefaultHelpFormatter(ctx)
{
    public override BaseHelpFormatter WithCommand(Command command)
    {
        EmbedBuilder.AddField(command.Name, command.Description);            
        return this;
    }

    public override BaseHelpFormatter WithSubcommands(IEnumerable<Command> cmds)
    {
        foreach (var cmd in cmds)
        {
            EmbedBuilder.AddField(cmd.Name, cmd.Description);            
            // _strBuilder.AppendLine($"{cmd.Name} - {cmd.Description}");
        }

        return this;
    }

    public override CommandHelpMessage Build()
    {
        EmbedBuilder.Color = DiscordColor.SpringGreen;
       // return base.Build();

        return new CommandHelpMessage(embed: EmbedBuilder);
    }
}
