using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ReMarkable.NET.Unix.Driver
{
    /// <summary>
    ///     Provides a set of methods that assist in interacting with hardware devices
    /// </summary>
    public class DeviceUtils
    {
        /// <summary>
        ///     Parses `/proc/bus/input/devices` to create a mapped dictionary of input device names and their event streams
        /// </summary>
        /// <returns>A <see cref="Dictionary{TKey,TValue}" /> mapping device names to handler event stream filenames</returns>
        public static Dictionary<string, string> GetInputDeviceEventHandlers()
        {
            using var r = new StreamReader("/proc/bus/input/devices");
            var nameRegex = new Regex("^N: Name=\"(?<name>.+)\"$", RegexOptions.Compiled);
            var handlersRegex = new Regex("^H: Handlers=(?<handlers>.+)$", RegexOptions.Compiled);

            var deviceMap = new Dictionary<string, string>();

            string currentDevice = null;

            while (!r.EndOfStream)
            {
                var line = r.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var entryType = line[0];

                switch (entryType)
                {
                    case 'N':
                    {
                        var match = nameRegex.Match(line);
                        if (!match.Success)
                            throw new InvalidDataException("Unexpected name formatting in /proc/bus/input/devices");

                        currentDevice = match.Groups["name"].Value;
                        break;
                    }
                    case 'H':
                    {
                        if (currentDevice == null)
                            throw new InvalidDataException("Unexpected handlers entry without device");

                        var match = handlersRegex.Match(line);
                        if (!match.Success)
                            throw new InvalidDataException(
                                "Unexpected handlers formatting in /proc/bus/input/devices");

                        var handlers = match.Groups["handlers"].Value.Split(' ');

                        var eventHandler = handlers.FirstOrDefault(s => s.StartsWith("event"));

                        deviceMap.Add(currentDevice, $"/dev/input/{eventHandler}");

                        currentDevice = null;
                        break;
                    }
                }
            }

            return deviceMap;
        }

        /// <summary>
        ///     Converts "micro" units to their base unit
        /// </summary>
        /// <param name="value">The value offset by 10^6</param>
        /// <returns>The value multiplied by 10^(-6)</returns>
        public static float MicroToBaseUnit(float value)
        {
            return value * (float)Math.Pow(10, -6);
        }
    }
}