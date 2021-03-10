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
                    firebaseLink.User.LocalId,
                    person?.FullName,
                    person?.CreatedAt ?? DateTime.MinValue,
                    firebaseLink.User.IsEmailVerified,
                    firebaseLink.User.Email,
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

                return persons?.FirstOrDefault()?.Object;
            }
            catch (FirebaseException)
            {
                return null;
            }
        }

        public async Task<User> RegisterAsync(RegisterCommand command)
        {
            
            try
            {
                if(await IsUserExistsAsync(command.Email))
                    throw new DuplicateWaitObjectException("Email already exists !");

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
                    firebaseAuthLink.User.LocalId,
                    firebaseAuthLink.User.DisplayName,
                    command.CreatedAt,
                    command.Status
                );

                var result = await FirebaseClient
                  .Child(Table)
                  .PostAsync(JsonConvert.SerializeObject(person));

                return new User
                (
                    person.Id,
                    person.FullName,
                    person.CreatedAt,
                    person.Status,
                    firebaseAuthLink.User.Email
                );
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> IsUserExistsAsync(string email)
        {
            // Get User
            try
            {
                var provider = await FirebaseAuthAdmin.GetUserByEmailAsync(email);
                return true;
            }
            catch (FirebaseAdmin.Auth.FirebaseAuthException)
            {
                return false;
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
                                user.Uid, 
                                person?.FullName, 
                                person?.CreatedAt ?? DateTime.MinValue, 
                                person?.Status ?? true, 
                                user.Email
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
                    user.Uid,
                    person?.FullName,
                    person?.CreatedAt ?? DateTime.MinValue,
                    person?.Status ?? true,
                    user.Email
                );
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task ForgottenPasswordAsync(ForgottenPasswordCommand command)
        {

            try
            {
                if(!await IsUserExistsAsync(command.Email))
                    throw new EntryPointNotFoundException("Email doesn't exists !");

                await FirebaseAuthProvider.SendPasswordResetEmailAsync(command.Email);
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
        public bool Status { get; private set; }

        public RegisterCommand(string email, string password, string name, DateTime createdAt,
            bool sendVerificationEmail, bool status)
        {
            Email = email;
            Password = password;
            Name = name;
            CreatedAt = createdAt;
            SendVerificationEmail = sendVerificationEmail;
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
