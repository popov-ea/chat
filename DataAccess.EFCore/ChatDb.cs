using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.EFCore
{
	internal class ChatDb : DbContext
	{
		private string _connectionString;

		public DbSet<Attachment> Attachments { get; set; }
		public DbSet<BlackList> BlackLists { get; set; }
		public DbSet<ChatAction> ChatActions { get; set; }
		public DbSet<Conversation> Conversations { get; set; }
		public DbSet<ConversationUser> ConversationUsers { get; set; }
		public DbSet<Message> Messages { get; set; }
		public DbSet<User> Users { get; set; }

		public ChatDb(DbConfig dbConfig)
		{
			_connectionString = dbConfig.ConnectionString;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseNpgsql(_connectionString);
		}
	}
}