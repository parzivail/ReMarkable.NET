namespace ReMarkable.NET.Unix.Driver.Keyboard
{
    public class KeyEventArgs
    {
        public KeyboardKey Key { get; }

        public KeyEventArgs(KeyboardKey key)
        {
            Key = key;
        }
    }
}