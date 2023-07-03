using System.Runtime.CompilerServices;
using OpenTK.Graphics.OpenGL4;

namespace Engine.Render.Buffers;

public class IndexBuffer
{
    private readonly int _bufferHandle;

    public IndexBuffer(uint[] data)
    {
        GL.GenBuffers(1, out _bufferHandle);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _bufferHandle);
        GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(uint) * data.Length, data, BufferUsageHint.StaticDraw);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Bind() => GL.BindBuffer(BufferTarget.ElementArrayBuffer, _bufferHandle);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void UnBind() => GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
}