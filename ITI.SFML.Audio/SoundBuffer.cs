using System;
using System.Runtime.InteropServices;
using System.Security;
using System.IO;
using SFML.System;

namespace SFML.Audio
{
    /// <summary>
    /// Storage for audio samples defining a sound.
    /// </summary>
    public class SoundBuffer : ObjectBase
    {
        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Construct a sound buffer from a file
        /// 
        /// Here is a complete list of all the supported audio formats:
        /// ogg, wav, flac, aiff, au, raw, paf, svx, nist, voc, ircam,
        /// w64, mat4, mat5 pvf, htk, sds, avr, sd2, caf, wve, mpc2k, rf64.
        /// </summary>
        /// <param name="filename">Path of the sound file to load</param>
        /// <exception cref="LoadingFailedException" />
        ////////////////////////////////////////////////////////////
        public SoundBuffer( string filename ) :
            base( sfSoundBuffer_createFromFile( filename ) )
        {
            if( CPointer == IntPtr.Zero )
                throw new LoadingFailedException( "sound buffer", filename );
        }

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Construct a sound buffer from a custom stream.
        ///
        /// Here is a complete list of all the supported audio formats:
        /// ogg, wav, flac, aiff, au, raw, paf, svx, nist, voc, ircam,
        /// w64, mat4, mat5 pvf, htk, sds, avr, sd2, caf, wve, mpc2k, rf64.
        /// </summary>
        /// <param name="stream">Source stream to read from</param>
        /// <exception cref="LoadingFailedException" />
        ////////////////////////////////////////////////////////////
        public SoundBuffer( Stream stream ) :
            base( IntPtr.Zero )
        {
            using( StreamAdaptor adaptor = new StreamAdaptor( stream ) )
            {
                CPointer = sfSoundBuffer_createFromStream( adaptor.InputStreamPtr );
            }

            if( CPointer == IntPtr.Zero )
                throw new LoadingFailedException( "sound buffer" );
        }

        /// <summary>
        /// Constructs a sound buffer from a file in memory.
        /// <para>
        /// Here is a complete list of all the supported audio formats:
        /// ogg, wav, flac, aiff, au, raw, paf, svx, nist, voc, ircam,
        /// w64, mat4, mat5 pvf, htk, sds, avr, sd2, caf, wve, mpc2k, rf64.
        /// </para>
        /// </summary>
        /// <param name="bytes">Byte array containing the file contents</param>
        /// <exception cref="LoadingFailedException" />
        public SoundBuffer( byte[] bytes )
            : base( IntPtr.Zero )
        {
            GCHandle pin = GCHandle.Alloc( bytes, GCHandleType.Pinned );
            try
            {
                CPointer = sfSoundBuffer_createFromMemory( pin.AddrOfPinnedObject(), Convert.ToUInt64( bytes.Length ) );
            }
            finally
            {
                pin.Free();
            }
            if( CPointer == IntPtr.Zero )
                throw new LoadingFailedException( "sound buffer" );
        }

        /// <summary>
        /// Constructs a sound buffer from an array of samples.
        /// </summary>
        /// <param name="samples">Array of samples</param>
        /// <param name="channelCount">Channel count</param>
        /// <param name="sampleRate">Sample rate</param>
        /// <exception cref="LoadingFailedException" />
        public SoundBuffer( short[] samples, uint channelCount, uint sampleRate )
            : base( IntPtr.Zero )
        {
            unsafe
            {
                fixed ( short* SamplesPtr = samples )
                {
                    CPointer = sfSoundBuffer_createFromSamples( SamplesPtr, (uint)samples.Length, channelCount, sampleRate );
                }
            }

            if( CPointer == IntPtr.Zero )
                throw new LoadingFailedException( "sound buffer" );
        }

        /// <summary>
        /// Constructs a sound buffer from another sound buffer.
        /// </summary>
        /// <param name="copy">Sound buffer to copy</param>
        public SoundBuffer( SoundBuffer copy ) :
            base( sfSoundBuffer_copy( copy.CPointer ) )
        {
        }

