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
        public string osname = "NanOS";
        public string osversion = "1.0";
        public string kernelversion = "NanOS_kernel_1";
        public string boottype = "Live USB/CD";
        public string shellname = "nansh";
        public string username = "";
        byte year = Cosmos.HAL.RTC.Year;
        byte month = Cosmos.HAL.RTC.Month;
        byte day = Cosmos.HAL.RTC.DayOfTheMonth;
        byte hour = Cosmos.HAL.RTC.Hour;
        byte Minutes = Cosmos.HAL.RTC.Minute;
      //  Sys.FileSystem.CosmosVFS fs;
        string current_directory = @"0:\";
        protected override void BeforeRun()
        {
            ConsoleClear();
            //fs = new Sys.FileSystem.CosmosVFS();
           // Sys.FileSystem.VFS.VFSManager.RegisterVFS(fs);
            Console.WriteLine("LOADING NanOS_kernel_1");
            Console.WriteLine("[ NanOS.nansh ] Kernel Loaded! ");
            ConsoleClear();
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
            ConsoleClear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(@"
              $$\   $$\                      $$$$$$\   $$$$$$\  
              $$$\  $$ |                    $$  __$$\ $$  __$$\ 
              $$$$\ $$ | $$$$$$\  $$$$$$$\  $$ /  $$ |$$ /  \__|
              $$ $$\$$ | \____$$\ $$  __$$\ $$ |  $$ |\$$$$$$\  
              $$ \$$$$ | $$$$$$$ |$$ |  $$ |$$ |  $$ | \____$$\ 
              $$ |\$$$ |$$  __$$ |$$ |  $$ |$$ |  $$ |$$\   $$ |
              $$ | \$$ |\$$$$$$$ |$$ |  $$ | $$$$$$  |\$$$$$$  |
              \__|  \__| \_______|\__|  \__| \______/  \______/ 
                                                                                 ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("                        ------------------------------");
            Console.WriteLine("                        Type help to show command list");
            Console.WriteLine("                        ------------------------------");
            Console.ForegroundColor = ConsoleColor.White;
            //               
        }
        protected override void Run()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("NanOS");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("@{0}", username);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write(" >>> ");
            Console.ForegroundColor = ConsoleColor.White;
            Commands();
        }
        public void Commands()
        {
            var path_file = "";
            var path_dir = "";
            var input = Console.ReadLine();
            switch (input)
            {
                case "whoami":
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("I am {0}", username);
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
                    ulong cpu_UPTIME = Cosmos.Core.CPU.GetCPUUptime();
                    long CPU_cyclespeed = Cosmos.Core.CPU.GetCPUCycleSpeed();
                    //string filesystemtype = fs.GetFileSystemType(@"0:\");
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
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case "meminfo":
                    amount_of_ram = Cosmos.Core.CPU.GetAmountOfRAM();
                    avialible_ram = Cosmos.Core.GCImplementation.GetAvailableRAM();
                    used_ram = Cosmos.Core.GCImplementation.GetUsedRAM();
                    Console.WriteLine("Amount of RAM: " + amount_of_ram + " MB");
                    Console.WriteLine("Avialible RAM: " + avialible_ram + " MB");
                    Console.WriteLine("Used RAM: " + used_ram + " B");
                    break;
                case "cpuinfo":
                    cpubrand = Cosmos.Core.CPU.GetCPUBrandString();
                    CPU_vendorname = Cosmos.Core.CPU.GetCPUVendorName();
                    Console.WriteLine("CPU: " + cpubrand);
                    Console.WriteLine("CPU Vendor Name: " + CPU_vendorname);
                    break;
                case "help":
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("=============================");
                    Console.WriteLine("   :::::Command list:::::");
                    Console.WriteLine("     :::::Page 1:::::");
                    Console.WriteLine("=============================");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("reboot - restart pc\nshutdown - Kills all processes and prepares your PC for shutdown" +
                        "\nhelp - Shows a list of commands\nclear - Clears all text from the screen\nsysinfo - Shows system information\n" +
                        "kernel - Shows info about the kernel\nbeep - Tests your PC Speaker\nchngeuname - Changes your username" +
                        "diskinfo - Shows disk information\nmkdir - Creates a directory\n" +
                        "mkfile - Creates a file\ncd - Change Directory\ndeldir - Delete a directory\ndelfile - Delete a file" +
                        "\ncdir - Shows current directory\nhelp2 - Shows the second page of commands");
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
                        "\ncopyfile - Copies a file to the selected path\ncow - Draws a cow" +
                        "\nmeminfo - Shows information about RAM\ncpuinfo - Shows information about the processor");
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
                    Console.WriteLine("Current Directory: " + current_directory);
                    var directory_list = Sys.FileSystem.VFS.VFSManager.GetDirectoryListing(current_directory);
                    foreach (var directoryEntry in directory_list)
                    {
                        try
                        {
                            var entry_type = directoryEntry.mEntryType;
                            if (entry_type == Sys.FileSystem.Listing.DirectoryEntryTypeEnum.File)
                            {
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                Console.WriteLine(" <File>       " + directoryEntry.mName);
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            if (entry_type == Sys.FileSystem.Listing.DirectoryEntryTypeEnum.Directory)
                            {
                                Console.ForegroundColor = ConsoleColor.Blue;
                                Console.WriteLine(" <Directory>      "+ directoryEntry.mName);
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
                case "cdir":
                    Console.WriteLine("Current Directory: " + current_directory);
                    break;
                case "mkdir":
                    Console.WriteLine("Enter Directory name");
                    var dirname = Console.ReadLine();
                    //fs.CreateDirectory(current_directory + dirname);
                    Console.WriteLine("Directory {0} created in {1}", dirname, current_directory);
                    break;
                case "mkfile":
                    Console.WriteLine("Enter file name");
                    var filename = Console.ReadLine();
                   // fs.CreateFile(current_directory + filename);
                    Console.WriteLine("File {0} created in {1}", filename, current_directory);
                    break;
                case "deldir":
                    Console.WriteLine("Enter directory name");
                    dirname = Console.ReadLine();
                    if (dirname == "System")
                    {
                        Console.WriteLine("This is system directory! You cannot delete it!");
                        break;
                    }
                    if (Directory.Exists(current_directory + dirname))
                    {
                        Sys.FileSystem.VFS.VFSManager.DeleteDirectory(current_directory + dirname, true);
                        Console.WriteLine("Directory {0} deleted in {1}", dirname, current_directory);
                    }
                    else
                    {
                        Console.WriteLine("Error: NanOS.Directory.Not.Found");
                    }

                    break;

                case "delfile":
                    //Удаление файла
                    Console.WriteLine("Enter file name");
                    filename = Console.ReadLine();
                    if (File.Exists(current_directory + filename))
                    {
                        Sys.FileSystem.VFS.VFSManager.DeleteFile(current_directory + filename);
                        Console.WriteLine("File {0} deleted in {1}", filename, current_directory);
                    }
                    else
                    {
                        Console.WriteLine("Error: NanOS.File.Not.Found");
                    }
                    break;
                case "writefile":
                    //Запись текста в файл
                    Console.WriteLine("Welcome to NanOS writestr app!");
                    Console.WriteLine("Please enter file name!");
                    filename = Console.ReadLine();
                    if (File.Exists(current_directory + filename))
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
                        Console.WriteLine("   " + filename);
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
                        File.Copy(current_directory + filename, current_directory + newfilename);
                        Console.WriteLine("File {0} renamed!", filename);
                        Sys.FileSystem.VFS.VFSManager.DeleteFile(current_directory + filename);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                    break;
                case "movefile":
                    string dirtomove;
                    Console.WriteLine("Enter file name");
                    filename = Console.ReadLine();
                    Console.WriteLine("Enter directoey");
                    Console.Write(@"0:\");
                    dirtomove = @"0:\" + Console.ReadLine();
                    File.Copy(current_directory + filename, dirtomove + filename);
                    Sys.FileSystem.VFS.VFSManager.DeleteFile(current_directory + filename);
                    break;
                case "diskinfo":
                    //fs.GetDisks();
                    //Получить тип файловой системы
                    long available_space = Sys.FileSystem.VFS.VFSManager.GetAvailableFreeSpace(@"0:\");
                    //filesystemtype = fs.GetFileSystemType(@"0:\");
                    //Получить размер диска
                   // long total_size = fs.GetTotalSize(@"0:\");
                    //Свободное место
                    available_space = Sys.FileSystem.VFS.VFSManager.GetAvailableFreeSpace(@"0:\");
                    Console.WriteLine("Available Free Space: " + available_space + " B");
                    //Console.WriteLine("Total size: " + total_size + " B");
                    //Console.WriteLine("File System type: " + filesystemtype);
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
                case "reboot":
                    Cosmos.System.Power.Reboot();
                    break;
                case "shutdown":
                    Cosmos.System.Power.Shutdown();
                    Console.WriteLine("Now you can power-off your PC!");
                    break;
                case "clear":
                    ConsoleClear();
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

                default:
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine(input + ": Command Not Found");
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;
            }

        }
        public void ConsoleClear()
        {
            year = Cosmos.HAL.RTC.Year;
            month = Cosmos.HAL.RTC.Month;
            day = Cosmos.HAL.RTC.DayOfTheMonth;
            hour = Cosmos.HAL.RTC.Hour;
            Minutes = Cosmos.HAL.RTC.Minute;
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" NanOS                                                          {0}.{1}.{2}   {3}:{4}",day,month,year,hour,Minutes);
            Console.BackgroundColor = ConsoleColor.Black;
        }
    }

}