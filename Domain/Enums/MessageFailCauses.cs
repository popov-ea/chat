using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Enums
{
	public enum MessageFailCauses
	{
		/// <summary>
		/// error while working with attachments
		/// </summary>
		AttachmentSaveFailed,
		/// <summary>
		/// sender is blocked by reciever
		/// </summary>
		UserBlocked
	}
}
