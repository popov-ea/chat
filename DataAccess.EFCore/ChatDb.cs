using Auth.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.EFCore
{
	internal class ChatDb : DbContext
	{
		private static string _connectionString = ConfigurationHelpers.GetConnectionString();

		public DbSet<Attachment> Attachments { get; set; }
		public DbSet<BlackList> BlackLists { get; set; }
		public DbSet<ChatAction> ChatActions { get; set; }
		public DbSet<Conversation> Conversations { get; set; }
		public DbSet<ConversationUser> ConversationUsers { get; set; }
		public DbSet<Message> Messages { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<UserCredentials> UserCredentials { get; set; }

		public ChatDb()
		{

		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseNpgsql(_connectionString);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			new ModelConfiguration().ConfigureModel(modelBuilder);
			base.OnModelCreating(modelBuilder);
		}
	}
}