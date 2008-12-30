using System;
using AltOxite.Core.Domain;
using AltOxite.Core.Domain.Persistence;
using NUnit.Framework;

namespace AltOxite.IntegrationTests.Domain_Persistence
{
    [TestFixture]
    public class PostPersistenceTester : PersistenceTesterContext<PostPersistenceMap, Post>
    {
        [Test]
        public void should_load_and_save_a_post()
        {
            Specification
                .CheckProperty(u => u.Title, "title, anything here")
                .CheckProperty(u => u.Published, DateTime.Parse("12-NOV-2008"))
                .CheckProperty(u => u.BodyShort, "body short, anything here")
                .CheckProperty(u => u.Body, "body, anything here")
                .CheckProperty(u => u.Slug, "slug, anything here")
                .VerifyTheMappings();
        }
    }
}