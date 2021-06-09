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

            containerViewGrid.Children.Clear();
            containerViewGrid.RowDefinitions.Clear();
            containerViewGrid.ColumnDefinitions.Clear();

            containerForDisplay.Draw(containerViewGrid);

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
