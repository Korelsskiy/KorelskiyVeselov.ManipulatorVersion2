using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace KorelskiyVeselov.ManipulatorVersion2.Models
{
    public class Cell
    {
        public int XCoordinate { get; set; }
        public int YCoordinate { get; set; }
        public bool IsEmpty { get; set; }

        public Image CellImage { get; set; }

        public Cell(int xCoord, int yCoord)
        {
            XCoordinate = xCoord;
            YCoordinate = yCoord;

            CellImage = new Image()
            {
                Source = new BitmapImage(new Uri("../Images/emtyTar.png", UriKind.Relative)),
                Height = 20,
                Width = 20
            };
        }
    }
}
