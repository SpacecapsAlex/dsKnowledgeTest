using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using dsKnowledgeTest.Constans;
using dsKnowledgeTest.Data;
using dsKnowledgeTest.Models;
using dsKnowledgeTest.ViewModels.UserViewModels;
using Microsoft.EntityFrameworkCore;

namespace dsKnowledgeTest.Services
{
    public interface IAccountService
    {
        public Task<UserViewModel?> Login(LoginUserViewModel loginUser);
        public Task<UserViewModel?> Register(RegisterUserViewModel registerUser);
    }

    public class AccountService : IAccountService
    {
        public const int LOGIN_MIN_LENGHT = 6;
        public const int PASSWORD_MIN_LENGHT = 6;
        public const int EMAIL_MIN_LENGHT = 6;

        private readonly AppDbContext _db;

        public AccountService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<UserViewModel?> Login(LoginUserViewModel loginUser)
        {
            try
            {
                var user = await _db.Users.AsNoTracking()
                    .Include("Role")
                    .Select(u => new UserViewModel
                    {
                        Id = u.Id,
                        Email = u.Email,
                        Login = u.Login,
                        Password = u.Password,
                        DataCreated = u.DataCreated.ToString(CultureInfo.InvariantCulture),
                        DataUpdated = u.DataUpdated.ToString(CultureInfo.InvariantCulture),
                        IsActivated = u.IsActivated,
                        IsDeleted = u.IsDeleted,
                        FirstName = u.FirstName,
                        SurName = u.SurName,
                        LastName = u.LastName,
                        Institution = u.Institution,
                        Specialization = u.Specialization,
                        CourseNumber = u.CourseNumber,
                        Group = u.Group,
                        PhoneNumber = u.PhoneNumber,
                        RoleName = u.Role.Name,
                    }).FirstOrDefaultAsync(u =>
                        u.Email == loginUser.Email && u.Password == HaspPassword(loginUser.Password));
                return user?.IsActivated == true
                    ? user
                    : null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<UserViewModel?> Register(RegisterUserViewModel registerUser)
        {
            try
            {
                if (registerUser.Password.Length < PASSWORD_MIN_LENGHT ||
                    registerUser.Email.Length < EMAIL_MIN_LENGHT) return null;

                await _db.Users.AddAsync(new User
                {
                    Email = registerUser.Email,
                    Password = HaspPassword(registerUser.Password),
                    DataCreated = DateTime.Now,
                    DataUpdated = DateTime.Now,
                    RoleId = (await _db.Roles.AsNoTracking()
                        .FirstAsync(r => r.Name == RolesConst.User.ToString())).Id,
                    IsActivated = true,
                    IsDeleted = false,
                    
                });
                await _db.SaveChangesAsync();

                return await _db.Users.AsNoTracking()
                    .Include("Role")
                    .Select(u => new UserViewModel
                    {
                        Id = u.Id,
                        Email = u.Email,
                        Login = u.Login,
                        Password = u.Password,
                        DataCreated = u.DataCreated.ToString(CultureInfo.InvariantCulture),
                        DataUpdated = u.DataUpdated.ToString(CultureInfo.InvariantCulture),
                        IsActivated = u.IsActivated,
                        IsDeleted = u.IsDeleted,
                        FirstName = u.FirstName,
                        SurName = u.SurName,
                        LastName = u.LastName,
                        Institution = u.Institution,
                        Specialization = u.Specialization,
                        CourseNumber = u.CourseNumber,
                        Group = u.Group,
                        PhoneNumber = u.PhoneNumber,
                        RoleName = u.Role.Name,
                    })
                    .FirstOrDefaultAsync(u => u.Email == registerUser.Email);
            }
            catch
            {
                return null;
            }
        }

        private static string HaspPassword(string password)
        {
            var md5 = MD5.Create();
            var b = Encoding.ASCII.GetBytes(password);
            var hash = md5.ComputeHash(b);
            var sb = new StringBuilder();
            foreach (var a in hash)
                sb.Append(a.ToString("X2"));
            return sb.ToString();
        }
    }
}
