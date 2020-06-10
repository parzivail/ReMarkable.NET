# ReMarkable.NET

## Research

### Output devices

#### Framebuffer (`/dev/fb0`)

From `fbset`:

```
mode "1404x1872-47"
        # D: 160.000 MHz, H: 88.594 kHz, V: 46.900 Hz
        geometry 1404 1872 1408 3840 16
        timings 6250 32 326 4 12 44 1
        accel false
        rgba 5/11,6/5,5/0,0/0
endmode
```

| Framebuffer property | Value |
| --- | --- |
| Virtual size | 1408x3840 |
| Physical/visible size | 1404x1872 |
| Default color format | 16-bit color, RGB565 |

### Input devices

For reference, all `/dev/input/event*` streams use the following format:

```csharp
[StructLayout(LayoutKind.Sequential)]
internal struct EvEvent
{
	public uint TimeWholeSeconds;
	public uint TimeFractionMicroseconds;
	public ushort Type;
	public ushort Code;
	public int Value;
}
```

The following output is from `evtest`.

#### Wacom digitizer (`/dev/input/event0`)

```
Select the device event number [0-2]: 0
Input driver version is 1.0.1
Input device ID: bus 0x18 vendor 0x56a product 0x0 version 0x36
Input device name: "Wacom I2C Digitizer"
Supported events:
  Event type 0 (EV_SYN)
  Event type 1 (EV_KEY)
    Event code 320 (BTN_TOOL_PEN)
    Event code 321 (BTN_TOOL_RUBBER)
    Event code 330 (BTN_TOUCH)
    Event code 331 (BTN_STYLUS)
    Event code 332 (BTN_STYLUS2)
  Event type 3 (EV_ABS)
    Event code 0 (ABS_X)
      Value   6018
      Min        0
      Max    20967
    Event code 1 (ABS_Y)
      Value   3862
      Min        0
      Max    15725
    Event code 24 (ABS_PRESSURE)
      Value      0
      Min        0
      Max     4095
    Event code 25 (ABS_DISTANCE)
      Value     40
      Min        0
      Max      255
    Event code 26 (ABS_TILT_X)
      Value      0
      Min    -9000
      Max     9000
    Event code 27 (ABS_TILT_Y)
      Value      0
      Min    -9000
      Max     9000
```

#### Cypress TrueTouch touchscreen (`/dev/input/event1`)

Uses the [type B identifiable multi-touch event protocol](https://www.kernel.org/doc/html/v4.17/input/multi-touch-protocol.html)

```
Select the device event number [0-2]: 1
Input driver version is 1.0.1
Input device ID: bus 0x0 vendor 0x0 product 0x0 version 0x0
Input device name: "cyttsp5_mt"
Supported events:
  Event type 0 (EV_SYN)
  Event type 1 (EV_KEY)
  Event type 2 (EV_REL)
  Event type 3 (EV_ABS)
    Event code 25 (ABS_DISTANCE)
      Value      0
      Min        0
      Max      255
    Event code 47 (ABS_MT_SLOT)
      Value      0
      Min        0
      Max       31
    Event code 48 (ABS_MT_TOUCH_MAJOR)
      Value      0
      Min        0
      Max      255
    Event code 49 (ABS_MT_TOUCH_MINOR)
      Value      0
      Min        0
      Max      255
    Event code 52 (ABS_MT_ORIENTATION)
      Value      0
      Min     -127
      Max      127
    Event code 53 (ABS_MT_POSITION_X)
      Value      0
      Min        0
      Max      767
    Event code 54 (ABS_MT_POSITION_Y)
      Value      0
      Min        0
      Max     1023
    Event code 55 (ABS_MT_TOOL_TYPE)
      Value      0
      Min        0
      Max        1
    Event code 57 (ABS_MT_TRACKING_ID)
      Value      0
      Min        0
      Max    65535
    Event code 58 (ABS_MT_PRESSURE)
      Value      0
      Min        0
      Max      255
```

#### Physical buttons (`/dev/input/event2`)

```
Select the device event number [0-2]: 2
Input driver version is 1.0.1
Input device ID: bus 0x19 vendor 0x1 product 0x1 version 0x100
Input device name: "gpio-keys"
Supported events:
  Event type 0 (EV_SYN)
  Event type 1 (EV_KEY)
    Event code 102 (KEY_HOME)
    Event code 105 (KEY_LEFT)
    Event code 106 (KEY_RIGHT)
    Event code 116 (KEY_POWER)
    Event code 143 (KEY_WAKEUP)
```