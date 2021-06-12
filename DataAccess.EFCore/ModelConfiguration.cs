using Auth.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.EFCore
{
	public class ModelConfiguration
	{
		public void ConfigureModel(ModelBuilder modelBuilder)
		{
			ConfigureUser(modelBuilder);
			ConfigureMessage(modelBuilder);
			ConfigureConversationUser(modelBuilder);
			ConfigureConversation(modelBuilder);
			ConfigureChatCation(modelBuilder);
			ConfigureBlackList(modelBuilder);
			ConfigureAttachment(modelBuilder);
			ConfigureUserCredentials(modelBuilder);
		}

		private void ConfigureUser(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>().HasKey((u) => u.Id);
			modelBuilder.Entity<User>()
				.HasIndex((u) => u.Username)
				.IsUnique();
		}

		private void ConfigureMessage(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Message>().HasKey((m) => m.Id);
		}

		private void ConfigureConversationUser(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ConversationUser>().HasKey((cu) => cu.Id);
		}

		private void ConfigureConversation(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Conversation>().HasKey((c) => c.Id);
		}

		private void ConfigureChatCation(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ChatAction>().HasKey((ca) => ca.Id);
		}

		private void ConfigureBlackList(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<BlackList>().HasKey((bl) => bl.Id);
		}

		private void ConfigureAttachment(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Attachment>().HasKey((a) => a.Id);
		}

		private void ConfigureUserCredentials(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<UserCredentials>().HasKey((uc) => uc.Id);
			modelBuilder.Entity<UserCredentials>().HasIndex((uc) => uc.Login)
				.IsUnique();
			modelBuilder.Entity<UserCredentials>().HasIndex((uc) => uc.UserId)
				.IsUnique();
		}
	}
}
