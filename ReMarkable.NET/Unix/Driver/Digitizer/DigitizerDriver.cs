using System;
using System.Collections.Generic;
using System.Drawing;
using ReMarkable.NET.Util;

namespace ReMarkable.NET.Unix.Driver.Digitizer
{
    public sealed class DigitizerDriver : InputDriver
    {
        public event EventHandler<StylusTool> ToolChanged;

        public event EventHandler<DigitizerKeyCode> Pressed;
        public event EventHandler<DigitizerKeyCode> Released;

        public event EventHandler<StylusState> StylusUpdate;
        
        public Dictionary<DigitizerKeyCode, KeyState> ButtonStates;

        private StylusTool _currentTool = StylusTool.None;
        private Point _currentPosition = Point.Empty;
        private int _currentPressure;
        private int _currentDistance;
        private Point _currentTilt = Point.Empty;

        public DigitizerDriver() : base("/dev/input/event0")
        {
            ButtonStates = new Dictionary<DigitizerKeyCode, KeyState>();
        }

        /// <inheritdoc />
        protected override void DataAvailable(object sender, DataAvailableEventArgs<EvEvent> e)
        {
            var data = e.Data;

            var eventType = (DigitizerType)data.Type;

            switch (eventType)
            {
                case DigitizerType.Syn:
                    StylusUpdate?.Invoke(null, new StylusState(_currentTool, _currentPosition, _currentPressure, _currentDistance, _currentTilt));
                    break;
                case DigitizerType.Key:
                    var key = (DigitizerKeyCode)data.Code;
                    var state = (KeyState)data.Value;
                    
                    ButtonStates[key] = state;

                    switch (key)
                    {
                        case DigitizerKeyCode.BtnToolPen:
                            _currentTool = StylusTool.Pen;
                            ToolChanged?.Invoke(null, _currentTool);
                            break;
                        case DigitizerKeyCode.BtnToolRubber:
                            _currentTool = StylusTool.Eraser;
                            ToolChanged?.Invoke(null, _currentTool);
                            break;
                        case DigitizerKeyCode.BtnTouch:
                        case DigitizerKeyCode.BtnStylus:
                        case DigitizerKeyCode.BtnStylus2:
                            if (state ==  KeyState.Pressed)
                                Pressed?.Invoke(null, key);
                            else
                                Released?.Invoke(null, key);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    break;
                case DigitizerType.Abs:
                    var payload = (DigitizerAbsCode) data.Code;

                    switch (payload)
                    {
                        case DigitizerAbsCode.AbsX:
                            _currentPosition.X = data.Value;
                            break;
                        case DigitizerAbsCode.AbsY:
                            _currentPosition.Y = data.Value;
                            break;
                        case DigitizerAbsCode.AbsPressure:
                            _currentPressure = data.Value;
                            break;
                        case DigitizerAbsCode.AbsDistance:
                            _currentDistance = data.Value;
                            break;
                        case DigitizerAbsCode.AbsTiltX:
                            _currentTilt.X = data.Value;
                            break;
                        case DigitizerAbsCode.AbsTiltY:
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

    public class StylusState
    {
        public StylusTool Tool { get; }
        public Point Position { get; }
        public int Pressure { get; }
        public int Distance { get; }
        public Point Tilt { get; }

        public StylusState(StylusTool tool, Point position, int pressure, int distance, Point tilt)
        {
            Tool = tool;
            Position = position;
            Pressure = pressure;
            Distance = distance;
            Tilt = tilt;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{nameof(Tool)}: {Tool}, {nameof(Position)}: {Position}, {nameof(Pressure)}: {Pressure}, {nameof(Distance)}: {Distance}, {nameof(Tilt)}: {Tilt}";
        }
    }

    public enum StylusTool
    {
        None,
        Pen,
        Eraser
    }
}
