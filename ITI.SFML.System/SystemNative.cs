namespace SFML
{
    public static class SystemNative
    {
        static bool _loaded;

        /// <summary>
        /// Ensures that the native <see cref="System.CSFML.System"/> is loaded.
        /// </summary>
        public static void Load()
        {
            if( !_loaded ) System.CSFML.LoadNative( typeof( SystemNative ).Assembly, System.CSFML.System );
            _loaded = true;
        }
    }
}
