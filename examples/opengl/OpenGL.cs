using System;
using System.Runtime.InteropServices;
using System.Security;

namespace opengl
{
    static class GL
    {
        public enum Capability : int
        {
            DepthTest = 0x0B71,
            Lighting = 0x0B50,
            Texture2D = 0x0DE1,
        }

        public enum ClearBufferMask : int
        {
            Depth = 0x00000100,
        }

        public enum ClientCapability : int
        {
            VertexArray = 0x8074,
            NormalArray = 0x8075,
            ColorArray = 0x8076,
            TextureCoordArray = 0x8078,
        }

        public enum Primitive : int
        {
            Triangles = 0x0004,
        }

        public enum Format : int
        {
            RGBA = 0x1908,
        }

        public enum InternalFormat : int
        {
            RGBA = 0x1908,
        }

        public enum Matrix : int
        {
            ModelView = 0x1700,
            Projection = 0x1701,
            Texture = 0x1702,
            Color = 0x1703,
        }

        public enum PixelType : int
        {
            U8 = 0x1401,
        }

        public enum Target : int
        {
            Texture2D = 0x0DE1,
        }

        public enum TextureParameter : int
        {
            TextureMagFilter = 0x2800,
            TextureMinFilter = 0x2801,
        }

        public enum TextureFilter : int
        {
            Linear = 0x2601,
        }

        public enum Type : int
        {
            Float = 0x1402,
        }

        [DllImport("OpenGL32", EntryPoint = "glBindTexture", CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void BindTexture(Target target, uint texture);

        [DllImport("OpenGL32", EntryPoint = "glClear", CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void Clear(ClearBufferMask mask);

        [DllImport("OpenGL32", EntryPoint = "glClearDepth", CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void ClearDepth(double depth);

        [DllImport("OpenGL32", EntryPoint = "glClearDepth", CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void ClearDepth(float depth);

        [DllImport("OpenGL32", EntryPoint = "glDeleteTextures", CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void DeleteTextures(int count, ref uint texture);

        public static void DepthMask(bool flag) => glDepthMask(flag ? 1 : 0);

        [DllImport("OpenGL32", EntryPoint = "glDisable", CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void Disable(Capability cap);

        [DllImport("OpenGL32", EntryPoint = "glDisableClientState", CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void DisableClientState(ClientCapability cap);

        [DllImport("OpenGL32", EntryPoint = "glDrawArrays", CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void DrawArrays(Primitive mode, int first, int count);

        [DllImport("OpenGL32", EntryPoint = "glEnable", CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void Enable(Capability cap);

        [DllImport("OpenGL32", EntryPoint = "glEnableClientState", CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void EnableClientState(ClientCapability cap);

        [DllImport("OpenGL32", EntryPoint = "glFrustum", CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void Frustum(double left, double right, double bottom, double top, double near, double far);

        [DllImport("OpenGL32", EntryPoint = "glGenTextures", CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void GenTextures(int count, out uint texture);

        [DllImport("OpenGL32", EntryPoint = "glLoadIdentity", CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void LoadIdentity();

        [DllImport("OpenGL32", EntryPoint = "glMatrixMode", CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void MatrixMode(Matrix mode);

        [DllImport("OpenGL32", EntryPoint = "glRotatef", CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void Rotate(float angle, float x, float y, float z);

        public static void TexCoordPointer(int size, Type type, int stride, float[] data, int offset)
        {
            unsafe
            {
                fixed (void* ptr = &data[offset])
                {
                    glTexCoordPointer(size, type, stride, ptr);
                }
            }
        }

        public static void TexImage2D(Target target, int level, InternalFormat internalFormat, int width, int height, int border, Format format, PixelType type, byte[] data)
        {
            unsafe
            {
                fixed (void* ptr = data)
                {
                    glTexImage2D(target, level, internalFormat, width, height, border, format, type, ptr);
                }
            }
        }

        public static void TexParameter(Target target, TextureParameter param, TextureFilter paramValue) => TexParameter(target, param, (int)paramValue);

        [DllImport("OpenGL32", EntryPoint = "glTexParameteri", CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void TexParameter(Target target, TextureParameter param, int paramValue);

        [DllImport("OpenGL32", EntryPoint = "glTranslatef", CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void Translate(float x, float y, float z);

        public static void VertexPointer(int size, Type type, int stride, float[] data, int offset)
        {
            unsafe
            {
                fixed (void* ptr = &data[offset])
                {
                    glVertexPointer(size, type, stride, ptr);
                }
            }
        }

        [DllImport("OpenGL32", EntryPoint = "glViewport", CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern void Viewport(int x, int y, int width, int height);

        [DllImport("OpenGL32", CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        private static extern void glDepthMask(int flag);

        [DllImport("OpenGL32", CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        private static unsafe extern void glTexImage2D(Target target, int level, InternalFormat internalFormat, int width, int height, int border, Format format, PixelType type, void* data);

        [DllImport("OpenGL32", CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        private static unsafe extern void glTexCoordPointer(int size, Type type, int stride, void* pointer);

        [DllImport("OpenGL32", CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        private static unsafe extern void glVertexPointer(int size, Type type, int stride, void* pointer);
    }
}
