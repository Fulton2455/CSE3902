using System;

namespace _3902sprint0
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            try
            {
                using (var game = new Game1())
                    game.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                System.IO.File.WriteAllText("crash.txt", ex.ToString());
                throw;
            }
        }
    }
}