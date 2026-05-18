namespace PoteryGVS.Models
{
    public class MPDataObject
    {
        public decimal Q_Calc { get; set; }
        public decimal VNR_Calc { get; set; }
        public decimal dV_Calc { get; set; }
        public string IsODPU { get; set; } // да/нет
        public string SysPU_Id { get; set; }
        public string Address { get; set; }
        public string LoadType { get; set; }
        public string BuildingId { get; set; }
        public string TU_AIIS_Id { get; set; }

        public override string ToString()
        {
            return $"{IsODPU}, {BuildingId}, {Q_Calc}, {Address}";
        }
    }
}