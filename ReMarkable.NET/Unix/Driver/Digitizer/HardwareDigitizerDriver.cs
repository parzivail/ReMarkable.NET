using System;
using System.Collections.Generic;
using ReMarkable.NET.Unix.Driver.Button;
using ReMarkable.NET.Unix.Driver.Generic;
using ReMarkable.NET.Util;
using SixLabors.ImageSharp;

namespace ReMarkable.NET.Unix.Driver.Digitizer
{
    public sealed class HardwareDigitizerDriver : UnixInputDriver, IDigitizerDriver
    {
        public event EventHandler<StylusTool> ToolChanged;

        public event EventHandler<DigitizerEventKeyCode> Pressed;
        public event EventHandler<DigitizerEventKeyCode> Released;

        public event EventHandler<StylusState> StylusUpdate;
        
        public Dictionary<DigitizerEventKeyCode, ButtonState> ButtonStates;

        public StylusState State { get; private set; }
        public int Width { get; }
        public int Height { get; }

        private StylusTool _currentTool = StylusTool.None;
        private Point _currentPosition = Point.Empty;
        private int _currentPressure;
        private int _currentDistance;
        private Point _currentTilt = Point.Empty;

        internal HardwareDigitizerDriver(string devicePath, int width, int height) : base(devicePath)
        {
            Width = width;
            Height = height;
            ButtonStates = new Dictionary<DigitizerEventKeyCode, ButtonState>();
        }

        /// <inheritdoc />
        protected override void DataAvailable(object sender, DataAvailableEventArgs<EvEvent> e)
        {
            var data = e.Data;

            var eventType = (DigitizerEventType)data.Type;

            switch (eventType)
            {
                case DigitizerEventType.Syn:
                    State = new StylusState(_currentTool, _currentPosition, _currentPressure, _currentDistance, _currentTilt);
                    StylusUpdate?.Invoke(null, State);
                    break;
                case DigitizerEventType.Key:
                    var key = (DigitizerEventKeyCode)data.Code;
                    var state = (ButtonState)data.Value;
                    
                    ButtonStates[key] = state;

                    switch (key)
                    {
                        case DigitizerEventKeyCode.BtnToolPen:
                            _currentTool = StylusTool.Pen;
                            ToolChanged?.Invoke(null, _currentTool);
                            break;
                        case DigitizerEventKeyCode.BtnToolRubber:
                            _currentTool = StylusTool.Eraser;
                            ToolChanged?.Invoke(null, _currentTool);
                            break;
                        case DigitizerEventKeyCode.BtnTouch:
                        case DigitizerEventKeyCode.BtnStylus:
                        case DigitizerEventKeyCode.BtnStylus2:
                            if (state ==  ButtonState.Pressed)
                                Pressed?.Invoke(null, key);
                            else
                                Released?.Invoke(null, key);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(key), key, key.GetType().Name);
                    }

                    break;
                case DigitizerEventType.Abs:
                    var eventCode = (DigitizerEventAbsCode) data.Code;

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
                            throw new ArgumentOutOfRangeException(nameof(eventCode), eventCode, eventCode.GetType().Name);
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(eventType), eventType, eventType.GetType().Name);
            }
        }
    }
}
