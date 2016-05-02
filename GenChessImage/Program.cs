using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenChessImage
{
    class Program
    {
        static void Main(string[] args)
        {
            // testing
            string startpos = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
            var image = new Image();
            image.build_image(startpos, "paneltest", "true");

            if (args.Length == 0)
            {
                Console.WriteLine("args is null"); // Check for null array
            }
            else
            {
                var newimage = new Image();
                newimage.build_image(args[0], args[1], args[2]);
            }
        }
    }
}
