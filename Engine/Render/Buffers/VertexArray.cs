using System.Runtime.CompilerServices;
using OpenTK.Graphics.OpenGL4;

namespace Engine.Render.Buffers;

internal class VertexArray : IDisposable
{
    private readonly int _vertexArrayHandle;

    public VertexArray()
    {
        GL.GenVertexArrays(1, out _vertexArrayHandle);
        GL.BindVertexArray(_vertexArrayHandle);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void BindAttribute(uint attribute, VertexBuffer vertexBuffer, int count, int stride, int offset)
    {
        Bind();
        vertexBuffer.Bind();
        GL.EnableVertexAttribArray(attribute);
        GL.VertexAttribPointer(attribute, count, VertexAttribPointerType.Float, false, stride, new IntPtr(offset));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Bind() => GL.BindVertexArray(_vertexArrayHandle);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void UnBind() => GL.BindVertexArray(0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ReleaseUnmanagedResources() => GL.DeleteVertexArray(_vertexArrayHandle);

    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }

    ~VertexArray() => ReleaseUnmanagedResources();
}