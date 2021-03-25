using Firebase.Database;
using Firebase.Database.Query;
using IucMarket.Service.Entities;
using IucMarket.Dtos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IucMarket.Common;
using System.Net.Http;
using System.Net.Sockets;

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
        public async Task<UserDto> LoginAsync(LoginCommand command)
        {
            try
            {
                var firebaseLink = await FirebaseAuthProvider.SignInWithEmailAndPasswordAsync
                (
                    command.Email,
                    command.Password
                );
                var person = await GetPersonByUserIdAsync(firebaseLink.User.LocalId);

                if(!(person.Value?.Status ?? false))
                    throw new UnauthorizedAccessException("This account has been disabled !");

                return GetUserDto
                (
                    person.Key,
                    new User
                    (
                        firebaseLink.User.LocalId,
                        person.Value?.FullName,
                        person.Value?.PhoneCountryCode,
                        person.Value?.PhoneNumber ?? 0,
                        person.Value?.CreatedAt ?? DateTime.MinValue,
                        person.Value?.Role ?? RoleOptions.Other,
                        firebaseLink.User.IsEmailVerified,
                        firebaseLink.User.Email,
                        null,
                        firebaseLink.FirebaseToken,
                        firebaseLink.ExpiresIn
                    )
                );
            }
            catch (Firebase.Auth.FirebaseAuthException ex)
            {
                if (ex.InnerException?.InnerException?.GetType() == typeof(SocketException))
                    throw new HttpRequestException("Cannot join the server. Please check your internet connexion.");
                throw new UnauthorizedAccessException("Email or password is invalid !", ex);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        internal static UserDto GetUserDto(string personId, User user)
        {
            if (user != null)
                return new UserDto
                (
                    personId,
                    user.FullName,
                    user.PhoneCountryCode,
                    user.PhoneNumber,
                    user.CreatedAt,
                    user.Role,
                    user.Status,
                    user.UserId,
                    user.Email,
                    null,
                    user.Token,
                    user.ExpiresIn
                );
            return null;
        }
        private async Task<KeyValuePair<string, Person>> GetPersonAsync(string personId)
        {
            try
            {
                if (!string.IsNullOrEmpty(personId))
                {
                    var person = await FirebaseClient
                           .Child(Table)
                           .Child(personId)
                           .OnceSingleAsync<Person>();

                    if (person != null)
                        return new KeyValuePair<string, Person>(personId, person);
                }
            }
            catch (Firebase.Auth.FirebaseAuthException ex)
            {
                if (ex.InnerException?.InnerException?.GetType() == typeof(SocketException))
                    throw new HttpRequestException("Cannot join the server. Please check your internet connexion.");
                throw ex;
            }
            catch (Firebase.Database.FirebaseException ex)
            {
                if (ex.InnerException?.InnerException?.GetType() == typeof(SocketException))
                    throw new HttpRequestException("Cannot join the server. Please check your internet connexion.");
                throw ex;
            }
            catch (Exception)
            {
            }
            return new KeyValuePair<string, Person>(string.Empty, null);
        }

        private async Task<KeyValuePair<string, Person>> GetPersonByUserIdAsync(string userId)
        {
            try
            {

                if (!string.IsNullOrEmpty(userId))
                {
                    var persons = await FirebaseClient
                           .Child(Table)
                           .OrderBy("UserId")
                           .EqualTo(userId)
                           .OnceAsync<Person>();

                    var person = persons?.FirstOrDefault();
                    if (person != null)
                        return new KeyValuePair<string, Person>(person.Key, person.Object);
                }
            }
            catch (Firebase.Database.FirebaseException ex)
            {
                if (ex.InnerException?.InnerException?.GetType() == typeof(SocketException))
                    throw new HttpRequestException("Cannot join the server. Please check your internet connexion.");
                throw ex;
            }
            catch (Exception)
            {
            }
            return new KeyValuePair<string, Person>(string.Empty, null);
        }

        public async Task<UserDto> RegisterAsync(RegisterCommand command)
        {
            
            try
            {
                if((await GetByEmailAsync(command.Email)).Value != null)
                    throw new DuplicateWaitObjectException($"{nameof(command.Email)} already exists !");

                // Create User
                var firebaseAuthLink = await FirebaseAuthProvider.CreateUserWithEmailAndPasswordAsync
                (
                   command.Email,
                   command.Password,
                   command.Name,
                   command.SendVerificationEmail
                );

                var newPerson = new Person
                (
                    firebaseAuthLink.User.LocalId,
                    firebaseAuthLink.User.DisplayName,
                    command.PhoneCountryCode,
                    command.PhoneNumber,
                    command.CreatedAt,
                    command.Role,
                    command.Status
                );

                var person = await FirebaseClient
                  .Child(Table)
                  .PostAsync(JsonConvert.SerializeObject(newPerson));

                return GetUserDto
                (
                    person.Key,
                    new User
                    (
                        newPerson.UserId,
                        newPerson.FullName,
                        newPerson.PhoneCountryCode,
                        newPerson.PhoneNumber,
                        newPerson.CreatedAt,
                        newPerson.Role,
                        newPerson.Status,
                        firebaseAuthLink.User.Email,
                        null
                    )
                );
            }
            catch (Firebase.Auth.FirebaseAuthException ex)
            {
                if (ex.InnerException?.InnerException?.GetType() == typeof(SocketException))
                    throw new HttpRequestException("Cannot join the server. Please check your internet connexion.");
                throw ex;
            }
            catch (Firebase.Database.FirebaseException ex)
            {
                if (ex.InnerException?.InnerException?.GetType() == typeof(SocketException))
                    throw new HttpRequestException("Cannot join the server. Please check your internet connexion.");
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task EditAsync(string id, RegisterCommand command)
        {
            try
            {
                var user1 = await GetAsync(id);
                if (user1.Value == null)
                    throw new KeyNotFoundException($"{nameof(User)} {command.Email} not found");

                var user2 = await GetByEmailAsync(command.Email);
                if (user2.Value != null &&  user2.Value.UserId != id)
                    throw new DuplicateWaitObjectException($"{nameof(command.Email)} {command.Email} already exists !");

                // Create User
                var userRecord = await FirebaseAuthAdmin.UpdateUserAsync
                (
                    new FirebaseAdmin.Auth.UserRecordArgs
                    {
                        Uid = id,
                        Email = command.Email,
                        Password = command.Password,
                        DisplayName = command.Name
                    }
                );

                var person = await GetPersonByUserIdAsync(id);
                if(person.Value == null)
                {
                    var newPerson = new Person
                    (
                        userRecord.Uid,
                        command.Name,
                        command.PhoneCountryCode,
                        command.PhoneNumber,
                        command.CreatedAt,
                        command.Role,
                        command.Status
                    );

                    var result = await FirebaseClient
                      .Child(Table)
                      .PostAsync(JsonConvert.SerializeObject(person));

                    person = new KeyValuePair<string, Person>(result.Key, newPerson);
                }
                else
                {
                    var newPerson = new Person
                    (
                        userRecord.Uid, 
                        command.Name, 
                        command.PhoneCountryCode,
                        command.PhoneNumber, 
                        person.Value.CreatedAt, 
                        command.Role, 
                        command.Status
                    );

                    await FirebaseClient
                      .Child(Table)
                      .Child(person.Key)
                      .PutAsync(JsonConvert.SerializeObject(newPerson));
                }

            }
            catch (Firebase.Database.FirebaseException ex)
            {
                if (ex.InnerException?.InnerException?.GetType() == typeof(SocketException))
                    throw new HttpRequestException("Cannot join the server. Please check your internet connexion.");
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<KeyValuePair<string, User>> GetByEmailAsync(string email)
        {
            // Get User
            try
            {
                var provider = await FirebaseAuthAdmin.GetUserByEmailAsync(email);
                var person = await GetPersonByUserIdAsync(provider.Uid);
                
                return new KeyValuePair<string, User>
                ( 
                    person.Key,
                    new User
                    (
                        provider.Uid,
                        person.Value?.FullName,
                        person.Value?.PhoneCountryCode,
                        person.Value?.PhoneNumber ?? 0,
                        person.Value?.CreatedAt ?? DateTime.UtcNow,
                        person.Value?.Role ?? RoleOptions.Other,
                        person.Value?.Status ?? false,
                        provider.Email,
                        null
                    )
                );
            }
            catch (FirebaseAdmin.Auth.FirebaseAuthException)
            {
                return new KeyValuePair<string, User>(string.Empty, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            // Get User
            try
            {
                var user = await GetByEmailAsync(email);
                return GetUserDto
                (
                    user.Key,
                    user.Value
                );
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ListDto<UserDto>> GetUsersAsync(string pageToken = null, int pageSize = 50)
        {
            var list = new ListDto<UserDto>();
            list.PageSize = pageSize;
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
                    list.PageToken = response.NextPageToken; 
                    foreach (FirebaseAdmin.Auth.ExportedUserRecord user in response.Users)
                    {
                        var person = await GetPersonByUserIdAsync(user.Uid);
                        list.Items.Add
                        (
                            GetUserDto
                            (
                                person.Key,
                                new User
                                (
                                    user.Uid, 
                                    person.Value?.FullName,
                                    person.Value?.PhoneCountryCode,
                                    person.Value?.PhoneNumber ?? 0,
                                    person.Value?.CreatedAt ?? DateTime.MinValue, 
                                    person.Value?.Role ?? RoleOptions.Other,
                                    person.Value?.Status ?? true, 
                                    user.Email,
                                    null
                                ) 
                            )
                        );
                    }
                }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

            list.Items = list.Items.OrderByDescending(x => x.CreatedAt).ToList();
            return list;
        }

        internal async Task<KeyValuePair<string, User>> GetAsync(string id)
        {
            try
            {
                if(string.IsNullOrEmpty(id))
                    return new KeyValuePair<string, User>(string.Empty, null);

                var user = await FirebaseAuthAdmin.GetUserAsync(id);
                var person = await GetPersonByUserIdAsync(user.Uid);
                return new KeyValuePair<string, User>
                (
                    person.Key,
                    new User
                    (
                        user.Uid,
                        person.Value?.FullName,
                        person.Value?.PhoneCountryCode,
                        person.Value?.PhoneNumber ?? 0,
                        person.Value?.CreatedAt ?? DateTime.MinValue,
                        person.Value?.Role ?? RoleOptions.Other,
                        person.Value?.Status ?? true,
                        user.Email,
                        null
                    )
                );
            }
            catch (FirebaseAdmin.Auth.FirebaseAuthException)
            {
                return new KeyValuePair<string, User>(string.Empty, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UserDto> GetUserAsync(string id)
        {
            try
            {
                var user = await GetAsync(id);
                return GetUserDto
                (
                     user.Key,
                     user.Value
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
                var person = await GetPersonByUserIdAsync(uid);
                if(person.Value != null)
                    await FirebaseClient
                        .Child(Table)
                        .Child(person.Key)
                        .DeleteAsync();
               
            }
            catch (FirebaseAdmin.Auth.FirebaseAuthException)
            {
                throw new KeyNotFoundException($"{nameof(User)} {uid} not found");
            }
            catch (Firebase.Database.FirebaseException ex)
            {
                if (ex.InnerException?.InnerException?.GetType() == typeof(SocketException))
                    throw new HttpRequestException("Cannot join the server. Please check your internet connexion.");
                throw ex;
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
                if((await GetByEmailAsync(command.Email)).Value == null)
                    throw new KeyNotFoundException($"{nameof(command.Email)} {command.Email} doesn't exists !");

                await FirebaseAuthProvider.SendPasswordResetEmailAsync(command.Email);
            }
            catch (Firebase.Auth.FirebaseAuthException ex)
            {
                if (ex.InnerException?.InnerException?.GetType() == typeof(SocketException))
                    throw new HttpRequestException("Cannot join the server. Please check your internet connexion.");
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<ListDto<UserDto>> GetOwnersAsync()
        {
            try
            {
                var list = new ListDto<UserDto>();
                var persons = await FirebaseClient
                       .Child(Table)
                       .OnceAsync<Person>();
                list.Items = persons.Select
                (
                    x =>
                    GetUserDto
                    (
                        x.Key,
                        new User
                        (
                            x.Object.UserId,
                            x.Object.FullName,
                            x.Object.PhoneCountryCode,
                            x.Object.PhoneNumber,
                            x.Object.CreatedAt,
                            x.Object.Role,
                            x.Object.Status,
                            null,
                            null
                        )
                    )
                ).ToList();
                return list;
            }
            catch (Firebase.Database.FirebaseException ex)
            {
                if (ex.InnerException?.InnerException?.GetType() == typeof(SocketException))
                    throw new HttpRequestException("Cannot join the server. Please check your internet connexion.");
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
