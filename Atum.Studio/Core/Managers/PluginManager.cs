using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Atum.Studio.Core.Managers
{
    public class PluginManager
    {
        internal static List<Plugins.IPlugin> LoadedPlugins = new List<Plugins.IPlugin>();

        internal static void Start()
        {
            //load default plugins
            LoadedPlugins.Add(new Core.Plugins.PostSlicer.AdvancedPostSlicer());
          //  LoadedPlugins.Add(new Core.Plugins.PostSlicer.WarpCorrectionPostSlicer());

            //load addition plugins
            var modulesBasePath = DAL.ApplicationSettings.Settings.ModulesPath;
            if (System.IO.Directory.Exists(modulesBasePath))
            {
                foreach (var modulePath in System.IO.Directory.GetDirectories(modulesBasePath))
                {
                    var sortedModulesPath = new SortedList<string, string>();
                    foreach (var moduleDllPath in System.IO.Directory.GetFiles(modulePath, "*.dll"))
                    {
                        sortedModulesPath.Add(moduleDllPath, moduleDllPath);
                    }

                    foreach (var moduleDll in sortedModulesPath.Keys)
                    {
                        //load modules using reflection
                        try
                        {
                            // your code

                            Assembly assembly = Assembly.LoadFile(moduleDll);
                            try
                            {
                                Type[] types = assembly.GetTypes();
                                Assembly core = null;
                                foreach (var assemblyType in AppDomain.CurrentDomain.GetAssemblies())
                                {
                                    if (assemblyType.GetName().Name.Equals("Atum.Studio"))
                                    {
                                        core = assemblyType;
                                    }
                                }

                                Type moduleInfoType = core.GetType("Atum.Studio.Core.Plugins.IPlugin");
                                foreach (var t in types)
                                    if (moduleInfoType.IsAssignableFrom((Type)t))
                                    {
                                        moduleInfoType = t;
                                        break;
                                    }
                                Object o = Activator.CreateInstance(moduleInfoType);
                                Core.Plugins.IPlugin loadedModuleInstance = (Core.Plugins.IPlugin)o;

                                //add to modules list
                                LoadedPlugins.Add(loadedModuleInstance);
                                // Debug.WriteLine(moduleInstance.ToString());
                            }
                            catch (ReflectionTypeLoadException ex)
                            {
                                // now look at ex.LoaderExceptions - this is an Exception[], so:
                                foreach (Exception inner in ex.LoaderExceptions)
                                {
                                    // write details of "inner", in particular inner.Message
                                }
                            }
                        }
                        catch (Exception exc)
                        {
                            Debug.WriteLine(exc.Message);
                        }
                    }
                }
            }
        }

        public static void LoadAssemblyFromDll(string assemblyPath)
        {
            //load modules using reflection
            Assembly assembly = Assembly.LoadFile(assemblyPath);
            Type[] types = assembly.GetTypes();
            Assembly core = null;
            foreach (var assemblyType in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assemblyType.GetName().Name.Equals("Atum.Studio"))
                {
                    core = assemblyType;
                }
            }
            Type moduleInfoType = core.GetType("Atum.Studio.Core.Plugins.IPlugin");
            foreach (var t in types)
                if (moduleInfoType.IsAssignableFrom((Type)t))
                {
                    moduleInfoType = t;
                    break;
                }
            Object o = Activator.CreateInstance(moduleInfoType);
            Core.Plugins.IPlugin loadedModuleInstance = (Core.Plugins.IPlugin)o;

            //add to modules list
            LoadedPlugins.Add(loadedModuleInstance);
            // Debug.WriteLine(moduleInstance.ToString());
        }

        internal static Plugins.IPlugin LoadAssemblyFromByteArray(byte[] assemblyByteArray)
        {
            //load modules using reflection
            try
            {
                Assembly assembly = Assembly.Load(assemblyByteArray);
                Type[] types = assembly.GetTypes();
                Assembly core = null;
                foreach (var assemblyType in AppDomain.CurrentDomain.GetAssemblies())
                {
                    if (assemblyType.GetName().Name.Equals("Atum.Studio"))
                    {
                        core = assemblyType;
                    }
                }
                Type moduleInfoType = core.GetType("Atum.Studio.Core.Plugins.IPlugin");
                foreach (var t in types)
                    if (moduleInfoType.IsAssignableFrom((Type)t))
                    {
                        moduleInfoType = t;
                        break;
                    }
                Object o = Activator.CreateInstance(moduleInfoType);
                Core.Plugins.IPlugin loadedModuleInstance = (Core.Plugins.IPlugin)o;

                //add to modules list
                LoadedPlugins.Add(loadedModuleInstance);

                return loadedModuleInstance;
                // Debug.WriteLine(moduleInstance.ToString());
            }
            catch (ReflectionTypeLoadException ex)
            {
                // now look at ex.LoaderExceptions - this is an Exception[], so:
                foreach (Exception inner in ex.LoaderExceptions)
                {
                    // write details of "inner", in particular inner.Message
                }
            }

            return null;
        }


    }
}
