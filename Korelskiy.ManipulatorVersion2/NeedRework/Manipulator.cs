using System;
using System.IO.Ports;

namespace Korelskiy.ManipulatorVersion2
{
    public class Manipulator
    {
        public int Speed { get; set; } = 10_000;
        public int Delay { get; set; } = 1000;

        public string standartHeight = "40";
        public string upperHeight = "80";

        public string platform = "156 60 ";
        public string platformHeiht = "55";

        public SerialPort SerialPort { get; set; } = new SerialPort();

        public Manipulator()
        {
            SerialPort.BaudRate = 115_200;
        }
        private void SendCommand(string command, SerialPort serialPort)
        {
            serialPort.Write(command);
            serialPort.ReadLine();
        }

        private void SendCommandRotate(string command, SerialPort serialPort)
        {
            serialPort.Write(command);

            string angle = "";

            while (!angle.Contains("V"))
                angle = serialPort.ReadExisting();

            if (angle.Contains("V"))
            {
                SendCommand($"#n G2202 N3 V{angle.Split('V')[1]}\n", serialPort);
            }

        }

        public string[] Cord(string s)
        {
            double x = Convert.ToDouble(s.Split(' ')[0]);
            double y = Convert.ToDouble(s.Split(' ')[1]);
            double z = Convert.ToDouble(s.Split(' ')[2]);

            string[] coordinate = new string[]{
                x.ToString().Replace(',', '.'),
                y.ToString().Replace(',', '.'),
                z.ToString().Replace(',', '.')
                };

            return coordinate;
        }

        public void Move(string s)
        {
            string[] coordinate = Cord(s);

            SendCommand($"#n G0 X{coordinate[0]} Y{coordinate[1]} Z{coordinate[2]} F{Speed}\n", SerialPort);
        }

        public void Move(string x, string y, string z)
        {
            SendCommand($"#n G0 X{x} Y{y} Z{z} F{Speed}\n", SerialPort);
        }

        public void Hand(bool enable)
        {
            SendCommand($"#n M2232 V{Convert.ToInt32(enable)}\n", SerialPort);
            SendCommand($"#n M2231 V{Convert.ToInt32(enable)}\n", SerialPort);
        }

        public void Grip(string s)
        {
            string[] coordinate = Cord(s);

            Move(coordinate[0], coordinate[1], standartHeight);
            Move(coordinate[0], coordinate[1], standartHeight);

            SendCommandRotate($"#n P2206 N0\n", SerialPort);

            Move(coordinate[0], coordinate[1], coordinate[2]);

            Hand(true);

            SendCommand($"#n G2004 P{Delay}\n", SerialPort);

            Move(coordinate[0], coordinate[1], standartHeight);
            Move(coordinate[0], coordinate[1], upperHeight);

            Move(platform + upperHeight);

            SendCommandRotate($"#n P2206 N0\n", SerialPort);

            Move(platform + platformHeiht);

            SendCommand($"#n G2004 P{Delay}\n", SerialPort);

            Move(platform + upperHeight);

            Move(coordinate[0], coordinate[1], standartHeight);

            SendCommandRotate($"#n P2206 N0\n", SerialPort);

            Move(coordinate[0], coordinate[1], coordinate[2]);

            Hand(false);

            SendCommand($"#n G2004 P{Delay}\n", SerialPort);

            Move(coordinate[0], coordinate[1], standartHeight);
        }
    }
}
