using System.Runtime.CompilerServices;
using OpenTK.Graphics.OpenGL4;

namespace Engine.Render.Buffers;

internal class VertexBuffer : IDisposable
{
    private readonly int _bufferHandle;

    public VertexBuffer(float[] data)
    {
        GL.GenBuffers(1, out _bufferHandle);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _bufferHandle);
        GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * data.Length, data, BufferUsageHint.StaticDraw);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Bind() => GL.BindBuffer(BufferTarget.ArrayBuffer, _bufferHandle);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void UnBind() => GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ReleaseUnmanagedResources() => GL.DeleteBuffer(_bufferHandle);

    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }

    ~VertexBuffer() => ReleaseUnmanagedResources();
}