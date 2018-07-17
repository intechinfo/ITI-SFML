using System;
using System.Runtime.InteropServices;
using System.Security;
using SFML.System;

namespace SFML.Audio
{
    /// <summary>
    /// Abstract base class for streamed audio sources.
    /// </summary>
    public abstract class SoundStream : ObjectBase
    {
        GetDataCallbackType _getDataCallback;
        SeekCallbackType _seekCallback;
        short[] _tempBuffer;

        /// <summary>
        /// Default constructor
        /// </summary>
        public SoundStream()
            : base( IntPtr.Zero )
        {
        }

        /// <summary>
        /// Starts or resumes playing the audio stream.
        /// <para>
        /// This function starts the stream if it was stopped, resumes
        /// it if it was paused, and restarts it from beginning if it
        /// was it already playing.
        /// This function uses its own thread so that it doesn't block
        /// the rest of the program while the stream is played.
        /// </para>
        /// </summary>
        public void Play()
        {
            sfSoundStream_play( CPointer );
        }

        /// <summary>
        /// Pauses the audio stream.
        /// <para>
        /// This function pauses the stream if it was playing,
        /// otherwise (stream already paused or stopped) it has no effect.
        /// </para>
        /// </summary>
        public void Pause()
        {
            sfSoundStream_pause( CPointer );
        }

        /// <summary>
        /// Stops playing the audio stream.
        /// <para>
        /// This function stops the stream if it was playing or paused,
        /// and does nothing if it was already stopped.
        /// It also resets the playing position (unlike pause()).
        /// </para>
        /// </summary>
        public void Stop()
        {
            sfSoundStream_stop( CPointer );
        }

        /// <summary>
        /// Gets the sample rate of the stream.
        /// <para>
        /// The sample rate is the number of audio samples played per
        /// second. The higher, the better the quality.
        /// </para>
        /// </summary>
        public uint SampleRate
        {
            get { return sfSoundStream_getSampleRate( CPointer ); }
        }

        /// <summary>
        /// Gets the number of channels (1 = mono, 2 = stereo).
        /// </summary>
        public uint ChannelCount
        {
            get { return sfSoundStream_getChannelCount( CPointer ); }
        }

        /// <summary>
        /// Gets the current status of the sound stream (see <see cref="SoundStatus"/> enum).
        /// </summary>
        public SoundStatus Status
        {
            get { return sfSoundStream_getStatus( CPointer ); }
        }

        /// <summary>
        /// Gets or sets whether the music should loop after reaching the end.
        /// The default looping state for music is false.
        /// <para>
        /// If set, the music will restart from beginning after
        /// reaching the end and so on, until it is stopped or
        /// Loop = false is set.
        /// </para>
        /// </summary>
        public bool Loop
        {
            get { return sfSoundStream_getLoop( CPointer ); }
            set { sfSoundStream_setLoop( CPointer, value ); }
        }

        /// <summary>
        /// Gets or sets the pitch of the stream.
        /// <para>
        /// The pitch represents the perceived fundamental frequency
        /// of a sound; thus you can make a sound more acute or grave
        /// by changing its pitch. A side effect of changing the pitch
        /// is to modify the playing speed of the sound as well.
        /// The default value for the pitch is 1.0.
        /// </para>
        /// </summary>
        public float Pitch
        {
            get { return sfSoundStream_getPitch( CPointer ); }
            set { sfSoundStream_setPitch( CPointer, value ); }
        }

        /// <summary>
        /// Gets or sets the volume of the stream.
        /// <para>
        /// The volume is a value between 0 (mute) and 100 (full volume).
        /// The default value for the volume is 100.
        /// </para>
        /// </summary>
        public float Volume
        {
            get { return sfSoundStream_getVolume( CPointer ); }
            set { sfSoundStream_setVolume( CPointer, value ); }
        }

        /// <summary>
        /// Gets or sets the 3D position of the stream in the audio scene.
        /// <para>
        /// Only sounds with one channel (mono sounds) can be
        /// spatialized.
        /// The default position of a sound is (0, 0, 0).
        /// </para>
        /// </summary>
        public Vector3f Position
        {
            get { return sfSoundStream_getPosition( CPointer ); }
            set { sfSoundStream_setPosition( CPointer, value ); }
        }

