using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DispApp
{
    public static class Perm
    {
        public static string GetMemory(string name)
        {
            using (var pr = new PerformanceCounter("Process", "Working Set - Private", name))
            {
                return (pr.NextValue() / 1024).ToString("#,##0 KB");
            }
        }

    }
}
