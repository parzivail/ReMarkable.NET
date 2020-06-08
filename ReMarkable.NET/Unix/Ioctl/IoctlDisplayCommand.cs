using ReMarkable.NET.Unix.Ioctl.Display;

namespace ReMarkable.NET.Unix.Ioctl
{
    /// <summary>
    /// Display driver commands, all OR'd with 0x40484600
    /// </summary>
    public enum IoctlDisplayCommand : uint
    {
        /// <summary>
        /// Takes struct <see cref="WaveformModes"/>
        /// </summary>
        SetWaveformModes = 0x4048462B,
        /// <summary>
        /// Takes uint, see also <seealso cref="DisplayTemp"/>
        /// </summary>
        SetTemperature = 0x4048462C,
        /// <summary>
        /// Takes uint, see also <seealso cref="AutoUpdateMode"/>
        /// </summary>
        SetAutoUpdateMode = 0x4048462D,
        /// <summary>
        /// Takes struct <see cref="FbUpdateData"/>
        /// </summary>
        SendUpdate = 0x4048462E,
        /// <summary>
        /// Takes struct <see cref="UpdateMarkerData"/>
        /// </summary>
        WaitForUpdateComplete = 0x4048462F,
        /// <summary>
        /// Takes uint
        /// </summary>
        SetPowerDownDelay = 0x40484630,
        /// <summary>
        /// Takes uint
        /// </summary>
        GetPowerDownDelay = 0x40484631,
        /// <summary>
        /// Takes uint, see also <seealso cref="UpdateScheme"/>
        /// </summary>
        SetUpdateScheme = 0x40484632,
        /// <summary>
        /// Takes ulong
        /// </summary>
        GetWorkBuffer = 0x40484634,
        /// <summary>
        /// Takes uint
        /// </summary>
        SetTempAutoUpdatePeriod = 0x40484636,
        /// <summary>
        /// No params
        /// </summary>
        DisableEpdcAccess = 0x40484635,
        /// <summary>
        /// No params
        /// </summary>
        EnableEpdcAccess = 0x40484636
    }
}