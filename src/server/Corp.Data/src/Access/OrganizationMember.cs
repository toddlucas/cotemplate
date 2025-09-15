namespace Corp.Access;

using TRecord = OrganizationMember;

public class OrganizationMember : OrganizationMemberModel, ITemporalRecord
{
    #region Internal properties

#if RESELLER
    /// <summary>
    /// The group ID this organization member belongs to.
    /// </summary>
    [Display(Name = "Group ID")]
    [Required]
    public Guid GroupId { get; set; }
#endif

    /// <summary>
    /// The tenant ID this organization member belongs to.
    /// </summary>
    [Display(Name = "Tenant ID")]
    [Required]
    public Guid TenantId { get; set; }

    #endregion Internal properties

    #region Navigation properties

    /// <summary>
    /// The organization.
    /// </summary>
    public Organization Organization { get; set; } = null!;

    /// <summary>
    /// The person.
    /// </summary>
    public Person Person { get; set; } = null!;

    /// <summary>
    /// The organization member role enumeration.
    /// </summary>
    public OrganizationMemberRoleEnum? OrganizationMemberRoleEnum { get; set; }

    #endregion Navigation properties

    #region ITemporalRecord

    /// <summary>
    /// The created timestamp.
    /// </summary>
    [Display(Name = "Created at")]
    [Description("The date and time this record was created, in the format defined by RFC 3339, section 5.6, for example, 2017-07-21T17:32:28Z.")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// The updated timestamp.
    /// </summary>
    [Display(Name = "Updated at")]
    [Description("The date and time this record was last updated, in the format defined by RFC 3339, section 5.6, for example, 2017-07-21T17:32:28Z.")]
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// The deleted timestamp.
    /// </summary>
    [Display(Name = "Deleted at")]
    [Description("The date and time this record was deleted, or null, in the format defined by RFC 3339, section 5.6, for example, 2017-07-21T17:32:28Z.")]
    public DateTime? DeletedAt { get; set; }

    #endregion ITemporalRecord

    public static void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Table
        modelBuilder.Entity<TRecord>().ToTable(nameof(OrganizationMember));

        // Column names (snake_case)
        modelBuilder.Entity<TRecord>().Property(x => x.Id).HasColumnName("id");
#if RESELLER
        modelBuilder.Entity<TRecord>().Property(x => x.GroupId).HasColumnName("group_id");
#endif
        modelBuilder.Entity<TRecord>().Property(x => x.TenantId).HasColumnName("tenant_id");
        modelBuilder.Entity<TRecord>().Property(x => x.OrgId).HasColumnName("org_id");
        modelBuilder.Entity<TRecord>().Property(x => x.PersonId).HasColumnName("person_id");
        modelBuilder.Entity<TRecord>().Property(x => x.RoleId).HasColumnName("role_id");
        modelBuilder.Entity<TRecord>().Property(x => x.Status).HasColumnName("status");
        modelBuilder.Entity<TRecord>().Property(x => x.StartAt).HasColumnName("start_at");
        modelBuilder.Entity<TRecord>().Property(x => x.EndAt).HasColumnName("end_at");
        modelBuilder.Entity<TRecord>().Property(x => x.Metadata).HasColumnName("metadata");
        modelBuilder.Entity<TRecord>().Property(x => x.CreatedAt).HasColumnName("created_at");
        modelBuilder.Entity<TRecord>().Property(x => x.UpdatedAt).HasColumnName("updated_at");
        modelBuilder.Entity<TRecord>().Property(x => x.DeletedAt).HasColumnName("deleted_at");

        // Relations
        modelBuilder.Entity<TRecord>()
            .HasOne(x => x.Organization)
            .WithMany(y => y.Members)
            .HasForeignKey(x => x.OrgId)
            .IsRequired();

        modelBuilder.Entity<TRecord>()
            .HasOne(x => x.Person)
            .WithMany(y => y.OrganizationMemberships)
            .HasForeignKey(x => x.PersonId)
            .IsRequired();

        // Enumeration relationships
        modelBuilder.Entity<TRecord>()
            .HasOne(x => x.OrganizationMemberRoleEnum)
            .WithMany()
            .HasForeignKey(x => x.RoleId)
            .IsRequired();

        // Indexes
#if RESELLER
        modelBuilder.Entity<TRecord>()
            .HasIndex(b => b.GroupId);
#endif
        modelBuilder.Entity<TRecord>()
            .HasIndex(b => b.TenantId);
        modelBuilder.Entity<TRecord>()
            .HasIndex(b => new { b.TenantId, b.OrgId });
        modelBuilder.Entity<TRecord>()
            .HasIndex(b => new { b.TenantId, b.PersonId });
        modelBuilder.Entity<TRecord>()
            .HasIndex(b => new { b.OrgId, b.PersonId })
            .IsUnique();

        // Seed data (optional)
        var createdAt1 = new DateTime(2024, 1, 15, 0, 0, 0, DateTimeKind.Utc);
        var createdAt2 = new DateTime(2024, 2, 10, 0, 0, 0, DateTimeKind.Utc);
        var createdAt3 = new DateTime(2024, 3, 20, 0, 0, 0, DateTimeKind.Utc);
        var updatedAt1 = new DateTime(2024, 12, 19, 0, 0, 0, DateTimeKind.Utc);
        var updatedAt2 = new DateTime(2024, 11, 30, 0, 0, 0, DateTimeKind.Utc);
        var updatedAt3 = new DateTime(2024, 12, 15, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<TRecord>().HasData(
            new TRecord
            {
                Id = 1,
#if RESELLER
                GroupId = IdentitySeedData.GroupId,
#endif
                TenantId = IdentitySeedData.TenantId,
                OrgId = 1,
                PersonId = 1,
                RoleId = nameof(OrganizationMemberRole.admin),
                Status = "active",
                StartAt = createdAt1,
                CreatedAt = createdAt1,
                UpdatedAt = updatedAt1
            },
            new TRecord
            {
                Id = 2,
#if RESELLER
                GroupId = IdentitySeedData.GroupId,
#endif
                TenantId = IdentitySeedData.TenantId,
                OrgId = 1,
                PersonId = 2,
                RoleId = nameof(OrganizationMemberRole.viewer),
                Status = "active",
                StartAt = createdAt2,
                CreatedAt = createdAt2,
                UpdatedAt = updatedAt2
            },
            new TRecord
            {
                Id = 3,
#if RESELLER
                GroupId = IdentitySeedData.GroupId,
#endif
                TenantId = IdentitySeedData.TenantId,
                OrgId = 2,
                PersonId = 3,
                RoleId = nameof(OrganizationMemberRole.manager),
                Status = "active",
                StartAt = createdAt3,
                CreatedAt = createdAt3,
                UpdatedAt = updatedAt3
            }
        );
    }
}
