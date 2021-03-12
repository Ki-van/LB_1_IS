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

        public static Dictionary<char, double> FrequencyAnalysis(string path)
        {
            int totalCount = 0;
            Dictionary<char, double> SymbolDistribution = new Dictionary<char, double>();

            StreamReader input = new StreamReader(path, System.Text.Encoding.UTF8);
            string line;
            while ((line = input.ReadLine()) != null)
            {
                for (int i = 0; i < line.Length; i++)
                {
                    if (SymbolDistribution.ContainsKey(line[i]))
                    {
                        SymbolDistribution[line[i]]++;
                        totalCount++;
                    }
                    else
                    {
                        if (alphabet.IndexOf(line[i]) > 0)
                        {
                            SymbolDistribution.Add(line[i], 1);
                            totalCount++;
                        }
                    }
                }
            }

            char[] symbols = new char[SymbolDistribution.Keys.Count]; 
            
            SymbolDistribution.Keys.CopyTo(symbols, 0);

            for(int i = 0; i < symbols.Length; i++)
            {
                SymbolDistribution[symbols[i]] = SymbolDistribution.GetValueOrDefault(symbols[i]) / totalCount;
            } 
            

            return SymbolDistribution;
        }

        public static bool EncodePleifer(string path, string key)
        {
            int matrixN = (int)Math.Sqrt(m);
            char[,] matrix = new char[6, 6];

            string formatKey = "";
            List<char> addedSymbols = new List<char>();

            for (int i = 0; i < key.Length; i++)
            {
                if (!addedSymbols.Contains(key[i]))
                {
                    addedSymbols.Add(key[i]);
                    if (alphabet.Contains(key[i]))
                    {
                        if (i == 0)
                            formatKey += key[i];
                    }
                    else
                        return false;
                }
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

            for (int i = 0; i < matrixN; i++)
            {
                for (int j = 0; j < matrixN; j++)
                {
                    Console.Write(matrix[i, j] + " ");
                }
                Console.WriteLine();
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
                    while (i < sourceLine.Length && !alphabet.Contains(sourceLine[i]))
                        i++;

                    if (i >= sourceLine.Length)
                        break;

                    bigramm += sourceLine[i];

                    while (i + 1 < sourceLine.Length && !alphabet.Contains(sourceLine[i + 1]))
                        i++;

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
                    //В одной строке
                    if (bigrAI == bigrBI)
                    {
                        if (bigrAJ == matrixN - 1)
                            outputLine += matrix[bigrAI, 0];
                        else
                            outputLine += matrix[bigrAI, bigrAJ + 1];

                        if (bigrBJ == matrixN - 1)
                            outputLine += matrix[bigrBI, 0];
                        else
                            outputLine += matrix[bigrBI, bigrBJ + 1];

                    }
                    else // Столбцы
                        if (bigrAJ == bigrBJ)
                    {
                        if (bigrAI == matrixN - 1)
                            outputLine += matrix[0, bigrAJ];
                        else
                            outputLine += matrix[bigrAI + 1, bigrAJ];

                        if (bigrBI == matrixN - 1)
                            outputLine += matrix[0, bigrBJ];
                        else
                            outputLine += matrix[bigrBI + 1, bigrBJ];
                    }
                    else
                    {
                        outputLine += matrix[bigrAI, bigrBJ];
                        outputLine += matrix[bigrBI, bigrAJ];
                    }

                }
                output.WriteLine(outputLine);
            }
            output.Close();
            input.Close();

            return true;
        }

        public static bool DecodePleifer(string path, string key)
        {
            int matrixN = (int)Math.Sqrt(m);
            char[,] matrix = new char[6, 6];

            string formatKey = "";
            List<char> addedSymbols = new List<char>();

            for (int i = 0; i < key.Length; i++)
            {
                if (!addedSymbols.Contains(key[i]))
                {
                    addedSymbols.Add(key[i]);
                    if (alphabet.Contains(key[i]))
                    {
                        if (i == 0)
                            formatKey += key[i];
                    }
                    else
                        return false;
                }
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
            StreamWriter output = new StreamWriter("decoded.txt", false, Encoding.UTF8);

            string sourceLine, outputLine, bigramm;

            while ((sourceLine = input.ReadLine()) != null)
            {
                outputLine = "";
                for (int i = 0; i < sourceLine.Length; i += 2)
                {
                    bigramm = "";
                    bigramm += sourceLine[i];
                    bigramm += sourceLine[i + 1];

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
                    //В одной строке
                    if (bigrAI == bigrBI)
                    {
                        if (bigrAJ == 0)
                            outputLine += matrix[bigrAI, matrixN - 1];
                        else
                            outputLine += matrix[bigrAI, bigrAJ - 1];

                        if (bigrBJ == 0)
                            outputLine += matrix[bigrBI, matrixN - 1];
                        else
                            outputLine += matrix[bigrBI, bigrBJ - 1];

                    }
                    else // Столбцы
                        if (bigrAJ == bigrBJ)
                    {
                        if (bigrAI == 0)
                            outputLine += matrix[matrixN - 1, bigrAJ];
                        else
                            outputLine += matrix[bigrAI - 1, bigrAJ];

                        if (bigrBI == 0)
                            outputLine += matrix[matrixN - 1, bigrBJ];
                        else
                            outputLine += matrix[bigrBI - 1, bigrBJ];
                    }
                    else
                    {
                        outputLine += matrix[bigrAI, bigrBJ];
                        outputLine += matrix[bigrBI, bigrAJ];
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
