using P01_StudentSystem.Data;

namespace P01_StudentSystem
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            StudentSystemContext studentSystemContext = new StudentSystemContext();
            studentSystemContext.Database.EnsureDeleted();
            studentSystemContext.Database.EnsureCreated();


        }
    }
}
