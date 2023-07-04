using System.Runtime.CompilerServices;
using Engine.AssetManager;
using Engine.Logging;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Engine.Render.Assets;

internal struct UniformInfo
{
    public ActiveUniformType UniformType;
    public string Name;
    public int Location;
    public object? Value;
}

internal class Shader : IAsset
{
    public string FilePath { get; set; }
    public string Name => Path.GetFileNameWithoutExtension(FilePath);
    public EAssetType AssetType => EAssetType.Shader;

    public List<UniformInfo> Uniforms { get; } = new();

    internal readonly int Id;

    private Dictionary<string, int> _uniformCache = new();

    public Shader(string filePath, int id)
    {
        FilePath = filePath;
        Id = id;
        QueryUniforms();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Bind() => GL.UseProgram(Id);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void UnBind() => GL.UseProgram(0);

    #region Get Method

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void GetUniform(string name, out bool res)
    {
        GL.GetUniform(Id, GetUniformLocation(name), out int temp);
        res = temp == 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void GetUniform(string name, out int res) => GL.GetUniform(Id, GetUniformLocation(name), out res);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void GetUniform(string name, out float res) => GL.GetUniform(Id, GetUniformLocation(name), out res);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void GetUniform(string name, out double res) => GL.GetUniform(Id, GetUniformLocation(name), out res);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe void GetUniform(string name, out Vector2 res)
    {
        Vector2 temp;
        GL.GetUniform(Id, GetUniformLocation(name), (float*)&temp);
        res = temp;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe void GetUniform(string name, out Vector3 res)
    {
        Vector3 temp;
        GL.GetUniform(Id, GetUniformLocation(name), (float*)&temp);
        res = temp;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe void GetUniform(string name, out Vector4 res)
    {
        Vector4 temp;
        GL.GetUniform(Id, GetUniformLocation(name), (float*)&temp);
        res = temp;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe void GetUniform(string name, out Matrix4 res)
    {
        Matrix4 temp;
        GL.GetUniform(Id, GetUniformLocation(name), (float*)&temp);
        res = temp;
    }

    #endregion

    #region Set Method

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetUniform(string name, int value) => GL.Uniform1(GetUniformLocation(name), value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetUniform(string name, uint value) => GL.Uniform1(GetUniformLocation(name), value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetUniform(string name, float value) => GL.Uniform1(GetUniformLocation(name), value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetUniform(string name, double value) => GL.Uniform1(GetUniformLocation(name), value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetUniform(string name, Vector2 value) => GL.Uniform2(GetUniformLocation(name), value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetUniform(string name, Vector3 value) => GL.Uniform3(GetUniformLocation(name), value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetUniform(string name, Vector4 value) => GL.Uniform4(GetUniformLocation(name), value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetUniform(string name, Matrix4 value) => GL.UniformMatrix4(GetUniformLocation(name), false, ref value);

    #endregion

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IsEngineUboMember(string name) => name.Contains("ubo_");

    public void QueryUniforms()
    {
        Uniforms.Clear();
        GL.GetProgram(Id, GetProgramParameterName.ActiveUniforms, out int numActiveUniforms);
        for (int i = 0; i < numActiveUniforms; i++)
        {
            GL.GetActiveUniform(Id, i, 256, out _, out _, out ActiveUniformType uniformType, out string name);
            if (!IsEngineUboMember(name))
            {
                object? defaultValue = default;
                bool hasValue = true;

                switch (uniformType)
                {
                    case ActiveUniformType.Bool:
                        GetUniform(name, out int b);
                        defaultValue = b != 0;
                        break;
                    case ActiveUniformType.Int:
                        GetUniform(name, out int iv);
                        defaultValue = iv;
                        break;
                    case ActiveUniformType.Float:
                        GetUniform(name, out float f);
                        defaultValue = f;
                        break;
                    case ActiveUniformType.FloatVec2:
                        GetUniform(name, out Vector2 v2);
                        defaultValue = v2;
                        break;
                    case ActiveUniformType.FloatVec3:
                        GetUniform(name, out Vector3 v3);
                        defaultValue = v3;
                        break;
                    case ActiveUniformType.FloatVec4:
                        GetUniform(name, out Vector4 v4);
                        defaultValue = v4;
                        break;
                    case ActiveUniformType.FloatMat4:
                        GetUniform(name, out Matrix4 m4);
                        defaultValue = m4;
                        break;
                    case ActiveUniformType.Sampler2D:
                        break;
                    default:
                        hasValue = false;
                        break;
                }

                if (hasValue)
                {
                    Uniforms.Add(new()
                    {
                        UniformType = uniformType,
                        Name = name,
                        Value = defaultValue,
                        Location = GetUniformLocation(name)
                    });
                }
            }
        }
    }

    public int GetUniformLocation(string uniformName)
    {
        if (_uniformCache.TryGetValue(uniformName, out int uniformLocation))
            return uniformLocation;

        int location = GL.GetUniformLocation(Id, uniformName);
        if (location == -1)
            Logger.CoreWarn($"Uniform: {uniformName} doesn't exist");

        _uniformCache[uniformName] = location;
        return location;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ReleaseUnmanagedResources() => GL.DeleteShader(Id);

    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }

    ~Shader() => ReleaseUnmanagedResources();
}