using VsTemplate.Core.Domain.Validation;

namespace VsTemplate.Core.Domain
{
    public class Alias : DomainEntity
    {
        [Required]
        public string Host{ get; set; }
        public bool Redirect { get; set; }
    }
}