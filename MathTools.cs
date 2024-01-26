using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2DP
{
    public static class MathTools
    {
        public static uint CalcCodeSymCipherText(uint shiftCoeff, uint codeSymPlain, uint alphabetSize) => (codeSymPlain - shiftCoeff + alphabetSize) % alphabetSize;

        public static uint CalcCodeSymPlainText(uint shiftCoeff, uint codeSymCipher, uint alphabetSize) => (codeSymCipher + shiftCoeff) % alphabetSize;

        public static void CreateTable((byte length, byte code)[] cipherTextAlphabet, string[] plainTextAlphabet, uint shiftCoeff)
        {
            (byte length, byte code)[] buf = new (byte length, byte code)[cipherTextAlphabet.Length];
            Array.Copy(cipherTextAlphabet, buf, cipherTextAlphabet.Length);
            for (int i = 0; i < plainTextAlphabet.Length; i++)
            {
                uint codeSym = CalcCodeSymCipherText(shiftCoeff, (uint)i, (uint)plainTextAlphabet.Length);
                cipherTextAlphabet[codeSym] = buf[i];
            }
        }
    }
}
