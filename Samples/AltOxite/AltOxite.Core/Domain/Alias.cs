using AltOxite.Core.Domain.Validation;

namespace AltOxite.Core.Domain
{
    public class Alias : DomainEntity
    {
        [Required]
        public string Host{ get; set; }
        public bool Redirect { get; set; }
    }
}