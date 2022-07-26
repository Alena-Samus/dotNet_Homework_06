﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;

namespace hwless6
{
    class Program
    {
/// <summary>
/// Метод для подсчета количества групп
/// </summary>
/// <param name="currentNumber">Считанное из файла число и переданное в качестве параметра</param>
/// <returns></returns>
    static int seeNumber(int currentNumber)
        {
            int m = 0;
            //Все числа, которые больше, чем N/2 относятся к одной группе, т.к. они не могут делиться друг на друга без остатка
            do
            {
                m++;
                currentNumber /= 2;
                
            } while (currentNumber >= 1);
            return m;
        }

    /// <summary>
    /// Метод для записи групп в файл
    /// </summary>
    /// <param name="currentNumber">В качестве параметра принимается считанное из файла число</param>
    static void writeGroups(int currentNumber)
        {//Поток для записи результатов в файл "result.txt"
            //true - разрешение перезаписи
            using (StreamWriter sr = new StreamWriter("result.txt", true, Encoding.UTF8))//using открывает и закрывает поток
            {
                int m = 0;
                do
                {
                    
                    sr.Write($"Группа {m + 1}: ");//Записываем в поток (= в файл)
                    for (int i = currentNumber / 2 + 1; i <= currentNumber; i++)
                    {
                        sr.Write($"{i} ");
                    }
                    m++;
                    currentNumber /= 2;
                    sr.WriteLine();
                } while (currentNumber >= 1);




            }
        }
        /// <summary>
        /// Метод для сжатия файли
        /// </summary>
        static void comp()
        {
            using (FileStream originalFile = new FileStream(@"result.txt", FileMode.OpenOrCreate))//Поток для считывания файла
            {
                using (FileStream writeCompress = File.Create(@"result.zip"))//Поток для записи в файл сжатого потока
                {
                    using (GZipStream zipFile = new GZipStream(writeCompress, CompressionMode.Compress))//Поток для сжатия (обязательно подключить using System.IO.Compression;
                    {
                        originalFile.CopyTo(zipFile);//Копируем исходный файл в сжимаемый поток
                        Console.WriteLine($"Сжатие файла завершено. Было: {originalFile.Length} Стало: {writeCompress.Length}");
                    }
                }
            }
        }
        static void Main(string[] args)
        {

            /// Домашнее задание
            ///
            /// Группа начинающих программистов решила поучаствовать в хакатоне с целью демонстрации
            /// своих навыков. 
            /// 
            /// Немного подумав они вспомнили, что не так давно на занятиях по математике
            /// они проходили тему "свойства делимости целых чисел". На этом занятии преподаватель показывал
            /// пример с использованием фактов делимости. 
            /// Пример заключался в следующем: 
            /// Написав на доске все числа от 1 до N, N = 50, преподаватель разделил числа на несколько групп
            /// так, что если одно число делится на другое, то эти числа попадают в разные руппы. 
            /// В результате этого разбиения получилось M групп, для N = 50, M = 6
            /// 
            /// N = 50
            /// Группы получились такими: 
            /// 
            /// Группа 1: 1
            /// Группа 2: 2 3 5 7 11 13 17 19 23 29 31 37 41 43 47
            /// Группа 3: 4 6 9 10 14 15 21 22 25 26 33 34 35 38 39 46 49
            /// Группа 4: 8 12 18 20 27 28 30 42 44 45 50
            /// Группа 5: 16 24 36 40
            /// Группа 6: 32 48
            /// 
            /// M = 6
            /// 
            /// ===========
            /// 
            /// N = 10
            /// Группы получились такими: 
            /// 
            /// Группа 1: 1
            /// Группа 2: 2 7 9
            /// Группа 3: 3 4 10
            /// Группа 4: 5 6 8
            /// 
            /// M = 4
            /// 
            /// Участники хакатона решили эту задачу, добавив в неё следующие возможности:
            /// 1. Программа считыват из файла (путь к которому можно указать) некоторое N, 
            ///    для которого нужно подсчитать количество групп
            ///    Программа работает с числами N не превосходящими 1000000000
            ///   
            /// 2. В ней есть два режима работы:
            ///   2.1. Первый - в консоли показывается только количество групп, т е значение M
            ///   2.2. Второй - программа получает заполненные группы и записывает их в файл используя один из
            ///                 вариантов работы с файлами
            ///            
            /// 3. После выполения пунктов 2.1 или 2.2 в консоли отображается время, за которое был выдан результат 
            ///    в секундах и миллисекундах
            /// 
            /// 4. После выполнения пунта 2.2 программа предлагает заархивировать данные и если пользователь соглашается -
            /// делает это.
            /// 
            /// Попробуйте составить конкуренцию начинающим программистам и решить предложенную задачу
            /// (добавление новых возможностей не возбраняется)
            ///
            /// * При выполнении текущего задания, необходимо документировать код 
            ///   Как пометками, так и xml документацией
            ///   В обязательном порядке создать несколько собственных методов
                   
            
            //Считываем информацию из файла
            int n = Convert.ToInt32(File.ReadAllText(@"count.txt"));
            Console.WriteLine($"Файл считан. N = {n} \nДля продолжения выберите режим (1/2):\n1 - просмотреть количество групп; " +
                $"\n2 - получить заполненные группы в файл. \nДля выбора режима введите соответствующую цифру.");

          
            //Удаляем предыдущий архив, если он есть
            File.Delete("result.zip");

            //Пользователь выбирает режим работы
            byte mode = Convert.ToByte(Console.ReadLine());
            Console.WriteLine($"Вы выбрали режим {mode}");
            
            //Начинаем отсчет времени
            DateTime beginTime = DateTime.Now;
            
            //Режим 1 - просмотр количества групп
            if (mode == 1)
            {
                Console.WriteLine($"Количество групп:  {seeNumber(n)}");
            }
           
            //Режим 2 - запись результатов в файл
            else
            {
                //Вызов метода да записи результатов в файл
                writeGroups(n);

                //Остановить отсчет времени выполнения программы
                DateTime endTime = DateTime.Now;

                TimeSpan span = endTime - beginTime;
                Console.WriteLine($"Операция выполнена.\nВремя выполнения: {span.Milliseconds} милисекунд или {span.Seconds} секунд");

                Console.WriteLine($"Заархивировать файл (д/н)");
                char compress = Convert.ToChar(Console.ReadLine());
               
                //Вызов метода для архивации файла при выборе пользователя
                if (compress == 'д')
                {
                    comp();
                }
            }
                 


            Console.ReadLine();
        }
    }
}
