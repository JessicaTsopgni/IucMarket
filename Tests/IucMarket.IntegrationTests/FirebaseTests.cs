using Firebase.Database.Query;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IucMarket.IntegratedTests
{
    public class FirebaseTests
    {
        private Service.UserService service;
        [SetUp]
        public void Setup()
        {  
             service = new Service.UserService();
       }

        [Test]
        public async Task FirebaseLoginWithRealAccountReturnUserWithIsUid()
        {
            var user = await service.LoginAsync
            (
                new Service.LoginCommand
                (
                    "willyjoeltchana@gmail.com", 
                    "Widy@2015"
                )
            );
            Assert.AreEqual("dSGNkzxOodeaBEK7pcbOLl0EZ2G3", user.Id);
        }

        [Test]
        public void FirebaseLoginWithFakeAccountReturnUnauthorizedAccessException()
        {
            Assert.ThrowsAsync<UnauthorizedAccessException>
            (
                async () => await service.LoginAsync
                (
                    new Service.LoginCommand
                    ( 
                        "hello@gmail.com", 
                        "xwrta"
                    )
                )
            );
        }

        [Test]
        public async Task FirebaseRegisterWithNewAccountReturnNewUserWithPersonAddedInDatabase()
        {
            string email = $"{Guid.NewGuid()}@gmail.com";

          
            var command = new Service.RegisterCommand
            (
                email,
                "admin12345",
                "Integration test name",
                "+237",
                67893936,
                false,
                Entities.Person.RoleOptions.Admin,
                true
            );

            var user = await service.RegisterAsync(command);
            var person = await service.FirebaseClient
                .Child(service.Table)
                .OrderBy("Id")
                .EqualTo(user.Id)
                .OnceAsync<Entities.Person>();

            Assert.AreEqual(user.Email, email);
            Assert.AreEqual(person.FirstOrDefault()?.Object.Id, user.Id);
        }

        [Test]
        public void FirebaseRegisterWithExistingEmailReturnDuplicateWaitObjectException()
        {

            Assert.ThrowsAsync<DuplicateWaitObjectException>
             (
                 async () => await service.RegisterAsync
                 (
                     new Service.RegisterCommand
                     (
                         "jess.tsopgni@gmail.com",
                         "",
                         "",
                         "",
                         0,
                         false,
                         Entities.Person.RoleOptions.Admin,
                         true
                     )
                 )
             );
        }

        //[Test]
        //public async Task FirebaseForgottenPasswordWithExistingEmailSendForgotLinkByMail()
        //{
        //    await service.ForgottenPasswordAsync
        //    (
        //        new Service.ForgottenPasswordCommand
        //        (
        //            "jess.tsopgni@gmail.com"
        //        )
        //    );
        //}

        [Test]
        public void FirebaseForgottenPasswordWithNotExistingEmailReturnKeyNotFoundException()
        {

            Assert.ThrowsAsync<KeyNotFoundException>
             (
                 async () => await service.ForgottenPasswordAsync
                 (
                     new Service.ForgottenPasswordCommand
                     (
                         "ayiiii@gmail.com"
                     )
                 )
             );
        }
        [Test]
        public async Task FirebaseGetUserByUidReturnAnUser()
        {

            var user = await service.GetUserAsync("3LzeMp9yBKa92afiheGkhMdEwWE2");

            Assert.AreEqual(user.Id, "3LzeMp9yBKa92afiheGkhMdEwWE2");
        }

        [Test]
        public async Task FirebaseGetUsersReturnMoreThanOneUser()
        {

           var userList = await service.GetUsersAsync();

            Assert.GreaterOrEqual(userList.Users.Count, 1);
        }
    }
}