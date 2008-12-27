using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AltOxite.Core.Domain;
using AltOxite.Core.Persistence;
using AltOxite.Tests;
using AltOxite.Web;
using FluentNHibernate.Framework;
using FubuMVC.Container.StructureMap.Config;
using NHibernate.Linq;
using NUnit.Framework;
using StructureMap;

namespace AltOxite.IntegrationTests.Persistence
{
    [TestFixture]
    public class NHibernateUnitOfWork_IntegrationTester
    {
        private ISessionSource _source;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            ControllerConfig.Configure = x => {};
            Bootstrapper.Restart();
            
            _source = ObjectFactory.GetInstance<ISessionSource>();
            ((SessionSource)_source).BuildSchema();
        }

        private NHibernateUnitOfWork NewUow()
        {
            var uow = new NHibernateUnitOfWork(_source);
            uow.Initialize();
            return uow;
        }

        [Test]
        public void SaveOrUpdate_insert_new_entities_into_the_DB()
        {
            ensure_no_users_exist_like("crud_user");

            var user1 = new User { Username = "crud_user1" };
            var user2 = new User { Username = "crud_user2" };
            var user3 = new User { Username = "crud_user3" };

            save_users(user1, user2, user3);

            ensure_users_exist("crud_user", 3);
        }

        [Test]
        public void save_new_then_update_the_same_object_instance()
        {
            ensure_no_users_exist_like("save_on_existing_user");

            var user1 = new User { Username = "save_on_existing_user1" };

            save_users(user1);

            ensure_users_exist("save_on_existing_user", 1);

            user1.Username = "save_on_existing_user_UPDATED_1";
            save_users(user1);

            ensure_users_exist("save_on_existing_user1", 0);
            ensure_users_exist("save_on_existing_user_UPDATED", 1);
        }

        [Test]
        public void update_existing_should_update_the_existing_user()
        {
            ensure_no_users_exist_like("update_existing_user");
            var user1 = new User { Username = "update_existing_user1" };
            save_users(user1);

            user1.Username = "update_existing_userUPDATED_1";
            save_users(user1);

            ensure_users_exist("update_existing_user1", 0);
            ensure_users_exist("update_existing_userUPDATED", 1);
        }

        [Test]
        public void delete_on_existing_object_should_delete_it()
        {
            ensure_no_users_exist_like("delete_existing_user");
            var user1 = new User { Username = "delete_existing_user1" };
            save_users(user1);

            delete_users(user1);

            ensure_no_users_exist_like("delete_existing_user");
        }

        [Test]
        public void save_update_and_delete_all_in_the_same_UOW()
        {
            ensure_no_users_exist_like("all_in_one");
            var userToSave = new User { Username = "all_in_one_save" };
            var userToDelete = new User { Username = "all_in_one_delete" };
            var userToUpdate = new User { Username = "all_in_one_update" };
            save_users(userToDelete, userToUpdate);

            userToUpdate.DisplayName = "UPDATED";

            using( var uow = NewUow())
            {
                uow.SaveOrUpdate(userToSave);
                uow.SaveOrUpdate(userToUpdate);
                uow.Delete(userToDelete);
                uow.Commit();
            }

            ensure_no_users_exist_like("all_in_one_delete");
            
            ensure_users_exist("all_in_one", 2);
            
            findUsers("all_in_one_update").Single()
                .DisplayName.ShouldEqual(userToUpdate.DisplayName);
        }

        private void save_users(params User[] users)
        {
            using (var uow = NewUow())
            {
                uow.SaveOrUpdate(users);
                uow.Commit();
            }
        }

        private void delete_users(params User[] users)
        {
            using (var uow = NewUow())
            {
                uow.Delete(users);
                uow.Commit();
            }
        }
        
        private void ensure_users_exist(string userPrefix, int count)
        {
            var foundUsers = findUsers(userPrefix);
            foundUsers.ShouldHaveCount(count);
        }

        private void ensure_no_users_exist_like(string userPrefix)
        {
            var foundUsers = findUsers(userPrefix);
            foundUsers.ShouldHaveCount(0);
        }

        private IEnumerable<User> findUsers(string userPrefix)
        {
            using (var controlUow = NewUow())
            {
                return controlUow.CurrentSession.Linq<User>().Where(u => u.Username.StartsWith(userPrefix)).ToArray();
            }
        }
    }
}