using GameRes.Formats.NonColor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SchemeTool
{
    class Program
    {
        static void Main(string[] args)
        {
            // Load database
            using (Stream stream = File.OpenRead(".\\GameData\\Formats.dat"))
            {
                GameRes.FormatCatalog.Instance.DeserializeScheme(stream);
            }

            GameRes.Formats.Entis.NoaOpener format = GameRes.FormatCatalog.Instance.ArcFormats
                           .FirstOrDefault(a => a is GameRes.Formats.Entis.NoaOpener) as GameRes.Formats.Entis.NoaOpener;
            string name = "Manyokko ☆ Makorin ~Boku mo Mahou Shoujo!?~";
            if (format != null)
            {
                GameRes.Formats.Entis.NoaScheme scheme = format.Scheme as GameRes.Formats.Entis.NoaScheme;
                

                if (scheme.KnownKeys.ContainsKey(name)) scheme.KnownKeys.Remove(name);
                scheme.KnownKeys[name] = new Dictionary<string, string>
                {
                    { "d01.dat", "cfduys54Rg(uycUIGD" },
                    { "d03.dat", "ijh&ubt0867t6FTYU" }
                };
            }
#if false
            if (format != null)
            {
                GameRes.Formats.KiriKiri.Xp3Scheme scheme = format.Scheme as GameRes.Formats.KiriKiri.Xp3Scheme;

                // Add scheme information here

#if false
                byte[] cb = File.ReadAllBytes(@"MEM_10014628_00001000.mem");
                var cb2 = MemoryMarshal.Cast<byte, uint>(cb);
                for (int i = 0; i < cb2.Length; i++)
                    cb2[i] = ~cb2[i];
                var cs = new GameRes.Formats.KiriKiri.CxScheme
                {
                    Mask = 0x000,
                    Offset = 0x000,
                    PrologOrder = new byte[] { 0, 1, 2 },
                    OddBranchOrder = new byte[] { 0, 1, 2, 3, 4, 5 },
                    EvenBranchOrder = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 },
                    ControlBlock = cb2.ToArray()
                };
                GameRes.Formats.KiriKiri.ICrypt crypt = new GameRes.Formats.KiriKiri.CxEncryption(cs);
#else
                GameRes.Formats.KiriKiri.ICrypt crypt = new GameRes.Formats.KiriKiri.XorCrypt(0x00);
#endif

                scheme.KnownSchemes.Add("game title", crypt);
            }
#endif

            var gameMap = typeof(GameRes.FormatCatalog).GetField("m_game_map", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .GetValue(GameRes.FormatCatalog.Instance) as Dictionary<string, string>;

            if (gameMap != null)
            {
                string game = "manyo2.exe";
                if (gameMap.ContainsKey(game)) gameMap.Remove(game);
                // Add file name here
                gameMap.Add(game, name);
                

            }

            // Save database
            using (Stream stream = File.Create(".\\GameData\\Formats.dat"))
            {
                GameRes.FormatCatalog.Instance.SerializeScheme(stream);
            }
        }
    }
}
