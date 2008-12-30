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
        [Ignore("This test is failing because it is missing the mappings from Comment and Tag, needs looking at")]
        public void should_load_and_save_a_post()
        {
            Specification
                .CheckProperty(u => u.Title, "title, anything here")
                .CheckProperty(u => u.Published, DateTime.Parse("12-NOV-2008"))
                .CheckProperty(u => u.BodyShort, "body short, anything here")
                .CheckProperty(u => u.Body, "body, anything here")
                .CheckProperty(u => u.Slug, "slug, anything here")
                .CheckProperty(u => u.Comments, new [] { new Comment() })
                .CheckProperty(u => u.Tags, new[] { new Tag() })
                .VerifyTheMappings();
        }
    }
}