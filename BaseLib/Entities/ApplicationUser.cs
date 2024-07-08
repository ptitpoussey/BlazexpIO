using BlazexpIO.Utils.Enums;

namespace BaseLib.Entities
{
    public class ApplicationUser
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public AuthorityType Authority { get; set; }
    }
}
