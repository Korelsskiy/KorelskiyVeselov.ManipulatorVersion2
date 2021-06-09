using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;


namespace Korelskiy.ManipulatorVersion2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Platform platform;
        private static Manipulator manipulator;
        private Container containerForDisplay;
        private const int zero = 0;
        private const string emtyTarImagePath = "../Images/emtyTar.png";
        private const string busyTarImagePath = "../Images/busyTar.png";
        private const int cellImageSize = 20;
        private const string usingPort = "COM7";


        public MainWindow()
        {
            InitializeComponent();
            LoadComboBoxes();
        }

        public static void SetManipulator(Manipulator man)
        {
            manipulator = man;
        }
        private void DisplayContainer(Container container)
        {
            containerViewGrid.Children.Clear();
            containerViewGrid.RowDefinitions.Clear();
            containerViewGrid.ColumnDefinitions.Clear();

            int columnsCount = container.GetNumberOfColumns();
            int rowsCount = container.GetNumberOfRows();
            int cellsCount = columnsCount * rowsCount;

            for (int i = zero; i < rowsCount; i++)
            {
                containerViewGrid.RowDefinitions.Add(new RowDefinition());
            }

            for (int i = zero; i < columnsCount; i++)
            {
                containerViewGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            int rowCounter = zero;
            int colCounter = zero;


            for (int j = zero; j < rowsCount; j++)
            {
                for (int i = zero; i < columnsCount; i++)
                {
                    Image image = new Image()
                    { 
                        Source = new BitmapImage(new Uri(emtyTarImagePath, UriKind.Relative)),
                        Height = cellImageSize,
                        Width = cellImageSize 
                    };
                    image.MouseDown += Image_MouseDown; ;
                    image.Tag = "Empty";

                    containerViewGrid.Children.Add(image);
                    Grid.SetRow(image, rowCounter);
                    Grid.SetColumn(image, colCounter++);
                }
                colCounter = zero;
                rowCounter++;
            }
            
            
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
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

        private void LoadComboBoxes()
        {
            AppDbContext context = new AppDbContext();

            newContainerComboBox.ItemsSource = context.Containers.ToList();
            newContainerComboBox.DisplayMemberPath = "Title";
            newContainerComboBox.SelectedIndex = zero;
        }

        private void MakeMeasure_Click(object sender, RoutedEventArgs e)
        {
            platform = new Platform(containerForDisplay, containerViewGrid);
            platform.NewMakeMeasure(usingPort, manipulator);
        }

        private void addContanierButton_Click(object sender, RoutedEventArgs e)
        {
            containerForDisplay = new Container();

            containerForDisplay.Title = containerTitle.Text;
            containerForDisplay.Size = columnsCont.Text + "x" + rowsCont.Text;
            containerForDisplay.FirstCellCoordinates = firstCell.Text;
            containerForDisplay.LowStep = double.Parse(lowStep.Text);
            containerForDisplay.BigStep = double.Parse(bigStep.Text);

            AppDbContext context = new AppDbContext();
            context.Containers.Add(containerForDisplay);
            context.SaveChanges();
        }
        private void newContainerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            containerForDisplay = newContainerComboBox.SelectedItem as Container;
            DisplayContainer(containerForDisplay);
        }

        private void speedTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        { 
            if(speedTextBox.Text != "" && manipulator != null)
            {
                manipulator.Speed = Convert.ToInt32(speedTextBox.Text);
            }
        }

        private void delayTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (delayTextBox.Text != "" && manipulator != null)
            {
                manipulator.Delay = Convert.ToInt32(delayTextBox.Text);
            }
        }
    }
}
