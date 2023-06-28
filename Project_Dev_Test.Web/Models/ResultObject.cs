namespace Project_Dev_Test.Web.Models
{
    public class ResultObject
    {
        public Guid Id { get; set; }

        public int User { get; set; }
        public string Image { get; set; }

        public float CPU { get; set; }
        public float Memory { get; set; } // in bytes
        public double TimeElapsed { get; set; } // time in ms
        public float Iterations { get; set; }

        public DateTime StartOperation { get; set; }
        public DateTime EndOperation { get; set; }
    }
}
