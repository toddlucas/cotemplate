using Microsoft.AspNetCore.Identity;

namespace Corp.Identity;

internal class IdentityModel
{
    public static void OnModelCreating(ModelBuilder builder)
    {

        // Seed data
        builder.Entity<IdentityRole>().HasData(new IdentityRole(AppRole.User) { Id = "4653f390-806f-480c-882c-a871efc0790a", NormalizedName = "USER" });
        builder.Entity<IdentityRole>().HasData(new IdentityRole(AppRole.Admin) { Id = "ea3105d8-d79f-44c1-b23a-657826cc6498", NormalizedName = "ADMIN" });
        //builder.Entity<ApplicationRole>().HasData(new ApplicationRole(AppRole.User) { Id = 1, NormalizedName = "USER" });
        //builder.Entity<ApplicationRole>().HasData(new ApplicationRole(AppRole.Admin) { Id = 9, NormalizedName = "ADMIN" });

        // builder.Entity<ApplicationTenant>().HasData(new ApplicationTenant { Id = 1 }); // Default tenant
        // builder.Entity<ApplicationTenant>().HasData(new ApplicationTenant { Id = 2 });

        //builder.Entity<ApplicationUser>().HasData(new ApplicationUser
        builder.Entity<ApplicationUser>().HasData(new ApplicationUser
        {
            Id = "479b7016-c534-47ca-8937-e048d9b7a299", // 1
            TenantId = "4cd9b651-8df2-4b44-a55a-5abe06c7907e",
            //TenantId = 2,
            //FirstName = "Billy Bob",
            UserName = "bb@example.com",
            NormalizedUserName = "BB@EXAMPLE.COM",
            Email = "bb@example.com",
            EmailConfirmed = true,
            NormalizedEmail = "BB@EXAMPLE.COM",
            PasswordHash = "AQAAAAIAAYagAAAAEFsSBLPPWRfosR0krYQr+JEBGl1h7gf3VlBNqVkDUuwRAbJIZSr/klF2UPswMFXx1w==",
            SecurityStamp = "C3QKFHJEFZUKZSZ7LALMZC7FCOT3JNEC",
            ConcurrencyStamp = "0c7ea102-5f5a-4a32-a56a-8799053a4a3b",
            LockoutEnabled = true,
        });

        //builder.Entity<IdentityUserRole<long>>().HasData(new IdentityUserRole<long> { UserId = 1, RoleId = 1 });
        //builder.Entity<IdentityUserRole<long>>().HasData(new IdentityUserRole<long> { UserId = 1, RoleId = 9 });
        builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string> { UserId = "479b7016-c534-47ca-8937-e048d9b7a299", RoleId = "4653f390-806f-480c-882c-a871efc0790a" });
        builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string> { UserId = "479b7016-c534-47ca-8937-e048d9b7a299", RoleId = "ea3105d8-d79f-44c1-b23a-657826cc6498" });

        builder.Entity<IdentityTenant>().HasData(new IdentityTenant { Id = "4cd9b651-8df2-4b44-a55a-5abe06c7907e" });
    }
}
