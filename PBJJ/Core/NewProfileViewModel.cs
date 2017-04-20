namespace PBJJ.Core {
    public class NewProfileViewModel {
        public NewProfileType Type { get; set; }
        public string Name { get; set; }
        public decimal FingerWidth { get; set; }
        public int FingerSlotCount { get; set; }
        public decimal OverallWidth { get; set; }
    }

    public enum NewProfileType {
        Standard,
        FingerSlotCount,
        Custom
    }
}