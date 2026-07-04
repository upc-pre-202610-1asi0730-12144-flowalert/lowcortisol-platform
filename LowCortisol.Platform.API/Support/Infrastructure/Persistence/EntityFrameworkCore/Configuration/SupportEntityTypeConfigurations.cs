using LowCortisol.Platform.API.Support.Domain.Model.Aggregates;
using LowCortisol.Platform.API.Support.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LowCortisol.Platform.API.Support.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public sealed class SupportTicketEntityTypeConfiguration : IEntityTypeConfiguration<SupportTicket>
{
    public void Configure(EntityTypeBuilder<SupportTicket> builder)
    {
        builder.ToTable("support_tickets");
        builder.HasKey(ticket => ticket.Id);
        builder.Property(ticket => ticket.Id).ValueGeneratedNever();
        builder.Property(ticket => ticket.UserId).IsRequired().HasMaxLength(80);
        builder.Property(ticket => ticket.SiteId).IsRequired().HasMaxLength(80);
        builder.Property(ticket => ticket.Title).IsRequired().HasMaxLength(180);
        builder.Property(ticket => ticket.Description).IsRequired().HasMaxLength(960);
        builder.Property(ticket => ticket.Category).IsRequired().HasMaxLength(80);
        builder.Property(ticket => ticket.Priority).HasConversion<string>().IsRequired().HasMaxLength(40);
        builder.Property(ticket => ticket.Status).HasConversion<string>().IsRequired().HasMaxLength(40);
        builder.Property(ticket => ticket.AssignedAgentId).IsRequired().HasMaxLength(80);
        builder.Property(ticket => ticket.CreatedAt).IsRequired();
        builder.Property(ticket => ticket.UpdatedAt).IsRequired();
        builder.HasIndex(ticket => ticket.UserId);
        builder.HasIndex(ticket => ticket.SiteId);
        builder.HasIndex(ticket => ticket.Status);
        builder.HasIndex(ticket => ticket.Priority);
    }
}

public sealed class SupportMessageEntityTypeConfiguration : IEntityTypeConfiguration<SupportMessage>
{
    public void Configure(EntityTypeBuilder<SupportMessage> builder)
    {
        builder.ToTable("support_messages");
        builder.HasKey(message => message.Id);
        builder.Property(message => message.Id).ValueGeneratedNever();
        builder.Property(message => message.TicketId).IsRequired();
        builder.Property(message => message.SenderId).IsRequired().HasMaxLength(80);
        builder.Property(message => message.SenderType).IsRequired().HasMaxLength(40);
        builder.Property(message => message.Content).IsRequired().HasMaxLength(1200);
        builder.Property(message => message.Status).IsRequired().HasMaxLength(40);
        builder.Property(message => message.CreatedAt).IsRequired();
        builder.Property(message => message.UpdatedAt).IsRequired();
        builder.HasIndex(message => message.TicketId);
    }
}

public sealed class SupportConversationEntityTypeConfiguration : IEntityTypeConfiguration<SupportConversation>
{
    public void Configure(EntityTypeBuilder<SupportConversation> builder)
    {
        builder.ToTable("support_conversations");
        builder.HasKey(conversation => conversation.Id);
        builder.Property(conversation => conversation.Id).ValueGeneratedNever();
        builder.Property(conversation => conversation.TicketId).IsRequired();
        builder.Property(conversation => conversation.Status).HasConversion<string>().IsRequired().HasMaxLength(40);
        builder.Property(conversation => conversation.CreatedAt).IsRequired();
        builder.Property(conversation => conversation.UpdatedAt).IsRequired();
        builder.HasIndex(conversation => conversation.TicketId).IsUnique();
    }
}

public sealed class SupportAgentEntityTypeConfiguration : IEntityTypeConfiguration<SupportAgent>
{
    public void Configure(EntityTypeBuilder<SupportAgent> builder)
    {
        builder.ToTable("support_agents");
        builder.HasKey(agent => agent.Id);
        builder.Property(agent => agent.Id).ValueGeneratedNever();
        builder.Property(agent => agent.Name).IsRequired().HasMaxLength(160);
        builder.Property(agent => agent.Specialty).IsRequired().HasMaxLength(120);
        builder.Property(agent => agent.Status).HasConversion<string>().IsRequired().HasMaxLength(40);
        builder.Property(agent => agent.CreatedAt).IsRequired();
        builder.Property(agent => agent.UpdatedAt).IsRequired();
        builder.HasIndex(agent => agent.Status);
    }
}

public sealed class KnowledgeArticleEntityTypeConfiguration : IEntityTypeConfiguration<KnowledgeArticle>
{
    public void Configure(EntityTypeBuilder<KnowledgeArticle> builder)
    {
        builder.ToTable("knowledge_articles");
        builder.HasKey(article => article.Id);
        builder.Property(article => article.Id).ValueGeneratedNever();
        builder.Property(article => article.Title).IsRequired().HasMaxLength(220);
        builder.Property(article => article.Summary).IsRequired().HasMaxLength(520);
        builder.Property(article => article.Category).IsRequired().HasMaxLength(80);
        builder.Property(article => article.HelpfulCount).IsRequired();
        builder.Property(article => article.Content).IsRequired().HasMaxLength(3000);
        builder.Property(article => article.CreatedAt).IsRequired();
        builder.Property(article => article.UpdatedAt).IsRequired();
        builder.HasIndex(article => article.Category);
    }
}
