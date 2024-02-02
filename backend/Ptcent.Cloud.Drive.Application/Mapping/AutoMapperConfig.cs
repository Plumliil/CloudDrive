﻿using AutoMapper;
using Ptcent.Cloud.Drive.WebApi.Domain.Dto.Request;
using Ptcent.Cloud.Drive.WebApi.Domain.Dto.Response;
using Ptcent.Cloud.Drive.WebApi.Domain.Entities;
using Ptcent.Cloud.Drive.WebApi.Domain.Entities.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ptcent.Cloud.Drive.WebApi.Application.Mapping
{
    public static class AutoMapperConfig
    {
        private readonly static IMapper mapper;
        /// <summary>
        /// 映射
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TDestination Map<TSource, TDestination>(TSource source)
        {
            return mapper.Map<TSource, TDestination>(source);
        }
        public static TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return mapper.Map(source, destination);
        }
        static AutoMapperConfig()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RegistrationAccountRequestDto, UserEntity>();
                cfg.CreateMap<FileEntity, FileReponseDto>();
            });
            mapper = config.CreateMapper();
        }
    }
}
