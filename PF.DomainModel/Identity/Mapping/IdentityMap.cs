using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PF.DomainModel.Identity.Mapping
{
    //Map User To Section
   public class SectionMap:EntityTypeConfiguration<Section>
    {
       public SectionMap()
       {
           this.HasMany<ApplicationUserSection>((Section s) => s.ApplicationUsers)
                .WithRequired()
                .HasForeignKey<string>((ApplicationUserSection aus) => aus.SectionId);
       }

     
    }
   public class ApplicationUserSectionMap : EntityTypeConfiguration<ApplicationUserSection>
   {
       public ApplicationUserSectionMap()
       {
           this.HasKey((ApplicationUserSection r) =>
               new
               {
                   ApplicationUserId = r.ApplicationUserId,
                   SectionId = r.SectionId
               }).ToTable("ApplicationUserSections");
       }
   }

   
   public class ApplicationGroupMap : EntityTypeConfiguration<ApplicationGroup>
   {
       public ApplicationGroupMap()
       {
           // Map Users to Groups:
          this.HasMany<ApplicationUserGroup>((ApplicationGroup g) => g.ApplicationUsers)
               .WithRequired()
               .HasForeignKey<string>((ApplicationUserGroup ag) => ag.ApplicationGroupId);

          // Map Roles to Groups:
          this.HasMany<ApplicationGroupRole>((ApplicationGroup g) => g.ApplicationRoles)
              .WithRequired()
              .HasForeignKey<string>((ApplicationGroupRole ap) => ap.ApplicationGroupId);
        
       }
   }
   public class ApplicationUserGroupMap : EntityTypeConfiguration<ApplicationUserGroup>
   {
       public ApplicationUserGroupMap()
       {
           this.HasKey((ApplicationUserGroup r) =>
                   new
                   {
                       ApplicationUserId = r.ApplicationUserId,
                       ApplicationGroupId = r.ApplicationGroupId
                   }).ToTable("ApplicationUserGroups");   
       }
   }
   public class ApplicationGroupRoleMap : EntityTypeConfiguration<ApplicationGroupRole>
   {
       public ApplicationGroupRoleMap()
       {
             this.HasKey((ApplicationGroupRole gr) =>
                new
                {
                    ApplicationRoleId = gr.ApplicationRoleId,
                    ApplicationGroupId = gr.ApplicationGroupId
                }).ToTable("ApplicationGroupRoles");
       }
   }
}
