using Domain.Enums;
using System;

namespace UseCases.Interfaces.Dtos
{
	public class AttachmentDto : IEntityDto
	{
		public long Id { get; set; }
		public AttachmentType Type { get; set; }
		public string ContentPath { get; set; }

		public long MessageId { get; set; }
		public MessageDto Message { get; set; }

		public DateTime? UploadedAt { get; set; }
		public DateTime? DeletedAt { get; set; }
	}
}