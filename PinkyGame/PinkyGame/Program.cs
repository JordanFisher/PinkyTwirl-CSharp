using System;

namespace PinkyGame
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (PinkyGame game = new PinkyGame())
            {
                game.Run();
            }
        }
    }
#endif
}

