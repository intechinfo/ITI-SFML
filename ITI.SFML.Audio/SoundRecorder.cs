using System;
using System.Runtime.InteropServices;
using System.Security;
using SFML.System;

namespace SFML.Audio
{
    /// <summary>
    /// Base class intended for capturing sound data.
    /// </summary>
    public abstract class SoundRecorder : ObjectBase
    {
        readonly StartCallback _startCallback;
        readonly ProcessCallback _processCallback;
        readonly StopCallback _stopCallback;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SoundRecorder()
            : base( IntPtr.Zero )
        {
            _startCallback = new StartCallback( OnStart );
            _processCallback = new ProcessCallback( ProcessSamples );
            _stopCallback = new StopCallback( OnStop );

            CPointer = sfSoundRecorder_create( _startCallback, _processCallback, _stopCallback, IntPtr.Zero );
        }

        /// <summary>
        /// Starts the capture.
        /// <para>
        /// The sampleRate parameter defines the number of audio samples
        /// captured per second. The higher, the better the quality
        /// (for example, 44100 samples/sec is CD quality).
        /// This function uses its own thread so that it doesn't block
        /// the rest of the program while the capture runs.
        /// </para>
        /// Please note that only one capture can happen at the same time.
        /// </summary>
        /// <param name="sampleRate">Sound frequency; the more samples, the higher the quality (44100 by default = CD quality).</param>
        public bool Start( uint sampleRate = 44100 )
        {
            return sfSoundRecorder_start( CPointer, sampleRate );
        }

        /// <summary>
        /// Stops the capture
        /// </summary>
        public void Stop()
        {
            sfSoundRecorder_stop( CPointer );
        }

        /// <summary>
        /// Gets the sample rate of the sound recorder.
        /// </summary>
        /// <remarks>
        /// The sample rate defines the number of audio samples
        /// captured per second. The higher, the better the quality
        /// (for example, 44100 samples/sec is CD quality).
        /// </remarks>
        public uint SampleRate
        {
            get { return sfSoundRecorder_getSampleRate( CPointer ); }
        }

        /// <summary>
        /// Gets or sets the channel count of the audio capture device.
        /// </summary>
        /// <remarks>
        /// This method allows you to specify the number of channels
        /// used for recording. Currently only 16-bit mono (1) and
        /// 16-bit stereo (2) are supported.
        /// </remarks>
        public uint ChannelCount
        {
            get { return sfSoundRecorder_getChannelCount( CPointer ); }
            set { sfSoundRecorder_setChannelCount( CPointer, value ); }
        }

        /// <summary>
        /// Checks if the system supports audio capture.
        /// </summary>
        /// <remarks>
        /// This function should always be called before using
        /// the audio capture features. If it returns false, then
        /// any attempt to use the SoundRecorder or one of its derived
        /// classes will fail.
        /// </remarks>
        public static bool IsAvailable
        {
            get { return sfSoundRecorder_isAvailable(); }
        }

        /// <summary>
        /// Provides a string describing the object.
        /// </summary>
        /// <returns>String description of the object.</returns>
        public override string ToString()
        {
            return "[SoundRecorder]" + " SampleRate(" + SampleRate + ")";
        }

        /// <summary>
        /// Starts capturing audio data.
        /// This virtual function may be overridden by a derived class
        /// if something has to be done every time a new capture
        /// starts. If not, this function can be ignored; the default
        /// implementation does nothing.
        /// </summary>
        /// <returns>False to abort recording audio data, true to continue</returns>
        protected virtual bool OnStart()
        {
            // Does nothing by default
            return true;
        }

        /// <summary>
        /// Processes a new chunk of recorded samples.
        ///
        /// This virtual function is called every time a new chunk of
        /// recorded data is available. The derived class can then do
        /// whatever it wants with it (storing it, playing it, sending
        /// it over the network, etc.).
        /// </summary>
        /// <param name="samples">Array of samples to process</param>
        /// <returns>False to stop recording audio data, true to continue</returns>
        protected abstract bool OnProcessSamples( short[] samples );

        /// <summary>
        /// Stops capturing audio data.
        /// This virtual function may be overridden by a derived class
        /// if something has to be done every time the capture
        /// ends. If not, this function can be ignored; the default
        /// implementation does nothing.
        /// </summary>
        protected virtual void OnStop()
        {
            // Does nothing by default
        }

