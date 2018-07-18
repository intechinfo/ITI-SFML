namespace SFML
{
    public static class AudioNative
    {
        static bool _loaded;

        /// <summary>
        /// Ensures that the native <see cref="System.CSFML.System"/> and <see cref="System.CSFML.Audio"/>
        /// are loaded.
        /// </summary>
        public static void Load()
        {
            SystemNative.Load();
            if( !_loaded ) System.CSFML.LoadNative( typeof( AudioNative ).Assembly, System.CSFML.Audio );
            _loaded = true;
        }
    }
}
