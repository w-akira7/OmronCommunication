// See https://aka.ms/new-console-template for more information
using System.Threading;


var length = 44444;
var lengthCode = new byte[2] { (byte)(length / 256), (byte)(length % 256) };

Console.WriteLine(length);


Console.WriteLine("字节数组：{0} ", BitConverter.ToString(lengthCode));
