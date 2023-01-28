﻿using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using dsKnowledgeTest.Constants;
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
        public const string DEFAULT_PASSWORD = "12345678";

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
                    .Select(u => new UserViewModel
                    {
                        Id = u.Id,
                        Email = u.Email,
                        Login = u.Login,
                        FirstName = u.FirstName,
                        SurName = u.SurName,
                        LastName = u.LastName,
                        IconUrl = u.IconUrl,
                        Organization = u.Organization,
                        Specialization = u.Specialization,
                        PhoneNumber = u.PhoneNumber,
                        RoleName = u.Role.ToString(),
                        Token = ""
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
                if (registerUser.Email.Length < EMAIL_MIN_LENGHT) return null;

                await _db.Users.AddAsync(new User
                {
                    Email = registerUser.Email,
                    Password = HaspPassword(DEFAULT_PASSWORD),
                    DataCreated = DateTime.Now,
                    DataUpdated = DateTime.Now,
                    Role = RolesConst.User,
                    IsActivated = true,
                    IsDeleted = false,
                    
                });
                await _db.SaveChangesAsync();

                return await _db.Users.AsNoTracking()
                    .Select(u => new UserViewModel
                    {
                        Id = u.Id,
                        Email = u.Email,
                        Login = u.Login,
                        Password = u.Password,
                        IsActivated = u.IsActivated,
                        FirstName = u.FirstName,
                        SurName = u.SurName,
                        LastName = u.LastName,
                        IconUrl = u.IconUrl,
                        Organization = u.Organization,
                        Specialization = u.Specialization,
                        PhoneNumber = u.PhoneNumber,
                        RoleName = u.Role.ToString(),
                        Token = ""
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
