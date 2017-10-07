using ProtoBuf;
using System;

namespace Shared
{
    /// <summary>
    /// Example for a number-heavy payload
    /// </summary>
    [ProtoContract]
    public class NumericPayload
    {
        [ProtoMember(1)]
        
        public long Id;

        [ProtoMember(2)]
        public double[] Doubles;

        public static NumericPayload Create(int arraySize)
        {
            var seed = DateTime.Now.Ticks % 100;
            //Initialize to some mid-sized values
            var numbers = new double[arraySize];
            for (int i = 0; i < arraySize; i++)
            {
                numbers[i] = seed * 500.987234 + i;
            }
            var payload = new NumericPayload()
            {
                Id = seed,
                Doubles = numbers
            };

            return payload;
        }
    }
}
