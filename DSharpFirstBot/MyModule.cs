using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;

namespace MyBot;

public class MyModule : BaseCommandModule
{
    [Command("first")]
    [Description("hello, world!を出力します。")]
    public async Task FirstCommand(CommandContext ctx)
    {
        await ctx.RespondAsync("hello, world!");
    }

    [Command("input")]
    [Description("入力テスト１")]
    public async Task InputCommand(CommandContext ctx, string input)
    {
        await ctx.RespondAsync($"input: {input}");
    }

    [Command("member")]
    [Description("入力テスト２")]
    public async Task MemberCommand(CommandContext ctx, DiscordMember member)
    {
        await ctx.RespondAsync($"{member.DisplayName}");
    }

    [Command("pageination")]
    [Description("ページネーション テスト")]
    public async Task PageInarionCommand(CommandContext ctx)
    {
        List<Page> pages = [new Page("あああ"), new Page("いいい")];

        await ctx.Channel.SendPaginatedMessageAsync(ctx.Member, pages);
    }

    [Command("reaction")]
    [Description("wait for reatction")]
    public async Task ReactionCommand(CommandContext ctx)
    {
        var emoji = DiscordEmoji.FromName(ctx.Client, ":ok_hand:");
        var message = await ctx.RespondAsync($"{ctx.Member!.Mention}, react with {emoji}");
        await message.CreateReactionAsync(emoji);
        var result = await message.WaitForReactionAsync(ctx.Member, emoji);

        if (!result.TimedOut)
            await ctx.RespondAsync("ok!");
    }
}
