using System;

namespace SFML.Window
{
    /// <summary>
    /// Keyboard event parameters.
    /// </summary>
    public class KeyEventArgs : EventArgs
    {
        /// <summary>
        /// Constructs the key arguments from a key event.
        /// </summary>
        /// <param name="e">Key event.</param>
        public KeyEventArgs( KeyEvent e )
        {
            Code = e.Code;
            Alt = e.Alt != 0;
            Control = e.Control != 0;
            Shift = e.Shift != 0;
            System = e.System != 0;
        }

        /// <summary>
        /// Provides a string describing the object.
        /// </summary>
        /// <returns>String description of the object.</returns>
        public override string ToString()
        {
            return "[KeyEventArgs]" +
                   " Code(" + Code + ")" +
                   " Alt(" + Alt + ")" +
                   " Control(" + Control + ")" +
                   " Shift(" + Shift + ")" +
                   " System(" + System + ")";
        }

        /// <summary>
        /// Code of the key (see KeyCode enum).
        /// </summary>
        public readonly Keyboard.Key Code;

        /// <summary>
        /// Is the Alt modifier pressed.
        /// </summary>
        public readonly bool Alt;

        /// <summary>
        /// Is the Control modifier pressed.
        /// </summary>
        public readonly bool Control;

        /// <summary>
        /// Is the Shift modifier pressed.
        /// </summary>
        public readonly bool Shift;

        /// <summary>
        /// Is the System modifier pressed.
        /// </summary>
        public readonly bool System;
    }

    /// <summary>
    /// Text event parameters.
    /// </summary>
    public class TextEventArgs : EventArgs
    {
        /// <summary>
        /// Constructs the text arguments from a text event.
        /// </summary>
        /// <param name="e">Text event.</param>
        public TextEventArgs( TextEvent e )
        {
            Unicode = Char.ConvertFromUtf32( (int)e.Unicode );
        }

        /// <summary>
        /// Provides a string describing the object.
        /// </summary>
        /// <returns>String description of the object.</returns>
        public override string ToString()
        {
            return "[TextEventArgs]" +
                   " Unicode(" + Unicode + ")";
        }

        /// <summary>
        /// UTF-16 value of the character.
        /// </summary>
        public readonly string Unicode;
    }

    /// <summary>
    /// Mouse move event parameters.
    /// </summary>
    public class MouseMoveEventArgs : EventArgs
    {
        /// <summary>
        /// Construct the mouse move arguments from a mouse move event.
        /// </summary>
        /// <param name="e">Mouse move event.</param>
        public MouseMoveEventArgs( MouseMoveEvent e )
        {
            X = e.X;
            Y = e.Y;
        }

        /// <summary>
        /// Provides a string describing the object.
        /// </summary>
        /// <returns>String description of the object.</returns>
        public override string ToString()
        {
            return "[MouseMoveEventArgs]" +
                   " X(" + X + ")" +
                   " Y(" + Y + ")";
        }

        /// <summary>
        /// X coordinate of the mouse cursor.
        /// </summary>
        public readonly int X;

        /// <summary>
        /// Y coordinate of the mouse cursor.
        /// </summary>
        public readonly int Y;
    }

    /// <summary>
    /// Mouse buttons event parameters
    /// </summary>
    public class MouseButtonEventArgs : EventArgs
    {
        /// <summary>
        /// Constructs the mouse button arguments from a mouse button event.
        /// </summary>
        /// <param name="e">Mouse button event.</param>
        public MouseButtonEventArgs( MouseButtonEvent e )
        {
            Button = e.Button;
            X = e.X;
            Y = e.Y;
        }

        /// <summary>
        /// Provides a string describing the object.
        /// </summary>
        /// <returns>String description of the object.</returns>
        public override string ToString()
        {
            return "[MouseButtonEventArgs]" +
                   " Button(" + Button + ")" +
                   " X(" + X + ")" +
                   " Y(" + Y + ")";
        }

