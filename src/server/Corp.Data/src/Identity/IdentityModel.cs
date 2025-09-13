using Microsoft.AspNetCore.Identity;

namespace Corp.Identity;

internal class IdentityModel
{
    public static void OnModelCreating(ModelBuilder builder)
    {

        // Seed data
        builder.Entity<IdentityRole<Guid>>().HasData(new IdentityRole<Guid>(AppRole.User) { Id = new Guid("4653f390-806f-480c-882c-a871efc0790a"), NormalizedName = "USER" });
        builder.Entity<IdentityRole<Guid>>().HasData(new IdentityRole<Guid>(AppRole.Admin) { Id = new Guid("ea3105d8-d79f-44c1-b23a-657826cc6498"), NormalizedName = "ADMIN" });
        //builder.Entity<ApplicationRole>().HasData(new ApplicationRole(AppRole.User) { Id = 1, NormalizedName = "USER" });
        //builder.Entity<ApplicationRole>().HasData(new ApplicationRole(AppRole.Admin) { Id = 9, NormalizedName = "ADMIN" });

        builder.Entity<ApplicationTenant>().HasData(new ApplicationTenant { Id = IdentitySeedData.TenantId }); // Default tenant
        builder.Entity<ApplicationTenant>().HasData(new ApplicationTenant { Id = IdentitySeedData.OtherTenantId });

        //builder.Entity<ApplicationUser>().HasData(new ApplicationUser
        builder.Entity<ApplicationUser>().HasData(new ApplicationUser
        {
            Id = new Guid("479b7016-c534-47ca-8937-e048d9b7a299"), // 1
            GroupId = IdentitySeedData.GroupId,
            TenantId = IdentitySeedData.TenantId,
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
        builder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid> { UserId = new Guid("479b7016-c534-47ca-8937-e048d9b7a299"), RoleId = new Guid("4653f390-806f-480c-882c-a871efc0790a") });
        builder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid> { UserId = new Guid("479b7016-c534-47ca-8937-e048d9b7a299"), RoleId = new Guid("ea3105d8-d79f-44c1-b23a-657826cc6498") });

        builder.Entity<IdentityGroup<Guid>>().HasData(new IdentityGroup<Guid> { Id = IdentitySeedData.GroupId });
        builder.Entity<IdentityGroup<Guid>>().HasData(new IdentityGroup<Guid> { Id = IdentitySeedData.TenantId });
    }
}

public static class IdentitySeedData
{
    public static Guid GroupId = new Guid("850d9d65-f198-478c-95d7-aeaaed5ffd71");
    public static Guid TenantId = new Guid("4cd9b651-8df2-4b44-a55a-5abe06c7907e");
    public static Guid OtherTenantId = new Guid("843e33f7-03ee-45e6-8d63-587d790c9668");
}
