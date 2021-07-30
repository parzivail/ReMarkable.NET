using ReMarkable.NET.Unix.Driver;
using System.Runtime.InteropServices;

namespace ReMarkable.NET.Util
{
    /// <summary>
    /// Possible Remarkable or emulated device types
    /// </summary>
    public enum Device
    {
        Emulator,
        RM1,
        RM2
    }

    public class DeviceType
    {
        private static Device ?result = null;

        /// <summary>
        /// Gets the device that in currently in use
        /// </summary>
        /// <returns>DeviceType</returns>
        public static Device GetDevice()
        {
            if (result != null)
            {
                return result.Value;
            }

            var ret = Device.Emulator;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                ret = Device.Emulator;
            }
            else
            {
                try
                {
                    var deviceMap = DeviceUtils.GetInputDeviceEventHandlers();

                    if (deviceMap.ContainsKey("30370000.snvs:snvs-powerkey")) // rm2 has a power button
                    {
                        ret = Device.RM2;
                    }
                    else if (deviceMap.ContainsKey("cyttsp5_mt")) // rm1 touch screen driver
                    {
                        ret = Device.RM1;
                    }
                }
                catch
                {
                }
            }

            result = ret;
            return ret;
        }
    }
}
