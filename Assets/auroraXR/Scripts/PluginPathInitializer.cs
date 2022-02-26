using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
class PluginPathValue
{
    public PlatformModes PlatformMode;

    [Tooltip("Start with %DATAPTH%/ to create a reference relative to the Application's Data Path")]
    public string Path;

    public PluginPathValue()
    {
    }

    public PluginPathValue(PlatformModes mode, string path)
    {
        PlatformMode = mode;
        Path = path;
    }
}

public class PluginPathInitializer : MonoBehaviour
{
    [SerializeField]
    private List<PluginPathValue> m_customPluginPaths;

    private void Reset()
    {
#if UNITY_ANDROID
        var dllPath = Path.Combine(Path.GetDirectoryName(Application.dataPath), "lib/arm");
#else
        var dllPath = "%DATAPATH%" +
            Path.DirectorySeparatorChar + "auroraXR" +
            Path.DirectorySeparatorChar + "auroraXRLib" +
            Path.DirectorySeparatorChar + "Deps" +
            Path.DirectorySeparatorChar + "Plugins";
#endif

        m_customPluginPaths.Add(new PluginPathValue(PlatformModes.PLATFORM_MODE_PLAYER, dllPath));
        m_customPluginPaths.Add(new PluginPathValue(PlatformModes.PLATFORM_MODE_UNITY_EDITOR_64, dllPath));
        m_customPluginPaths.Add(new PluginPathValue(PlatformModes.PLATFORM_MODE_UNITY_EDITOR_32, dllPath));
        m_customPluginPaths.Add(new PluginPathValue(PlatformModes.PLATOFRM_MODE_UNITY_ANDROID, dllPath));
    }

    private void Awake()
    {
        SetupPluginPath();
    }

    public void SetupPluginPath()
    {

#if UNITY_ANDROID
        var dllPath = Path.Combine(Path.GetDirectoryName(Application.dataPath), "lib/arm");
        AddPluginPath(dllPath);
#elif UNITY_EDITOR_32
        Debug.Log("32 Version?");
        var dllPath = "%DATAPATH%" +
                Path.DirectorySeparatorChar + "auroraXR" +
                Path.DirectorySeparatorChar + "auroraXRLib" +
                Path.DirectorySeparatorChar + "Deps" +
                Path.DirectorySeparatorChar + "Plugins";
            AddPluginPath(dllPath);

            foreach (var pluginPath in m_customPluginPaths)
            {
                if (pluginPath.PlatformMode == PlatformModes.PLATFORM_MODE_UNITY_EDITOR_32)
                {
                    AddPluginPath(pluginPath.Path);
                }
            }

#elif UNITY_EDITOR_64
        Debug.Log("64 Version?");
        var dllPath = "%DATAPATH%" +
            Path.DirectorySeparatorChar + "auroraXR" +
            Path.DirectorySeparatorChar + "auroraXRLib" +
            Path.DirectorySeparatorChar + "Deps" +
            Path.DirectorySeparatorChar + "Plugins";
        AddPluginPath(dllPath);

        foreach (var pluginPath in m_customPluginPaths)
        {
            if (pluginPath.PlatformMode == PlatformModes.PLATFORM_MODE_UNITY_EDITOR_64)
            {
                AddPluginPath(pluginPath.Path);
            }
        }
#else //Player
        Debug.Log("Player Version?");
        var dllPath = "%DATAPATH%" +
            Path.DirectorySeparatorChar + "Plugins" +
            Path.DirectorySeparatorChar + "x86_64";
        AddPluginPath(dllPath);

        foreach (var pluginPath in m_customPluginPaths)
        {
            if (pluginPath.PlatformMode == PlatformModes.PLATFORM_MODE_PLAYER)
            {
                AddPluginPath(pluginPath.Path);
            }
        }
#endif

        //AURORA.ClientAPI.AuroraZmqContext.AddSupplementalDllPath(dllPath);



        // We must load the underlying dependencies from Unity.  If the 3rd party dll tries to load the 
        // unmanaged library it will fail.
        //if (false)
        //{
        //    ZeroMQ.ZContext tempContext = ZeroMQ.ZContext.Create();
        //}

#if UNITY_EDITOR_32 || UNITY_EDITOR_64

#else
        //Debug.LogWarning("CURRENT PATH=" + currentPath);
        //throw new System.ApplicationException("CURRENT PATH=" + currentPath);
#endif

    }

    public void AddPluginPath(string dllPath)
    {
        {
            var currentPath = System.Environment.GetEnvironmentVariable("PATH",
                System.EnvironmentVariableTarget.Process);

            dllPath = dllPath.Replace('/', Path.DirectorySeparatorChar);

            dllPath = Path.GetFullPath(dllPath.Replace("%DATAPATH%", Application.dataPath));

            if (currentPath != null && currentPath.Contains(dllPath) == false)
            {
                System.Console.WriteLine("Adding to dll path: " + dllPath);

                System.Environment.SetEnvironmentVariable("PATH", dllPath +
                    Path.PathSeparator + currentPath, System.EnvironmentVariableTarget.Process);
                Debug.Log("Path: " + System.Environment.GetEnvironmentVariable("PATH"));
            }
            else
            {
                System.Console.WriteLine("Already in dll path: " + dllPath);
            }
#if UNITY_EDITOR_32 || UNITY_EDITOR_64

#else
        //Debug.LogWarning("CURRENT PATH=" + currentPath);
        //throw new System.ApplicationException("CURRENT PATH=" + currentPath);
#endif
        }
    }
}
