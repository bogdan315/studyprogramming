using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace DTOs
{
    [Serializable]

    public class Car
    {
        public int Year { get; set; }
        public string Make { get; set; }

        public Car() { }

        public Car(int Year, string Make) 
        {
            this.Year = Year;
            this.Make = Make;
        }
    }
}


