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
            string[] convertedSize = Size.Split("x");

            int cols = int.Parse(convertedSize[0]);
            int rows = int.Parse(convertedSize[1]);

            string[] convertedCoordinates = FirstCellCoordinates.Split(" ");

            int xCoord = int.Parse(convertedCoordinates[0]);
            int yCoord = int.Parse(convertedCoordinates[1]);
            //int zCoord = int.Parse(convertedCoordinates[2]);

            Cells = new Cell[cols, rows];
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    Cells[i, j] = new Cell(xCoord, yCoord);      
                    xCoord += j != 5 ? (int)LowStep : (int)BigStep;
                }
                yCoord += i != 5 ? (int)LowStep : (int)BigStep;
            }
        }

        public void Draw(Grid gridToDraw)
        {
           

            LoadCells();

        }


    }
}
