using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using OTAPI;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;

namespace WorldMapper
{
    [ApiVersion(2, 1)]
    public class Plugin : TerrariaPlugin
    {
        public override string Name => "World Mapper";
        public override string Author => "James Puleo";
        public override string Description => "Generates a PNG map of the entire world";
        public override Version Version => Assembly.GetExecutingAssembly().GetName().Version;
        private Config _config;

        public Plugin(Main game) : base(game)
        {
        }

        public override void Initialize()
        {
            _config = File.Exists(Config.DefaultPath)
                ? JsonConvert.DeserializeObject<Config>(File.ReadAllText(Config.DefaultPath))
                : new Config();

            Hooks.World.IO.PostLoadWorld += OnPostLoadWorld;
            Hooks.World.IO.PostSaveWorld += OnPostSaveWorld;

            Commands.ChatCommands.Add(new Command("worldmapper.generatemap", args =>
            {
                var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                var fileName = args.Parameters.Count >= 1
                    ? args.Parameters[0]
                    : string.Format(_config.MapFileNameFormat, Main.worldName, "manual", now);

                using var bitmap = MapGenerator.Create();
                Save(bitmap, fileName);
                args.Player.SendSuccessMessage($"Map saved as {fileName}");
            }, "generatemap"));
        }

        private void OnPostSaveWorld(bool usecloudsaving, bool resettime)
        {
            DoAutomaticGenerate("save");
        }

        private void OnPostLoadWorld(bool loadfromcloud)
        {
            DoAutomaticGenerate("load");
        }

        private void DoAutomaticGenerate(string why)
        {
            var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            using var bitmap = MapGenerator.Create();
            Save(bitmap, string.Format(_config.MapFileNameFormat, Main.worldName, why, now));
        }

        private static void Save(DirectBitmap bitmap, string fileName)
        {
            bitmap.Bitmap.Save(fileName);
            TShock.Log.ConsoleInfo($"Map generated and saved as {fileName}");
        }
    }
}