        /// <summary>
        /// Code of the button (see MouseButton enum).
        /// </summary>
        public readonly Mouse.Button Button;

        /// <summary>
        /// X coordinate of the mouse cursor.
        /// </summary>
        public readonly int X;

        /// <summary>
        /// Y coordinate of the mouse cursor.
        /// </summary>
        public readonly int Y;
    }


    /// <summary>
    /// Mouse wheel scroll event parameters.
    /// </summary>
    public class MouseWheelScrollEventArgs : EventArgs
    {
        /// <summary>
        /// Constructs the mouse wheel scroll arguments from a mouse wheel scroll event.
        /// </summary>
        /// <param name="e">Mouse wheel scroll event</param>
        public MouseWheelScrollEventArgs( MouseWheelScrollEvent e )
        {
            Delta = e.Delta;
            Wheel = e.Wheel;
            X = e.X;
            Y = e.Y;
        }

        /// <summary>
        /// Provides a string describing the object.
        /// </summary>
        /// <returns>String description of the object.</returns>
        public override string ToString()
        {
            return "[MouseWheelScrollEventArgs]" +
                   " Wheel(" + Wheel + ")" +
                   " Delta(" + Delta + ")" +
                   " X(" + X + ")" +
                   " Y(" + Y + ")";
        }

        /// <summary>
        /// Mouse Wheel which triggered the event.
        /// </summary>
        public readonly Mouse.Wheel Wheel;

        /// <summary>
        /// Scroll amount.
        /// </summary>
        public readonly float Delta;

        /// <summary>
        /// X coordinate of the mouse cursor.
        /// </summary>
        public readonly int X;

        /// <summary>
        /// Y coordinate of the mouse cursor.
        /// </summary>
        public readonly int Y;
    }

    /// <summary>
    /// Joystick axis move event parameters
    /// </summary>
    public class JoystickMoveEventArgs : EventArgs
    {
        /// <summary>
        /// Constructs the joystick move arguments from a joystick move event.
        /// </summary>
        /// <param name="e">Joystick move event.</param>
        public JoystickMoveEventArgs( JoystickMoveEvent e )
        {
            JoystickId = e.JoystickId;
            Axis = e.Axis;
            Position = e.Position;
        }

        /// <summary>
        /// Provides a string describing the object.
        /// </summary>
        /// <returns>String description of the object.</returns>
        public override string ToString()
        {
            return "[JoystickMoveEventArgs]" +
                   " JoystickId(" + JoystickId + ")" +
                   " Axis(" + Axis + ")" +
                   " Position(" + Position + ")";
        }

        /// <summary>
        /// Index of the joystick which triggered the event.
        /// </summary>
        public readonly uint JoystickId;

        /// <summary>
        /// Joystick axis (see JoyAxis enum).
        /// </summary>
        public readonly Joystick.Axis Axis;

        /// <summary>
        /// Current position of the axis.
        /// </summary>
        public readonly float Position;
    }

    /// <summary>
    /// Joystick buttons event parameters.
    /// </summary>
    public class JoystickButtonEventArgs : EventArgs
    {
        /// <summary>
        /// Constructs the joystick button arguments from a joystick button event.
        /// </summary>
        /// <param name="e">Joystick button event.</param>
        public JoystickButtonEventArgs( JoystickButtonEvent e )
        {
            JoystickId = e.JoystickId;
            Button = e.Button;
        }

        /// <summary>
        /// Provides a string describing the object.
        /// </summary>
        /// <returns>String description of the object.</returns>
        public override string ToString()
        {
            return "[JoystickButtonEventArgs]" +
                   " JoystickId(" + JoystickId + ")" +
                   " Button(" + Button + ")";
        }

        /// <summary>
        /// Index of the joystick which triggered the event.
        /// </summary>
        public readonly uint JoystickId;

        /// <summary>
        /// Index of the button.
        /// </summary>
        public readonly uint Button;
    }

