using System;

namespace Transaction
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var aeroDal = new AeroDal())
            {
                //var result = aeroDal.RegisterPassenger(1100, "Ramis", 150, "Las-Vegas");
                var result = aeroDal.GetPassengersByTripName("Rostov");

                Console.WriteLine(result);
            }
        }
    }
}
