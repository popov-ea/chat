using Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UseCases.Interfaces.Dtos;

namespace UseCases.Interfaces.Providers
{
	/// <summary>
	/// Use it to implement file storage logic
	/// </summary>
	public interface IAttachmentContentProvider
	{
		public Stream GetAttachmentContentStream(AttachmentDto attachment);
		public bool SaveAttachmentFiles(AttachmentDto attachment, Stream attachmentFileStream);

		public bool DidUpload(AttachmentDto attachment);
		public Task WaitForContentLoading(AttachmentDto attachment);
	}
}
