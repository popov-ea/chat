using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Services.Dtos
{
	public class MessageServiceResult : EntityManipulationResult<Message, MessageFailCauses>
	{
	}
}
