using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Метод_потенциалов__курсовая_
{
    class Program
    {
        static double min;
        static int min_i;
        static int min_j;
        static void Main(string[] args)
        {
            Console.WriteLine("Методы оптимизации. Курсовая работа студентки гр. АВ-121 Кумар Татьяны");
            Console.WriteLine();
            StreamReader sr1 = new StreamReader("Массив_с_a[i].txt");//считываем массив А
            int m_vectorA = Int32.Parse(sr1.ReadLine());
            string vector1 = sr1.ReadToEnd();
            string[] separ = { " ", "\r", "\n", "" };
            string[] V_a = vector1.Split(separ, StringSplitOptions.RemoveEmptyEntries);
            double[] A1 = new double[m_vectorA];//массив со значениями а[i], т.е.со значениями объемов производства продукта в каждом из пунктов производства
            double[] A2 = new double[m_vectorA];//массив со значениями а[i], т.е.со значениями объемов производства продукта в каждом из пунктов производства
            for (int i = 0; i < m_vectorA; i++)
            {
                A1[i] = Convert.ToDouble((V_a[i]));
                A2[i] = Convert.ToDouble((V_a[i]));
            }
            Console.WriteLine("Объемы продукта, производимые в каждом из пунктов производства");
            PrintArray(A1, m_vectorA); //вызов функции вывода на экран одномерного массива
            StreamReader sr2 = new StreamReader("Массив_с_b[j].txt");//считываем массив B
            int n_vectorB = Int32.Parse(sr2.ReadLine());
            string vector2 = sr2.ReadToEnd();
            string[] V_b = vector2.Split(separ, StringSplitOptions.RemoveEmptyEntries);
            double[] B1 = new double[n_vectorB];//массив со значениями b[j], т.е.со значениями объемов потребления продукта в каждом из пунктов потребления
            double[] B2 = new double[n_vectorB];//массив со значениями b[j], т.е.со значениями объемов потребления продукта в каждом из пунктов потребления
            for (int j = 0; j < n_vectorB; j++)
            {
                B1[j] = Convert.ToDouble((V_b[j]));
                B2[j] = Convert.ToDouble((V_b[j]));
            }
            Console.WriteLine("Объемы продукта, необходимые в каждом из пунктов потребления");
            PrintArray(B1, n_vectorB); //вызов функции вывода на экран одномерного массива
            StreamReader sr3 = new StreamReader("Массив_с_С[i,j].txt");//считываем матрицу стоимостей С
            string vector3 = sr3.ReadToEnd();
            string[] V_C = vector3.Split(separ, StringSplitOptions.RemoveEmptyEntries);
            double[,] C = new double[m_vectorA, n_vectorB];//массив со значениями b[j], т.е.со значениями объемов потребления продукта в каждом из пунктов потребления
            int k = 0;
            for (int i = 0; i < m_vectorA; i++)
                for (int j = 0; j < n_vectorB; j++)
                {
                    C[i, j] = Convert.ToDouble((V_C[k]));
                    k++;
                }
            Console.WriteLine("Матрица стоимостей перевозок из каждого пункта производства ");
            Console.WriteLine("в каждый пункт потребления");
            Print2DArray(C, n_vectorB);//выводим матрицу стоимостей на экран
            SZU(A1, B1, C, m_vectorA, n_vectorB);//вызов метода северо-западного угла
            MethodNaimElementov(A2, B2, C, m_vectorA, n_vectorB);//вызов метода наименьших элементоы
            Console.ReadLine();
        }


        private static void SZU(double[] A1, double[] B1, double[,] C1, int m_vectorA, int n_vectorB)//определение опорного плана методом северо-западного угла
        {
            double F = 0;
            int iteration_counter_SZU = 0;
            Console.WriteLine();
            Console.WriteLine("__________________________________");
            Console.WriteLine();
            Console.WriteLine("\t\tМЕТОД СЕВЕРО-ЗАПАДНОГО УГЛА");
            Console.WriteLine();
            double[,] C_ishodn = new double[m_vectorA, n_vectorB];//в этот массив продублируем матрицу стоимостей, т.к. мы ее в массиве С1 будем видоизменять
            for (int i = 0; i < m_vectorA; i++)
                for (int j = 0; j < n_vectorB; j++)
                    C_ishodn[i, j] = C1[i, j];

            double[,] Q1 = new double[m_vectorA, n_vectorB];
            for (int i = 0; i < m_vectorA; i++)
                for (int j = 0; j < n_vectorB; j++)
                    Q1[i, j] = 0;

            for (int i = 0; i < m_vectorA; i++)
                for (int j = 0; j < n_vectorB; j++)

                    if (B1[j] < A1[i])
                    {
                        Q1[i, j] = B1[j];
                        A1[i] = A1[i] - B1[j];
                        B1[j] = 0;
                    }
                    else
                    {
                        Q1[i, j] = A1[i];
                        B1[j] = B1[j] - A1[i];
                        A1[i] = 0;
                    }
            iteration_counter_SZU++;//1й шаг выполнен
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("\tИТЕРАЦИЯ {0}:", iteration_counter_SZU.ToString());
            Console.WriteLine();
            Console.WriteLine("Опорный план:");
            Print2DArray(Q1, n_vectorB);
            for (int i = 0; i < m_vectorA; i++)
                for (int j = 0; j < n_vectorB; j++)
                    F += Q1[i, j] * C_ishodn[i, j]; //Считаем значение целевой функции
            Console.WriteLine();
            Console.WriteLine("Значение целевой функции F= {0}", F.ToString());
            Console.WriteLine();

            PodschetDelta(Q1, C_ishodn, m_vectorA, n_vectorB, iteration_counter_SZU);
        }


        private static void PodschetDelta(double[,] Q1, double[,] C_ishodn, int m_vectorA, int n_vectorB, int iteration_counter)
        {
            double F = 0;
            double[,] delta = new double[m_vectorA, n_vectorB];
            bool planOptimalen;
            double max;
            int i_max;
            int j_max;

            double[] U = new double[m_vectorA];
            double[] V = new double[n_vectorB];

        startPodschetDelta:

            Console.WriteLine();
            Console.WriteLine("Проверим опорный план на оптимальность.");
            Console.WriteLine();
            for (int i = 0; i < m_vectorA; i++)
            {
                U[i] = 0;
                V[i] = 0;
            }

            Find_U_V(U, V, 0, Q1, C_ishodn, m_vectorA, n_vectorB);//находим все U и все V

            Console.WriteLine("Значения U:");
            PrintArray(U, m_vectorA);
            Console.WriteLine("Значения V:");
            PrintArray(V, n_vectorB);

            for (int i = 0; i < m_vectorA; i++)//считаем дельта (потенциалы элементов со значением 0 в опорном плане)
                for (int j = 0; j < n_vectorB; j++)
                    if (Q1[i, j] == 0)
                        delta[i, j] = U[i] + V[j] - C_ishodn[i, j];
                    else delta[i, j] = 0;
            Console.WriteLine("Матрица, содержащая потенциалы (дельта[i,j]) нулевых элементов опорного плана ");
            Console.WriteLine("(значениям 0 чаще всего здесь соответствуют по месту расположения ");
            Console.WriteLine("ненулевые элементы - особой смысловой нагрузки они здесь не несут):");
            Console.WriteLine();
            Print2DArray(delta, n_vectorB);

            planOptimalen = true;
            for (int i = 0; i < m_vectorA; i++)
                for (int j = 0; j < n_vectorB; j++)
                    if (delta[i, j] > 0)
                        planOptimalen = false;
            max = 0;
            i_max = -1;
            j_max = -1;
            for (int i = 0; i < m_vectorA; i++)
                for (int j = 0; j < n_vectorB; j++)
                    if (delta[i, j] > max)
                    {
                        max = delta[i, j];
                        i_max = i;
                        j_max = j;
                    }


            while (planOptimalen == false)//пересчитываем опорный план
            {
                iteration_counter = PereschetOpornPlana(i_max, j_max, Q1, m_vectorA, n_vectorB, C_ishodn, iteration_counter);
                goto startPodschetDelta;
            }
            Console.WriteLine("Среди потенциалов элементов опорного плана, равных нулю, нет положительных.");
            Console.WriteLine("Опорный план улучшить нельзя.");
            Console.WriteLine();
            Console.WriteLine("НАЙДЕНО ОПТИМАЛЬНОЕ РЕШЕНИЕ:");
            Print2DArray(Q1, n_vectorB);
            for (int i = 0; i < m_vectorA; i++)
                for (int j = 0; j < n_vectorB; j++)
                    F += Q1[i, j] * C_ishodn[i, j]; //Считаем значение целевой функции
            Console.WriteLine();
            Console.WriteLine("Значение целевой функции F(опт.)= {0}", F.ToString());
            Console.WriteLine();
            Console.WriteLine("Количество итераций: {0}", iteration_counter.ToString());

        }

        private static void Find_U_V(double[] U, double[] V, int x, double[,] Q1, double[,] C_ishodn, int m_vectorA, int n_vectorB)
        {
            for (int j = 0; j < n_vectorB; j++)
                if (Q1[x, j] > 0)
                    if (V[j] == 0)
                    {
                        V[j] = C_ishodn[x, j] - U[x];
                        for (int i = 0; i < m_vectorA; i++)
                            if (Q1[i, j] > 0)
                                if (U[i] == 0)
                                {
                                    U[i] = C_ishodn[i, j] - V[j];
                                    Find_U_V(U, V, i, Q1, C_ishodn, m_vectorA, n_vectorB);
                                }
                    }
        }


        private static int PereschetOpornPlana(int i_max, int j_max, double[,] Q1, int m_vectorA, int n_vectorB, double[,] C, int iteration_counter)
        {
            Console.WriteLine("Среди потенциалов элементов опорного плана, равных нулю, есть положительные.");
            Console.WriteLine("Опорный план можем улучшить.");
            Console.WriteLine();
            double F = 0;
            Console.ReadLine();
            StreamReader sr4 = new StreamReader("ПлюсМинус.txt");
            string vectorPlMin = sr4.ReadToEnd();
            string[] separ = { " ", "\r", "\n", "" };
            string[] vector_PlusMinus = vectorPlMin.Split(separ, StringSplitOptions.RemoveEmptyEntries);
            double[,] PlusMinus = new double[m_vectorA, n_vectorB];

            int p = 0;
            for (int w = 0; w < m_vectorA; w++)
                for (int t = 0; t < n_vectorB; t++)
                {
                    PlusMinus[w, t] = Convert.ToDouble(vector_PlusMinus[p]);
                    p++;
                }// считали все плюсы и минусы. Далее с их учетом делаем пересчет опорного плана

            
            Console.WriteLine("Места расположения + и - для пересчета (улучшения) опорного плана:");
            Console.WriteLine();
            Print2DArray(PlusMinus, n_vectorB);

            double minElemSoZnakMinus = 0;
            for (int i = 0; i < m_vectorA; i++)
                for (int j = 0; j < n_vectorB; j++)
                    if (PlusMinus[i, j] == -1)
                        minElemSoZnakMinus = Q1[i, j];

            for (int i = 0; i < m_vectorA; i++)
                for (int j = 0; j < n_vectorB; j++)
                    if (PlusMinus[i, j] == -1)
                        if (Q1[i, j] < minElemSoZnakMinus)
                            minElemSoZnakMinus = Q1[i, j];//нашли минимальный элемент опорного плана, отмеченый знаком -


            for (int i = 0; i < m_vectorA; i++)
                for (int j = 0; j < n_vectorB; j++)
                {
                    if (PlusMinus[i, j] == -1)
                        Q1[i, j] -= minElemSoZnakMinus;
                    if (PlusMinus[i, j] == 1)
                        Q1[i, j] += minElemSoZnakMinus;//пересчитали элементы опорного плана
                }
            sr4.Close();
            iteration_counter++;
            Console.WriteLine("____________________");
            Console.WriteLine();
            Console.WriteLine("\tИТЕРАЦИЯ {0}:", iteration_counter.ToString());
            Console.WriteLine();
            Console.WriteLine("Опорный план:");
            Print2DArray(Q1, n_vectorB);
            for (int i = 0; i < m_vectorA; i++)
                for (int j = 0; j < n_vectorB; j++)
                    F += Q1[i, j] * C[i, j]; //Считаем значение целевой функции
            Console.WriteLine();
            Console.WriteLine("Значение целевой функции F= {0}", F.ToString());
            Console.WriteLine();
            return iteration_counter;
        }
        private static void MethodNaimElementov(double[] A1, double[] B1, double[,] C1, int m_vectorA, int n_vectorB)//определение опорного плана методом наименьших элементов
        {
            double F = 0;
            int iteration_counter_MethodNaimElem = 0;
            Console.WriteLine();
            Console.WriteLine("__________________________________");
            Console.WriteLine();
            Console.WriteLine("\t\tМЕТОД МИНИМАЛЬНОГО ЭЛЕМЕНТА");
            double[,] C_ishodn = new double[m_vectorA, n_vectorB];//в этот массив продублируем матрицу стоимостей, т.к. мы ее в массиве С1 будем видоизменять
            for (int i = 0; i < m_vectorA; i++)
                for (int j = 0; j < n_vectorB; j++)
                    C_ishodn[i, j] = C1[i, j];

            double[,] Q2 = new double[m_vectorA, n_vectorB];
            for (int i = 0; i < m_vectorA; i++)
                for (int j = 0; j < n_vectorB; j++)
                    Q2[i, j] = 0;

            FindMinFromC(C1, m_vectorA, n_vectorB);
            bool ProdoljMetod = true;
            while (ProdoljMetod == true)
            {
                ProdoljMetod = false;
                if (B1[min_j] < A1[min_i])
                {
                    Q2[min_i, min_j] = B1[min_j];
                    A1[min_i] = A1[min_i] - B1[min_j];
                    B1[min_j] = 0;
                    for (int k = 0; k < n_vectorB; k++)
                        C1[k, min_j] = -1;//сделали элементы всего столбца отрицательными,чтобы потом этот столбец в матрице стоимостей при последующих поисках min эл-та игнорировалась
                }
                else
                {
                    Q2[min_i, min_j] = A1[min_i];
                    B1[min_j] = B1[min_j] - A1[min_i];
                    A1[min_i] = 0;
                    for (int k = 0; k < n_vectorB; k++)
                        C1[min_i, k] = -1;//сделали элементы всей строки отрицательными,чтобы потом эта строка в матрице стоимостей при последующих поисках min эл-та игнорировалась
                }

                FindMinFromC(C1, m_vectorA, n_vectorB);

                for (int i = 0; i < m_vectorA; i++)
                    if (A1[i] > 0 | B1[i] > 0)
                        ProdoljMetod = true;//продолжить выполнение метода, если есть хоть один элемент матрицы А или В больше нуля
            }

            for (int i = 0; i < m_vectorA; i++)
                for (int j = 0; j < n_vectorB; j++)
                    F += Q2[i, j] * C_ishodn[i, j]; //Считаем значение целевой функции
            iteration_counter_MethodNaimElem++;
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("\tИТЕРАЦИЯ {0}:", iteration_counter_MethodNaimElem.ToString());
            Console.WriteLine();
            Console.WriteLine("Опорный план:");
            Print2DArray(Q2, n_vectorB);
            Console.WriteLine();
            Console.WriteLine("Значение целевой функции F= {0}", F.ToString());
            Console.WriteLine();
            PodschetDelta(Q2, C_ishodn, m_vectorA, n_vectorB, iteration_counter_MethodNaimElem);
        }



        private static void FindMinFromC(double[,] C, int m, int n)//метод, который ищет минимальный элемент матрицы стоимостей 
        {

            min = C[0, 0];
            min_i = 0;
            min_j = 0;
            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                    if (C[i, j] >= 0)
                    {
                        min = C[i, j];
                        min_i = i;
                        min_j = j;
                        break;
                    }
            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                    if (C[i, j] >= 0)//отрицательными дальше будем делать те строки(столбцы), в которых а=0(b=0)
                    {
                        if (C[i, j] < min)
                        {
                            min = C[i, j];
                            min_i = i;
                            min_j = j;
                        }
                    }
        }

        private static void Print2DArray(double[,] array, int n) //функция для вывода на экран двумерной матрицы
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write(array[i, j] + "    ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private static void PrintArray(double[] array, int n) //функция вывода на экран вектора
        {
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine(array[i]);
            }
            Console.WriteLine();
        }

    }
}
