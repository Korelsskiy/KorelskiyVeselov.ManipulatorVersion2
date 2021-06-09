using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Korelskiy.ManipulatorVersion2
{
    /// <summary>
    /// Interaction logic for Page.xaml
    /// </summary>
    public partial class VladTest : Page
    {
        public bool SerialCommand { get; set; } = false;
        public bool SerialConnected { get; set; } = false;

        private Manipulator manipulator = new Manipulator();
        public VladTest()
        {
            InitializeComponent();
        }

        private void SerialListening()
        {
            while (SerialConnected)
            {
                try
                {
                    if (!SerialCommand)
                    {
                        string serialRead = manipulator.SerialPort.ReadExisting();
                        if (serialRead != "")
                        {
                            Dispatcher.Invoke(() => ConsolePanel.Text += serialRead.ToString() + "\n");

                        }
                    }
                }
                catch
                {

                }
            }
        }

        private void Find_Click(object sender, RoutedEventArgs e)
        {
            string[] PortsNumber = SerialPort.GetPortNames();

            if (PortsNumber.Length == 0)
            {
                ConsolePanel.Text += "Порты не найдены\n";
            }
            else
            {
                PortsNameBox.Items.Clear();
                foreach (string p in PortsNumber)
                {
                    PortsNameBox.Items.Add(p);
                }
            }
        }
        public Manipulator GetManipulator()
        {
            return manipulator;
        }

        private void Connetion_Click(object sender, RoutedEventArgs e)
        {
            if (Connetion.Content.ToString() == "Подключиться")
            {
                if (PortsNameBox.SelectedItem == null)
                {
                    ConsolePanel.Text += "Выберите порт\n";
                }
                else
                {
                    manipulator.SerialPort.PortName = PortsNameBox.SelectedItem.ToString();
                    try
                    {
                        manipulator.SerialPort.Open();
                        SerialConnected = true;

                        Thread thread = new Thread(new ThreadStart(SerialListening));
                        thread.Start();

                        MainWindow.SetManipulator(manipulator);

                        Connetion.Content = "Отключиться";
                        Connetion.Background = Brushes.LightGreen;
                    }
                    catch (Exception ex)
                    {
                        ConsolePanel.Text += $"{ex}\n";
                    }
                }
            }
            else if (Connetion.Content.ToString() == "Отключиться")
            {
                SerialConnected = false;
                manipulator.SerialPort.Close();

                Connetion.Content = "Подключиться";
                Connetion.Background = Brushes.Red;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (SerialConnected)
            {
                string coords = "";
                if (xNumber.Text != "")
                {
                    coords += xNumber.Text + " ";
                }
                if (yNumber.Text != "")
                {
                    coords += yNumber.Text + " ";
                }
                if (zNumber.Text != "")
                {
                    coords += zNumber.Text;
                }
                SerialCommand = true;
                manipulator.Move(coords);
                SerialCommand = false;
            }
            else
            {
                ConsolePanel.Text += "Устройство не подключено\n";
            }
        }
    }
}
