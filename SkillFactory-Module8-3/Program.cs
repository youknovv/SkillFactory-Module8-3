using System;
using System.IO;

namespace SkillFactory_Module8_3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string path = @"\";
            long memoryBeforeCollection = GetFolderSize(path);
            Console.WriteLine($"Исходный размер папки:{memoryBeforeCollection}");
            DeleteUnusedFiles(path);
            long releasedMemory = memoryBeforeCollection - GetFolderSize(path);
            Console.WriteLine($"Освобождено: {releasedMemory}");
            Console.WriteLine($"Текущий размер папки:{GetFolderSize(path)}");
        }

        static long GetFolderSize(string path)
        {
            try
            {
                long size = 0;
                if (Directory.Exists(path))
                {
                    DirectoryInfo directory = new DirectoryInfo(path);
                    FileInfo[] files = directory.GetFiles();
                    foreach (FileInfo file in files)
                    {
                        size += file.Length;
                    }
                    DirectoryInfo[] folders = directory.GetDirectories();
                    foreach (DirectoryInfo folder in folders)
                    {
                        size += GetFolderSize(folder.FullName);
                    }
                    return size;
                }
                else
                {
                    Console.WriteLine("Каталога не существует либо введён неверный путь");
                    return 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

        static void DeleteUnusedFiles(string path)
        {
            try
            {
                DateTime currentTime = DateTime.Now;
                DateTime lifeSpan = currentTime.AddMinutes(-30);
                if (Directory.Exists(path))
                {
                    DirectoryInfo directory = new DirectoryInfo(path);
                    DirectoryInfo[] folders = directory.GetDirectories();
                    foreach (DirectoryInfo folder in folders)
                    {
                        DateTime lastAccess = Directory.GetLastWriteTime(folder.FullName);
                        if (lastAccess <= lifeSpan)
                        {
                            DeleteUnusedFiles(folder.FullName);
                            folder.Delete(true);
                            Console.WriteLine("Неиспользуемая папка удалена");
                        }
                    }
                    FileInfo[] files = directory.GetFiles();
                    foreach (FileInfo file in files)
                    {
                        DateTime lastAccess = File.GetLastWriteTime(file.FullName);
                        if (lastAccess <= lifeSpan)
                        {
                            file.Delete();
                            Console.WriteLine("Неиспользуемый файл удалён");
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

    }
}
