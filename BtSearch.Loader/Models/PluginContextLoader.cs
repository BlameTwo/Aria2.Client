﻿using System.Reflection;
using System.Runtime.Loader;

namespace BtSearch.Loader.Models;

public sealed class PluginContextLoader : AssemblyLoadContext
{
    private AssemblyDependencyResolver _resolver;

    public PluginContextLoader(string LoaderPath) : base(true)
    {
        _resolver = new AssemblyDependencyResolver(LoaderPath);
    }

    protected override Assembly Load(AssemblyName assemblyName)
    {
        string assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
        if (assemblyPath != null)
        {
            return LoadFromAssemblyPath(assemblyPath);
        }

        return null;
    }



    protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
    {
        string libraryPath = _resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
        if (libraryPath != null)
        {
            return LoadUnmanagedDllFromPath(libraryPath);
        }

        return IntPtr.Zero;
    }
}