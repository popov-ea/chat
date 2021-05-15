using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace UseCases.Interfaces.Dtos
{
	public class BlackListResultDto : EntityManipulationResultDto<BlackListDto, BlackListFailCauses>
	{
	}
}
