using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Engine.Render.Assets;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Engine.Render.Buffers;

internal class UniformBuffer : IDisposable
{
    private readonly int _bufferHandle;

    public UniformBuffer(int size, int bindingPoint = 0, uint offset = 0,
        BufferUsageHint bufferUsage = BufferUsageHint.DynamicDraw)
    {
        GL.GenBuffers(1, out _bufferHandle);
        GL.BindBuffer(BufferTarget.UniformBuffer, _bufferHandle);
        GL.BufferData(BufferTarget.UniformBuffer, size, nint.Zero, bufferUsage);

        GL.BindBuffer(BufferTarget.UniformBuffer, 0);
        GL.BindBufferRange(BufferRangeTarget.UniformBuffer, bindingPoint, _bufferHandle, new IntPtr(offset),
            new IntPtr(size));

        SetData(new Matrix4(), 0);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Bind() => GL.BindBuffer(BufferTarget.UniformBuffer, _bufferHandle);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void UnBind() => GL.BindBuffer(BufferTarget.UniformBuffer, 0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetData<T>(T data, int offsetInOut) where T : unmanaged
    {
        Bind();
        GL.BufferSubData(BufferTarget.UniformBuffer, new IntPtr(offsetInOut), Marshal.SizeOf<T>(), ref data);
        UnBind();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void BindBlockToShader(Shader shader, string name, int bindingPoint = 0) =>
        GL.UniformBlockBinding(shader.Id, GetBlockLocation(shader, name), bindingPoint);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void BindBlockToShader(Shader shader, int uniformBlockLocation, int bindingPoint = 0) =>
        GL.UniformBlockBinding(shader.Id, uniformBlockLocation, bindingPoint);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int GetBlockLocation(Shader shader, string name) =>
        GL.GetUniformBlockIndex(shader.Id, name);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ReleaseUnmanagedResources() => GL.DeleteBuffer(_bufferHandle);

    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }

    ~UniformBuffer() => ReleaseUnmanagedResources();
}