using System.Windows.Controls;

namespace KorelskiyVeselov.ManipulatorVersion2.Models
{
    public class Cell
    {
        public int XCoordinate { get; set; }
        public int YCoordinate { get; set; }
        public bool IsEmpty { get; set; }

        public Image Image { get; set; }
    }
}