        /// Gets or sets whether the stream's position is relative to the listener or absolute.
        /// <para>
        /// Making a sound relative to the listener will ensure that it will always
        /// be played the same way regardless the position of the listener.
        /// This can be useful for non-spatialized sounds, sounds that are
        /// produced by the listener, or sounds attached to it.
        /// The default value is false (position is absolute).
        /// </para>
        /// </summary>
        public bool RelativeToListener
        {
            get { return sfSoundStream_isRelativeToListener( CPointer ); }
            set { sfSoundStream_setRelativeToListener( CPointer, value ); }
        }

        /// <summary>
        /// Gets or sets the minimum distance of the music.
        /// <para>
        /// The "minimum distance" of a sound is the maximum
        /// distance at which it is heard at its maximum volume. Further
        /// than the minimum distance, it will start to fade out according
        /// to its attenuation factor. A value of 0 ("inside the head
        /// of the listener") is an invalid value and is forbidden.
        /// The default value of the minimum distance is 1.
        /// </para>
        /// </summary>
        public float MinDistance
        {
            get { return sfSoundStream_getMinDistance( CPointer ); }
            set { sfSoundStream_setMinDistance( CPointer, value ); }
        }

        /// <summary>
        /// Attenuation factor of the stream.
        /// <para>
        /// The attenuation is a multiplicative factor which makes
        /// the music more or less loud according to its distance
        /// from the listener. An attenuation of 0 will produce a
        /// non-attenuated sound, i.e. its volume will always be the same
        /// whether it is heard from near or from far. On the other hand,
        /// an attenuation value such as 100 will make the sound fade out
        /// very quickly as it gets further from the listener.
        /// The default value of the attenuation is 1.
        /// </para>
        /// </summary>
        public float Attenuation
        {
            get { return sfSoundStream_getAttenuation( CPointer ); }
            set { sfSoundStream_setAttenuation( CPointer, value ); }
        }

        /// <summary>
        /// Gets or sets the current playing position of the stream.
        /// The playing position can be changed when the music is
        /// either paused or playing.
        /// </summary>
        public Time PlayingOffset
        {
            get { return sfSoundStream_getPlayingOffset( CPointer ); }
            set { sfSoundStream_setPlayingOffset( CPointer, value ); }
        }

        /// <summary>
        /// Provides a string describing the object.
        /// </summary>
        /// <returns>String description of the object.</returns>
        public override string ToString()
        {
            return "[SoundStream]" +
                   " SampleRate(" + SampleRate + ")" +
                   " ChannelCount(" + ChannelCount + ")" +
                   " Status(" + Status + ")" +
                   " Loop(" + Loop + ")" +
                   " Pitch(" + Pitch + ")" +
                   " Volume(" + Volume + ")" +
                   " Position(" + Position + ")" +
                   " RelativeToListener(" + RelativeToListener + ")" +
                   " MinDistance(" + MinDistance + ")" +
                   " Attenuation(" + Attenuation + ")" +
                   " PlayingOffset(" + PlayingOffset + ")";
        }

        /// <summary>
        /// Sets the audio stream parameters, you must call it before calling <see cref="Play"/>.
        /// </summary>
        /// <param name="channelCount">Number of channels.</param>
        /// <param name="sampleRate">Sample rate, in samples per second.</param>
        protected void Initialize( uint channelCount, uint sampleRate )
        {
            _getDataCallback = new GetDataCallbackType( GetData );
            _seekCallback = new SeekCallbackType( Seek );
            CPointer = sfSoundStream_create( _getDataCallback, _seekCallback, channelCount, sampleRate, IntPtr.Zero );
        }

        /// <summary>
        /// Virtual function called each time new audio data is needed to feed the stream.
        /// </summary>
        /// <param name="samples">Array of samples to fill for the stream.</param>
        /// <returns>True to continue playback, false to stop.</returns>
        protected abstract bool OnGetData( out short[] samples );

        /// <summary>
        /// Virtual function called to seek into the stream.
        /// </summary>
        /// <param name="timeOffset">New position.</param>
        protected abstract void OnSeek( Time timeOffset );

        /// <summary>
        /// Handles the destruction of the object.
        /// </summary>
        /// <param name="disposing">Is the GC disposing the object, or is it an explicit call ?</param>
        protected override void Destroy( bool disposing )
        {
            sfSoundStream_destroy( CPointer );
        }

