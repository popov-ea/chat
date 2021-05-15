using Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UseCases.Interfaces.Dtos;
using UseCases.Interfaces.Providers;

namespace UseCases.Test
{
	class TestAttachmentContentProvider : IAttachmentContentProvider
	{
		public bool DidUpload(AttachmentDto attachment)
		{
			return true;
		}

		public Stream GetAttachmentContentStream(AttachmentDto attachment)
		{
			return new MemoryStream();
		}

		public bool SaveAttachmentFiles(AttachmentDto attachment, Stream attachmentFileStream)
		{
			return true;
		}

		public Task WaitForContentLoading(AttachmentDto attachment)
		{
			return Task.CompletedTask;
		}
	}
}
