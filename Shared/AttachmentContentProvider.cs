using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UseCases.Interfaces.Dtos;
using UseCases.Interfaces.Providers;

namespace Shared
{
	public class AttachmentContentProvider : IAttachmentContentProvider
	{
		//TODO: implement
		public bool DidUpload(AttachmentDto attachment)
		{
			throw new NotImplementedException();
		}

		public Stream GetAttachmentContentStream(AttachmentDto attachment)
		{
			throw new NotImplementedException();
		}

		public bool SaveAttachmentFiles(AttachmentDto attachment, Stream attachmentFileStream)
		{
			throw new NotImplementedException();
		}

		public Task WaitForContentLoading(AttachmentDto attachment)
		{
			throw new NotImplementedException();
		}
	}
}
