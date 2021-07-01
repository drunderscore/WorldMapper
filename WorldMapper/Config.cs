namespace WorldMapper
{
    public class Config
    {
        public const string DefaultPath = "ServerPlugins/WorldMapper.json";

        public bool SaveMapOnWorldLoad { get; set; }
        public bool SaveMapOnWorldSave { get; set; }
        public string MapFileNameFormat { get; set; } = "{0}_{1}_{2}.png";
    }
}