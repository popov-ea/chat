using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Domain.Entities;
using UseCases.Interfaces.Dtos;

namespace UseCases.Implementations
{
	public static class MapperHelpers
	{
		public static Mapper GetConfiguredMapper()
		{
			var config = CreateMapperConfiguration();
			return new Mapper(config);
		}

		private static MapperConfiguration CreateMapperConfiguration()
		{
			return new MapperConfiguration((cfg) =>
			{
				cfg.AddEntitiesMappingConfiguration();
			});
		}

		private static IMapperConfigurationExpression AddEntitiesMappingConfiguration(this IMapperConfigurationExpression cfg)
		{
			cfg.CreateMap<UserDto, User>();
			cfg.CreateMap<User, UserDto>();

			cfg.CreateMap<MessageDto, Message>();
			cfg.CreateMap<Message, MessageDto>();

			cfg.CreateMap<ChatActionDto, ChatAction>();
			cfg.CreateMap<ChatAction, ChatActionDto>();

			cfg.CreateMap<AttachmentDto, Attachment>();
			cfg.CreateMap<Attachment, AttachmentDto>();

			cfg.CreateMap<BlackListDto, BlackList>();
			cfg.CreateMap<BlackList, BlackListDto>();

			cfg.CreateMap<ConversationDto, Conversation>();
			cfg.CreateMap<Conversation, ConversationDto>();

			cfg.CreateMap<ConversationUserDto, ConversationUser>();
			cfg.CreateMap<ConversationUser, ConversationUserDto>();

			return cfg;
		}
	}
}
