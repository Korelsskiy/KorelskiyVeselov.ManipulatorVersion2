using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Korelskiy.ManipulatorVersion2
{
    public class Platform
    {
        public List<StackPanel> Columns { get; set; }

        public Platform() { }
        private string[,] field;

        private const int containerSizeFormat = 2;
        private const int axisCount = 3;
        private const int zero = 0;
        private const string emtyTarImagePath = "../Images/emtyTar.png";
        private const string busyTarImagePath = "../Images/busyTar.png";

        public Platform(Container container, Grid containerGrid)
        {
            double xFirst;
            double yFirst;
            double zFirst;

            double fullStep;
            double lowStep;

            int xCoordinateIndex = 0;
            int yCoordinateIndex = 1;
            int zCoordinateIndex = 2;

            int columnsIndex = 0;
            int rowsIndex = 1;
            int fullStepIndex = 4;
            int roundIndex = 1;

            string[] size = new string[containerSizeFormat];
            size = container.Size.Split("x");

            string[] coordinates = new string[axisCount];
            coordinates = container.FirstCellCoordinates.Split(" ");

            xFirst = int.Parse(coordinates[xCoordinateIndex]);
            yFirst = int.Parse(coordinates[yCoordinateIndex]);
            zFirst = int.Parse(coordinates[zCoordinateIndex]);

            int numOfColumns = int.Parse(size[columnsIndex]);
            int numOfRows = int.Parse(size[rowsIndex]);

            lowStep = container.LowStep;
            fullStep = container.BigStep;

            Columns = new List<StackPanel>();

            for (int i = zero; i < numOfColumns; i++)
            {
                Columns.Add(new StackPanel());
            }

            int counter = 0;
            for (int i = zero; i < numOfRows; i++)
            {
                foreach (StackPanel panel in Columns)
                {
                    
                    Image tare = new Image() 
                    { 
                        Source = new BitmapImage(new Uri(emtyTarImagePath, UriKind.Relative)),
                        Height = 20,
                        Width = 20
                    };
                    tare.MouseDown += Tare_MouseDown;
                    
                    if ((containerGrid.Children[counter++] as Image).Tag.ToString() == "Busy")
                    {
                        tare.Tag = "Busy";
                    }
                    else
                    {
                        tare.Tag = "Empty";
                    }
                    panel.Children.Add(tare);
                }
            }
            field = new string[numOfColumns, numOfRows];

            double xCord = xFirst;
            double yCord = yFirst;

            for (int i = zero; i < numOfRows; i++)
            {
                for (int j = zero; j < numOfColumns; j++)
                {
                    field[j, i] = $"{xCord} {yCord} {zFirst}";
                    yCord += j != fullStepIndex ? lowStep : fullStep;
                    yCord = Math.Round(yCord, roundIndex);
                }

                xCord += i != fullStepIndex ? lowStep : fullStep;
                xCord = Math.Round(xCord, roundIndex);
                yCord = yFirst;
            }
            
            
        }

        private void Tare_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Image image = sender as Image;
            if (image.Tag.ToString() == "Empty")
            {
                image.Source = new BitmapImage(new Uri(busyTarImagePath, UriKind.Relative));
                image.Tag = "Busy";
            }
            else
            {
                image.Source = new BitmapImage(new Uri(emtyTarImagePath, UriKind.Relative));
                image.Tag = "Empty";
            }
        }
        public bool NewMakeMeasure(string comPortName, Manipulator manipulator)
        {
            VladTest test = new VladTest();
            int maxGripCount = 10;
            int firstGripIndex = 0;

            bool isTest = false;
            string[] grip = new string[maxGripCount];
            string text = $"";


            if (!isTest)
            {

                for (int i = zero; i < Columns.Count; i++)
                {
                    for (int j = zero; j < Columns[i].Children.Count; j++)
                    {

                        Image image = Columns[i].Children[j] as Image;
                        if (image.Tag.ToString() == "Busy")
                        {
                            text += $"{field[i, j]}|";
                        }
                    }


                }

                int removeStartIndex = text.Length - 1;
                int removeSymbolsCount = 1;
                text = text.Remove(removeStartIndex, removeSymbolsCount);
            }
            else
            {
                string firstCellCoordinates = "122 -93 18";
                text = firstCellCoordinates;
            }


            if (text.Contains('|'))
            {
                grip = text.Split('|');
            }
            else
            {
                
                grip[firstGripIndex] = text;
            }

            string grip1 = grip[firstGripIndex];

            foreach (var item in grip)
            {
                if (item != null)
                {
                    int oneSecond = 1000;
                    test.SerialCommand = true;
                    Thread.Sleep(oneSecond);
                    manipulator.Grip(item);
                    test.SerialCommand = false;
                }

            }


            return true;
        }
    }
}
