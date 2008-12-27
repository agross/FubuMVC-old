using System;
using AltOxite.Core.Domain;
using NUnit.Framework;

namespace AltOxite.Tests.Domain
{
    [TestFixture]
    public class DomainEntityTester
    {
        [Test]
        public void two_entities_with_the_same_ID_should_be_equal()
        {
            var testID = Guid.NewGuid();
            new DomainEntity {ID = testID}.ShouldEqual(new DomainEntity {ID = testID});
        }

        [Test]
        public void two_entities_with_different_ID_should_not_be_equal()
        {
            new DomainEntity { ID = Guid.NewGuid() }.ShouldNotEqual(new DomainEntity { ID = Guid.NewGuid() });
        }

        [Test]
        public void two_entities_with_the_same_ID_should_have_the_same_hashcode()
        {
            var testID = Guid.NewGuid();
            new DomainEntity { ID = testID }.GetHashCode()
                .ShouldEqual(new DomainEntity { ID = testID }.GetHashCode());
        }

        [Test]
        public void two_entities_with_different_ID_should_not_have_the_same_hashcode()
        {
            new DomainEntity { ID = Guid.NewGuid() }.GetHashCode()
                .ShouldNotEqual(new DomainEntity { ID = Guid.NewGuid() }.GetHashCode());
        }

        [Test]
        public void derived_entities_with_the_same_ID_and_same_type_should_be_equal()
        {
            var testID = Guid.NewGuid();
            new EntityTypeA { ID = testID }.ShouldEqual(new EntityTypeA { ID = testID });
        }

        [Test]
        public void derived_entities_with_different_IDs_and_same_type_should_not_be_equal()
        {
            new EntityTypeA { ID = Guid.NewGuid() }
                .ShouldNotEqual(new EntityTypeA { ID = Guid.NewGuid() });
        }

        [Test]
        public void different_typed_derived_entities_with_them_same_ID_should_not_be_equal()
        {
            var testID = Guid.NewGuid();
            new EntityTypeA { ID = testID }.ShouldNotEqual(new EntityTypeB { ID = testID });
        }

        [Test]
        public void derived_types_can_override_equals_with_their_own_logic_and_ID_equality_still_works()
        {
            var testID = Guid.NewGuid();
            var first = new EqualityTesterEntity {ID = testID, SomeValue = 99};
            var second = new EqualityTesterEntity {ID = testID, SomeValue = 99};
            first.ShouldEqual(second);
        }

        [Test]
        public void derived_types_can_override_equals_with_their_own_logic_can_affect_equality()
        {
            var testID = Guid.NewGuid();
            var first = new EqualityTesterEntity { ID = testID, SomeValue = 5 };
            var second = new EqualityTesterEntity { ID = testID, SomeValue = 99 };
            first.ShouldNotEqual(second);
        }


        public class EntityTypeA : DomainEntity { }
        public class EntityTypeB : DomainEntity { }

        public class EqualityTesterEntity : DomainEntity, IEquatable<EqualityTesterEntity>
        {
            public int SomeValue { get; set; }

            public override bool Equals(DomainEntity other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                if (other.GetType() != GetType()) return false;
                return Equals((EqualityTesterEntity)other);
            }

            public bool Equals(EqualityTesterEntity obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                return base.Equals(obj) && obj.SomeValue == SomeValue;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                return Equals(obj as EqualityTesterEntity);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    {
                        return (base.GetHashCode()*397) ^ SomeValue;
                    }
                }
            }

            public static bool operator ==(EqualityTesterEntity left, EqualityTesterEntity right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(EqualityTesterEntity left, EqualityTesterEntity right)
            {
                return !Equals(left, right);
            }
        }
    }
}