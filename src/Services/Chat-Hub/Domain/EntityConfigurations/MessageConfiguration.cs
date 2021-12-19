﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.EntityConfigurations
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {

            builder.HasOne(u => u.Recipient)
                                          .WithMany(m => m.MessagesReceived)
                                          .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(u => u.Sender)
                                          .WithMany(m => m.MessagesSent)
                                          .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
