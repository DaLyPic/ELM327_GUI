using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace ELM327_GUI.MVVM.Model
{
    public class DbcMessage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Dlc { get; set; }
        public string Transmitter { get; set; }
        public List<DbcSignal> Signals { get; set; } = new List<DbcSignal>();
    }

    public class DbcSignal
    {
        public string Name { get; set; }
        public string Multiplexer { get; set; }
        public int StartBit { get; set; }
        public int Length { get; set; }
        public bool IsLittleEndian { get; set; }
        public bool IsSigned { get; set; }
        public double Factor { get; set; }
        public double Offset { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
        public string Unit { get; set; }
        public string Sender { get; set; }
    }

    public class DbcParser
    {
        private static Regex msgRegex = new Regex(@"^BO_\s+(\d+)\s+(\w+)\s*:\s*(\d+)\s+(\w+)");
        //private static Regex sigRegex = new Regex(@"^SG_\s+(\w+)\s+(\w+)\s*:\s*(\d+)\|(\d+)@(\d)([+-])\s+\(([^,]+),([^)]+)\)\s+\[[^\]]*\]\s+""([^""]*)""\s+(.*)$");
        private static Regex sigRegex = new Regex(@"^\sSG_\s+(\w+)\s+(\w+)\s*:\s*(\d+)\|(\d+)@(\d)([+-])\s+\(([^,]+),([^)]+)\)\s+\[(-?\d+)\|(\d+\.?\d*?)]\s+""([^""]*)""\s+(.*)$");

        public static List<DbcMessage> Parse(string filePath)
        {
            var messages = new List<DbcMessage>();
            DbcMessage currentMsg = null;

            foreach (var line in File.ReadLines(filePath))
            {
                var msgMatch = msgRegex.Match(line);
                if (msgMatch.Success)
                {
                    currentMsg = new DbcMessage
                    {
                        Id = int.Parse(msgMatch.Groups[1].Value),
                        Name = msgMatch.Groups[2].Value,
                        Dlc = int.Parse(msgMatch.Groups[3].Value),
                        Transmitter = msgMatch.Groups[4].Value
                    };
                    messages.Add(currentMsg);
                    continue;
                }

                var sigMatch = sigRegex.Match(line);
                if (sigMatch.Success && currentMsg != null)
                {
                    var signal = new DbcSignal
                    {
                        Name = sigMatch.Groups[1].Value,
                        Multiplexer = sigMatch.Groups[2].Value,
                        StartBit = int.Parse(sigMatch.Groups[3].Value),
                        Length = int.Parse(sigMatch.Groups[4].Value),
                        IsLittleEndian = sigMatch.Groups[5].Value == "1",
                        IsSigned = sigMatch.Groups[6].Value == "-",
                        Factor = double.Parse(sigMatch.Groups[7].Value, CultureInfo.InvariantCulture),
                        Offset = double.Parse(sigMatch.Groups[8].Value, CultureInfo.InvariantCulture),
                        Min = double.Parse(sigMatch.Groups[9].Value, CultureInfo.InvariantCulture),
                        Max = double.Parse(sigMatch.Groups[10].Value, CultureInfo.InvariantCulture),
                        Unit = sigMatch.Groups[11].Value,
                        Sender = sigMatch.Groups[12].Value
                    };
                    currentMsg.Signals.Add(signal);
                }
                else if (line.Trim().StartsWith("SG_"))
                {
                    Console.WriteLine("Nem illeszkedő SG_ sor: " + line);
                }
            }

            return messages;
        }
    }
}

//class Program
//{
//    static void Main()
//    {
//        var messages = DbcParser.Parse("11-bit-OBD2-v4.0.dbc");

//        foreach (var msg in messages)
//        {
//            Console.WriteLine($"[ID {msg.Id}] {msg.Name} ({msg.Dlc} byte) - {msg.Transmitter}");
//            foreach (var sig in msg.Signals)
//            {
//                Console.WriteLine($"   - Signal: {sig.Name}, Multiplexer: {sig.Multiplexer}, StartBit: {sig.StartBit}, Length: {sig.Length}, Factor: {sig.Factor}, Offset: {sig.Offset}, Signal value range MIN: {sig.Min}, MAX: {sig.Max}, Unit: {sig.Unit}, Sender: {sig.Sender}");
//            }
//        }
//        Console.ReadKey();
//    }

//}