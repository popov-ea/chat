using Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Interfaces.Providers
{
	/// <summary>
	/// Use it to implement file storage logic
	/// </summary>
	public interface IAttachmentContentProvider
	{
		public Stream GetAttachmentContentStream(Attachment attachment);
		public bool SaveAttachmentFiles(Attachment attachment, Stream attachmentFileStream);

		public bool DidUpload(Attachment attachment);
		public Task WaitForContentLoading(Attachment attachment);
	}
}
