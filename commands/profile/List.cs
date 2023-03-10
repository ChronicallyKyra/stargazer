using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Spectre.Console.Cli;
using Spectre.Console;
using Stargazer.Dbus;


namespace Stargazer.Commands {
    public class List : AsyncCommand<ListSettings> {
        public override async Task<int> ExecuteAsync([NotNull] CommandContext context, [NotNull] ListSettings settings) {
            await DbusClient.ConnectAsync();

            ProfileInfo[] profiles = await DbusClient.ListProfilesAsync();

            if (profiles.Length <= 0) {
                AnsiConsole.WriteLine("There are no profiles to list...");
                return 0;
            }

            foreach (ProfileInfo profileInfo in profiles) {
                Mod[] mods = await DbusClient.ListModsAsync(profileInfo.Name.ToLower());

                AnsiConsole.MarkupLine("# [underline]{0}[/] -- {1} {2}", profileInfo.Name, profileInfo.Loader, profileInfo.MinecraftVersion);
                
                foreach (Mod mod in mods) {
                    AnsiConsole.MarkupLine("    - [bold]{0}[/] / [italic]{1}[/] [dim]({2})[/]", mod.Title, mod.ClientDependency, mod.FileName);
                }
            }

            return 0;
        }
    }
    public class ListSettings : CommandSettings {
        [CommandOption("-v|--version <version>")]
        public string Version { get; set; }

        [CommandOption("-l|--loader <loader>")]
        public string Loader { get; set; }
    }
}