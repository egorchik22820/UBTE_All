namespace PoteryGVS.Configuration.jsonModels
{
    public class PoteryGVSConfig
    {
        public WithODPUConfig with_odpu { get; set; } = new WithODPUConfig();
        public WithoutODPUConfig without_odpu { get; set; } = new WithoutODPUConfig();
        public WithITPConfig with_itp { get; set; } = new WithITPConfig();
    }
}