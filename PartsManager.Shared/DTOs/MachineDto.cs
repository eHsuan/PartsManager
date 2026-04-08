namespace PartsManager.Shared.DTOs
{
    public class MachineDto
    {
        public int MachineID { get; set; }
        public string MachineCode { get; set; } = string.Empty;
        public string MachineName { get; set; } = string.Empty;
    }

    public class CreateMachineDto
    {
        public string MachineCode { get; set; } = string.Empty;
        public string MachineName { get; set; } = string.Empty;
    }

    public class UpdateMachineDto
    {
        public string MachineCode { get; set; } = string.Empty;
        public string MachineName { get; set; } = string.Empty;
    }
}
