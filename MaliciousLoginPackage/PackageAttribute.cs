using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaliciousLoginPackage
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class PackageAttribute : Attribute
    {
        public string Name { get; }

        public string Version { get; }

        public PackageAttribute(string name, string version)
        {
            Name = name;
            Version = version;
        }
    }
}
