using ProtoBuf;
using System;

namespace Shared
{
    /// <summary>
    /// A payload of various data types
    /// </summary>
    [ProtoContract]
    public class MixedPayload
    {
        /// <summary>
        /// Id of the object
        /// </summary>
        [ProtoMember(1)]
        public int Id { get; set; }

        /// <summary>
        /// Current local date time
        /// </summary>
        [ProtoMember(2)]
        public DateTime CurrentTime { get; set; }

        /// <summary>
        /// Number of Processors on the machine
        /// </summary>
        [ProtoMember(3)]
        public int ProcessorCount { get; set; }

        /// <summary>
        /// Number of Processors on the machine
        /// </summary>
        [ProtoMember(4)]
        public string OSVersion { get; set; }

        public static MixedPayload Create(int id)
        {
            var payload = new MixedPayload()
            {
                Id = id,
                CurrentTime = DateTime.Now,
                ProcessorCount = Environment.ProcessorCount,
                OSVersion = Environment.OSVersion.ToString()
            };
            return payload;
        }
    }
}