        /// <summary>
        /// The processing interval controls the period
        /// between calls to the onProcessSamples function. You may
        /// want to use a small interval if you want to process the
        /// recorded data in real time, for example.
        /// <para>
        /// Note: this is only a hint, the actual period may vary.
        /// So don't rely on this parameter to implement precise timing.
        /// </para>
        /// The default processing interval is 100 ms.
        /// </summary>
        protected void SetProcessingInterval( Time interval )
        {
            sfSoundRecorder_setProcessingInterval( CPointer, interval );
        }

        /// <summary>
        /// Gets the list of the names of all available audio capture devices.
        /// </summary>
        public static string[] AvailableDevices
        {
            get
            {
                unsafe
                {
                    uint Count;
                    IntPtr* DevicesPtr = sfSoundRecorder_getAvailableDevices( out Count );
                    string[] Devices = new string[Count];
                    for( uint i = 0; i < Count; ++i )
                        Devices[i] = Marshal.PtrToStringAnsi( DevicesPtr[i] );

                    return Devices;
                }
            }
        }

        /// <summary>
        /// Gets the name of the default audio capture device.
        /// </summary>
        public static string DefaultDevice
        {
            get { return Marshal.PtrToStringAnsi( sfSoundRecorder_getDefaultDevice() ); }
        }

        /// <summary>
        /// Sets the audio capture device.
        /// </summary>
        /// <param name="Name">The name of the audio capture device.</param>
        /// <returns>True, if it was able to set the requested device.</returns>
        public bool SetDevice( string Name )
        {
            return sfSoundRecorder_setDevice( CPointer, Name );
        }

        /// <summary>
        /// Gets the name of the current audio capture device.
        /// </summary>
        /// <returns>The name of the current audio capture device.</returns>
        public string GetDevice()
        {
            return Marshal.PtrToStringAnsi( sfSoundRecorder_getDevice( CPointer ) );
        }

        /// <summary>
        /// Handles the destruction of the object.
        /// </summary>
        /// <param name="disposing">Is the GC disposing the object, or is it an explicit call ?</param>
        protected override void Destroy( bool disposing )
        {
            sfSoundRecorder_destroy( CPointer );
        }

        /// <summary>
        /// Function called directly by the C library ; convert
        /// arguments and forward them to the internal virtual function
        /// </summary>
        /// <param name="samples">Pointer to the array of samples</param>
        /// <param name="nbSamples">Number of samples in the array</param>
        /// <param name="userData">User data -- unused</param>
        /// <returns>False to stop recording audio data, true to continue</returns>
        private bool ProcessSamples( IntPtr samples, uint nbSamples, IntPtr userData )
        {
            short[] samplesArray = new short[nbSamples];
            Marshal.Copy( samples, samplesArray, 0, samplesArray.Length );
            return OnProcessSamples( samplesArray );
        }

        [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
        private delegate bool StartCallback();

        [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
        private delegate bool ProcessCallback( IntPtr samples, uint nbSamples, IntPtr userData );

        [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
        private delegate void StopCallback();

        #region Imports
        [DllImport( CSFML.Audio, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern IntPtr sfSoundRecorder_create( StartCallback OnStart, ProcessCallback OnProcess, StopCallback OnStop, IntPtr UserData );

        [DllImport( CSFML.Audio, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfSoundRecorder_destroy( IntPtr SoundRecorder );

        [DllImport( CSFML.Audio, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern bool sfSoundRecorder_start( IntPtr SoundRecorder, uint SampleRate );

        [DllImport( CSFML.Audio, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfSoundRecorder_stop( IntPtr SoundRecorder );

        [DllImport( CSFML.Audio, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern uint sfSoundRecorder_getSampleRate( IntPtr SoundRecorder );

        [DllImport( CSFML.Audio, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern bool sfSoundRecorder_isAvailable();

        [DllImport( CSFML.Audio, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfSoundRecorder_setProcessingInterval( IntPtr SoundRecorder, Time Interval );

        [DllImport( CSFML.Audio, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        unsafe static extern IntPtr* sfSoundRecorder_getAvailableDevices( out uint Count );

        [DllImport( CSFML.Audio, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern IntPtr sfSoundRecorder_getDefaultDevice();

        [DllImport( CSFML.Audio, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern bool sfSoundRecorder_setDevice( IntPtr SoundRecorder, string Name );

        [DllImport( CSFML.Audio, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern IntPtr sfSoundRecorder_getDevice( IntPtr SoundRecorder );

        [DllImport( CSFML.Audio, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfSoundRecorder_setChannelCount( IntPtr SoundRecorder, uint channelCount );

        [DllImport( CSFML.Audio, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern uint sfSoundRecorder_getChannelCount( IntPtr SoundRecorder );
        #endregion
    }
}