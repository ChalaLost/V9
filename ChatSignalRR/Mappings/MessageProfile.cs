﻿using AutoMapper;
using ChatSignalRR.Helpers;
using ChatSignalRR.Models;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatSignalRR.Mappings
{
    public class MessageProfile:Profile
    {
        public MessageProfile()
        { // test
            CreateMap<Message, MessageViewModel>()
                /*.ForMember(dst => dst.From, opt => opt.MapFrom(x => x.FromUser.FullName))*/
                .ForMember(dst => dst.Room, opt => opt.MapFrom(x => x.ToRoom.Name))
                /*.ForMember(dst => dst.Avatar, opt => opt.MapFrom(x => x.FromUser.Avatar))*/
                .ForMember(dst => dst.Content, opt => opt.MapFrom(x => BasicEmojis.ParseEmojis(x.Content)))
                .ForMember(dst => dst.Timestamp, opt => opt.MapFrom(x => x.Timestamp));
            CreateMap<MessageViewModel, Message>();
        }
    }
}
