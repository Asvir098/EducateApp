using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducateApp.Models
{
    public class RoleInitializer
    {
        public static async Task InitializerAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            // так как в приложении не требуется роли кроме Администраиора и Зарегистрированный пользователь
            // роли задаются при помощи инициализации
            // в будущем вы сможете добавить список ролей и работу с ними, так же как и с пользователями

            // обычно таике данные об администраторе хранится с помощью разработки MS Azure,
            // но пользование данной системой только год

            string adminEmail = "admin@mail.ru";
            string password = "_Aa123456";

            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("registeredUser") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("registeredUser"));
            }
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                User admin = new User
                {
                    Email = adminEmail,
                    UserName = adminEmail,
                    LastName = "Лубков",
                    FirstName = "Михаил",
                    Patronymic = "Александрович",
                    EmailConfirmed = true
                };

                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
        }
    }
}
