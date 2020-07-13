namespace ReMarkable.NET.Unix.Driver.Keyboard
{
    /// <summary>
    ///     Defines the possible event codes an attached keyboard can raise through the KEY event
    /// </summary>
    public enum KeyboardKey
    {
        /// <summary>
        ///     "KEY_ESC" in input-event-codes.h
        /// </summary>
        Esc = 1,

        /// <summary>
        ///     "KEY_1" in input-event-codes.h
        /// </summary>
        NumberRow1 = 2,

        /// <summary>
        ///     "KEY_2" in input-event-codes.h
        /// </summary>
        NumberRow2 = 3,

        /// <summary>
        ///     "KEY_3" in input-event-codes.h
        /// </summary>
        NumberRow3 = 4,

        /// <summary>
        ///     "KEY_4" in input-event-codes.h
        /// </summary>
        NumberRow4 = 5,

        /// <summary>
        ///     "KEY_5" in input-event-codes.h
        /// </summary>
        NumberRow5 = 6,

        /// <summary>
        ///     "KEY_6" in input-event-codes.h
        /// </summary>
        NumberRow6 = 7,

        /// <summary>
        ///     "KEY_7" in input-event-codes.h
        /// </summary>
        NumberRow7 = 8,

        /// <summary>
        ///     "KEY_8" in input-event-codes.h
        /// </summary>
        NumberRow8 = 9,

        /// <summary>
        ///     "KEY_9" in input-event-codes.h
        /// </summary>
        NumberRow9 = 10,

        /// <summary>
        ///     "KEY_0" in input-event-codes.h
        /// </summary>
        NumberRow0 = 11,

        /// <summary>
        ///     "KEY_MINUS" in input-event-codes.h
        /// </summary>
        Minus = 12,

        /// <summary>
        ///     "KEY_EQUAL" in input-event-codes.h
        /// </summary>
        Equal = 13,

        /// <summary>
        ///     "KEY_BACKSPACE" in input-event-codes.h
        /// </summary>
        Backspace = 14,

        /// <summary>
        ///     "KEY_TAB" in input-event-codes.h
        /// </summary>
        Tab = 15,

        /// <summary>
        ///     "KEY_Q" in input-event-codes.h
        /// </summary>
        Q = 16,

        /// <summary>
        ///     "KEY_W" in input-event-codes.h
        /// </summary>
        W = 17,

        /// <summary>
        ///     "KEY_E" in input-event-codes.h
        /// </summary>
        E = 18,

        /// <summary>
        ///     "KEY_R" in input-event-codes.h
        /// </summary>
        R = 19,

        /// <summary>
        ///     "KEY_T" in input-event-codes.h
        /// </summary>
        T = 20,

        /// <summary>
        ///     "KEY_Y" in input-event-codes.h
        /// </summary>
        Y = 21,

        /// <summary>
        ///     "KEY_U" in input-event-codes.h
        /// </summary>
        U = 22,

        /// <summary>
        ///     "KEY_I" in input-event-codes.h
        /// </summary>
        I = 23,

        /// <summary>
        ///     "KEY_O" in input-event-codes.h
        /// </summary>
        O = 24,

        /// <summary>
        ///     "KEY_P" in input-event-codes.h
        /// </summary>
        P = 25,

        /// <summary>
        ///     "KEY_LEFTBRACE" in input-event-codes.h
        /// </summary>
        LeftBrace = 26,

        /// <summary>
        ///     "KEY_RIGHTBRACE" in input-event-codes.h
        /// </summary>
        RightBrace = 27,

        /// <summary>
        ///     "KEY_ENTER" in input-event-codes.h
        /// </summary>
        Enter = 28,

        /// <summary>
        ///     "KEY_LEFTCTRL" in input-event-codes.h
        /// </summary>
        LeftCtrl = 29,

        /// <summary>
        ///     "KEY_A" in input-event-codes.h
        /// </summary>
        A = 30,

        /// <summary>
        ///     "KEY_S" in input-event-codes.h
        /// </summary>
        S = 31,

        /// <summary>
        ///     "KEY_D" in input-event-codes.h
        /// </summary>
        D = 32,

        /// <summary>
        ///     "KEY_F" in input-event-codes.h
        /// </summary>
        F = 33,

        /// <summary>
        ///     "KEY_G" in input-event-codes.h
        /// </summary>
        G = 34,

        /// <summary>
        ///     "KEY_H" in input-event-codes.h
        /// </summary>
        H = 35,

        /// <summary>
        ///     "KEY_J" in input-event-codes.h
        /// </summary>
        J = 36,

        /// <summary>
        ///     "KEY_K" in input-event-codes.h
        /// </summary>
        K = 37,

        /// <summary>
        ///     "KEY_L" in input-event-codes.h
        /// </summary>
        L = 38,

        /// <summary>
        ///     "KEY_SEMICOLON" in input-event-codes.h
        /// </summary>
        Semicolon = 39,

        /// <summary>
        ///     "KEY_APOSTROPHE" in input-event-codes.h
        /// </summary>
        Apostrophe = 40,

        /// <summary>
        ///     "KEY_GRAVE" in input-event-codes.h
        /// </summary>
        Grave = 41,

        /// <summary>
        ///     "KEY_LEFTSHIFT" in input-event-codes.h
        /// </summary>
        LeftShift = 42,

        /// <summary>
        ///     "KEY_BACKSLASH" in input-event-codes.h
        /// </summary>
        Backslash = 43,

        /// <summary>
        ///     "KEY_Z" in input-event-codes.h
        /// </summary>
        Z = 44,

        /// <summary>
        ///     "KEY_X" in input-event-codes.h
        /// </summary>
        X = 45,

        /// <summary>
        ///     "KEY_C" in input-event-codes.h
        /// </summary>
        C = 46,

        /// <summary>
        ///     "KEY_V" in input-event-codes.h
        /// </summary>
        V = 47,

        /// <summary>
        ///     "KEY_B" in input-event-codes.h
        /// </summary>
        B = 48,

        /// <summary>
        ///     "KEY_N" in input-event-codes.h
        /// </summary>
        N = 49,

        /// <summary>
        ///     "KEY_M" in input-event-codes.h
        /// </summary>
        M = 50,

        /// <summary>
        ///     "KEY_COMMA" in input-event-codes.h
        /// </summary>
        Comma = 51,

        /// <summary>
        ///     "KEY_DOT" in input-event-codes.h
        /// </summary>
        Period = 52,

        /// <summary>
        ///     "KEY_SLASH" in input-event-codes.h
        /// </summary>
        Slash = 53,

        /// <summary>
        ///     "KEY_RIGHTSHIFT" in input-event-codes.h
        /// </summary>
        RightShift = 54,

        /// <summary>
        ///     "KEY_KPASTERISK" in input-event-codes.h
        /// </summary>
        KeypadAsterisk = 55,

        /// <summary>
        ///     "KEY_LEFTALT" in input-event-codes.h
        /// </summary>
        LeftAlt = 56,

        /// <summary>
        ///     "KEY_SPACE" in input-event-codes.h
        /// </summary>
        Space = 57,

        /// <summary>
        ///     "KEY_CAPSLOCK" in input-event-codes.h
        /// </summary>
        CapsLock = 58,

        /// <summary>
        ///     "KEY_F1" in input-event-codes.h
        /// </summary>
        F1 = 59,

        /// <summary>
        ///     "KEY_F2" in input-event-codes.h
        /// </summary>
        F2 = 60,

        /// <summary>
        ///     "KEY_F3" in input-event-codes.h
        /// </summary>
        F3 = 61,

        /// <summary>
        ///     "KEY_F4" in input-event-codes.h
        /// </summary>
        F4 = 62,

        /// <summary>
        ///     "KEY_F5" in input-event-codes.h
        /// </summary>
        F5 = 63,

        /// <summary>
        ///     "KEY_F6" in input-event-codes.h
        /// </summary>
        F6 = 64,

        /// <summary>
        ///     "KEY_F7" in input-event-codes.h
        /// </summary>
        F7 = 65,

        /// <summary>
        ///     "KEY_F8" in input-event-codes.h
        /// </summary>
        F8 = 66,

        /// <summary>
        ///     "KEY_F9" in input-event-codes.h
        /// </summary>
        F9 = 67,

        /// <summary>
        ///     "KEY_F10" in input-event-codes.h
        /// </summary>
        F10 = 68,

        /// <summary>
        ///     "KEY_NUMLOCK" in input-event-codes.h
        /// </summary>
        NumberLock = 69,

        /// <summary>
        ///     "KEY_SCROLLLOCK" in input-event-codes.h
        /// </summary>
        ScrollLock = 70,

        /// <summary>
        ///     "KEY_KP7" in input-event-codes.h
        /// </summary>
        Keypad7 = 71,

        /// <summary>
        ///     "KEY_KP8" in input-event-codes.h
        /// </summary>
        Keypad8 = 72,

        /// <summary>
        ///     "KEY_KP9" in input-event-codes.h
        /// </summary>
        Keypad9 = 73,

        /// <summary>
        ///     "KEY_KPMINUS" in input-event-codes.h
        /// </summary>
        KeypadMinus = 74,

        /// <summary>
        ///     "KEY_KP4" in input-event-codes.h
        /// </summary>
        Keypad4 = 75,

        /// <summary>
        ///     "KEY_KP5" in input-event-codes.h
        /// </summary>
        Keypad5 = 76,

        /// <summary>
        ///     "KEY_KP6" in input-event-codes.h
        /// </summary>
        Keypad6 = 77,

        /// <summary>
        ///     "KEY_KPPLUS" in input-event-codes.h
        /// </summary>
        KeypadPlus = 78,

        /// <summary>
        ///     "KEY_KP1" in input-event-codes.h
        /// </summary>
        Keypad1 = 79,

        /// <summary>
        ///     "KEY_KP2" in input-event-codes.h
        /// </summary>
        Keypad2 = 80,

        /// <summary>
        ///     "KEY_KP3" in input-event-codes.h
        /// </summary>
        Keypad3 = 81,

        /// <summary>
        ///     "KEY_KP0" in input-event-codes.h
        /// </summary>
        Keypad0 = 82,

        /// <summary>
        ///     "KEY_KPDOT" in input-event-codes.h
        /// </summary>
        KeypadDot = 83,

        /// <summary>
        ///     "KEY_ZENKAKUHANKAKU" in input-event-codes.h
        /// </summary>
        ZenkakuHankaku = 85,

        /// <summary>
        ///     "KEY_102ND" in input-event-codes.h
        /// </summary>
        NonUsBackslashAndPipe = 86,

        /// <summary>
        ///     "KEY_F11" in input-event-codes.h
        /// </summary>
        F11 = 87,

        /// <summary>
        ///     "KEY_F12" in input-event-codes.h
        /// </summary>
        F12 = 88,

        /// <summary>
        ///     "KEY_RO" in input-event-codes.h
        /// </summary>
        Ro = 89,

        /// <summary>
        ///     "KEY_KATAKANA" in input-event-codes.h
        /// </summary>
        Katakana = 90,

        /// <summary>
        ///     "KEY_HIRAGANA" in input-event-codes.h
        /// </summary>
        Hiragana = 91,

        /// <summary>
        ///     "KEY_HENKAN" in input-event-codes.h
        /// </summary>
        Henkan = 92,

        /// <summary>
        ///     "KEY_KATAKANAHIRAGANA" in input-event-codes.h
        /// </summary>
        KatakanaHiragana = 93,

        /// <summary>
        ///     "KEY_MUHENKAN" in input-event-codes.h
        /// </summary>
        Muhenkan = 94,

        /// <summary>
        ///     "KEY_KPJPCOMMA" in input-event-codes.h
        /// </summary>
        KeypadJpComma = 95,

        /// <summary>
        ///     "KEY_KPENTER" in input-event-codes.h
        /// </summary>
        KeypadEnter = 96,

        /// <summary>
        ///     "KEY_RIGHTCTRL" in input-event-codes.h
        /// </summary>
        RightCtrl = 97,

        /// <summary>
        ///     "KEY_KPSLASH" in input-event-codes.h
        /// </summary>
        KeypadSlash = 98,

        /// <summary>
        ///     "KEY_SYSRQ" in input-event-codes.h
        /// </summary>
        SysRq = 99,

        /// <summary>
        ///     "KEY_RIGHTALT" in input-event-codes.h
        /// </summary>
        RightAlt = 100,

        /// <summary>
        ///     "KEY_HOME" in input-event-codes.h
        /// </summary>
        Home = 102,

        /// <summary>
        ///     "KEY_UP" in input-event-codes.h
        /// </summary>
        Up = 103,

        /// <summary>
        ///     "KEY_PAGEUP" in input-event-codes.h
        /// </summary>
        PageUp = 104,

        /// <summary>
        ///     "KEY_LEFT" in input-event-codes.h
        /// </summary>
        Left = 105,

        /// <summary>
        ///     "KEY_RIGHT" in input-event-codes.h
        /// </summary>
        Right = 106,

        /// <summary>
        ///     "KEY_END" in input-event-codes.h
        /// </summary>
        End = 107,

        /// <summary>
        ///     "KEY_DOWN" in input-event-codes.h
        /// </summary>
        Down = 108,

        /// <summary>
        ///     "KEY_PAGEDOWN" in input-event-codes.h
        /// </summary>
        PageDown = 109,

        /// <summary>
        ///     "KEY_INSERT" in input-event-codes.h
        /// </summary>
        Insert = 110,

        /// <summary>
        ///     "KEY_DELETE" in input-event-codes.h
        /// </summary>
        Delete = 111,

        /// <summary>
        ///     "KEY_MUTE" in input-event-codes.h
        /// </summary>
        Mute = 113,

        /// <summary>
        ///     "KEY_VOLUMEDOWN" in input-event-codes.h
        /// </summary>
        VolumeDown = 114,

        /// <summary>
        ///     "KEY_VOLUMEUP" in input-event-codes.h
        /// </summary>
        VolumeUp = 115,

        /// <summary>
        ///     "KEY_POWER" in input-event-codes.h
        /// </summary>
        Power = 116,

        /// <summary>
        ///     "KEY_KPEQUAL" in input-event-codes.h
        /// </summary>
        KeypadEqual = 117,

        /// <summary>
        ///     "KEY_PAUSE" in input-event-codes.h
        /// </summary>
        Pause = 119,

        /// <summary>
        ///     "KEY_KPCOMMA" in input-event-codes.h
        /// </summary>
        KeypadComma = 121,

        /// <summary>
        ///     "KEY_HANGUEL" in input-event-codes.h
        /// </summary>
        Hanguel = 122,

        /// <summary>
        ///     "KEY_HANJA" in input-event-codes.h
        /// </summary>
        Hanja = 123,

        /// <summary>
        ///     "KEY_YEN" in input-event-codes.h
        /// </summary>
        Yen = 124,

        /// <summary>
        ///     "KEY_LEFTMETA" in input-event-codes.h
        /// </summary>
        LeftMeta = 125,

        /// <summary>
        ///     "KEY_RIGHTMETA" in input-event-codes.h
        /// </summary>
        RightMeta = 126,

        /// <summary>
        ///     "KEY_COMPOSE" in input-event-codes.h
        /// </summary>
        Compose = 127,

        /// <summary>
        ///     "KEY_STOP" in input-event-codes.h
        /// </summary>
        Stop = 128,

        /// <summary>
        ///     "KEY_AGAIN" in input-event-codes.h
        /// </summary>
        Again = 129,

        /// <summary>
        ///     "KEY_PROPS" in input-event-codes.h
        /// </summary>
        Props = 130,

        /// <summary>
        ///     "KEY_UNDO" in input-event-codes.h
        /// </summary>
        Undo = 131,

        /// <summary>
        ///     "KEY_FRONT" in input-event-codes.h
        /// </summary>
        Front = 132,

        /// <summary>
        ///     "KEY_COPY" in input-event-codes.h
        /// </summary>
        Copy = 133,

        /// <summary>
        ///     "KEY_OPEN" in input-event-codes.h
        /// </summary>
        Open = 134,

        /// <summary>
        ///     "KEY_PASTE" in input-event-codes.h
        /// </summary>
        Paste = 135,

        /// <summary>
        ///     "KEY_FIND" in input-event-codes.h
        /// </summary>
        Find = 136,

        /// <summary>
        ///     "KEY_CUT" in input-event-codes.h
        /// </summary>
        Cut = 137,

        /// <summary>
        ///     "KEY_HELP" in input-event-codes.h
        /// </summary>
        Help = 138,

        /// <summary>
        ///     "KEY_CALC" in input-event-codes.h
        /// </summary>
        Calc = 140,

        /// <summary>
        ///     "KEY_SLEEP" in input-event-codes.h
        /// </summary>
        Sleep = 142,

        /// <summary>
        ///     "KEY_WWW" in input-event-codes.h
        /// </summary>
        Www = 150,

        /// <summary>
        ///     "KEY_SCREENLOCK" in input-event-codes.h
        /// </summary>
        ScreenLock = 152,

        /// <summary>
        ///     "KEY_BACK" in input-event-codes.h
        /// </summary>
        Back = 158,

        /// <summary>
        ///     "KEY_FORWARD" in input-event-codes.h
        /// </summary>
        Forward = 159,

        /// <summary>
        ///     "KEY_EJECTCD" in input-event-codes.h
        /// </summary>
        EjectCd = 161,

        /// <summary>
        ///     "KEY_NEXTSONG" in input-event-codes.h
        /// </summary>
        NextSong = 163,

        /// <summary>
        ///     "KEY_PLAYPAUSE" in input-event-codes.h
        /// </summary>
        PlayPause = 164,

        /// <summary>
        ///     "KEY_PREVIOUSSONG" in input-event-codes.h
        /// </summary>
        PreviousSong = 165,

        /// <summary>
        ///     "KEY_STOPCD" in input-event-codes.h
        /// </summary>
        StopCd = 166,

        /// <summary>
        ///     "KEY_REFRESH" in input-event-codes.h
        /// </summary>
        Refresh = 173,

        /// <summary>
        ///     "KEY_EDIT" in input-event-codes.h
        /// </summary>
        Edit = 176,

        /// <summary>
        ///     "KEY_SCROLLUP" in input-event-codes.h
        /// </summary>
        ScrollUp = 177,

        /// <summary>
        ///     "KEY_SCROLLDOWN" in input-event-codes.h
        /// </summary>
        ScrollDown = 178,

        /// <summary>
        ///     "KEY_KPLEFTPAREN" in input-event-codes.h
        /// </summary>
        KeypadLeftParen = 179,

        /// <summary>
        ///     "KEY_KPRIGHTPAREN" in input-event-codes.h
        /// </summary>
        KeypadRightParen = 180,

        /// <summary>
        ///     "KEY_F13" in input-event-codes.h
        /// </summary>
        F13 = 183,

        /// <summary>
        ///     "KEY_F14" in input-event-codes.h
        /// </summary>
        F14 = 184,

        /// <summary>
        ///     "KEY_F15" in input-event-codes.h
        /// </summary>
        F15 = 185,

        /// <summary>
        ///     "KEY_F16" in input-event-codes.h
        /// </summary>
        F16 = 186,

        /// <summary>
        ///     "KEY_F17" in input-event-codes.h
        /// </summary>
        F17 = 187,

        /// <summary>
        ///     "KEY_F18" in input-event-codes.h
        /// </summary>
        F18 = 188,

        /// <summary>
        ///     "KEY_F19" in input-event-codes.h
        /// </summary>
        F19 = 189,

        /// <summary>
        ///     "KEY_F20" in input-event-codes.h
        /// </summary>
        F20 = 190,

        /// <summary>
        ///     "KEY_F21" in input-event-codes.h
        /// </summary>
        F21 = 191,

        /// <summary>
        ///     "KEY_F22" in input-event-codes.h
        /// </summary>
        F22 = 192,

        /// <summary>
        ///     "KEY_F23" in input-event-codes.h
        /// </summary>
        F23 = 193,

        /// <summary>
        ///     "KEY_F24" in input-event-codes.h
        /// </summary>
        F24 = 194,

        /// <summary>
        ///     "KEY_UNKNOWN" in input-event-codes.h
        /// </summary>
        Unknown = 240
    }
}