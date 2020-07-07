namespace ReMarkable.NET.Unix.Driver.Keyboard
{
    public class KeyPressEventArgs : KeyEventArgs
    {
        public bool Repeat { get; }

        public KeyPressEventArgs(KeyboardKey key, bool repeat) : base(key)
        {
            Repeat = repeat;
        }
    }
}