using System;
using System.Text;
using System.IO;
using Sys = Cosmos.System;
using System.Diagnostics;
using Cosmos.System.Graphics;
using Point = Cosmos.System.Graphics.Point;
using Cosmos.HAL;
using Cosmos.System.FileSystem.VFS;
using Cosmos.System.FileSystem;
using System.Drawing;
using IL2CPU.API.Attribs;
using NanOS;
using Cosmos.HAL.Network;
using Cosmos.Core.IOGroup;
using Cosmos.System.ExtendedASCII;

namespace NanOS
{
    public class Kernel : Sys.Kernel
    {
        public string osname = "NanOS";
        public string osversion = "1.1";
        public string kernelversion = "NanOS_kernel_1";
        public string boottype = "Live USB/CD";
        public string shellname = "nansh";
        public string username = "";
        public string password = "";
        Sys.FileSystem.CosmosVFS fs;
        string current_directory = @"cdir.empty";
        public bool fsinitialized;
        public bool passwordON;

        public object ConfigurationManager { get; private set; }

        //В СЛУЧАЕ ОШИБКИ string current_directory = @"0:\";
        protected override void BeforeRun()
        {
            #region NanOS Launch
            Console.WriteLine("[ NanOS.nansh ] Kernel Loaded! ");
            ConsoleClear();
            Console.WriteLine("[ NanOS.nansh ] Getting information about the time");
            Console.WriteLine(DateTime.Now);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[ OK ] Getting information about the time");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("[ NanOS.nansh ] File System Initialization");
            try
            {
                fs = new Sys.FileSystem.CosmosVFS();
                Sys.FileSystem.VFS.VFSManager.RegisterVFS(fs);
                fs.CreateDirectory(@"0:\FileSystemInitialization");
                Sys.FileSystem.VFS.VFSManager.DeleteDirectory(@"0:\FileSystemInitialization", true);
                Console.ForegroundColor = ConsoleColor.Green;
                current_directory = @"0:\";
                var disks = fs.GetDisks();
                Console.WriteLine("[ OK ] File System Initialization");
                fsinitialized = true;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("[ NanOS.nansh ] Search for system folders...");
                if (File.Exists(@"0:\System\DataBase\Users\Password.ndb"))
                {
                    password = File.ReadAllText(@"0:\System\DataBase\Users\Password.ndb");
                    passwordON = true;
                }
                try
                {
                    if (!Directory.Exists(@"0:\System\"))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[ ERROR ] The System folder was not found. Folder Recovery...");
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("[ NanOS.nansh ] Creating a System Folder......");
                        fs.CreateDirectory(@"0:\System\");
                        Console.WriteLine("[ NanOS.nansh ] Creating a DataBase Folder......");
                        fs.CreateDirectory(@"0:\System\DataBase\");
                        Console.WriteLine("[ NanOS.nansh ] Creating a Users Folder......");
                        fs.CreateDirectory(@"0:\System\DataBase\Users\");
                        Console.WriteLine("[ NanOS.nansh ] Creating a file with username information...");
                        fs.CreateFile(@"0:\System\DataBase\Users\Users.ndb");
                        Console.WriteLine("[ NanOS.nansh ] Enter your UserName!");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("Username: ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        var usrnmeforfile = Console.ReadLine();
                        Console.ForegroundColor = ConsoleColor.White;
                        File.WriteAllText(@"0:\System\DataBase\Users\Users.ndb", usrnmeforfile);
                        username = File.ReadAllText(@"0:\System\DataBase\Users\Users.ndb");

                        Console.WriteLine("[ NanOS.nansh ] Do you want to set a password for the {0} account?", username);
                    gtchoosepswrd:
                        string choosePSWRD = Console.ReadLine();
                        switch (choosePSWRD)
                        {
                            case "Y":
                                Console.WriteLine("[ NanOS.nansh ] Enter a new password for the account {0}", username);
                                var NewPassword = Console.ReadLine();
                                fs.CreateFile(@"0:\System\DataBase\Passwords\Password.ndb");
                                File.WriteAllText(@"0:\System\DataBase\Users\Password.ndb", NewPassword);
                                password = File.ReadAllText(@"0:\System\DataBase\Users\Password.ndb");
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("[ OK ] The password for the account {0} has been successfully created!", username);
                                passwordON = true;
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            case "y":
                                Console.WriteLine("[ NanOS.nansh ] Enter a new password for the account {0}", username);
                                NewPassword = Console.ReadLine();
                                fs.CreateFile(@"0:\System\DataBase\Passwords\Password.ndb");
                                File.WriteAllText(@"0:\System\DataBase\Users\Password.ndb", NewPassword);
                                password = File.ReadAllText(@"0:\System\DataBase\Users\Password.ndb");
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("[ OK ] The password for the account {0} has been successfully created!", username);
                                passwordON = true;
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            case "N":

                                break;
                            case "n":

                                break;
                            default:
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("[ ERROR ] You entered the wrong letter!");
                                Console.ForegroundColor = ConsoleColor.White;
                                goto gtchoosepswrd;
                                break;
                        }

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("[ OK ] The system folder has been restored!");
                        Console.ForegroundColor = ConsoleColor.White; ;
                    }
                    else if (Directory.Exists(@"0:\System\"))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("[ OK ] System folder found!");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("[ NanOS.nansh ] User search.....");
                        username = File.ReadAllText(@"0:\System\DataBase\Users\Users.ndb");
                        if (passwordON == true)
                        {
                            password = File.ReadAllText(@"0:\System\DataBase\Users\Password.ndb");
                        }
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("[ OK ] Found user {0} | Password protection = {1}", username,passwordON);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                catch (Exception ex)
                {

                }

                Console.WriteLine("[ NanOS.nansh ] Press Any Key To Continue");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[ ERROR ] File System Initialization");
                fsinitialized = false;
                Console.ForegroundColor = ConsoleColor.White;
            gtchoose:
                Console.WriteLine("Continue without a file system? Y - Yes N - No");
                string chooseYN = Console.ReadLine();
                switch (chooseYN)
                {
                    case "Y":
                        Console.WriteLine("[ NanOS.nansh ] Attention! The OS works without a file system!");
                        break;
                    case "y":
                        Console.WriteLine("[ NanOS.nansh ] Attention! The OS works without a file system!");
                        break;
                    case "N":
                        Cosmos.System.Power.Reboot();
                        break;
                    case "n":
                        Cosmos.System.Power.Reboot();
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[ ERROR ] You entered the wrong letter!");
                        Console.ForegroundColor = ConsoleColor.White;
                        goto gtchoose;
                        break;
                }
            }
            #endregion
            ConsoleClear();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(@"                    NN   NN   AAA   NN   NN  OOOOO   SSSSS  
                    NNN  NN  AAAAA  NNN  NN OO   OO SS      
                    NN N NN AA   AA NN N NN OO   OO  SSSSS  
                    NN  NNN AAAAAAA NN  NNN OO   OO      SS 
                    NN   NN AA   AA NN   NN  OOOO0   SSSSS  ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("                        NanOS successfully loaded!");
            if (fsinitialized == false)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Enter your username");
                Console.Write("Username: ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                username = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("             Welcome to NanOS {0}. Press any key to get started!", username);
                if (username == "")
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("          You didn't provide a username. You will continue as userNanOS");
                    Console.ForegroundColor = ConsoleColor.White;
                    username = "userNanOS";
                }
            }
            else if (!File.Exists(@"0:\System\DataBase\Passwords\Password.ndb"))
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("             Welcome to NanOS {0}. Press any key to get started!", username);
                Console.ForegroundColor = ConsoleColor.White;
            }
            else if (File.Exists(@"0:\System\DataBase\Passwords\Password.ndb"))
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("             Welcome to NanOS {0}. Please enter a password.", username);
            enterpswrd:
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Password: ");
                Console.ForegroundColor = ConsoleColor.Green;
                string chckpswrd = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.White;
                if (chckpswrd == password)
                {
                    Console.WriteLine("             Welcome to NanOS {0}. Press any key to get started!", username);
                }
                else
                {
                    Console.WriteLine("                    Invalid password. Please try again!");
                    goto enterpswrd;
                }
            }
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
              \__|  \__| \_______|\__|  \__| \______/  \______/ ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n                        Visit our website: www.nanos.tk" +
                "\n");
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
                case "lockscr":
                    LockScreen();
                    break;
                case "whoami":
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("I am {0}", username);
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case "date":
                    Console.WriteLine(DateTime.Now);
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
                    Console.WriteLine(@"               :!!!:              OS NAME: {0}
             .7YJYY57.            OS VERSION: {1}
         .^7JYYJJJJYYYY!..        KERNEL: {2}
   .^^^!?JJJJ?!?JJY?!JY5YYJ!^^^.  SHELL: {3}
  .Y?????Y?^.  :JJY:  .^J5YY555P. USER: {4}
  .J777??J?~:. ~J?J~ .:!J5YYYYYY  CPU: {5}
   .J77J!?JJJJ?J???JJJYYYJ7YYY5.  CPU VENDOR: {6}
   .?77!  .^?????????JJ^.  7YJ5   AMOUNT OF RAM: {7} MB
   .?!7!  .^?????????J?^.  7JJY   AVIALIBLE RAM: {8} MB
   .?!7J!7?????????JJJJJJJ7YJJY.  USED RAM: {9} B
  .?!!!77?7~:  ~?7J~  :!?YJJJJJY   
  .J7!77!J7^.  :?7J:  .^?Y?JJJJ5.  
   .::^~7777?7~7?7?7!?JJJJ?!^^^.   
         .:!7?77777??J?!^.         
             .!?777J!.             
               :~~~:               ", osname, osversion, kernelversion, shellname, username, cpubrand, CPU_vendorname, amount_of_ram, avialible_ram, used_ram,
               current_directory);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case "raminfo":
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

                    if (Environment.Is64BitProcess)
                    {
                        Console.WriteLine("Bit architecture: 64-bit");
                    }
                    else
                    {
                        Console.WriteLine("Bit architecture: 32-bit");
                    }
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
                        "kernel - Shows info about the kernel\nbeep - Tests your PC Speaker\nchngeuname - Changes your username\n" +
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
                        "\nraminfo - Shows information about RAM\ncpuinfo - Shows information about the processor");
                    break;
                case "beep":
                    Sys.PCSpeaker.Beep();
                    break;
                case "chngeuname":
                    if (fsinitialized == false)
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("Write your new UserName");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        username = Console.ReadLine();
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("Your username changed succeful! Hello {0}", username);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else if (fsinitialized == true)
                    {
                        try
                        {
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.WriteLine("Write your new UserName");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            username = Console.ReadLine();
                            File.WriteAllText(@"0:\System\DataBase\Users\Users.ndb", username);
                            username = File.ReadAllText(@"0:\System\DataBase\Users\Users.ndb");
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine("Your username changed succeful! Hello {0}", username);
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        catch (Exception ex)
                        {

                        }
                    }

                    break;
                case "dir":
                    Console.WriteLine("Current Directory: " + current_directory);
                    try
                    {
                        var directory_list = Sys.FileSystem.VFS.VFSManager.GetDirectoryListing(current_directory);
                        foreach (var directoryEntry in directory_list)
                        {
                            try
                            {
                                var entry_type = directoryEntry.mEntryType;
                                if (entry_type == Sys.FileSystem.Listing.DirectoryEntryTypeEnum.File)
                                {
                                    Console.ForegroundColor = ConsoleColor.Magenta;
                                    Console.WriteLine("| <File>       " + directoryEntry.mName);
                                    Console.ForegroundColor = ConsoleColor.White;
                                }
                                if (entry_type == Sys.FileSystem.Listing.DirectoryEntryTypeEnum.Directory)
                                {
                                    Console.ForegroundColor = ConsoleColor.Blue;
                                    Console.WriteLine("| <Directory>      " + directoryEntry.mName);
                                    Console.ForegroundColor = ConsoleColor.White;
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Error: Directory not found");
                                Console.WriteLine(e.ToString());
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        break;
                    }
                    break;
                case "cdir":
                    Console.WriteLine("Current Directory: " + current_directory);
                    break;
                case "mkdir":
                    Console.WriteLine("Enter Directory name");
                    var dirname = Console.ReadLine();
                    try
                    {
                        fs.CreateDirectory(current_directory + dirname);
                        Console.WriteLine("Directory {0} created in {1}", dirname, current_directory);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        break;
                    }

                    break;
                case "mkfile":
                    Console.WriteLine("Enter file name");
                    var filename = Console.ReadLine();
                    try
                    {
                        fs.CreateFile(current_directory + filename);
                        Console.WriteLine("File {0} created in {1}", filename, current_directory);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        break;
                    }
                    break;
                case "deldir":
                    Console.WriteLine("Enter directory name");
                    dirname = Console.ReadLine();
                    try
                    {
                        //   if (dirname == "System")
                        //   {
                        //       Console.WriteLine("This is system directory! You cannot delete it!");
                        //       break;
                        //   }
                        if (dirname == "system")
                        {
                            Console.WriteLine("This is system directory! You cannot delete it!");
                            break;
                        }
                        if (dirname == "DataBase")
                        {
                            Console.WriteLine("This is system directory! You cannot delete it!");
                            break;
                        }
                        if (dirname == "database")
                        {
                            Console.WriteLine("This is system directory! You cannot delete it!");
                            break;
                        }
                        if (dirname == "Users")
                        {
                            Console.WriteLine("This is system directory! You cannot delete it!");
                            break;
                        }
                        if (dirname == "users")
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
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        break;
                    }
                    break;

                case "delfile":
                    //Удаление файла
                    Console.WriteLine("Enter file name");
                    filename = Console.ReadLine();
                    try
                    {
                        if (filename == "Users.ndb")
                        {
                            Console.WriteLine("You cannot delete this file!");
                            break;
                        }
                        if (File.Exists(current_directory + filename))
                        {
                            Sys.FileSystem.VFS.VFSManager.DeleteFile(current_directory + filename);
                            Console.WriteLine("File {0} deleted in {1}", filename, current_directory);
                        }
                        else
                        {
                            Console.WriteLine("Error: NanOS.File.Not.Found");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        break;
                    }
                    break;
                case "writefile":
                    //Запись текста в файл
                    Console.WriteLine("Welcome to NanOS writestr app!");
                    Console.WriteLine("Please enter file name!");
                    filename = Console.ReadLine();
                    try
                    {
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
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
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
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        break;
                    }
                    break;
                case "copyfile":
                    string dirtocopy = @"0:\";
                    Console.WriteLine("Enter the directory where you want to copy the file");
                    Console.Write(@"0:\");
                    dirtocopy = @"0:\" + Console.ReadLine();
                    try
                    {
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
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        break;
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
                        break;
                    }
                    break;
                case "movefile":
                    string dirtomove;
                    Console.WriteLine("Enter file name");
                    filename = Console.ReadLine();
                    Console.WriteLine("Enter directoey");
                    Console.Write(@"0:\");
                    dirtomove = @"0:\" + Console.ReadLine();
                    try
                    {
                        File.Copy(current_directory + filename, dirtomove + filename);
                        Sys.FileSystem.VFS.VFSManager.DeleteFile(current_directory + filename);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                    break;
                case "diskinfo":
                    //Получить тип файловой системы
                    try
                    {
                        long available_space = Sys.FileSystem.VFS.VFSManager.GetAvailableFreeSpace(@"0:\");
                        //Получить размер диска
                        long total_size = fs.GetTotalSize(@"0:\");
                        //Свободное место
                        available_space = Sys.FileSystem.VFS.VFSManager.GetAvailableFreeSpace(@"0:\");
                        Console.WriteLine("Available Free Space: " + available_space + " B");
                        Console.WriteLine("Total size: " + total_size + " B");

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        break;
                    }
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
                        break;
                    }
                    break;
                case "":

                    break;
                case " ":
                    Console.WriteLine("");
                    break;
                case "reboot":
                    Console.WriteLine("Rebooting in 3 seconds!");
                    System.Threading.Thread.Sleep(1000);
                    Console.WriteLine("Rebooting in 2 seconds!");
                    System.Threading.Thread.Sleep(1000);
                    Console.WriteLine("Rebooting in 1 second!");
                    System.Threading.Thread.Sleep(1000);
                    Cosmos.System.Power.Reboot();
                    break;
                case "shutdown":
                    Console.WriteLine("Shutting down in 3 seconds!");
                    System.Threading.Thread.Sleep(1000);
                    Console.WriteLine("Shutting down in 2 seconds!");
                    System.Threading.Thread.Sleep(1000);
                    Console.WriteLine("Shutting down in 1 second!");
                    System.Threading.Thread.Sleep(1000);
                    Cosmos.System.Power.Shutdown();
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
                    Console.WriteLine("NanOS_kernel_1. Kernel created May 4, 2022\nKernel.NanOS");
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
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" NanOS                                                       {0}",
            DateTime.Now);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine("");
        }

        public void LockScreen()
        {
            Console.Clear();
            if(File.Exists(@"0:\System\DataBase\Users\Password.ndb"))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(@"        :::        ::::::::   ::::::::  :::    ::: :::::::::: :::::::::  
        :+:       :+:    :+: :+:    :+: :+:   :+:  :+:        :+:    :+: 
        +:+       +:+    +:+ +:+        +:+  +:+   +:+        +:+    +:+ 
        +#+       +#+    +:+ +#+        +#++:++    +#++:++#   +#+    +:+ 
        +#+       +#+    +#+ +#+        +#+  +#+   +#+        +#+    +#+ 
        #+#       #+#    #+# #+#    #+# #+#   #+#  #+#        #+#    #+# 
        ########## ########   ########  ###    ### ########## #########  ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("User {0}'s computer is locked. Please enter a password!", username);
                Console.WriteLine("User: {0}", username);
                Console.Write("Password: ");
                lckscr:
                Console.ForegroundColor = ConsoleColor.Green;
                var lckscrpswr = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.White;
                if (lckscrpswr == password)
                {
                    ConsoleClear();
                    Commands();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Invalid password! Please try again!");
                    Console.ForegroundColor = ConsoleColor.White;
                    goto lckscr;
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(@"        :::        ::::::::   ::::::::  :::    ::: :::::::::: :::::::::  
        :+:       :+:    :+: :+:    :+: :+:   :+:  :+:        :+:    :+: 
        +:+       +:+    +:+ +:+        +:+  +:+   +:+        +:+    +:+ 
        +#+       +#+    +:+ +#+        +#++:++    +#++:++#   +#+    +:+ 
        +#+       +#+    +#+ +#+        +#+  +#+   +#+        +#+    +#+ 
        #+#       #+#    #+# #+#    #+# #+#   #+#  #+#        #+#    #+# 
        ########## ########   ########  ###    ### ########## #########  ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("User {0}'s computer is locked. Press any key to unlock!", username);
                Console.ReadKey();
            }
           
        }
        
    }

}