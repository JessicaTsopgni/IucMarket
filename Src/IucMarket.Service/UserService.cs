using Firebase.Database;
using Firebase.Database.Query;
using IucMarket.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IucMarket.Service
{
    public class UserService : BaseService
    {
        public readonly string Table = "Persons";
        /// <summary>
        /// Log in with firebase authentication
        /// </summary>
        /// <param name="command">An instance of login command object</param>
        /// <exception cref="System.UnauthorizedAccessException">Thrown when username or password is invalid</exception>
        /// <returns>A Service.User</returns>
        public async Task<User> LoginAsync(LoginCommand command)
        {
            try
            {
                var firebaseLink = await FirebaseAuthProvider.SignInWithEmailAndPasswordAsync
                (
                    command.Email,
                    command.Password
                );
                Person person = await GetPersonAsync(firebaseLink.User.LocalId);
                return new User
                (
                    person?.Key,
                    firebaseLink.User.LocalId,
                    person?.FullName,
                    person?.CreatedAt ?? DateTime.MinValue,
                    person?.Role ?? Person.RoleOptions.Customer,
                    firebaseLink.User.IsEmailVerified,
                    firebaseLink.User.Email,
                    null,
                    firebaseLink.FirebaseToken,
                    firebaseLink.ExpiresIn
                );
            }
            catch (Firebase.Auth.FirebaseAuthException ex)
            {
                throw new UnauthorizedAccessException("Email or password is invalid !", ex);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<Person> GetPersonAsync(string uid)
        {
            try
            {
                var persons = await FirebaseClient
                       .Child(Table)
                       .OrderBy("Id")
                       .EqualTo(uid)
                       .OnceAsync<Person>();

                var person = persons?.FirstOrDefault()?.Object;
                person.Key = persons?.FirstOrDefault()?.Key;
                return person;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<User> RegisterAsync(RegisterCommand command)
        {
            
            try
            {
                if(await GetUserByEmailAsync(command.Email) != null)
                    throw new DuplicateWaitObjectException($"{nameof(command.Email)} already exists !");

                // Create User
                var firebaseAuthLink = await FirebaseAuthProvider.CreateUserWithEmailAndPasswordAsync
                (
                   command.Email,
                   command.Password,
                   command.Name,
                   command.SendVerificationEmail
                );

                var person = new Person
                (
                    null,
                    firebaseAuthLink.User.LocalId,
                    firebaseAuthLink.User.DisplayName,
                    command.CreatedAt,
                    command.Role,
                    command.Status
                );

                var result = await FirebaseClient
                  .Child(Table)
                  .PostAsync(JsonConvert.SerializeObject(person));
                person.Key = result.Key;

                return new User
                (
                    person.Key,
                    person.Id,
                    person.FullName,
                    person.CreatedAt,
                    person.Role,
                    person.Status,
                    firebaseAuthLink.User.Email,
                    null
                );
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<User> EditAsync(string uid, RegisterCommand command)
        {

            try
            {
                var user = await GetUserByEmailAsync(command.Email);

                if (user == null)
                    throw new KeyNotFoundException($"{nameof(User)} {command.Email} not found");
                if (user != null &&  user.Id != uid)
                    throw new DuplicateWaitObjectException($"{nameof(command.Email)} {command.Email} already exists !");

                // Create User
                var userRecord = await FirebaseAuthAdmin.UpdateUserAsync
                (
                    new FirebaseAdmin.Auth.UserRecordArgs
                    {
                        Uid = uid,
                        Email = command.Email,
                        Password = command.Password,
                        DisplayName = command.Name
                    }
                );

                var person = await GetPersonAsync(uid);
                if(person == null)
                {
                    person = new Person
                    (
                        null,
                        uid,
                        command.Name,
                        command.CreatedAt,
                        command.Role,
                        command.Status
                    );

                    var result = await FirebaseClient
                      .Child(Table)
                      .PostAsync(JsonConvert.SerializeObject(person));

                    person.Key = result.Key;
                }
                else
                {
                    person.FullName = command.Name;
                    person.Status = command.Status;

                    await FirebaseClient
                      .Child(Table)
                      .Child(person.Key)
                      .PutAsync(JsonConvert.SerializeObject(person));
                }

                return new User
                (
                    person.Key,
                    person.Id,
                    person.FullName,
                    person.CreatedAt,
                    person.Role,
                    person.Status,
                    userRecord.Email,
                    null
                );
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<User> GetUserByEmailAsync(string email)
        {
            // Get User
            try
            {
                var provider = await FirebaseAuthAdmin.GetUserByEmailAsync(email);
                Person person = await GetPersonAsync(provider.Uid);
                return new User
                (
                    person?.Key,
                    provider.Uid,
                    person?.FullName,
                    person?.CreatedAt ?? DateTime.UtcNow,
                    person?.Role ?? Person.RoleOptions.Customer,
                    person?.Status ?? false,
                    provider.Email,
                    null
                );
            }
            catch (FirebaseAdmin.Auth.FirebaseAuthException)
            {
                return null;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }


        public async Task<UserList> GetUsersAsync(string pageToken = null, int pageSize = 50)
        {
            UserList userList = new UserList();
            userList.PageSize = pageSize;
            //try
            //{
                var options = new FirebaseAdmin.Auth.ListUsersOptions
                {
                    PageSize = 50
                };
                if (!string.IsNullOrEmpty(pageToken))
                    options.PageToken = pageToken;

                var pagedEnumerable = FirebaseAuthAdmin.ListUsersAsync(options);
                var responses = pagedEnumerable.AsRawResponses().GetAsyncEnumerator();
                while (await responses.MoveNextAsync())
                {
                    FirebaseAdmin.Auth.ExportedUserRecords response = responses.Current;
                    userList.PageToken = response.NextPageToken; 
                    foreach (FirebaseAdmin.Auth.ExportedUserRecord user in response.Users)
                    {
                        var person = await GetPersonAsync(user.Uid);
                        userList.Users.Add
                        (
                            new User
                            (
                                person?.Key,
                                user.Uid, 
                                person?.FullName, 
                                person?.CreatedAt ?? DateTime.MinValue, 
                                person?.Role ?? Person.RoleOptions.Customer,
                                person?.Status ?? true, 
                                user.Email,
                                null
                            ) 
                        );
                    }
                }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            return userList;
        }

        public async Task<User> GetUserAsync(string uid)
        {
            try
            {
                var user = await FirebaseAuthAdmin.GetUserAsync(uid);
                var person = await GetPersonAsync(user.Uid);
                return new User
                (
                    person?.Key,
                    user.Uid,
                    person?.FullName,
                    person?.CreatedAt ?? DateTime.MinValue,
                    person?.Role ?? Person.RoleOptions.Customer,
                    person?.Status ?? true,
                    user.Email,
                    null
                );
            }
            catch(FirebaseAdmin.Auth.FirebaseAuthException)
            {
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteAsync(string uid)
        {
            // Get User
            try
            {
               await FirebaseAuthAdmin.DeleteUserAsync(uid);
            }
            catch (FirebaseAdmin.Auth.FirebaseAuthException)
            {
                throw new KeyNotFoundException($"{nameof(User)} {uid} not found");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task ForgottenPasswordAsync(ForgottenPasswordCommand command)
        {

            try
            {
                if(await GetUserByEmailAsync(command.Email) == null)
                    throw new KeyNotFoundException($"{nameof(command.Email)} {command.Email} doesn't exists !");

                await FirebaseAuthProvider.SendPasswordResetEmailAsync(command.Email);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<IEnumerable<User>> GetOwnersAsync()
        {
            try
            {
                var persons = await FirebaseClient
                       .Child(Table)
                       .OnceAsync<Person>();
                return persons.Select
                (
                    x =>
                    new User
                    (
                        x.Key,
                        x.Object.Id,
                        x.Object.FullName,
                        x.Object.CreatedAt,
                        x.Object.Role,
                        x.Object.Status,
                        null,
                        null
                    )
                ).ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
    public class LoginCommand
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public LoginCommand(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }

    public class RegisterCommand
    {
        public string Email { get; private set; }
        public string Password { get; private set; }
        public string Name { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public bool SendVerificationEmail { get; private set; }
        public Person.RoleOptions Role { get; private set; }
        public bool Status { get; private set; }

        public RegisterCommand(string email, string password, string name,
            bool sendVerificationEmail, Person.RoleOptions role, bool status)
        {
            Email = email;
            Password = password;
            Name = name;
            CreatedAt = DateTime.UtcNow;
            SendVerificationEmail = sendVerificationEmail;
            Role = role;
            Status = status;
        }
    }

    public class ForgottenPasswordCommand
    {
        public string Email { get; set; }
        public ForgottenPasswordCommand(string email)
        {
            Email = email;
        }
    }
}
