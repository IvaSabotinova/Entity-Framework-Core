namespace Artillery.DataProcessor.ExportDto
{
    public class ShellOutputModel
    {
        public double ShellWeight { get; set; }
        public string Caliber { get; set; }
        public GunJsonOutputModel[] Guns { get; set; }
    }

    public class GunJsonOutputModel
    {
        public string GunType { get; set; }
        public int GunWeight { get; set; }
        public double BarrelLength { get; set; }
        public string Range { get; set; }
        
    }
}
