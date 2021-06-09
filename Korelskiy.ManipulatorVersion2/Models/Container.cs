using KorelskiyVeselov.ManipulatorVersion2.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Controls;

namespace Korelskiy.ManipulatorVersion2
{
    public class Container
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Size { get; set; }
        [Required]
        public string FirstCellCoordinates { get; set; }
        [Required]
        public double LowStep { get; set; }
        [Required]
        public double BigStep { get; set; }
        [NotMapped]
        public Cell[,] Cells { get; set; }

        private void LoadCells()
        {
            string[] cols = Size.Split("x");
        }

        public void Draw(Grid gridToDraw)
        {
           

            LoadCells();

        }


    }
}
