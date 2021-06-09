using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Korelskiy.ManipulatorVersion2
{
    public class Container
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Size { get; set; }
        public string FirstCellCoordinates { get; set; }
        public double LowStep { get; set; }
        public double BigStep { get; set; }

        public Container() { }
        public Container(string title, string size, string firstCellCoordinates,
            double lowStep, double bigStep)
        {
            Title = title;
            Size = size;
            FirstCellCoordinates = firstCellCoordinates;
            LowStep = lowStep;
            BigStep = bigStep;
        }

        public int GetNumberOfRows()
        {
            string[] size = new string[2];
            size = Size.Split("x");
            return int.Parse(size[0]);
        }

        public int GetNumberOfColumns()
        {
            string[] size = new string[2];
            size = Size.Split("x");
            return int.Parse(size[1]);
        }

        private string[] ConvertSizeToArray()
        {
            return Size.Split("x");
        }

        private string[] ConvertCoordinatesToArray()
        {
            return FirstCellCoordinates.Split(" ");
        }

        //public string[,] TransformingContainer()
        //{

        //}

        public string GetCoordinates()
        {
            return "";
        }
    }
}
