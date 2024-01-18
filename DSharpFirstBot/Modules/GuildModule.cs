using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;

namespace DSharpFirstBot.Modules;

[SlashCommandGroup("guild", "サーバー関係だよ")]
public class GuildModule : ApplicationCommandModule
{
    [SlashCommand("user", "ユーザーの情報をだすよ")]
    public async Task UserCommand(InteractionContext ctx, [Option("keyword", "検索キーワード(ユーザー名")] string keyword = "", 
        [Option("member", "ユーザーオブジェクト")] DiscordUser? member = null)
    {
        var interactivity = ctx.Client.GetInteractivity();
        if (!string.IsNullOrEmpty(keyword))
        {
            var members = ctx.Guild.GetAllMembersAsync().Result.Where(x => x.DisplayName.Contains(keyword)).ToArray();
            if (members.Length >= 1 && member != null) // + オブジェクト検索
            {
                Array.Resize(ref members, members.Length + 1);
                members[^1] = (DiscordMember)member;
            }
            else if (members.Length == 0 && member != null) // キーワード検索:0件, オブジェクトのみ
            {
                await ctx.CreateResponseAsync(embed: CreateUserInfo((DiscordMember)member));
                return;
            }
            else if (members.Length == 0 && member == null) // 見つからない
            {
                await ctx.CreateResponseAsync("ユーザーが見つかりませんでした。", true);
                return;
            }

            var pages = new List<Page>();
            foreach (var m in members) 
            {
                Console.WriteLine(m.Id);
                pages.Add(new Page(embed: CreateUserInfo(m)));
            }

            await ctx.Interaction.SendPaginatedResponseAsync(false, ctx.Member, pages);
        }
    }

    private DiscordEmbedBuilder CreateUserInfo(DiscordMember member)
    {
        var roles = string.Join(',', member.Roles.Select(x => x.Mention).ToArray());
        var embedBuilder = new DiscordEmbedBuilder()
            .WithTitle($"ユーザー情報 - {member.DisplayName}")
            .WithDescription($"**{member.DisplayName}** の情報を表示しています。\n\n" +
                             $"**基本情報**\n" + 
                             $"名前 (ID): **{member.DisplayName}** ({member.Id}) \n" +
                             $"Bot?: {member.IsBot}\n" +
                             $"Flags: {string.Join(',', member.OAuthFlags)}\n" +
                             $"作成日時: **{member.CreationTimestamp}**\n\n");
       return embedBuilder;
    }
}
