using System;

namespace Graphout
{
    public class PhoutRow
    {
        public double time { get; set; }
        public string tag { get; set; }
        public float interval_real { get; set; }
        public float connect_time { get; set; }
        public float send_time { get; set; }
        public float latency { get; set; }
        public float receive_time { get; set; }
        public float interval_event { get; set; }
        public float size_out { get; set; }
        public float size_in { get; set; }
        public int net_code { get; set; }
        public int proto_code { get; set; }
    }
}