    /// <summary>
    /// Joystick connection/disconnection event parameters.
    /// </summary>
    public class JoystickConnectEventArgs : EventArgs
    {
        /// <summary>
        /// Constructs the joystick connect arguments from a joystick connect event.
        /// </summary>
        /// <param name="e">Joystick button event.</param>
        public JoystickConnectEventArgs( JoystickConnectEvent e )
        {
            JoystickId = e.JoystickId;
        }

        /// <summary>
        /// Provides a string describing the object.
        /// </summary>
        /// <returns>String description of the object.</returns>
        public override string ToString()
        {
            return "[JoystickConnectEventArgs]" +
                   " JoystickId(" + JoystickId + ")";
        }

        /// <summary>
        /// Index of the joystick which triggered the event.
        /// </summary>
        public readonly uint JoystickId;
    }

    /// <summary>
    /// Size event parameters
    /// </summary>
    public class SizeEventArgs : EventArgs
    {
        /// <summary>
        /// Constructs the size arguments from a size event.
        /// </summary>
        /// <param name="e">Size event</param>
        public SizeEventArgs( SizeEvent e )
        {
            Width = e.Width;
            Height = e.Height;
        }

        /// <summary>
        /// Provides a string describing the object.
        /// </summary>
        /// <returns>String description of the object.</returns>
        public override string ToString()
        {
            return "[SizeEventArgs]" +
                   " Width(" + Width + ")" +
                   " Height(" + Height + ")";
        }

        /// <summary>
        /// New width of the window.
        /// </summary>
        public readonly uint Width;

        /// <summary>
        /// New height of the window.
        /// </summary>
        public readonly uint Height;
    }

    /// <summary>
    /// Touch event parameters
    /// </summary>
    public class TouchEventArgs : EventArgs
    {
        /// <summary>
        /// Constructs the touch arguments from a touch event.
        /// </summary>
        /// <param name="e">Touch event.</param>
        public TouchEventArgs( TouchEvent e )
        {
            Finger = e.Finger;
            X = e.X;
            Y = e.Y;
        }

        /// <summary>
        /// Provides a string describing the object.
        /// </summary>
        /// <returns>String description of the object.</returns>
        public override string ToString()
        {
            return "[TouchEventArgs]" +
                   " Finger(" + Finger + ")" +
                   " X(" + X + ")" +
                   " Y(" + Y + ")";
        }

        /// <summary>
        /// Index of the finger in case of multi-touch events.
        /// </summary>
        public readonly uint Finger;

        /// <summary>
        /// X position of the touch, relative to the left of the owner window.
        /// </summary>
        public readonly int X;

        /// <summary>
        /// Y position of the touch, relative to the top of the owner window.
        /// </summary>
        public readonly int Y;
    }

    /// <summary>
    /// Sensor event parameters
    /// </summary>
    public class SensorEventArgs : EventArgs
    {
        /// <summary>
        /// Constructs the sensor arguments from a sensor event.
        /// </summary>
        /// <param name="e">Sensor event.</param>
        public SensorEventArgs( SensorEvent e )
        {
            Type = e.Type;
            X = e.X;
            Y = e.Y;
            Z = e.Z;
        }

        /// <summary>
        /// Provide a string describing the object.
        /// </summary>
        /// <returns>String description of the object.</returns>
        public override string ToString()
        {
            return "[SensorEventArgs]" +
                   " Type(" + Type + ")" +
                   " X(" + X + ")" +
                   " Y(" + Y + ")" +
                   " Z(" + Z + ")";
        }

        /// <summary>
        /// Type of the sensor.
        /// </summary>
        public readonly Sensor.Type Type;

        /// <summary>
        /// Current value of the sensor on X axis.
        /// </summary>
        public readonly float X;

        /// <summary>
        /// Current value of the sensor on Y axis.
        /// </summary>
        public readonly float Y;

        /// <summary>
        /// Current value of the sensor on Z axis.
        /// </summary>
        public readonly float Z;
    }
}
