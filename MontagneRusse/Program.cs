using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MontagneRusse
{
    class Program
    {
        static void Main(string[] args)
        {
            var currentStage = 6;
#if (!DEBUG)
            string[] inputs = Console.ReadLine().Split(' ');
#else
            string[] inputs = InputData.GetInput(currentStage).Split(' ');
#endif

            int NumberOfSeats = int.Parse(inputs[0]);
            int Capacity = int.Parse(inputs[1]);
            int NumberOfGroups = int.Parse(inputs[2]);

            var queue = new Queue<Group>();
            Enumerable.Range(0, NumberOfGroups)
                .ToList()
                .ForEach(i =>
                {
#if (!DEBUG)
                    queue.Enqueue(new Group { Lenght = int.Parse(Console.ReadLine()) } );
#else
                    queue.Enqueue(new Group { Lenght = int.Parse(InputData.GetStage(currentStage)()[i]) });
#endif
                });

            var rideService = new RideService { Ride = new Ride { MaxCapacity = Capacity, MaxNumberOfSeats = NumberOfSeats }, Queue = queue, NumberOfGroups = NumberOfGroups };
            //rideService.Previsualisation();
            //rideService.DoWork();

            var betterRideSerice = new BetterRideService { Ride = new Ride { MaxCapacity = Capacity, MaxNumberOfSeats = NumberOfSeats }, Queue = queue.ToList(), NumberOfGroups = NumberOfGroups };
            // betterRideSerice.Previsualisation();
            betterRideSerice.DoWork();
            // Write an action using Console.WriteLine()
            // To debug: Console.Error.WriteLine("Debug messages...");

            Console.WriteLine(betterRideSerice.Money);
        }
    }

    public class Ride
    {
        /// <summary>
        /// Le nombre de tour possible
        /// </summary>
        public int MaxCapacity { get; set; }
        /// <summary>
        /// Le nombre de siège disponible
        /// </summary>
        public int MaxNumberOfSeats { get; set; }
    }

    public class Group
    {
        public bool OnRide { get; set; } = false;
        public int Lenght { get; set; }
        public Guid Id { get; set; } = Guid.NewGuid();
    }

    public class RideService
    {
        /// <summary>
        /// Le mananège
        /// </summary>
        public Ride Ride { get; set; }
        /// <summary>
        /// L'argent gagné
        /// </summary>
        public long Money { get; set; } = 0;
        /// <summary>
        /// La file d'attente
        /// </summary>
        public Queue<Group> Queue { get; set; }

        public int NumberOfTurn { get; private set; } = 0;
        public int AvailableSeat { get; set; }

        public int NumberOfGroups { get; set; }

        public void Previsualisation()
        {
            char quote = '"';
            Console.Error.WriteLine($"MaxNumberOfSeats : {Ride.MaxNumberOfSeats} - MaxCapacity : {Ride.MaxCapacity} - NbGroup : {Queue.Count}");
            // Console.Error.WriteLine($"MaxNumberOfSeats : {Ride.MaxNumberOfSeats} - MaxCapacity : {Ride.MaxCapacity} - NbGroup : {Queue.Count} - Queue {String.Join(", ", Queue.ToList().Select(i => $"{quote}{i.Lenght}{quote}"))}");
        }

        public virtual void DoWork()
        {
            // https://fr.wikipedia.org/wiki/Algorithme_du_li%C3%A8vre_et_de_la_tortue


            var sw = new Stopwatch();
            sw.Start();
            var hs = new HashSet<Guid>();
            for (long i = 0; i < Ride.MaxCapacity; i++)
            {
                // Console.Error.WriteLine($"({i}) New turn will start !");
                Reset();
                hs.Clear();
                while (AvailableSeat > 0 && Queue.Peek().Lenght <= AvailableSeat && hs.Add(Queue.Peek().Id))
                {
                    var currentGroup = Queue.Dequeue();
                    AvailableSeat -= currentGroup.Lenght;
                    Money += currentGroup.Lenght;
                    Queue.Enqueue(currentGroup);
                    hs.Add(currentGroup.Id);
                    // Console.Error.WriteLine($"{currentGroup.Lenght} get in the ride ");
                }
                // Console.Error.WriteLine($"Ellapsed time for adding 1 group : {sw.Elapsed.TotalSeconds}");

            }
            sw.Stop();
            Console.Error.WriteLine($"Ellapsed time : {sw.Elapsed.TotalSeconds}");
        }

        public void Reset()
        {
            this.AvailableSeat = this.Ride.MaxNumberOfSeats;
            // trop lent
            //Queue.ToList().ForEach(g => g.OnRide = false);
        }
    }

    public class BetterRideService
        : RideService
    {
        public new List<Group> Queue { get; set; }

        public override void DoWork()
        {
            var maxLoop = Ride.MaxCapacity;
            var previousResults = new Dictionary<int, Memory>();
            var i = 0;
            while(maxLoop > 0)
            {
                this.Reset();
                if(previousResults.ContainsKey(i))
                {
                    var currentResult = previousResults[i];
                    Money += currentResult.MoneyEarned;
                    i = currentResult.Index;
                } 
                else
                {
                    var currentIndexCycle = i;
                    var moneyBeforeCycle = Money;
                    int numberOfGroupInside = 0; 

                    while(AvailableSeat >= Queue[i].Lenght)
                    {
                        if (numberOfGroupInside >= this.NumberOfGroups)
                            break;

                        numberOfGroupInside++;

                        var currentGroup = Queue[i];

                        int numberOfPerson = currentGroup.Lenght;
                        Money += numberOfPerson;
                        this.AvailableSeat -= numberOfPerson;

                        i++;

                        if (i >= NumberOfGroups)
                            i = 0;
                    }
                    previousResults.Add(currentIndexCycle, new Memory { MoneyEarned = Money - moneyBeforeCycle, Index = i });
                }


                maxLoop--;
            }

        }
    }


    public class Memory
    {
        public long MoneyEarned { get; set; }
        public int Index { get; set; }
    }
}
