using ReMarkable.NET.Unix.Driver;
using ReMarkable.NET.Util;

internal class Program
{
    private static void Main(string[] args)
    {
        var devices = DeviceUtils.GetInputDeviceEventHandlers();
        foreach (var kv in devices)
        {
            Console.Out.WriteLine($"{kv.Key} = {kv.Value}");
        }

        Console.Out.WriteLine("Detected: " + DeviceType.GetDevice());
    }
}