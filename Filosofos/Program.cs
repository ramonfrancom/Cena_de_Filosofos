using System;
using System.Threading;

namespace Filosofos
{
    class Program
    {
        static int numberPhylosophersAndForks = 5;
        static Thread[] phylosophers = new Thread[numberPhylosophersAndForks];
        static Semaphore[] forks = new Semaphore[numberPhylosophersAndForks];
        static readonly object writtingLock = new object();
        static void Main(string[] args)
        {
            DrawInitialInterface();
            //Inicializacion de Semaforos
            for (int i = 0; i < numberPhylosophersAndForks; i++)
            {
                forks[i] = new Semaphore(1, 1);
            }


            //Inicializacion de Hilos
            for (int j = 0; j < numberPhylosophersAndForks; j++)
            {
                phylosophers[j] = new Thread(functionPhylosopher);
                phylosophers[j].Start(j);
            }

        }

        static void functionPhylosopher(Object ObjectPhylosopherPosition)
        {
            int phylosopherPosition = (int)ObjectPhylosopherPosition;

            int meals = 0;
            int position = phylosopherPosition;
            int forkPosition1 = phylosopherPosition;
            int forkPosition2 = (phylosopherPosition == 0 ? 4 : phylosopherPosition - 1);

            Random rng = new Random();

            while (meals < 5)
            {
                DrawStateValue(position, "Esperando");
                bool isFork1Available = forks[forkPosition1].WaitOne(TimeSpan.FromSeconds(4));
                bool isFork2Available = forks[forkPosition2].WaitOne(TimeSpan.FromSeconds(4));


                if (isFork1Available && isFork2Available)
                {
                    DrawStateValue(position, "Comiendo");

                    int waitTime = rng.Next(1, 3);
                    while (waitTime > 0)
                    {
                        DrawTimeLeft(position, waitTime);
                        Thread.Sleep(1000);
                        waitTime--;
                    }
                    DrawTimeLeft(position, waitTime);

                    meals++;
                    DrawMeals(position, meals);
                }

                if (isFork1Available)
                {
                    forks[forkPosition1].Release();
                }

                if (isFork2Available)
                {
                    forks[forkPosition2].Release();
                }
                DrawStateValue(position, "Esperando");
            }
        }
        static void DrawStateValue(int idPhylosopher, string state)
        {

            int line = 3 + idPhylosopher;
            lock (writtingLock)
            {
                Console.SetCursorPosition(4, line);
                Console.WriteLine($"{idPhylosopher + 1}.");
                Console.SetCursorPosition(11, line);
                Console.WriteLine($"{state}");
            }

        }

        static void DrawTimeLeft(int idPhylosopher, int time)
        {
            int line = 3 + idPhylosopher;
            lock (writtingLock)
            {
                Console.SetCursorPosition(24, line);
                Console.WriteLine($"{time}");
            }
        }
        static void DrawMeals(int idPhylosopher, int meals)
        {
            int line = 3 + idPhylosopher;
            lock (writtingLock)
            {
                Console.SetCursorPosition(30, line);
                for (int i = 0; i < meals; i++)
                {
                    Console.Write($"X ");
                }
            }
        }

        static void DrawInitialInterface()
        {
            Console.SetCursorPosition(50, 0);
            Console.WriteLine("Cena de Fílosofos");
            // ----------- Fílosofos ------------------- //
            Console.SetCursorPosition(1, 2);
            Console.WriteLine("Fílosofos");
            Console.SetCursorPosition(11, 2);
            Console.WriteLine("Estado");
            Console.SetCursorPosition(22, 2);
            Console.WriteLine("Tiempo");
            Console.SetCursorPosition(30, 2);
            Console.WriteLine("Comidas");

        }
    }
}
