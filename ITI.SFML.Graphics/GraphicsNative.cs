namespace SFML
{
    public static class GraphicsNative
    {
        static bool _loaded;

        /// <summary>
        /// Ensures that the native <see cref="System.CSFML.System"/>, <see cref="System.CSFML.Window"/>
        /// and <see cref="System.CSFML.Graphics"/> are loaded.
        /// </summary>
        public static void Load()
        {
            WindowNative.Load();
            if( !_loaded ) System.CSFML.LoadNative( typeof( GraphicsNative ).Assembly, System.CSFML.Graphics );
            _loaded = true;
        }
    }
}