        /// <summary>
        /// Structure mapping the C library arguments passed to the data callback
        /// </summary>
        [StructLayout( LayoutKind.Sequential )]
        struct Chunk
        {
            unsafe public short* samples;
            public uint sampleCount;
        }

        /// <summary>
        /// Called each time new audio data is needed to feed the stream.
        /// </summary>
        /// <param name="dataChunk">Data chunk to fill with new audio samples.</param>
        /// <param name="userData">User data -- unused</param>
        /// <returns>True to continue playback, false to stop.</returns>
        bool GetData( ref Chunk dataChunk, IntPtr userData )
        {
            if( OnGetData( out _tempBuffer ) )
            {
                unsafe
                {
                    fixed ( short* samplesPtr = _tempBuffer )
                    {
                        dataChunk.samples = samplesPtr;
                        dataChunk.sampleCount = (uint)_tempBuffer.Length;
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Called to seek in the stream.
        /// </summary>
        /// <param name="timeOffset">New position</param>
        /// <param name="userData">User data -- unused</param>
        /// <returns>If false is returned, the playback is aborted</returns>
        void Seek( Time timeOffset, IntPtr userData )
        {
            OnSeek( timeOffset );
        }

        [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
        delegate bool GetDataCallbackType( ref Chunk dataChunk, IntPtr UserData );

        [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
        delegate void SeekCallbackType( Time timeOffset, IntPtr UserData );


        #region Imports
        [DllImport( CSFML.Audio, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern IntPtr sfSoundStream_create( GetDataCallbackType OnGetData, SeekCallbackType OnSeek, uint ChannelCount, uint SampleRate, IntPtr UserData );

        [DllImport( CSFML.Audio, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfSoundStream_destroy( IntPtr SoundStreamStream );

        [DllImport( CSFML.Audio, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfSoundStream_play( IntPtr SoundStream );

        [DllImport( CSFML.Audio, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfSoundStream_pause( IntPtr SoundStream );

        [DllImport( CSFML.Audio, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfSoundStream_stop( IntPtr SoundStream );

        [DllImport( CSFML.Audio, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern SoundStatus sfSoundStream_getStatus( IntPtr SoundStream );

        [DllImport( CSFML.Audio, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern uint sfSoundStream_getChannelCount( IntPtr SoundStream );

        [DllImport( CSFML.Audio, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern uint sfSoundStream_getSampleRate( IntPtr SoundStream );

        [DllImport( CSFML.Audio, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfSoundStream_setLoop( IntPtr SoundStream, bool Loop );

        [DllImport( CSFML.Audio, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfSoundStream_setPitch( IntPtr SoundStream, float Pitch );

        [DllImport( CSFML.Audio, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfSoundStream_setVolume( IntPtr SoundStream, float Volume );

        [DllImport( CSFML.Audio, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfSoundStream_setPosition( IntPtr SoundStream, Vector3f position );

        [DllImport( CSFML.Audio, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfSoundStream_setRelativeToListener( IntPtr SoundStream, bool Relative );

        [DllImport( CSFML.Audio, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfSoundStream_setMinDistance( IntPtr SoundStream, float MinDistance );

        [DllImport( CSFML.Audio, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfSoundStream_setAttenuation( IntPtr SoundStream, float Attenuation );

        [DllImport( CSFML.Audio, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfSoundStream_setPlayingOffset( IntPtr SoundStream, Time TimeOffset );

        [DllImport( CSFML.Audio, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern bool sfSoundStream_getLoop( IntPtr SoundStream );

        [DllImport( CSFML.Audio, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern float sfSoundStream_getPitch( IntPtr SoundStream );

        [DllImport( CSFML.Audio, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern float sfSoundStream_getVolume( IntPtr SoundStream );

        [DllImport( CSFML.Audio, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern Vector3f sfSoundStream_getPosition( IntPtr SoundStream );

        [DllImport( CSFML.Audio, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern bool sfSoundStream_isRelativeToListener( IntPtr SoundStream );

        [DllImport( CSFML.Audio, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern float sfSoundStream_getMinDistance( IntPtr SoundStream );

        [DllImport( CSFML.Audio, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern float sfSoundStream_getAttenuation( IntPtr SoundStream );

        [DllImport( CSFML.Audio, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern Time sfSoundStream_getPlayingOffset( IntPtr SoundStream );
        #endregion
    }
}
