using System;
using System.Collections.Generic;
using System.Drawing;
using ReMarkable.NET.Unix.Driver.Button;
using ReMarkable.NET.Util;

namespace ReMarkable.NET.Unix.Driver.Digitizer
{
    public sealed class HardwareDigitizerDriver : UnixInputDriver, IDigitizerDriver
    {
        public event EventHandler<StylusTool> ToolChanged;

        public event EventHandler<DigitizerEventKeyCode> Pressed;
        public event EventHandler<DigitizerEventKeyCode> Released;

        public event EventHandler<StylusState> StylusUpdate;
        
        public Dictionary<DigitizerEventKeyCode, ButtonState> ButtonStates;

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
                    StylusUpdate?.Invoke(null, new StylusState(_currentTool, _currentPosition, _currentPressure, _currentDistance, _currentTilt));
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
                            throw new ArgumentOutOfRangeException();
                    }

                    break;
                case DigitizerEventType.Abs:
                    var payload = (DigitizerEventAbsCode) data.Code;

                    switch (payload)
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
                            throw new ArgumentOutOfRangeException();
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
