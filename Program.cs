using System;

namespace Lab2DP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            string[] plainTextAlphabet = ["а", "б", "в", "г", "д", "е", "ж", "з", 
                                          "и", "й", "к", "л", "м", "н", "о", "п", "р", 
                                          "с", "т", "у", "ф", "х", "ц", "ч", "ш", 
                                          "щ", "ы", "ъ", "ь", "э", "ю", "я", "1", 
                                          "2", "3", "4", "5", "6", "7", "8", "9", 
                                          "0", ".", ",", ":", ";", "!", "\"", "(",
                                          ")", "-", "/", "Начало", "Конец", " "];
            (byte length, byte code)[] morseCode = [(2, 2), (4, 7), (3, 4), (3, 1), (3, 3), (1, 1), (4, 14), (4, 3),
                                                             (2, 3), (4, 8), (3, 2), (4, 11), (2, 0), (2, 1), (3, 0), (4, 9),
                                                             (3, 5), (3, 7), (1, 0), (3, 6), (4, 13), (4, 15), (4, 5), (4, 1),
                                                             (4, 0), (4, 2), (4, 4), (4, 6), (5, 6), (5, 27), (4, 12), (4, 10),
                                                             (5, 16), (5, 24), (5, 28), (5, 30), (5, 31), (5, 15), (5, 7), (5, 3),
                                                             (5, 1), (5, 0), (6, 63), (6, 42), (6, 7), (5, 10), (6, 12), (6, 45),
                                                             (6, 18), (6, 18), (6, 30), (5, 13), (6, 25), (5, 21), (5, 17)];

            uint shiftCoeff = 11;
            MathTools.CreateTable(morseCode, plainTextAlphabet, shiftCoeff);

            string filePath;
            EncryptorDecryptor encryptorDecryptor = new EncryptorDecryptor();

            Console.WriteLine("Зашифровать - e\n Расшифровать - d\n Выйти - esc\n");
            ConsoleKey chosen = ConsoleKey.None;
            while (chosen != ConsoleKey.Escape)
            {
                chosen = Console.ReadKey().Key;
                
                if (chosen == ConsoleKey.E)
                {
                    Console.WriteLine("\nВведите путь к файлу, который хотите зашифровать: ");
                    filePath = Console.ReadLine();
                    encryptorDecryptor.Encrypt(filePath, morseCode, plainTextAlphabet, shiftCoeff);
                    Console.WriteLine();
                    Console.WriteLine("Текст зашифрован.");
                }
                else if (chosen == ConsoleKey.D)
                {
                    Console.WriteLine("\nВведите путь к файлу, который хотите расшифровать: ");
                    filePath = Console.ReadLine();
                    encryptorDecryptor.Decrypt(filePath, morseCode, plainTextAlphabet, shiftCoeff);
                    Console.WriteLine("Текст расшифрован.");
                }
            }
        }
    }
}