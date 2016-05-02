using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.IO;
using System.Configuration;

namespace GenChessImage
{
    class Image
    {
        public void build_image(string position, string filename, string includeCoords = "false")
        {
            // This number controls the size of the board and which images to use
            // Board size = 8 x tile size + 1px border + 20px right + 20px bottom
            int tile_size = Int32.Parse(ConfigurationManager.AppSettings["TileSize"]);
            string dir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string chessFont = ConfigurationManager.AppSettings["ChessFont"];
            bool drawBorderTiles = false;
            bool.TryParse(ConfigurationManager.AppSettings["DrawBorderTiles"].ToLower(), out drawBorderTiles);            

            // Load all images for Chess pieces
            var pieces = new Dictionary<string, System.Drawing.Image>();
            pieces.Add("B", System.Drawing.Image.FromFile(String.Concat(dir, @"\images\", chessFont, @"\" + tile_size.ToString(), @"\wb.png")));
            pieces.Add("K", System.Drawing.Image.FromFile(String.Concat(dir, @"\images\", chessFont, @"\" + tile_size.ToString(), @"\wk.png")));
            pieces.Add("N", System.Drawing.Image.FromFile(String.Concat(dir, @"\images\", chessFont, @"\" + tile_size.ToString(), @"\wn.png")));
            pieces.Add("P", System.Drawing.Image.FromFile(String.Concat(dir, @"\images\", chessFont, @"\" + tile_size.ToString(), @"\wp.png")));
            pieces.Add("Q", System.Drawing.Image.FromFile(String.Concat(dir, @"\images\", chessFont, @"\" + tile_size.ToString(), @"\wq.png")));
            pieces.Add("R", System.Drawing.Image.FromFile(String.Concat(dir, @"\images\", chessFont, @"\" + tile_size.ToString(), @"\wr.png")));
            pieces.Add("b", System.Drawing.Image.FromFile(String.Concat(dir, @"\images\", chessFont, @"\" + tile_size.ToString(), @"\bb.png")));
            pieces.Add("k", System.Drawing.Image.FromFile(String.Concat(dir, @"\images\", chessFont, @"\" + tile_size.ToString(), @"\bk.png")));
            pieces.Add("n", System.Drawing.Image.FromFile(String.Concat(dir, @"\images\", chessFont, @"\" + tile_size.ToString(), @"\bn.png")));
            pieces.Add("p", System.Drawing.Image.FromFile(String.Concat(dir, @"\images\", chessFont, @"\" + tile_size.ToString(), @"\bp.png")));
            pieces.Add("q", System.Drawing.Image.FromFile(String.Concat(dir, @"\images\", chessFont, @"\" + tile_size.ToString(), @"\bq.png")));
            pieces.Add("r", System.Drawing.Image.FromFile(String.Concat(dir, @"\images\", chessFont, @"\" + tile_size.ToString(), @"\br.png")));

            char[] separater = { ',', ' ', '\t' };
            int[] blkTileArray = ConfigurationManager.AppSettings["BlackTileColor"].Split(separater, StringSplitOptions.RemoveEmptyEntries).Select(h => Int32.Parse(h)).ToArray();
            int[] whtTileArray = ConfigurationManager.AppSettings["WhiteTileColor"].Split(separater, StringSplitOptions.RemoveEmptyEntries).Select(h => Int32.Parse(h)).ToArray();
            int[] bkgrdArray = ConfigurationManager.AppSettings["BackGroundColor"].Split(separater, StringSplitOptions.RemoveEmptyEntries).Select(h => Int32.Parse(h)).ToArray();
            int[] letterArray = ConfigurationManager.AppSettings["TextColor"].Split(separater, StringSplitOptions.RemoveEmptyEntries).Select(h => Int32.Parse(h)).ToArray();
            int[] borderArray = ConfigurationManager.AppSettings["BorderColor"].Split(separater, StringSplitOptions.RemoveEmptyEntries).Select(h => Int32.Parse(h)).ToArray();
            int[] tileBorderArray = ConfigurationManager.AppSettings["TileBorderColor"].Split(separater, StringSplitOptions.RemoveEmptyEntries).Select(h => Int32.Parse(h)).ToArray();
            

            // new image correct size
            int coordborder = 0;
            if (includeCoords == "true") { coordborder = 15; }
            Bitmap bm = new Bitmap((tile_size * 8) + 2 + coordborder, (tile_size * 8) + 2 + coordborder);
            using (Graphics board = Graphics.FromImage(bm))
            using (SolidBrush backgroundBrush = new SolidBrush(Color.FromArgb(bkgrdArray[0], bkgrdArray[1], bkgrdArray[2])))
            using (SolidBrush letterBrush = new SolidBrush(Color.FromArgb(letterArray[0], letterArray[1], letterArray[2])))
            using (SolidBrush blackBrush = new SolidBrush(Color.FromArgb(blkTileArray[0], blkTileArray[1], blkTileArray[2])))
            using (SolidBrush whiteBrush = new SolidBrush(Color.FromArgb(whtTileArray[0], whtTileArray[1], whtTileArray[2])))
            using (Pen borderPen = new Pen(Color.FromArgb(borderArray[0], borderArray[1], borderArray[2]), 1))
            using (Pen tileBorderPen = new Pen(Color.FromArgb(tileBorderArray[0], tileBorderArray[1], tileBorderArray[2]), 1))
            {
                // Set up blank board
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if ((j % 2 == 0 && i % 2 == 0) || (j % 2 != 0 && i % 2 != 0))
                        {
                            board.FillRectangle(whiteBrush, (i * tile_size) + 1, (j * tile_size) + 1, tile_size, tile_size);
                            if (drawBorderTiles) board.DrawRectangle(tileBorderPen, new Rectangle((i * tile_size) + 1, (j * tile_size) + 1, tile_size, tile_size));
                        }
                        else if ((j % 2 == 0 && i % 2 != 0) || (j % 2 != 0 && i % 2 == 0))
                        {
                            board.FillRectangle(blackBrush, (i * tile_size) + 1, (j * tile_size) + 1, tile_size, tile_size);
                            if (drawBorderTiles) board.DrawRectangle(tileBorderPen, (i * tile_size) + 1, (j * tile_size) + 1, tile_size, tile_size);
                        }
                    }
                }

                // Add a border
                board.DrawRectangle(borderPen, new Rectangle(0, 0, (tile_size * 8) + 1, (tile_size * 8) + 1));

                // Add in coordinates if selected
                if (includeCoords == "true")
                {
                    // add text space
                    board.FillRectangle(backgroundBrush, 8 * tile_size + 2, 0, 8 * tile_size + 2 + 15, 8 * tile_size + 2 + 15);
                    board.FillRectangle(backgroundBrush, 0, 8 * tile_size + 2, 8 * tile_size + 2 + 15, 8 * tile_size + 2 + 15);

                    // add letters and numbers
                    string[] alphabetArray = { string.Empty, "A", "B", "C", "D", "E", "F", "G", "H" };
                    PointF numberpoint = new PointF();
                    numberpoint.X = (tile_size * 8) + 3; // fixed
                    PointF letterpoint = new PointF();
                    letterpoint.Y = (tile_size * 8) + 1; // fixed
                    Font textfont = new Font("Helvetica", 10);
                    for (int i = 0; i < 8; i++)
                    {
                        numberpoint.Y = (tile_size * (i)) + (tile_size / 2) - 5; // varied
                        board.DrawString((8 - i).ToString(), textfont, letterBrush, numberpoint);
                        letterpoint.X = (tile_size * (i)) + (tile_size / 2) - 5; // varied
                        board.DrawString(alphabetArray[i + 1], textfont, letterBrush, letterpoint);
                    }
                }

                // Loop through first field in position to find where pieces 
                // are located and put correct image there
                char[] splitchars = { ' ', '/' };
                string[] rows = position.Split(splitchars);
                for (int j = 0; j < 8; j++)
                {
                    string row = rows[j];
                    var col = 0;
                    for (int i = 0; i < row.Count(); i++)
                    {
                        string currlocation = row[i].ToString();
                        int spaces;
                        if (Int32.TryParse(currlocation, out spaces))
                        {
                            col += spaces;
                        }
                        else
                        {
                            Point boardlocation = new Point();
                            boardlocation.X = (col * tile_size) + 1;
                            boardlocation.Y = (j * tile_size) + 1;
                            board.DrawImage(pieces[currlocation], boardlocation);
                            col++;
                        }
                    }
                }
                string imgdir = ConfigurationManager.AppSettings["ImageDirectory"];
                string filePath = String.Concat(dir, @"\", imgdir, @"\");
                (new FileInfo(filePath)).Directory.Create();
                bm.Save(String.Concat(filePath, filename, ".png"), System.Drawing.Imaging.ImageFormat.Png);
                pieces.Clear();
                bm.Dispose();
                board.Dispose();
            }
        }
    }
}
