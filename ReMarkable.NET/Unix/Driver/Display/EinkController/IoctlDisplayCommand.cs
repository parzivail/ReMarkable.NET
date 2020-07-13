namespace ReMarkable.NET.Unix.Driver.Display.EinkController
{
    /// <summary>
    ///     Display driver commands, e-paper commands OR'd with 0x40484600
    /// </summary>
    public enum IoctlDisplayCommand : uint
    {
        /// <summary>
        ///     Takes <see cref="WaveformModes" />
        /// </summary>
        SetWaveformModes = 0x4048462B,

        /// <summary>
        ///     Takes <seealso cref="DisplayTemp" />
        /// </summary>
        SetTemperature = 0x4048462C,

        /// <summary>
        ///     Takes <seealso cref="AutoUpdateMode" />
        /// </summary>
        SetAutoUpdateMode = 0x4048462D,

        /// <summary>
        ///     Takes <see cref="FbUpdateData" />
        /// </summary>
        SendUpdate = 0x4048462E,

        /// <summary>
        ///     Takes <see cref="UpdateMarkerData" />
        /// </summary>
        WaitForUpdateComplete = 0x4048462F,

        /// <summary>
        ///     Takes uint
        /// </summary>
        SetPowerDownDelay = 0x40484630,

        /// <summary>
        ///     Takes uint
        /// </summary>
        GetPowerDownDelay = 0x40484631,

        /// <summary>
        ///     Takes <seealso cref="UpdateScheme" />
        /// </summary>
        SetUpdateScheme = 0x40484632,

        /// <summary>
        ///     Takes ulong
        /// </summary>
        GetWorkBuffer = 0x40484634,

        /// <summary>
        ///     Takes uint
        /// </summary>
        SetTempAutoUpdatePeriod = 0x40484636,

        /// <summary>
        ///     No params
        /// </summary>
        DisableEpdcAccess = 0x40484635,

        /// <summary>
        ///     No params
        /// </summary>
        EnableEpdcAccess = 0x40484636,
        GetVariableScreenInfo = 0x4600,
        PutVariableScreenInfo = 0x4601,
        GetFixedScreenInfo = 0x4602,
        GetCmap = 0x4604,
        PutCmap = 0x4605,
        PanDisplay = 0x4606,
        Cursor = 0x4608
    }
}