using Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UseCases.Interfaces.Providers;

namespace UseCases.Test
{
	class TestAttachmentContentProvider : IAttachmentContentProvider
	{
		public bool DidUpload(Attachment attachment)
		{
			return true;
		}

		public Stream GetAttachmentContentStream(Attachment attachment)
		{
			return new MemoryStream();
		}

		public bool SaveAttachmentFiles(Attachment attachment, Stream attachmentFileStream)
		{
			return true;
		}

		public Task WaitForContentLoading(Attachment attachment)
		{
			return Task.CompletedTask;
		}
	}
}
