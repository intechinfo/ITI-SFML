namespace SFML.Graphics
{
    /// <summary>
    /// Interface for every object that can be drawn to a render window.
    /// </summary>
    public interface IDrawable
    {
        /// <summary>
        /// Draws the object to a render target.
        /// <para>
        /// This is a function that has to be implemented by the
        /// derived class to define how the drawable should be drawn.
        /// </para>
        /// </summary>
        /// <param name="target">Render target to draw to.</param>
        /// <param name="states">Render states to use.</param>
        void Draw(IRenderTarget target, in RenderStates states);
    }
}
