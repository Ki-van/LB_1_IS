using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace LB_1_IS
{
    class Crypto
    {
        private static string alphabet = "абвгдеёжзийклмнопрстуфхцчшщъьыэюя_,.";
        private static int m = alphabet.Length;

        public static bool EncodePleifer(string path, string key)
        {
            int matrixN = (int)Math.Sqrt(m);
            char[,] matrix = new char[6, 6];

            string formatKey = "";
            for (int i = 0; i < key.Length; i++)
            {
                if (alphabet.Contains(key[i]))
                {
                    if (i == 0 || key[i] != key[i - 1])
                        formatKey += key[i];
                }
                else
                    return false;
            }
            if (formatKey.Length > matrixN * matrixN)
                return false;

            int formatKeyI = 0, alphabetI = 0;
            for (int i = 0; i < matrixN; i++)
            {
                for (int j = 0; j < matrixN; j++)
                {
                    if (formatKeyI < formatKey.Length)
                    {
                        matrix[i, j] = formatKey[formatKeyI];
                        formatKeyI++;
                    }
                    else
                    {
                        while (formatKey.Contains(alphabet[alphabetI]))
                            alphabetI++;
                        matrix[i, j] = alphabet[alphabetI];
                        alphabetI++;
                    }
                }
            }

            StreamReader input = new StreamReader(path, Encoding.UTF8);
            StreamWriter output = new StreamWriter("encoded.txt", false, Encoding.UTF8);

            string sourceLine, outputLine, bigramm;
            while ((sourceLine = input.ReadLine()) != null)
            {
                outputLine = "";
                for (int i = 0; i < sourceLine.Length; i += 2)
                {
                    bigramm = "";
                    bigramm += sourceLine[i];
                    if (i + 1 < sourceLine.Length)
                    {
                        if (bigramm[0] == sourceLine[i + 1])
                        {
                            bigramm += 'ь';
                            i--;
                        }
                        else
                            bigramm += sourceLine[i + 1];


                    }
                    else
                        bigramm += 'ь';

                    int bigrAI = -1, bigrAJ = -1,
                        bigrBI = -1, bigrBJ = -1;

                    for (int j = 0; j < matrixN && (bigrAI == -1 || bigrBI == -1); j++)
                    {
                        for (int k = 0; k < matrixN; k++)
                        {
                            if (matrix[j, k] == bigramm[0])
                            {
                                bigrAI = j;
                                bigrAJ = k;
                            }
                            else
                                if (matrix[j, k] == bigramm[1])
                            {
                                bigrBI = j;
                                bigrBJ = k;
                            }
                        }
                    }
                    //Строки
                    if (bigrAI == bigrBI)
                    {
                        if (bigrAJ < bigrBJ)
                        {
                            int tmp = bigrAJ;
                            bigrAJ = bigrBJ;
                            bigrBJ = tmp;
                        }

                        if (bigrAJ == matrixN - 1)
                        {
                            outputLine += matrix[bigrAI, 0];
                            outputLine += matrix[bigrBI, bigrBJ];
                        }
                        else
                        {
                            outputLine += matrix[bigrBI, bigrBJ];
                            outputLine += matrix[bigrAI, bigrAJ];
                        }

                    }
                    else // столбцы
                        if (bigrAJ == bigrBJ)
                    {
                        if (bigrAI < bigrBI)
                        {
                            int tmp = bigrAI;
                            bigrAI = bigrBI;
                            bigrBI = tmp;
                        }

                        if (bigrAI == matrixN - 1)
                        {
                            outputLine += matrix[0, bigrAJ];
                            outputLine += matrix[bigrBJ, bigrBJ];
                        }
                        else
                        {
                            outputLine += matrix[bigrBI, bigrBJ];
                            outputLine += matrix[bigrAI, bigrAJ];
                        }

                    }
                    else
                    {
                        if(bigrAJ > bigrBJ)
                        {

                        }
                    }

                }
                output.WriteLine(outputLine);
            }
            output.Close();
            input.Close();

            return true;
        }

        public static void EncodeCaesar(string path, int offset)
        {
            offset %= m;
            StreamReader input = new StreamReader(path, Encoding.UTF8);
            StreamWriter output = new StreamWriter("encoded.txt", false, Encoding.UTF8);

            string sourceLine, outputLine = "";
            while ((sourceLine = input.ReadLine()) != null)
            {
                for (int i = 0; i < sourceLine.Length; i++)
                {
                    if (alphabet.Contains(sourceLine[i]))
                        outputLine += alphabet[((alphabet.IndexOf(sourceLine[i]) + offset) % m)];
                }
                output.WriteLine(outputLine);
                outputLine = "";
            }
            output.Close();
            input.Close();
        }

        public static void DecodeCaesar(string path, int offset)
        {
            offset %= m;
            StreamReader input = new StreamReader(path, Encoding.UTF8);
            StreamWriter output = new StreamWriter("decoded.txt", false, Encoding.UTF8);

            string sourceLine, outputLine = "";
            while ((sourceLine = input.ReadLine()) != null)
            {
                for (int i = 0; i < sourceLine.Length; i++)
                {
                    if (alphabet.Contains(sourceLine[i]))
                    {
                        int tmp = alphabet.IndexOf(sourceLine[i]) - offset;
                        outputLine += alphabet[((tmp < 0 ? tmp + m : tmp) % m)];
                    }
                }
                output.WriteLine(outputLine);
                outputLine = "";
            }
            output.Close();
            input.Close();
        }

        public static bool EncodeMultCipher(string path, int key)
        {
            key %= m;
            if (Crypto.gcd(key, m) != 1)
                return false;

            StreamReader input = new StreamReader(path, Encoding.UTF8);
            StreamWriter output = new StreamWriter("encoded.txt", false, Encoding.UTF8);

            string sourceLine, outputLine = "";
            while ((sourceLine = input.ReadLine()) != null)
            {
                for (int i = 0; i < sourceLine.Length; i++)
                {
                    if (alphabet.Contains(sourceLine[i]))
                        outputLine += alphabet[((alphabet.IndexOf(sourceLine[i]) * key) % m)];
                }
                output.WriteLine(outputLine);
                outputLine = "";
            }
            output.Close();
            input.Close();
            return true;
        }

        public static bool DecodeMultCipher(string path, int key)
        {
            key %= m;
            if (Crypto.gcd(key, m) != 1)
                return false;

            int nkey;
            bool fl = false;
            for (nkey = 0; nkey < m; nkey++)
            {
                if ((key * nkey) % m == 1)
                {
                    fl = true;
                    break;
                }
            }

            if (!fl)
                return fl;

            StreamReader input = new StreamReader(path, Encoding.UTF8);
            StreamWriter output = new StreamWriter("decoded.txt", false, Encoding.UTF8);

            string sourceLine, outputLine = "";
            while ((sourceLine = input.ReadLine()) != null)
            {
                for (int i = 0; i < sourceLine.Length; i++)
                {
                    if (alphabet.Contains(sourceLine[i]))
                    {
                        outputLine += alphabet[((alphabet.IndexOf(sourceLine[i]) * nkey) % m)];
                    }
                }
                output.WriteLine(outputLine);
                outputLine = "";
            }
            output.Close();
            input.Close();
            return true;
        }

        private static int gcd(int u, int v)
        {
            // Base cases
            //  gcd(n, n) = n
            if (u == v)
                return u;

            //  Identity 1: gcd(0, n) = gcd(n, 0) = n
            if (u == 0)
                return v;
            if (v == 0)
                return u;

            if (u % 2 == 0)
            { // u is even
                if (v % 2 == 1) // v is odd
                    return gcd(u / 2, v); // Identity 3
                else // both u and v are even
                    return 2 * gcd(u / 2, v / 2); // Identity 2

            }
            else
            { // u is odd
                if (v % 2 == 0) // v is even
                    return gcd(u, v / 2); // Identity 3

                // Identities 4 and 3 (u and v are odd, so u-v and v-u are known to be even)
                if (u > v)
                    return gcd((u - v) / 2, v);
                else
                    return gcd((v - u) / 2, u);
            }
        }

    }
}
