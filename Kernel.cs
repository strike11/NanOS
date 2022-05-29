using System;
using System.Text;
using System.IO;
using Sys = Cosmos.System;
using Cosmos.System.Graphics;
using Point = Cosmos.System.Graphics.Point;
using Cosmos.HAL;
using Cosmos.System.FileSystem.VFS;
using Cosmos.System.FileSystem;
using System.Drawing;
using IL2CPU.API.Attribs;
namespace NanOS
{
    public class Kernel : Sys.Kernel
    { 
        public Point point;
        public Pen pen;
        public static uint screenWidth = 1920;
        public static uint screenHeight = 1080; 
        public string osname = "NanOS";
        public string osversion = "1.0";
        public string kernelversion = "NanOS_kernel_1";
        public string boottype = "Live USB/CD";
        public string shellname = "nansh";
        public string username = "";
        public static Bitmap wallpaper;
        Canvas canvas;
        Sys.FileSystem.CosmosVFS fs;
        string current_directory = @"0:\";
        [ManifestResourceStream(ResourceName = "Kernel.wallpaper.bmp")]
        static byte[] file;
        protected override void BeforeRun()
        {
            fs = new Sys.FileSystem.CosmosVFS();
            Sys.FileSystem.VFS.VFSManager.RegisterVFS(fs);
            fs.CreateDirectory(@"0:\System");
            fs.CreateDirectory(@"0:\System\Users");
            fs.CreateFile(@"0:\System\Users\Users.dax");
            Console.WriteLine("LOADING NanOS_kernel_1");
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(@"                    NN   NN   AAA   NN   NN  OOOOO   SSSSS  
                    NNN  NN  AAAAA  NNN  NN OO   OO SS      
                    NN N NN AA   AA NN N NN OO   OO  SSSSS  
                    NN  NNN AAAAAAA NN  NNN OO   OO      SS 
                    NN   NN AA   AA NN   NN  OOOO0   SSSSS  ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("             NanOS successfully loaded! Please write your username");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Username: ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            username = Console.ReadLine();
            fs.GetDirectory(@"0:\System\Users\");
            fs.GetFile(@"0:\System\Users\Users.dax").GetFileStream();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("             Welcome to NanOS {0}. Press any key to get started!", username);
            Console.ReadKey();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(@"               ::::    :::     :::     ::::    :::  ::::::::   :::::::: 
              :+:+:   :+:   :+: :+:   :+:+:   :+: :+:    :+: :+:    :+: 
             :+:+:+  +:+  +:+   +:+  :+:+:+  +:+ +:+    +:+ +:+         
            +#+ +:+ +#+ +#++:++#++: +#+ +:+ +#+ +#+    +:+ +#++:++#++   
           +#+  +#+#+# +#+     +#+ +#+  +#+#+# +#+    +#+        +#+    
          #+#   #+#+# #+#     #+# #+#   #+#+# #+#    #+# #+#    #+#     
         ###    #### ###     ### ###    ####  ########   ########       ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("                        ------------------------------");
            Console.WriteLine("                        Type help to show command list");
            Console.WriteLine("                        ------------------------------");
            Console.ForegroundColor = ConsoleColor.White;
            
        }
        protected override void Run()
        {
            // canvas.DrawImage(wallpaper, 0,0);
            
            #region os
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("NanOS");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("@{0}", username);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write(" {0}>>> ", current_directory);
            Console.ForegroundColor = ConsoleColor.White;
            var input = Console.ReadLine();
            switch (input)
            {
                case "help":
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("=============================");
                    Console.WriteLine("   :::::Command list:::::");
                    Console.WriteLine("=============================");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("restart - restart pc\nshutdown - Kills all processes and prepares your PC for shutdown" +
                        "\nhelp - Shows a list of commands\nclear - Clears all text from the screen\nsysinfo - Shows system information\n" +
                        "kernel - shows info about the kernel\nbeep - Tests your PC Speaker\nchngeuname - Changes your username" +
                        "\ngfx on - Enables graphics mode" +
                        "\ngfx off - Disables graphics mode\ndiskinfo - Shows disk information\nmkdir - Creates a directory\n" +
                        "mkfile - Creates a file\ncd - Change Directory\ndeldir - Delete a directory\ndelfile - Delete a file");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;

                case "gfx on":

                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("                     WARNING! Graphics mode is in testing!" +
                        "\n                If you want to turn it off, just type gfx off!" +
                        " \n                         Press any key to continue.");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.ReadKey();
                    Sys.PCSpeaker.Beep();
                    canvas = FullScreenCanvas.GetFullScreenCanvas(new Mode(1024, 768, ColorDepth.ColorDepth32));
                    canvas.Clear(Color.FromArgb(41, 51, 64));
                    canvas.Display();
                    // canvas.DrawImage(wallpaper, new Point (320, 100));
                    /*  background.FromFile(@"0:\Wallpaper.bmp");
                      VGADriverII.Initialize(VGAMode.Pixel320x200DB);
                      VGAGraphics.DrawFilledRect(60, 50, 400, 20, VGAColor.Black12);
                      VGAGraphics.Clear(VGAColor.Cyan9);
                      VGAGraphics.DrawImage(0, 0, background);
                      VGAGraphics.DrawFilledRect(0, 190, 400, 10, VGAColor.Cyan11);
                      VGAGraphics.DrawFilledRect(0, 190, 46, 15, VGAColor.Cyan2);
                      VGAGraphics.DrawString(0, 192, "Start", VGAColor.White, VGAFont.Font8x8);
                      VGAGraphics.DrawString(139, 3, "NanOS", VGAColor.White, VGAFont.Font8x16);
                      VGAGraphics.DrawFilledRect(40, 30, 100, 100, VGAColor.White);
                      VGAGraphics.DrawString(56, 33, "DiskInfo", VGAColor.Black, VGAFont.Font8x16);
                      VGAGraphics.Display();
                     */
                    break;

                case "gfx off":
                    Console.Clear();
                    canvas.Disable();
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("                 Graphics mode is off. To enable write gfx.");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;

                case "beep":
                    Sys.PCSpeaker.Beep();
                    break;
                case "chngeuname":
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("Write your new UserName");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    username = Console.ReadLine();
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Your username changed succeful! Hello {0}", username);
                    Console.ForegroundColor = ConsoleColor.White;
                    break;

                case "mkdir":
                    Console.WriteLine(@"Enter the path or directory name (example: 0:\NanOSdirectory\MyDirectory)");
                    Console.WriteLine(@"If you want to create a directory in the directory you are currently in, then press Enter)");
                    var path_dir = Console.ReadLine();
                    Console.WriteLine("Enter Directory name");
                    var dirname = Console.ReadLine();
                    //Если просто нажали Enter
                    if (path_dir == "")
                    {
                        fs.CreateDirectory(current_directory + dirname);
                        Console.WriteLine("Directory {0} created in {1}", dirname, current_directory);
                    }
                    else
                    {
                        //Если ввели путь
                        fs.CreateDirectory(path_dir + dirname);
                        Console.WriteLine("Directory {0} created in {1}", dirname, path_dir);
                    }
                    break;
                case "mkfile":
                    Console.WriteLine(@"Enter the path to the file (example: 0:\NanOSfiles\family.txt)");
                    Console.WriteLine("If you want to create a file in the directory you are currently in, then press Enter");
                    var path_file = Console.ReadLine();
                    Console.WriteLine("Enter file name");
                    var filename = Console.ReadLine();
                    //Если просто нажали Enter
                    if (path_file == "")
                    {
                        fs.CreateFile(current_directory + filename);
                        Console.WriteLine("File {0} created in {1}", filename, current_directory);
                    }
                    else
                    {
                        //Если просто ввели путь
                        fs.CreateFile(path_file + filename);
                        Console.WriteLine("File {0} created in {1}", filename, path_file);
                    }
                    break;
                case "deldir":
                    Console.WriteLine(@"Enter the path to the directory (example: 0:\NanOSfiles\)");
                    Console.WriteLine("If you want delete a directory in the directory you are currently in, then press Enter");
                    path_dir = Console.ReadLine();
                    Console.WriteLine("Enter directory name");
                    dirname = Console.ReadLine();
                    if (path_dir == @"0:\System\")
                    {
                        Console.WriteLine("This is system directory! You cannot delete it!");
                        break;
                    }
                    if (dirname == @"System")
                    {
                        Console.WriteLine("This is system directory! You cannot delete it!");
                        break;
                    }
                    if (path_dir == "")
                    {
                        Sys.FileSystem.VFS.VFSManager.DeleteDirectory(current_directory + dirname, true);
                        Console.WriteLine("Directory {0} deleted in {1}", dirname, current_directory);
                    }
                    else
                    {
                        //Если просто ввели путь
                        Sys.FileSystem.VFS.VFSManager.DeleteDirectory(path_dir + dirname, true);
                        Console.WriteLine("Directory {0} deleted in {1}", dirname, path_dir);
                    }

                    break;

                case "delfile":
                    Console.WriteLine(@"Enter the path to the file (example: 0:\NanOSfiles\family.txt)");
                    Console.WriteLine("If you want delete a file in the directory you are currently in, then press Enter");
                    path_file = Console.ReadLine();
                    Console.WriteLine("Enter file name");
                    filename = Console.ReadLine();
                    if (path_file == "")
                    {
                        Sys.FileSystem.VFS.VFSManager.DeleteFile(current_directory + filename);
                        Console.WriteLine("File {0} deleted in {1}", filename, current_directory);
                    }
                    else
                    {
                        //Если просто ввели путь
                        Sys.FileSystem.VFS.VFSManager.DeleteFile(path_file + filename);
                        Console.WriteLine("File {0} deleted in {1}", filename, path_file);
                    }
                    break;
                case "diskinfo":
                    fs.GetDisks();
                    //Получить тип файловой системы
                    string filesystemtype = fs.GetFileSystemType(@"0:\");
                    //Получить размер диска
                    long total_size = fs.GetTotalSize(@"0:\");
                    //Свободное место
                    long available_space = Sys.FileSystem.VFS.VFSManager.GetAvailableFreeSpace(@"0:\");
                    Console.WriteLine("Available Free Space: " + available_space + " MB");
                    Console.WriteLine("Total size: " + total_size + " MB");
                    Console.WriteLine("File System type: " + filesystemtype);
                    break;

                case "cd":
                    //Тут думаю всё понятно
                    Console.WriteLine(@"Enter the path (example: 0:\Apps\)");
                    current_directory = Console.ReadLine();
                    fs.GetDirectory(current_directory);
                    break;
                case "":

                    break;
                case " ":
                    Console.WriteLine("");
                    break;
                case "sysinfo":
                    //Получить vendorname (сам хз че это, но пусть будет)
                    string CPU_vendorname = Cosmos.Core.CPU.GetCPUVendorName();
                    // Оперативка
                    uint amount_of_ram = Cosmos.Core.CPU.GetAmountOfRAM();
                    // Название процессора
                    string cpubrand = Cosmos.Core.CPU.GetCPUBrandString();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("OS NAME: " + osname);
                    Console.WriteLine("OS VERSION: " + osversion);
                    Console.WriteLine("KERNEL VERSION: " + kernelversion);
                    Console.WriteLine("BOOT TYPE: " + boottype);
                    Console.WriteLine("SHELL: " + shellname);
                    Console.WriteLine("CURRENT USER: " + username);
                    Console.WriteLine("CPU: " + cpubrand);
                    Console.WriteLine("Amount of RAM: " + amount_of_ram + " MB");
                    Console.WriteLine("CPU Vendor Name: " + CPU_vendorname);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                default:
                    Console.WriteLine(input + ": Command Not Found");
                    break;
                case "restart":
                    Cosmos.System.Power.Reboot();
                    break;
                case "shutdown":
                    Cosmos.System.Power.Shutdown();
                    Console.WriteLine("Now you can power-off your PC!");
                    break;
                case "clear":
                    Console.Clear();
                    break;
                case "kernel":
                    Console.WriteLine("kernel -i Shows the kernel version\nkernel -a Shows all information about the kernel");
                    break;
                case "kernel -i":
                    Console.WriteLine("NanOS_kernel = v1");
                    break;
                case "kernel -a":
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("NanOS_kernel_1. Core created May 4, 2022\nKernel.NanOS");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case "dir":
                    var directory_list = Sys.FileSystem.VFS.VFSManager.GetDirectoryListing(current_directory);
                    foreach (var directoryEntry in directory_list)
                    {
                        Console.WriteLine(directoryEntry.mName);
                    }
                    break;

                case "catall":
                    var directory_list1 = Sys.FileSystem.VFS.VFSManager.GetDirectoryListing(@"0:\");
                    try
                    {
                        foreach (var directoryEntry in directory_list1)
                        {
                            var file_stream = directoryEntry.GetFileStream();
                            var entry_type = directoryEntry.mEntryType;
                            if (entry_type == Sys.FileSystem.Listing.DirectoryEntryTypeEnum.File)
                            {
                                byte[] content = new byte[file_stream.Length];
                                file_stream.Read(content, 0, (int)file_stream.Length);
                                Console.WriteLine("File name: " + directoryEntry.mName);
                                Console.WriteLine("File size: " + directoryEntry.mSize);
                                Console.WriteLine("Content: ");
                                foreach (char ch in content)
                                {
                                    Console.Write(ch.ToString());
                                }
                                Console.WriteLine();
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                    break;
            }

            #endregion
        }
    }
}