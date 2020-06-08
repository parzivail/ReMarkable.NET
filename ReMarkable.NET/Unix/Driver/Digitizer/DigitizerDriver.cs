using System;
using ReMarkable.NET.Unix.Driver.Button;
using ReMarkable.NET.Util;

namespace ReMarkable.NET.Unix.Driver.Digitizer
{
    public sealed class DigitizerDriver : InputDriver
    {
        public event EventHandler<StylusTool> ToolChanged;

        public event EventHandler<DigitizerKeyCode> Pressed; 
        public event EventHandler<DigitizerKeyCode> Released; 

        public DigitizerDriver() : base("/dev/input/event2")
        {
        }

        /// <inheritdoc />
        protected override void DataAvailable(object sender, DataAvailableEventArgs<EvEvent> e)
        {
            var data = e.Data;

            var eventType = (DigitizerType)data.Type;

            switch (eventType)
            {
                case DigitizerType.Syn:
                    break;
                case DigitizerType.Key:
                    var key = (DigitizerKeyCode)data.Code;
                    var state = (KeyState)data.Value;

                    switch (key)
                    {
                        case DigitizerKeyCode.BtnToolPen:
                            ToolChanged?.Invoke(null, StylusTool.Pen);
                            break;
                        case DigitizerKeyCode.BtnToolRubber:
                            ToolChanged?.Invoke(null, StylusTool.Eraser);
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
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public enum StylusTool
    {
        Pen,
        Eraser
    }
}
