using System;

namespace AltOxite.Core.Domain
{
    public class DomainEntity : IEquatable<DomainEntity>
    {
        public virtual Guid ID { get; set; }

        /// <summary>
        /// Indicates whether the current <see cref="T:AltOxite.Core.Domain.DomainEntity" /> is equal to another <see cref="T:AltOxite.Core.Domain.DomainEntity" />.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">A DomainEntity to compare with this object.</param>
        public virtual bool Equals(DomainEntity other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.ID.Equals(ID);
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:AltOxite.Core.Domain.DomainEntity" /> is equal to the current <see cref="T:System.Object" />.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:AltOxite.Core.Domain.DomainEntity" /> is equal to the current <see cref="T:System.Object" />; otherwise, false.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object" /> to compare with the current <see cref="T:System.Object" />. </param>
        /// <exception cref="T:System.NullReferenceException">The <paramref name="obj" /> parameter is null.</exception><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((DomainEntity) obj);
        }

        /// <summary>
        /// Serves as a hash function for a DomainEntity. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object" />.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public static bool operator ==(DomainEntity left, DomainEntity right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(DomainEntity left, DomainEntity right)
        {
            return !Equals(left, right);
        }
    }
}