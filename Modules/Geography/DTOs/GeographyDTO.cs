namespace CondigiBack.Modules.Geography.DTOs
{
    public class GeographyDTO
    {

        public class ProvinceResponseDTO
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class CantonResponseDTO
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class ParishResponseDTO
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
