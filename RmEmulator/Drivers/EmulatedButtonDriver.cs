using System;
using System.Collections.Generic;
using System.Linq;
using OpenToolkit.Windowing.Common;
using OpenToolkit.Windowing.Common.Input;
using ReMarkable.NET.Unix.Driver;
using ReMarkable.NET.Unix.Driver.Button;

namespace RmEmulator.Drivers
{
    public class EmulatedButtonDriver : IPhysicalButtonDriver
    {
        private static readonly Dictionary<Key, PhysicalButton> KeyMap = new Dictionary<Key, PhysicalButton>
        {
            { Key.Left, PhysicalButton.KeyLeft },
            { Key.Space, PhysicalButton.KeyHome },
            { Key.Right, PhysicalButton.KeyRight },
            { Key.Enter, PhysicalButton.KeyPower },
            { Key.Slash, PhysicalButton.KeyWakeUp } // No physical mapping
        };

        private readonly EmulatorWindow _window;

        public event EventHandler<PhysicalButton> Pressed;
        public event EventHandler<PhysicalButton> Released;

        public Dictionary<PhysicalButton, ButtonState> ButtonStates { get; }

        public EmulatedButtonDriver(EmulatorWindow window)
        {
            _window = window;
            ButtonStates = KeyMap.Values.ToDictionary(button => button, button => ButtonState.Released);
        }

        public void ConsumeKeyUp(KeyboardKeyEventArgs obj)
        {
            if (obj.IsRepeat || !KeyMap.TryGetValue(obj.Key, out var value))
                return;

            ButtonStates[value] = ButtonState.Pressed;
            Pressed?.Invoke(this, value);
        }

        public void ConsumeKeyDown(KeyboardKeyEventArgs obj)
        {
            if (obj.IsRepeat || !KeyMap.TryGetValue(obj.Key, out var value))
                return;

            ButtonStates[value] = ButtonState.Released;
            Released?.Invoke(this, value);
        }
    }
}