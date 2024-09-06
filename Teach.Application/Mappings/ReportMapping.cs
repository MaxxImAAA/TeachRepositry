using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teach.Domain.Dto.Report;
using Teach.Domain.Entity;

namespace Teach.Application.Mappings
{
    public class ReportMapping : Profile
    {
        public ReportMapping()
        {
            CreateMap<Report, ReportDto>()
                .ReverseMap();
            
                
                
                
        }
    }
}
