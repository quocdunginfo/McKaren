using System;
using System.IO;
using System.Reflection;
using System.Web.Compilation;
using System.Web.Hosting;
namespace McKaren.Helper
{
    public class Dllhelper
    {
        public static void LoadAllFromAreaBin()
        {
            var loadedAsm = AppDomain.CurrentDomain.GetAssemblies();
            var areaPath = HostingEnvironment.MapPath("~/Areas");

            foreach (var module in new DirectoryInfo(areaPath).GetDirectories())
            {
                var rPath = string.Format("~/Areas/{0}/bin", module.Name);
                AppDomain.CurrentDomain.AppendPrivatePath(HostingEnvironment.MapPath(rPath));
                foreach (var dll in new DirectoryInfo(HostingEnvironment.MapPath(rPath)).GetFiles("*.dll"))
                {
                    Assembly assembly = Assembly.LoadFile(dll.FullName);
                    if (assembly == null)
                    {
                        throw new FileNotFoundException(module.Name + " DLL not found");
                    }
                    bool loaded = false;
                    foreach (var item in loadedAsm)
                    {
                        if (item.FullName.Equals(assembly.FullName))
                        {
                            loaded = true;
                            break;
                        }
                    }
                    if (loaded)
                    {
                        continue;
                    }

                    BuildManager.AddReferencedAssembly(assembly);
                }
            }
        }
    }
}