using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;
using Saned.Delco.Data.Core.Enum;
using Saned.Delco.Data.Core.Models;
using Saned.Delco.Data.Persistence;
using Saned.Delco.Data.Persistence.Infrastructure;
using Saned.Delco.Data.Providers;

namespace Saned.Delco.Data.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

       

        protected override void Seed(ApplicationDbContext context)
        {
           

            if (!context.Clients.Any())
            {
                context.Clients.AddRange(BuildClientsList());
                context.SaveChanges();
            }
            string adminRoleId;
            string userRoleId;
            string agentRoleId;

            #region RolesEnum
            if (!context.Roles.Any())
            {
                adminRoleId = context.Roles.Add(new IdentityRole(RolesEnum.Administrator.ToString())).Id;
                userRoleId = context.Roles.Add(new IdentityRole(RolesEnum.User.ToString())).Id;
                agentRoleId = context.Roles.Add(new IdentityRole(RolesEnum.Agent.ToString())).Id;
            }
            else
            {
                adminRoleId = context.Roles.First(c => c.Name == RolesEnum.Administrator.ToString()).Id;
                userRoleId = context.Roles.First(c => c.Name == RolesEnum.User.ToString()).Id;
                agentRoleId = context.Roles.First(c => c.Name == RolesEnum.Agent.ToString()).Id;

            }
            context.SaveChanges();
            #endregion


            //foreach (PremisionEnum val in Enum.GetValues(typeof(PremisionEnum)))
            //{
            //    context.Roles.AddOrUpdate(new IdentityRole(val.ToString()));
             
            //}
            context.SaveChanges();
            if (!context.Users.Any())

            {
                var administrator =
                    context.Users.Add(new ApplicationUser()
                    {
                        FullName = "Administrator",
                        UserName = "administrator",
                        Email = "admin@somesite.com",                     
                        EmailConfirmed = true,


                    });
                administrator.Roles.Add(new IdentityUserRole { RoleId = adminRoleId });

                context.SaveChanges();

                var store = new ApplicationUserStoreImpl();

                store.SetPasswordHashAsync(administrator, new ApplicationUserManagerImpl().PasswordHasher.HashPassword("111111"));
                context.SaveChanges();

            }
            if (!context.EmailSettings.Any())
            {

                context.EmailSettings.AddRange(BuildEmailSetting());
                context.SaveChanges();
            }


        }

        private static IEnumerable<EmailSetting> BuildEmailSetting()
        {
            var list = new List<EmailSetting>
            {
                //relay-hosting.secureserver.net
                new EmailSetting
                {
                    EmailSettingType = "1",
                    FromEmail = "confirm@saned.sa",
                    Host = "relay-hosting.secureserver.net",
                    Id = 1,

                    MessageBodyAr = @"����� @FullName
��� ������� ��
@code",
                    MessageBodyEn = "",
                    Password = "con@saned123#",
                    Port = 25,
                    SubjectAr = "����� ������ ����������",
                    SubjectEn = ""
                },
                new EmailSetting
                {
                     EmailSettingType = "2",
                    FromEmail = "confirm@saned.sa",
                    Host = "relay-hosting.secureserver.net",
                    Id = 1,

                    MessageBodyAr = @"����� @FullName
��� ����� ���� ���� ��
@code
�� ������� ����� ���� �� �����",
                    MessageBodyEn = "",
                    Password = "con@saned123#",
                    Port = 25,
                    SubjectAr = "���� ���� ����",
                    SubjectEn = ""
                }
            };

            return list;
        }

        private static IEnumerable<Client> BuildClientsList()
        {

            var clientsList = new List<Client>
            {
                new Client
                { Id = "ngAuthApp",
                    Secret= Helper.GetHash("abc@123"),
                    Name="AngularJS front-end Application",
                    ApplicationType =ApplicationTypes.JavaScript,
                    Active = true,
                    RefreshTokenLifeTime = 7200,
                    AllowedOrigin = "http://localhost:32150"
                },
                new Client
                { Id = "consoleApp",
                    Secret=Helper.GetHash("123@abc"),
                    Name="Console Application",
                    ApplicationType =ApplicationTypes.NativeConfidential,
                    Active = true,
                    RefreshTokenLifeTime = 14400,
                    AllowedOrigin = "*"
                }
            };

            return clientsList;
        }
    }
}
