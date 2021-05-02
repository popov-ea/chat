using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
	public class Attachment : IEntity
	{
		public long Id { get; set; }
		public AttachmentType Type { get; set; }
		public string ContentPath { get; set; }

		public long MessageId { get; set; }
		public Message Message { get; set; }

		public DateTime? UploadedAt { get; set; }
		public DateTime? DeletedAt { get; set; }
	}
}
