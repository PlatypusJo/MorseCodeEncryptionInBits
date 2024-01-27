using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lab2DP
{
    public class EncryptorDecryptor
    {
        private byte _mask;
        private byte _curByte;
        private BinaryWriter _binWriter;
        private BinaryReader _binReader;
        private StreamWriter _writer;
        private StreamReader _reader;

        public void Encrypt(string filePath, (byte length, byte code)[] cipherTextAlphabet, string[] plainTextAlphabet, uint shiftCoeff)
        {
            _mask = 128;
            _curByte = 0;
            string encryptFile = "./encryptedText.txt";
            _binWriter = new BinaryWriter(File.Open(encryptFile, FileMode.Create));
            _reader = new StreamReader(filePath);

            string text = _reader.ReadToEnd();
            text = text.ToLower();
            (byte length, byte code) morseCode = (0, 0);
            string sym = string.Empty;
            for (int i=-1; i <= text.Length; i++)
            {
                if (i < 0)
                    sym = "Начало";
                else if (i >= text.Length)
                    sym = "Конец";
                else
                    sym = text[i].ToString();

                int codeSymPlain = Array.IndexOf(plainTextAlphabet, sym);
                uint codeSymCipher = MathTools.CalcCodeSymCipherText(shiftCoeff, (uint)codeSymPlain, (uint)plainTextAlphabet.Length);
                morseCode = cipherTextAlphabet[codeSymCipher];

                Write(morseCode.length, 3);
                Write(morseCode.code, morseCode.length);

                if (sym == "Конец")
                {
                    while (_mask != 128)
                    {
                        Write(0, 1);
                    }
                }
            }
            _reader.Close();
            _binWriter.Close();
        }

        public void Decrypt(string filePath, (byte length, byte code)[] cipherTextAlphabet, string[] plainTextAlphabet, uint shiftCoeff)
        {
            _mask = 128;
            _curByte = 0;
            string decryptFile = "./decryptedText.txt";
            File.WriteAllText(decryptFile, string.Empty);
            _writer = new StreamWriter(decryptFile);
            _binReader = new BinaryReader(File.Open(filePath, FileMode.Open));

            string text = string.Empty;
            (byte length, byte code) morseCode = (0, 0);
            string sym = string.Empty;
            bool isBracketOpen = false;

            while(morseCode != (5, 21))
            {
                byte length = ReadByte(3);
                byte code = ReadByte(length);
                morseCode = (length, code);

                int codeSymCipher = Array.IndexOf(cipherTextAlphabet, morseCode);
                uint codeSymPlain = MathTools.CalcCodeSymPlainText(shiftCoeff, (uint)codeSymCipher, (uint)cipherTextAlphabet.Length);
                sym = plainTextAlphabet[codeSymPlain];

                if (sym == "(")
                {
                    if (isBracketOpen) sym = ")";
                    isBracketOpen = !isBracketOpen;
                }

                if (sym == "Начало")
                {
                    Console.WriteLine(sym);
                }
                else if (sym == "Конец")
                {
                    Console.WriteLine(sym);
                }
                else
                {
                    _writer.Write(sym);
                    text += sym;
                }
            }
            Console.WriteLine(text);
            _writer.Close();
            _binReader.Close();
        }

        public void Write(byte code, byte length)
        {
            byte bitPosition = (byte)(1 << (length - 1));
            while (bitPosition != 0)
            {
                if ((bitPosition & code) > 0)
                {
                    _curByte |= _mask; // записываем по биту в байт
                    Console.Write("1");
                }
                else Console.Write("0");

                _mask >>= 1; // маска для записи сдвиг вправо
                            // если байт заполнен
                if (_mask == 0)
                {
                    // записывай байт в выходной поток
                    _binWriter.Write(_curByte);
                    // обнуляем записываемый байт и маску
                    _curByte = 0;
                    _mask = 128;
                }
                bitPosition >>= 1;
            }
        }

        public byte ReadByte(byte length)
        {
            byte bitPosition;
            byte return_value;

            bitPosition = (byte)(1L << (length - 1));
            return_value = 0;
            while (bitPosition != 0)
            {
                if (_mask == 128)
                    _curByte = _binReader.ReadByte();

                if ((_curByte & _mask) > 0)
                {
                    return_value |= bitPosition;
                    Console.Write("1");
                }
                else Console.Write("0");
                    
                bitPosition >>= 1;
                _mask >>= 1;

                if (_mask == 0)
                    _mask = 128;

            }
            return return_value;
        }
    }
}