        /// <summary>
        /// Saves the sound buffer to an audio file.
        /// <para>
        /// Here is a complete list of all the supported audio formats:
        /// ogg, wav, flac, aiff, au, raw, paf, svx, nist, voc, ircam,
        /// w64, mat4, mat5 pvf, htk, sds, avr, sd2, caf, wve, mpc2k, rf64.
        /// </para>
        /// </summary>
        /// <param name="filename">Path of the sound file to write.</param>
        /// <returns>True if saving has been successful.</returns>
        public bool SaveToFile( string filename )
        {
            return sfSoundBuffer_saveToFile( CPointer, filename );
        }

        /// <summary>
        /// Gets the sample rate of the sound buffer.
        /// <para>
        /// The sample rate is the number of audio samples played per
        /// second. The higher, the better the quality.
        /// </para>
        /// </summary>
        public uint SampleRate
        {
            get { return sfSoundBuffer_getSampleRate( CPointer ); }
        }

        /// <summary>
        /// Gets the number of channels (1 = mono, 2 = stereo).
        /// </summary>
        public uint ChannelCount
        {
            get { return sfSoundBuffer_getChannelCount( CPointer ); }
        }

        /// <summary>
        /// Gets the total duration of the buffer.
        /// </summary>
        public Time Duration
        {
            get { return sfSoundBuffer_getDuration( CPointer ); }
        }

        /// <summary>
        /// Gets the array of audio samples stored in the buffer.
        /// The format of the returned samples is 16 bits signed integer (Int16).
        /// </summary>
        public short[] Samples
        {
            get
            {
                short[] SamplesArray = new short[sfSoundBuffer_getSampleCount( CPointer )];
                Marshal.Copy( sfSoundBuffer_getSamples( CPointer ), SamplesArray, 0, SamplesArray.Length );
                return SamplesArray;
            }
        }

        /// <summary>
        /// Provides a string describing the object.
        /// </summary>
        /// <returns>String description of the object.</returns>
        public override string ToString()
        {
            return "[SoundBuffer]" +
                   " SampleRate(" + SampleRate + ")" +
                   " ChannelCount(" + ChannelCount + ")" +
                   " Duration(" + Duration + ")";
        }

        /// <summary>
        /// Handles the destruction of the object.
        /// </summary>
        /// <param name="disposing">Is the GC disposing the object, or is it an explicit call ?</param>
        protected override void Destroy( bool disposing )
        {
            sfSoundBuffer_destroy( CPointer );
        }

        #region Imports
        [DllImport( CSFML.Audio, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern IntPtr sfSoundBuffer_createFromFile( string Filename );

        [DllImport( CSFML.Audio, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        unsafe static extern IntPtr sfSoundBuffer_createFromStream( IntPtr stream );

        [DllImport( CSFML.Audio, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        unsafe static extern IntPtr sfSoundBuffer_createFromMemory( IntPtr data, ulong size );

        [DllImport( CSFML.Audio, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        unsafe static extern IntPtr sfSoundBuffer_createFromSamples( short* Samples, uint SampleCount, uint ChannelsCount, uint SampleRate );

        [DllImport( CSFML.Audio, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern IntPtr sfSoundBuffer_copy( IntPtr SoundBuffer );

        [DllImport( CSFML.Audio, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfSoundBuffer_destroy( IntPtr SoundBuffer );

        [DllImport( CSFML.Audio, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern bool sfSoundBuffer_saveToFile( IntPtr SoundBuffer, string Filename );

        [DllImport( CSFML.Audio, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern IntPtr sfSoundBuffer_getSamples( IntPtr SoundBuffer );

        [DllImport( CSFML.Audio, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern uint sfSoundBuffer_getSampleCount( IntPtr SoundBuffer );

        [DllImport( CSFML.Audio, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern uint sfSoundBuffer_getSampleRate( IntPtr SoundBuffer );

        [DllImport( CSFML.Audio, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern uint sfSoundBuffer_getChannelCount( IntPtr SoundBuffer );

        [DllImport( CSFML.Audio, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern Time sfSoundBuffer_getDuration( IntPtr SoundBuffer );
        #endregion
    }
}
