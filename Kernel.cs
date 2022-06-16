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
using NanOS.GUI.Graphics;
using NanOS.Commands;
using NanOS;
using Cosmos.HAL.Network;
using Cosmos.Core.IOGroup;

namespace NanOS
{
    public class Kernel : Sys.Kernel
    {
        public Mouse m = new Mouse();
        public static Graphics gui;
        public static int Width;
        public static int Height;
        public string osname = "NanOS";
        public string osversion = "1.0";
        public string kernelversion = "NanOS_kernel_1";
        public string boottype = "Live USB/CD";
        public string shellname = "nansh";
        public string username = "";
        public static Bitmap settingsico;
        public static Bitmap otherappsico;
        public static Bitmap wallpaper;
        public static Bitmap poweroffimg;
        public static Bitmap consoleico;
        public static Bitmap pcinfoico;
        public static Bitmap appicon;
        public Canvas canvas;
        byte year = Cosmos.HAL.RTC.Year;
        byte month = Cosmos.HAL.RTC.Month;
        byte day = Cosmos.HAL.RTC.DayOfTheMonth;
        byte hour = Cosmos.HAL.RTC.Hour;
        byte Minutes = Cosmos.HAL.RTC.Minute;
        Sys.FileSystem.CosmosVFS fs;
        string current_directory = @"0:\";
        //bmp картинки
        [ManifestResourceStream(ResourceName = "NanOS.Wallpapers.wallpaper1.bmp")]
        static byte[] wallpaperbyte;
        [ManifestResourceStream(ResourceName = "NanOS.resources.poweroffAlpha.bmp")]
        static byte[] powerofficon;
        [ManifestResourceStream(ResourceName = "NanOS.resources.console.bmp")]
        static byte[] consoleappicon;
        [ManifestResourceStream(ResourceName = "NanOS.resources.pcinfoicon.bmp")]
        static byte[] pcinfobye;
        [ManifestResourceStream(ResourceName = "NanOS.resources.appicon.bmp")]
        static byte[] appiconbyte;
        [ManifestResourceStream(ResourceName = "NanOS.resources.settingsico.bmp")]
        static byte[] settingsicobyte;
        [ManifestResourceStream(ResourceName = "NanOS.resources.otherapps.bmp")]
        static byte[] otherappsbyte;
        protected override void BeforeRun()
        {
            Console.Clear();
            fs = new Sys.FileSystem.CosmosVFS();
            Sys.FileSystem.VFS.VFSManager.RegisterVFS(fs);
            Console.WriteLine("LOADING NanOS_kernel_1");
            Console.WriteLine("[ NanOS.nansh ] Creating System directory ");
            Console.WriteLine("[ NanOS.nansh ] Creating Users directory ");
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
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("NanOS");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("@{0}", username);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write(" {0}>>> ", current_directory);
            Console.ForegroundColor = ConsoleColor.White;
            Commands();            
        }
        public void Commands()
        {
            var input = Console.ReadLine();
            switch (input)
            {
                case "whoami":
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("I am {0}",username);
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case "date":
                    year = Cosmos.HAL.RTC.Year;
                    month = Cosmos.HAL.RTC.Month;
                    day = Cosmos.HAL.RTC.DayOfTheMonth;
                    hour = Cosmos.HAL.RTC.Hour;
                    Minutes = Cosmos.HAL.RTC.Minute;
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("");
                    Console.Write(day);
                    Console.Write("." + month);
                    Console.Write("." + year);
                    Console.WriteLine("");
                    Console.Write(hour);
                    Console.Write(":" + Minutes);
                    Console.WriteLine("");
                    Console.ForegroundColor = ConsoleColor.White;
                    //Сюда надо как-то время запихать чтобы отображалсь часы и минуты в видео текста
                    break;
                case "cow":
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(@"\|/          (__)    
     `\------(oo)
       ||    (__)
       ||w--||     \|/
   \|/");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case "sysinfo":
                    string filesystemtype = fs.GetFileSystemType(@"0:\");            
                    //Доступно ОЗУ
                    ulong avialible_ram = Cosmos.Core.GCImplementation.GetAvailableRAM();
                    //Использованно ОЗУ
                    uint used_ram =  Cosmos.Core.GCImplementation.GetUsedRAM();
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
                    Console.WriteLine("CPU Vendor Name: " + CPU_vendorname);
                    Console.WriteLine("Amount of RAM: " + amount_of_ram + " MB");
                    Console.WriteLine("Avialible RAM: " + avialible_ram + " MB");
                    Console.WriteLine("Used RAM: " + used_ram + " B");
                    Console.WriteLine("File System: "+ filesystemtype);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    break;

                case "help":
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("=============================");
                    Console.WriteLine("   :::::Command list:::::");
                    Console.WriteLine("     :::::Page 1:::::");
                    Console.WriteLine("=============================");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("restart - restart pc\nshutdown - Kills all processes and prepares your PC for shutdown" +
                        "\nhelp - Shows a list of commands\nclear - Clears all text from the screen\nsysinfo - Shows system information\n" +
                        "kernel - shows info about the kernel\nbeep - Tests your PC Speaker\nchngeuname - Changes your username" +
                        "\ngfx on - Enables graphics mode" +
                        "\ngfx off - Disables graphics mode\ndiskinfo - Shows disk information\nmkdir - Creates a directory\n" +
                        "mkfile - Creates a file\ncd - Change Directory\ndeldir - Delete a directory\ndelfile - Delete a file" +
                        "\nhelp2 - Shows the second page of commands");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case "help2":
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("=============================");
                    Console.WriteLine("   :::::Command list:::::");
                    Console.WriteLine("     :::::Page 2:::::");
                    Console.WriteLine("=============================");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("whoami - Shows your user name\ndate - Shows the current date" +
                        "\nwritefile - Writes text to your file\nreadfile - Reads text from the selected file" +
                        "\ncopyfile - Copies a file to the selected path\ncow - Draws a cow");
                    break;
                case "gfx on":
                    Console.Clear();
                    cpubrand = Cosmos.Core.CPU.GetCPUBrandString();
                    Point p1 = new Point();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("[ NanOS.nansh ] Loading the basic VideoDriver");
                    Console.WriteLine("[ NanOS.nansh ] Desktop loading");
                    wallpaper = new Bitmap(wallpaperbyte);
                    Console.WriteLine("[ NanOS.nansh ] Loading GUI Elements...");
                    otherappsico = new Bitmap(otherappsbyte);
                    settingsico = new Bitmap(settingsicobyte);
                    appicon = new Bitmap(appiconbyte);
                    pcinfoico = new Bitmap(pcinfobye);
                    poweroffimg = new Bitmap(powerofficon);
                    consoleico = new Bitmap(consoleappicon);
                    Pen penmouse = new Pen(Color.Red);
                    Console.WriteLine("[ NanOS.nansh ] Loaded!");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("                     WARNING! Graphics mode is in testing!" +
                        "\n                  If you want to turn it off, just type gfx off!" +
                        " \n                     Please write the width and height of the screen.");
                    Console.WriteLine("");
                    Console.Write("Width: ");
                    Width = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("");
                    Console.Write("Height: ");
                    Height = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("");
                    Console.WriteLine("This is right?\n" + Width + "x" + Height);
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.ReadKey();
                    Sys.PCSpeaker.Beep();
                    canvas = FullScreenCanvas.GetFullScreenCanvas(new Mode(Width, Height, ColorDepth.ColorDepth32));
                    Pen pen = new Pen(Color.Red);
                    canvas.Clear(Color.FromArgb(41, 51, 64));
                    canvas.DrawImage(wallpaper, 0, 0);
                    pen.Color = Color.FromArgb(7, 71, 94);
                    //Верхняя белая линия
                    canvas.DrawFilledRectangle(pen, 0, 0, 1920, 40);
                    pen.Color = Color.FromArgb(23, 53, 89);
                    //Нижняя линия для приложений
                    canvas.DrawFilledRectangle(pen, 740, 1048, 450, 200);
                    //Картинка выключения
                    canvas.DrawImageAlpha(poweroffimg, 1880, 8);
                    //Картинки приложений снизу
                    canvas.DrawImage(consoleico, 758, 1020);
                    canvas.DrawImage(appicon, 830, 1020);
                    canvas.DrawImageAlpha(pcinfoico, 902, 1020);
                    canvas.DrawImageAlpha(settingsico, 974, 1020);
                    canvas.DrawImageAlpha(otherappsico, 1120, 1020);
                    pen.Color = Color.White;
                    //Дата сверху
                    p1.X = 935;
                    p1.Y = 8;
                    canvas.DrawString(""+day+"."+month+"."+year, Cosmos.System.Graphics.Fonts.PCScreenFont.Default, pen, p1);
                    p1.X = 943;
                    p1.Y = 20;
                    //Часы сверху
                    canvas.DrawString("" + hour + ":" + Minutes, Cosmos.System.Graphics.Fonts.PCScreenFont.Default, pen, p1);
                    p1.X = 38;
                    p1.Y = 13;
                    //Имя пользователя
                    canvas.DrawString("User: " + username, Cosmos.System.Graphics.Fonts.PCScreenFont.Default, pen, p1);
                     PCinfoAPP();
                    canvas.Display();
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
                case "dir":
                    var directory_list = Sys.FileSystem.VFS.VFSManager.GetDirectoryListing(current_directory);
                    foreach (var directoryEntry in directory_list)
                    {
                        try
                        {
                            var entry_type = directoryEntry.mEntryType;
                            if (entry_type == Sys.FileSystem.Listing.DirectoryEntryTypeEnum.File)
                            {
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                Console.WriteLine(directoryEntry.mName + "                         <FILE>");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            if (entry_type == Sys.FileSystem.Listing.DirectoryEntryTypeEnum.Directory)
                            {
                                Console.ForegroundColor = ConsoleColor.Blue;
                                Console.WriteLine(directoryEntry.mName + "                         <DIRECTORY>");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Error: Directory not found");
                            Console.WriteLine(e.ToString());
                        }
                        
                    }

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
                    //Удаление файла
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
                case "writefile":
                    //Запись текста в файл
                    Console.WriteLine("Welcome to NanOS writestr app!");
                    Console.WriteLine("Please enter file name!");
                    filename = Console.ReadLine();
                    if (File.Exists(current_directory+filename))
                    {
                        Console.WriteLine("Write text");
                        var StringTXT = Console.ReadLine();
                        try
                        {
                            File.WriteAllText(current_directory + filename, StringTXT);
                            Console.WriteLine("Text writed succeful!");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Error!");
                            Console.WriteLine(e.ToString());
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error: NanOS.File.Not.Found");
                        break;
                    }
                    break;
                case "readfile":
                    //Чтение из файла
                    Console.WriteLine("Please enter file name!");
                    filename = Console.ReadLine();
                    try
                    {
                        Console.WriteLine("---------------------------------------");
                        Console.WriteLine("   "+filename);
                        Console.WriteLine("---------------------------------------");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(File.ReadAllText(current_directory + filename));
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("---------------------------------------");
                    }
                    catch (Exception e)
                    {
                        Console.Write("Error: ");
                        Console.Write(e.ToString());
                        Console.WriteLine("");
                    }
                    break;
                case "copyfile":
                    string dirtocopy = @"0:\";
                    Console.WriteLine("Enter the directory where you want to copy the file");
                    Console.Write(@"0:\");
                    dirtocopy = @"0:\" + Console.ReadLine();
                    if (Directory.Exists(dirtocopy))
                    {
                        Console.WriteLine("Please enter file name");
                        filename = Console.ReadLine();
                        File.Copy(current_directory + filename, dirtocopy);
                        Console.WriteLine("File {0} copied to {1}", filename, dirtocopy);
                    }
                    else
                    {
                        Console.WriteLine("Error: NanOS.Directory.Not.Found");
                    }
                    break;
                case "renamefile":
                    string newfilename = "";
                    Console.WriteLine("Enter file name");
                    filename = Console.ReadLine();
                    Console.WriteLine("Write new name for {0}", filename);
                    newfilename = Console.ReadLine();
                    try
                    {
                        File.Copy(current_directory + filename,current_directory + newfilename);
                        Console.WriteLine("File {0} renamed!", filename);
                        Sys.FileSystem.VFS.VFSManager.DeleteFile(current_directory + filename);
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                    break;
                case "diskinfo":
                    fs.GetDisks();
                    //Получить тип файловой системы
                    long available_space = Sys.FileSystem.VFS.VFSManager.GetAvailableFreeSpace(@"0:\");
                    filesystemtype = fs.GetFileSystemType(@"0:\");
                    //Получить размер диска
                    long total_size = fs.GetTotalSize(@"0:\");
                    //Свободное место
                     available_space = Sys.FileSystem.VFS.VFSManager.GetAvailableFreeSpace(@"0:\");
                    Console.WriteLine("Available Free Space: " + available_space + " B");
                    Console.WriteLine("Total size: " + total_size + " B");
                    Console.WriteLine("File System type: " + filesystemtype);
                    break;

                case "cd":
                    //Смена директорий
                    Console.WriteLine(@"Enter the path (example: 0:\Apps\)");
                    Console.Write(@"0:\");
                    try
                    {
                        current_directory = @"0:\" + Console.ReadLine();
                        if (Directory.Exists(current_directory))
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Directory changed!");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            current_directory = @"0:\";
                            Console.WriteLine("Error: NanOS.Directory.Not.Found");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                    break;
                case "":

                    break;
                case " ":
                    Console.WriteLine("");
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
                

                case "catall":
                    var directory_list1 = Sys.FileSystem.VFS.VFSManager.GetDirectoryListing(current_directory);
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
        }
        public void PCinfoAPP()
        {
            //Доступно ОЗУ
            ulong avialible_ram = Cosmos.Core.GCImplementation.GetAvailableRAM();
            //Использованно ОЗУ
            uint used_ram = Cosmos.Core.GCImplementation.GetUsedRAM();
            //Получить vendorname (сам хз че это, но пусть будет)
            string CPU_vendorname = Cosmos.Core.CPU.GetCPUVendorName();
            // Оперативка
            uint amount_of_ram = Cosmos.Core.CPU.GetAmountOfRAM();
            // Название процессора
            string cpubrand = Cosmos.Core.CPU.GetCPUBrandString();
            Point pointPCinfo = new Point();
            Pen pcinfopen = new Pen(Color.White);
            pointPCinfo.X = 105;
            pointPCinfo.Y = 467;
            canvas.DrawFilledRectangle(pcinfopen, pointPCinfo, 400, 200);
            pcinfopen = new Pen(Color.Black);
            canvas.DrawString("Processor: " + cpubrand,Cosmos.System.Graphics.Fonts.PCScreenFont.Default, pcinfopen, pointPCinfo);
            pointPCinfo.X = 105;
            pointPCinfo.Y = 487;
            canvas.DrawString("CPU Vendor: " + CPU_vendorname, Cosmos.System.Graphics.Fonts.PCScreenFont.Default, pcinfopen, pointPCinfo);
            pointPCinfo.X = 105;
            pointPCinfo.Y = 507;
            canvas.DrawString("RAM : " + amount_of_ram + "MB", Cosmos.System.Graphics.Fonts.PCScreenFont.Default, pcinfopen, pointPCinfo);
            pointPCinfo.X = 105;
            pointPCinfo.Y = 527;
            canvas.DrawString("Avialible RAM : " + avialible_ram + "MB", Cosmos.System.Graphics.Fonts.PCScreenFont.Default, pcinfopen, pointPCinfo);
            pointPCinfo.X = 105;
            pointPCinfo.Y = 547;
            canvas.DrawString("Used RAM : " + used_ram + "B", Cosmos.System.Graphics.Fonts.PCScreenFont.Default, pcinfopen, pointPCinfo);
            canvas.Display();
        }

    }
    
}