using Project_Dev_Test.Core.SharedKernel;

namespace Project_Dev_Test.Core.Entities
{
    public class ImageEntry : BaseEntity
    {
        public List<string> X_Vector { get; set; }
        public List<string> Y_Vector { get; set; }
        public DateTime DateTimeCreated { get; set; } //= DateTime.UtcNow;
    }
}
