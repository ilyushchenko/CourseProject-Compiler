﻿using System;
using System.Collections.Generic;

namespace LR
{
    public class UpAnalysis
    {
        /// <summary>
        /// Структура символов грамматики
        /// </summary>
        public struct MyWord
        {
            public MyWord(int l, string w)
            {
                L = l;
                W = w;
                T = "";
            }

            public int L;
            public string W, T;
        }

        /// <summary>
        /// Структура правила
        /// </summary>
        public struct MyRule
        {
            public MyRule(int p, int[] m)
            {
                P = p;
                M = m;
            }
            public int P;
            public int L => M.Length;
            public int[] M;
        }

        public delegate void PrintDelegate(string text);

        public event PrintDelegate PrintCompileInfo = (str) => { };
        public event PrintDelegate PrintCompileResult = (str) => { };
        public event PrintDelegate PrintMessage = (str) => { };

        public MyWord[] ArrS = new MyWord[1000];

        /// <summary>
        /// Коллекция правил, для компиляции
        /// </summary>
        public MyRule[] Rule =
        {
            new MyRule(1, new[] {1, 5, 6, 2, 3, 3}),
            new MyRule(1, new[] {1, 5, 6, 2, 3, 3, 0}),
            new MyRule(2, new[] {7, 16}),
            new MyRule(2, new[] {7, 16, 8, 2, 9}),
            new MyRule(3, new[] {5}),
            new MyRule(4, new[] {1, 5}),
            new MyRule(6, new[] {16}),
            new MyRule(6, new[] {17}),
            new MyRule(6, new[] {16, 8, 2, 9}),
            new MyRule(6, new[] {11, 5}),
            new MyRule(6, new[] {10, 5}),
            new MyRule(6, new[] {4, 5}),
            new MyRule(5, new[] {14, 5}),
            new MyRule(5, new[] {15, 5}),
            new MyRule(5, new[] {12, 5}),
            new MyRule(5, new[] {13, 5})
        };

        /// <summary>
        /// Коллекция символов грамматики, для компиляции
        /// </summary>
        public MyWord[] ArrWords =
        {
            new MyWord(1, "S"),
            new MyWord(1, "X"),
            new MyWord(1, "Y"),
            new MyWord(1, "W"),
            new MyWord(1, "R"),
            new MyWord(1, "A"),
            new MyWord(1, "for"),
            new MyWord(1, "set"),
            new MyWord(1, "["),
            new MyWord(1, "]"),
            new MyWord(1, "not"),
            new MyWord(1, "sqrt"),
            new MyWord(1, "neq"),
            new MyWord(1, "or"),
            new MyWord(1, "mult"),
            new MyWord(1, "div"),
            new MyWord(1, "id"),
            new MyWord(1, "const"),
            new MyWord(1, "$")
        };

        /// <summary>
        /// Коллекций номеров правил
        /// </summary>
        public List<int> Rules = new List<int>();

