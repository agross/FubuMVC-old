using System;
using AltOxite.Core.Domain;
using AltOxite.Core.Domain.Persistence;
using NUnit.Framework;

namespace AltOxite.IntegrationTests.Domain_Persistence
{
    [TestFixture]
    public class PostPersistenceTester : PersistenceTesterContext<PostPersistenceMap, Post>
    {
        public override void ReferencesAdditionalMaps(TestPersistenceModel<PostPersistenceMap, Post> model)
        {
            model.IncludeMapping<CommentPersistenceMap, Comment>();
            model.IncludeMapping<TagPersistenceMap, Tag>();
            model.IncludeMapping<UserPersistenceMap, User>();
        }

        [Test]
        public void should_load_and_save_a_post()
        {
            Specification
                .CheckProperty(p => p.Title, "title, anything here")
                .CheckProperty(p => p.Published, DateTime.Parse("12-NOV-2008"))
                .CheckProperty(p => p.BodyShort, "body short, anything here")
                .CheckProperty(p => p.Body, "body, anything here")
                .CheckProperty(p => p.Slug, "slug, anything here")
                //.CheckList(p=>p.Comments, new [] { new Comment() })
                //.CheckList(p => p.Tags, new[] { new Tag() })
                .VerifyTheMappings();
        }
    }
}