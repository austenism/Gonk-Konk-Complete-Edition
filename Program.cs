using System;

namespace Gonk_Konk_Complete_Edition
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new MainLoop())
                game.Run();
        }
    }
}