        /// <summary>
        /// Управляющая таблица восходящего разбора
        /// </summary>
        public int[,] ArrZ = {
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3},
            {0, 0, 0, 0, 1, 2, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0},
            {0, 1, 0, 2, 0, 0, 0, 1, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {2, 1, 0, 2, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3},
            {0, 0, 0, 0, 1, 2, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0},
            {0, 0, 0, 0, 0, 0, 2, 3, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3},
            {0, 0, 2, 0, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0},
            {0, 0, 2, 0, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0},
            {0, 0, 0, 0, 0, 0, 3, 3, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3},
            {0, 0, 0, 0, 1, 2, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0},
            {0, 0, 0, 0, 1, 2, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0},
            {0, 0, 0, 0, 1, 2, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0},
            {0, 0, 0, 0, 1, 2, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0},
            {0, 0, 0, 0, 1, 2, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0},
            {0, 0, 0, 0, 1, 2, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0},
            {0, 0, 0, 0, 0, 0, 3, 3, 2, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3},
            {0, 0, 0, 0, 0, 0, 3, 3, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3},
            {0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        };

        public void PrintInfo(int yy, int zz, int xx, MyWord[] splittedWords)
        {
            string s1 = "";
            int i2 = yy;
            while (i2 < splittedWords.Length)
            {
                s1 = s1 + ArrWords[splittedWords[i2].L].W + " ";
                i2 = i2 + 1;
            }
            PrintCompileInfo.Invoke(@"Строка:" + s1 + '\n');
            string s2 = "";
            i2 = 0;
            while (i2 <= zz)
            {
                s2 = s2 + ArrWords[ArrS[i2].L].W + " ";
                i2 = i2 + 1;
            }
            PrintCompileInfo.Invoke(@"Магазин:" + s2 + '\n');
            string s3 = "";
            i2 = 1;
            while (i2 < xx)
            {
                s3 = s3 + (Rules[i2] + 1) + " ";
                i2 = i2 + 1;
            }
            PrintCompileInfo.Invoke(@"Правила:" + s3 + '\n');
            PrintCompileInfo.Invoke(@"      " + '\n');
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ruleNumber">Номер правила</param>
        /// <param name="ts1"></param>
        public void MyCompil(int ruleNumber, int ts1)
        {
            ruleNumber = ruleNumber + 1;
            if (ruleNumber == 1)
                ArrS[ts1 - 5].T = "for ( " + ArrS[ts1 - 5].T + ArrS[ts1 - 4].T + "; " + ArrS[ts1 - 2].T + "; " + ArrS[ts1 - 1].T + " ) {" + ArrS[ts1].T + "} ;";
            if (ruleNumber == 2)
                ArrS[ts1 - 6].T = "for ( " + ArrS[ts1 - 6].T + ArrS[ts1 - 5].T + "; " + ArrS[ts1 - 3].T + "; " + ArrS[ts1 - 2].T + " ) {" + ArrS[ts1 - 1].T + "} ;" + Environment.NewLine + ArrS[ts1].T;
            if (ruleNumber == 4)
                ArrS[ts1 - 4].T = ArrS[ts1 - 3].W + "[ " + ArrS[ts1 - 1].T + " ] = ";
            if (ruleNumber == 3)
                ArrS[ts1 - 1].T = ArrS[ts1].W + " = ";
            if (ruleNumber == 6)
                ArrS[ts1 - 1].T = ArrS[ts1 - 1].T + " " + ArrS[ts1].T;
            if (ruleNumber == 8)
                ArrS[ts1].T = ArrS[ts1].W;
            if (ruleNumber == 7)
                ArrS[ts1].T = ArrS[ts1].W;
            if (ruleNumber == 11)
                ArrS[ts1 - 1].T = "! ( " + ArrS[ts1].T + " )";
            if (ruleNumber == 10)
                ArrS[ts1 - 1].T = "sqrt ( " + ArrS[ts1].T + " )";
            if (ruleNumber == 12)
                ArrS[ts1 - 1].T = ArrS[ts1 - 1].T + " " + ArrS[ts1].T;
            if (ruleNumber == 9)
                ArrS[ts1 - 3].T = ArrS[ts1 - 3].W + "[ " + ArrS[ts1 - 1].T + " ] ";
            if (ruleNumber == 15)
                ArrS[ts1 - 1].T = ArrS[ts1].T + " != ";
            if (ruleNumber == 16)
                ArrS[ts1 - 1].T = ArrS[ts1].T + " ^ ";
            if (ruleNumber == 13)
                ArrS[ts1 - 1].T = ArrS[ts1].T + " * ";
            if (ruleNumber == 14)
                ArrS[ts1 - 1].T = ArrS[ts1].T + " / ";

        }

        public void Algorithm(MyWord[] splittedWords)
        {
            int currentSplittedWord = 0;
            int currentMagazine = 0;
            int go = 0;
            ArrS[0].L = 18;
            ArrS[0].W = "$";
            PrintInfo(currentSplittedWord, currentMagazine, Rules.Count, splittedWords);
            while (currentSplittedWord <= splittedWords.Length)
            {
                if (splittedWords[currentSplittedWord].L == 18)
                {
                    if (currentMagazine == 1)
                    {
                        if ((ArrS[currentMagazine].L == 0) && (ArrS[currentMagazine - 1].L == 18))
                        {
                            // Тут можно делать выход из алгоритма
                            go = 4;
                        }
                    }
                }
                if (((ArrZ[ArrS[currentMagazine].L, splittedWords[currentSplittedWord].L] == 1) && go != 4) || ((ArrZ[ArrS[currentMagazine].L, splittedWords[currentSplittedWord].L] == 2) && go != 4))
                {
                    currentMagazine = currentMagazine + 1;
                    ArrS[currentMagazine].L = splittedWords[currentSplittedWord].L;
                    ArrS[currentMagazine].W = splittedWords[currentSplittedWord].W;
                    currentSplittedWord = currentSplittedWord + 1;
                    PrintInfo(currentSplittedWord, currentMagazine, Rules.Count, splittedWords);
                    go = 2;
                }
                if ((ArrZ[ArrS[currentMagazine].L, splittedWords[currentSplittedWord].L] == 3) && go != 2 && go != 4)
                {
                    int koli = 0;
                    while (ArrZ[ArrS[currentMagazine - koli].L, ArrS[currentMagazine - koli + 1].L] != 1)
                    {
                        koli = koli + 1;
                    }
                    for (var p10 = 0; p10 < 16; p10++)
                    {
                        if (Rule[p10].L == koli && go != 2)
                        {
                            int pr11 = 0;
                            for (var pr12 = 0; pr12 < koli; pr12++)
                            {
                                if (Rule[p10].M[pr12] == ArrS[currentMagazine - koli + 1 + pr12].L)
                                {
                                    pr11 = pr11 + 1;
                                }
                            }
                            if (pr11 == koli)
                            {
                                MyCompil(p10, currentMagazine);
                                currentMagazine = currentMagazine - koli + 1;
                                ArrS[currentMagazine].L = Rule[p10].P - 1;
                                ArrS[currentMagazine].W = ArrWords[(ArrS[currentMagazine].L)].W;
                                Rules.Add(p10);
                                PrintInfo(currentSplittedWord, currentMagazine, Rules.Count, splittedWords);
                                go = 2;
                            }
                        }
                    }
                }
                if (go != 2 && go != 4)
                {
                    PrintCompileInfo.Invoke(@"Ошибка при выполнении восходящего разбора!");
                    //richTextBox2.Text = @"Ошибка при выполнении восходящего разбора!";
                    PrintCompileResult.Invoke("");
                    //textBox2.Text = "";
                    go = 3;
                    currentSplittedWord = 10000;
                    for (int i = 0; i < ArrS.Length; i++)
                    {
                        ArrS[i].L = 0;
                        ArrS[i].T = "";
                        ArrS[i].W = "";
                    }
                    //splittedWords = null;
                    for (int i = 0; i < Rules.Count; i++)
                    {
                        Rules[i] = 0;
                    }
                }
                if (go != 3)
                {
                    PrintCompileResult.Invoke(ArrS[1].T);
                    //textBox2.Text = ArrS[1].T;
                }
                if (go == 4)
                {
                    // финальная точка
                    // очистка нафиг не нужна
                    currentSplittedWord = 10000;
                }
                go = 0;
            }
        }
        public bool IsNumber(string str2, string dopstr, int k)
        {
            int pr2 = 0;
            for (var i = 0; i < dopstr.Length; i++)
            {
                if (str2[k + 1] == i) pr2 = pr2 + 1;
            }
            return pr2 == 0 ? false : true;
        }

        /// <summary>
        /// Проверяет значение на тип
        /// </summary>
        /// <param name="str">Строка, в которой осуществляется поиск</param>
        /// <param name="startPos">Начальная позиция, для поиска</param>
        /// <param name="endPos">Конечная позиция, для поиска</param>
        /// <param name="splittedWords"></param>
        /// <returns></returns>
        public int IsThisNumber(string str, int startPos, int endPos, List<MyWord> splittedWords)
        {
            //Проверка булевых переменных
            if (str.Substring(startPos, endPos + 1 - startPos) == " true" || str.Substring(startPos, endPos + 1 - startPos) == " false")
            {
                splittedWords.Add(new MyWord(17, str.Substring(startPos, endPos + 1 - startPos)));
                return 1;
            }
            //Проверка на число
            if (IsNumber(str, "0123456789", startPos))
            {
                int srchLength = 0;
                for (var i = startPos; i < endPos; i++)
                {
                    if (IsNumber(str, "0123456789.Ee-", i))
                        srchLength = srchLength + 1;
                }
                if (srchLength == endPos - startPos)
                {
                    splittedWords.Add(new MyWord(17, str.Substring(startPos, endPos + 1 - startPos)));

                    return 1;
                }
                PrintMessage.Invoke(@"Нужно вводить вещественные" + '\n' + @" числа с порядком!" + '\r' + @"Ошибка --> " + str.Substring(startPos, endPos + 1 - startPos));
                //MessageBox.Show(@"Нужно вводить вещественные" + '\n' + @" числа с порядком!" + '\r' + @"Ошибка --> " + str.Substring(startPos, endPos + 1 - startPos));
                return -1;
            }
            return 0;
        }

        /// <summary>
        /// Метод поиска (парсинга) входной строки
        /// Заполняется массив <see cref="ArrWords"/> нашими символами грамматики (:=, if, и т.д.)
        /// </summary>
        /// <param name="str">Строка, в которой осуществляется поиск</param>
        /// <param name="startPos">Начальная позиция, для поиска</param>
        /// <param name="endPos">Конечная позиция, для поиска</param>
        /// <param name="splittedWords"></param>
        /// <returns> Возвращяется номер символа в грамматике </returns>
        public int IsThisOperator(string str, int startPos, int endPos, List<MyWord> splittedWords)
        {
            if (str[startPos] == ' ') startPos += 1;
            for (var i = 6; i < 16; i++)
            {
                if (endPos - startPos + 1 == (ArrWords[i].W).Length)
                {
                    int srchLength = 0;
                    for (var j = 1; j <= (ArrWords[i].W).Length; j++)
                    {
                        if (str[startPos + j - 1] == ArrWords[i].W[j - 1]) srchLength = srchLength + 1;
                    }
                    if (endPos - startPos + 1 == srchLength)
                    {
                        splittedWords.Add(new MyWord(i, ArrWords[i].W));
                        return i;
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// Парсит входную строку и вызывает алгоритм
        /// </summary>
        /// <param name="str">Строка, для компиляции</param>
        public MyWord[] Up(string str)
        {
            // Коллекция строк, которые были получены в результате парсинга строки
            List<MyWord> splittedWords = new List<MyWord>();
        ArrS[1].T = "";

            str += " ";
            int nach = 0;
            int probel = 1;

            // Флаг ошибки при компиляции
            bool fail = false;

            for (var i = 0; i < str.Length; i++)
            {
                if (str[i] == ' ')
                    if (probel == 0)
                    {
                        probel = 1;
                        // Если вернулся 0, то будет производиться поиск на число или булевский тип
                        if (IsThisOperator(str, nach, i - 1, splittedWords) == 0)
                        {
                            int dop1 = IsThisNumber(str, nach, i - 1, splittedWords);
                            if (dop1 == 0)
                            {
                                if ((str.Substring(nach + 1, i - nach - 1)).Length <= 8 && (str.Substring(nach + 1, i - nach - 1)).Length > 0)
                                {
                                    splittedWords.Add(new MyWord(16, str.Substring(nach, i + 1 - nach)));
                                }
                                else
                                {
                                    if ((str.Substring(nach + 1, i - nach - 1)).Length > 8)
                                    {
                                        fail = true;
                                        PrintMessage.Invoke(@"Длина идентификатора не может быть больше 8 символов!" + '\n' + @"Ошибка --> " + str.Substring(nach, i + 1 - nach));
                                        //MessageBox.Show(@"Длина идентификатора не может быть больше 8 символов!" + '\n' + @"Ошибка --> " + str.Substring(nach, i + 1 - nach));
                                    }
                                    if ((str.Substring(nach + 1, i - nach - 1)).Length == 0)
                                    {
                                        fail = true;
                                        PrintMessage.Invoke(@"Длина идентификатора не может быть меньше 0 символов!" + '\n' + @"Сивмол № " + (i + 1).ToString() + @" является пробелом.");
                                        //MessageBox.Show(@"Длина идентификатора не может быть меньше 0 символов!" + '\n' + @"Сивмол № " + (i + 1).ToString() + @" является пробелом.");
                                    }
                                }
                            }
                            if (dop1 == -1)
                            {
                                fail = true;
                            }
                        }
                    }
                if (!fail)
                {
                    if (probel == 1) nach = i;
                    probel = 0;
                }
            }
            if (!fail)
            {
                splittedWords.Add(new MyWord(18, "$"));
                return splittedWords.ToArray();
            }
            return null;
        }

        public void Run(string str)
        {
            for (var i = 0; i < ArrS.Length; i++)
            {
                ArrS[i].L = 0;
                ArrS[i].T = "";
                ArrS[i].W = "";
            }
            Rules.Clear();

            MyWord[] words = Up(str);
            if (words != null)
            {
                Algorithm(words);
            }
            
        }
    }
}
