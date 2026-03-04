namespace DistribuidoraX.Domain.Objects.CostObjects
{
    public record FixedCostsPercentsObject
    {
        public int SalaryCost_PercentAppliedForUnit { get; } = 0;
        public int ElectricalEnery_PercentAppliedForUnit { get; } = 0;
        public int WaterService_PercentAppliedForUnit { get; } = 0;
        public int InternetService_PercentAppliedForUnit { get; } = 0; 
    }

}
