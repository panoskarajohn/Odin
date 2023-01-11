using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Slip.Service.Domain;

namespace Slip.Service.DAL.Configurations;

public class SlipConfiguration : IEntityTypeConfiguration<Domain.Slip>
{
    public void Configure(EntityTypeBuilder<Domain.Slip> builder)
    {
        builder
            .HasKey(s => s.Id)
            .HasName("PK_Slip");
        
        builder
            .HasMany(s => s.Bets)
            .WithOne(b => b.Slip)
            .HasForeignKey(b => b.SlipId);
    }
}

public class BetConfiguration : IEntityTypeConfiguration<Domain.Bet>
{
    public void Configure(EntityTypeBuilder<Domain.Bet> builder)
    {
        builder.HasKey(b => b.Id).HasName("PK_Bet");

        builder.HasMany(b => b.Selections)
            .WithOne(bs => bs.Bet)
            .HasForeignKey(bs => bs.BetId);
    }
}

public class BetSelectionConfiguration : IEntityTypeConfiguration<Domain.BetSelection>
{
    public void Configure(EntityTypeBuilder<Domain.BetSelection> builder)
    {
        builder.HasKey(bs => bs.Id).HasName("PK_BetSelection");

        builder
            .HasIndex(bs => bs.EventId, "IX_BetSelection_EventId")
            .IsUnique(false);
    }
}