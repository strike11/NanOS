using System;
using System.Text;
using System.IO;
using Sys = Cosmos.System;
using Cosmos.System.Graphics;
using Cosmos.HAL;
using Cosmos.System.FileSystem.VFS;
using Cosmos.System.FileSystem;
using System.Drawing;
using IL2CPU.API.Attribs;
using NanOS.GUI.Graphics;
using NanOS.Commands;
using Cosmos.HAL.Network;
using Cosmos.Core.IOGroup;

namespace NanOS
{
    public class Errors
    {
        public void ErrorFileNotFound()
        {
            Console.WriteLine("Error: NanOS.File.Not.Found");
        }
        public void ErrorDirectoryNotFound()
        {
            Console.WriteLine("Error: NanOS.Directory.Not.Found");
        }
    }
}
