using System;

namespace ProjetXamarinWear.Models
{
    public class Item
    {
        public string id { get; set; }
        public long student_id { get; set; }
        public double gps_lat { get; set; }
        public double gps_long { get; set; }
        public string student_message { get; set; }
    }
}