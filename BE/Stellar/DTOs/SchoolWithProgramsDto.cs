namespace Stellar.DTOs
{
    public class SchoolWithProgramsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ProgramInfoDto> Programs { get; set; }
    }
}
