using System;
using FGA.Steam;

namespace FGA
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {

            // Steam initialization
            if (!Steam_Manager.Initialize())
                return;

            try
            {
                using (Game1 game = new Game1())
                {
                    game.Run();
                }
            }
            catch (Exception e)
            {
                Steam_Manager.Unload();
                Console.WriteLine("Error occured while running game: " + e);

                // Throw e again after catching error :)
                throw e;
            }

            // Steam shutdown
            Steam_Manager.Unload();
        }
    }
#endif
}

