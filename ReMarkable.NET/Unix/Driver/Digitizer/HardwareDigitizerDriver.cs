using System;
using System.Collections.Generic;
using ReMarkable.NET.Calibration;
using ReMarkable.NET.Calibration.Builtin;
using ReMarkable.NET.Unix.Driver.Generic;
using SixLabors.ImageSharp;

namespace ReMarkable.NET.Unix.Driver.Digitizer
{
    /// <summary>
    ///     Provides methods for monitoring the digitizer installed in the device
    /// </summary>
    public sealed class HardwareDigitizerDriver : UnixInputDriver, IDigitizerDriver
    {
        /// <inheritdoc />
        public event EventHandler<DigitizerEventKeyCode> Pressed;

        /// <inheritdoc />
        public event EventHandler<DigitizerEventKeyCode> Released;

        /// <inheritdoc />
        public event EventHandler<StylusState> StylusUpdate;

        /// <inheritdoc />
        public event EventHandler<StylusTool> ToolChanged;

        /// <summary>
        ///     Temporary distance value accumulated for event dispatch
        /// </summary>
        private int _currentDistance;

        /// <summary>
        ///     Temporary position value accumulated for event dispatch
        /// </summary>
        private Point _currentPosition = Point.Empty;

        /// <summary>
        ///     Temporary pressure value accumulated for event dispatch
        /// </summary>
        private int _currentPressure;

        /// <summary>
        ///     Temporary tilt value accumulated for event dispatch
        /// </summary>
        private Point _currentTilt = Point.Empty;

        /// <summary>
        ///     Temporary tool value accumulated for event dispatch
        /// </summary>
        private StylusTool _currentTool = StylusTool.None;

        /// <inheritdoc />
        public Dictionary<DigitizerEventKeyCode, ButtonState> ButtonStates { get; }

        /// <inheritdoc />
        public TouchscreenCalibrator Calibrator { get; set; }

        /// <inheritdoc />
        public int Height { get; }

        /// <inheritdoc />
        public StylusState State { get; private set; }

        /// <inheritdoc />
        public int Width { get; }

        /// <summary>
        ///     Creates a new <see cref="HardwareDigitizerDriver" />
        /// </summary>
        /// <param name="devicePath">The device event stream to poll for new events</param>
        /// <param name="width">The virtual width of the device</param>
        /// <param name="height">The virtual height of the device</param>
        public HardwareDigitizerDriver(string devicePath, int width, int height) : base(devicePath)
        {
            Width = width;
            Height = height;
            ButtonStates = new Dictionary<DigitizerEventKeyCode, ButtonState>();
            Calibrator = new TouchscreenCalibrator
            {
                Calibration = BuiltinStylusCalibrations.ReMarkableMarker
            };
        }

        /// <inheritdoc />
        protected override void DataAvailable(object sender, DataAvailableEventArgs<EvEvent> e)
        {
            var data = e.Data;

            var eventType = (DigitizerEventType)data.Type;

            switch (eventType)
            {
                case DigitizerEventType.Syn:
                    State = new StylusState(_currentTool, _currentPosition, _currentPressure, _currentDistance,
                        _currentTilt);
                    StylusUpdate?.Invoke(null, State);

                    if (_currentTool == StylusTool.None)
                    {
                        _currentDistance = 255;
                        _currentPressure = 0;
                        _currentPosition = Point.Empty;
                        _currentTilt = Point.Empty;
                    }
                    else if (_currentDistance > 0) _currentPressure = 0;
                    else if (_currentPressure > 0) _currentDistance = 0;

                    break;
                case DigitizerEventType.Key:
                    var key = (DigitizerEventKeyCode)data.Code;
                    var state = (ButtonState)data.Value;

                    ButtonStates[key] = state;

                    switch (key)
                    {
                        case DigitizerEventKeyCode.ToolPen:
                            _currentTool = state == ButtonState.Pressed ? StylusTool.Pen : StylusTool.None;
                            ToolChanged?.Invoke(null, _currentTool);
                            break;
                        case DigitizerEventKeyCode.ToolRubber:
                            _currentTool = state == ButtonState.Pressed ? StylusTool.Eraser : StylusTool.None;
                            ToolChanged?.Invoke(null, _currentTool);
                            break;
                        case DigitizerEventKeyCode.Touch:
                            // Stylus touch input unreliable, but data is redundant
                            // because of ABS_PRESSURE
                            break;
                        case DigitizerEventKeyCode.Stylus:
                        case DigitizerEventKeyCode.Stylus2:
                            if (state == ButtonState.Pressed)
                                Pressed?.Invoke(null, key);
                            else
                                Released?.Invoke(null, key);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(key), key, key.GetType().Name);
                    }

                    break;
                case DigitizerEventType.Abs:
                    var eventCode = (DigitizerEventAbsCode)data.Code;

                    switch (eventCode)
                    {
                        case DigitizerEventAbsCode.AbsX:
                            _currentPosition.X = data.Value;
                            break;
                        case DigitizerEventAbsCode.AbsY:
                            _currentPosition.Y = data.Value;
                            break;
                        case DigitizerEventAbsCode.AbsPressure:
                            _currentPressure = data.Value;
                            break;
                        case DigitizerEventAbsCode.AbsDistance:
                            _currentDistance = data.Value;
                            break;
                        case DigitizerEventAbsCode.AbsTiltX:
                            _currentTilt.X = data.Value;
                            break;
                        case DigitizerEventAbsCode.AbsTiltY:
                            _currentTilt.Y = data.Value;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(eventCode), eventCode,
                                eventCode.GetType().Name);
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(eventType), eventType, eventType.GetType().Name);
            }
        }
    }
}