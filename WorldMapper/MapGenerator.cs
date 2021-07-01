using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Map;

namespace WorldMapper
{
    public static class MapGenerator
    {
        public static DirectBitmap Create(Rectangle? region = null)
        {
            Rectangle realRegion;
            realRegion = region ?? new Rectangle(0, 0, Main.maxTilesX, Main.maxTilesY);

            var bitmap = new DirectBitmap(realRegion.Width, realRegion.Height);

            var replacedMap = false;
            if (Main.Map == null)
            {
                replacedMap = true;
                Main.Map = new WorldMap(0, 0);
            }

            MapHelper.Initialize();

            for (var x = 0; x < realRegion.Width; x++)
            {
                for (var y = 0; y < realRegion.Height; y++)
                {
                    var tile = MapHelper.CreateMapTile(x + realRegion.X, y + realRegion.Y, byte.MaxValue);
                    var col = MapHelper.GetMapTileXnaColor(ref tile);

                    bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(col.A, col.R, col.G, col.B));
                }
            }

            if (replacedMap)
                Main.Map = null;
            return bitmap;
        }
    }
}