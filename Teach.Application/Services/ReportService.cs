using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teach.Application.Resources;
using Teach.Domain.Dto.Report;
using Teach.Domain.Entity;
using Teach.Domain.Enum;
using Teach.Domain.Interfaces.Repositories;
using Teach.Domain.Interfaces.Services;
using Teach.Domain.Interfaces.Validations;
using Teach.Domain.Result;
using Teach.Application.Common.CustomExceptions;
using Teach.Producer.Interfaces;
using Teach.Domain.Settings;
using Microsoft.Extensions.Options;

namespace Teach.Application.Services
{
    public class ReportService : IReportService
    {
        private readonly IBaseRepository<Report> _reportRepository;
        private readonly IBaseRepository<User> _userRepository;
        private readonly IReportValidator _reportvalidator;
        private readonly IMessageProducer _messageProducer;
        private readonly IOptions<RabbitMqSettings> _rabbitMqSettings;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public ReportService(IBaseRepository<Report> _reportRepository, IBaseRepository<User> _userRepository,
             IReportValidator _reportvalidator, IMapper _mapper, ILogger _logger,
             IMessageProducer _messageProducer, IOptions<RabbitMqSettings> _rabbitMqSettings)
        {
            this._reportRepository = _reportRepository;
            this._userRepository = _userRepository;
            this._reportvalidator = _reportvalidator;
            this._mapper = _mapper;
            this._logger = _logger;
            this._messageProducer = _messageProducer;
            this._rabbitMqSettings = _rabbitMqSettings;
        }

        public async Task<BaseResult<ReportDto>> CreateReportAsync(CreateReportDto reportDto)
        {
            
           
                var user = await _userRepository.GetAll()
                    .FirstOrDefaultAsync(x => x.Id == reportDto.UserId);

                var report = await _reportRepository.GetAll()
                    .FirstOrDefaultAsync(x => x.Name == reportDto.Name);

                var result = _reportvalidator.CreateValidator(report, user);

                if (!result.IsSuccess)
                {
                    throw new CustomBaseException(result.ErrorMessage, (int)result.ErrorCode);
                    /*return new BaseResult<ReportDto>()
                    {
                        ErrorMessage = result.ErrorMessage,
                        ErrorCode = result.ErrorCode
                    };*/
                }

                report = new Report()
                {
                    Name = reportDto.Name,
                    Description = reportDto.Description,
                    UserId = user.Id

                };

                await _reportRepository.CreateAsync(report);
                await _reportRepository.SaveChangesAsync();

            _messageProducer.SendMessage(report, _rabbitMqSettings.Value.RoutingKey, exchange: _rabbitMqSettings.Value.ExchangeName);

                return new BaseResult<ReportDto>()
                {
                    
                    Data = _mapper.Map<ReportDto>(report)
                };


            
            
            
        }

        public async Task<BaseResult<ReportDto>> DeleteReportAsync(long reportId)
        {
            
                var report = await _reportRepository.GetAll()
                    .FirstOrDefaultAsync(x=>x.Id == reportId);

                var result = _reportvalidator.ValidateOnNull(report);

                if (!result.IsSuccess)
                {
                    throw new CustomBaseException(result.ErrorMessage, (int)result.ErrorCode);
                   /* return new BaseResult<ReportDto>()
                    {
                        ErrorMessage = result.ErrorMessage,
                        ErrorCode = result.ErrorCode
                    };*/
                }

                 _reportRepository.Delete(report);
            await _reportRepository.SaveChangesAsync();

            return new BaseResult<ReportDto>()
                {
                    Data = _mapper.Map<ReportDto>(report)
                };
            
            
        }

        public async Task<CollectionResult<ReportDto>> GetAllReportsAsync(long userId)
        {
            ReportDto[] reports;
            
                reports = await _reportRepository.GetAll()
                    .Where(x => x.UserId == userId)
                    .Select(x => new ReportDto(x.Id, x.Name, x.Description, x.CreatedAt.ToLongDateString()))
                    .ToArrayAsync();

            
            

            if(!reports.Any())
            {
                _logger.Warning(ErrorMessage.ReportsNotFound, reports.Length);
                throw new CustomBaseException(ErrorMessage.ReportsNotFound, (int)ErrorCodes.ReportsNotFound);
        
            }

            return new CollectionResult<ReportDto>
            {
                Data = reports,
                Count = reports.Length
            };
            
        }

        /// <summary>
        /// Получение отчета с id
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        public async  Task<BaseResult<ReportDto>> GetReportByIdAsync(long reportId)
        {
            Report report;
           
                report = await _reportRepository.GetAll()
                    .FirstOrDefaultAsync(x => x.Id == reportId);

                var result = _reportvalidator.ValidateOnNull(report);

                if (!result.IsSuccess)
                {
                    _logger.Warning($"Отчет с Id:{reportId} не найден", reportId);
                    throw new CustomBaseException(ErrorMessage.ReportNotFound, (int)ErrorCodes.ReportNotFound);
                    
                    
                }
               

                return new BaseResult<ReportDto>()
                {
                    Data = _mapper.Map<ReportDto>(report),

                };

           
        }

        public async Task<BaseResult<ReportDto>> UpdateReportAsync(UpdateReportDto updateReportDto)
        {
            
                var report = await _reportRepository.GetAll()
                    .FirstOrDefaultAsync(x=>x.Id == updateReportDto.Id);

                var result = _reportvalidator.ValidateOnNull(report);

                if (!result.IsSuccess)
                {
                    throw new CustomBaseException(result.ErrorMessage, (int)result.ErrorCode);
                }

                report.Name = updateReportDto.Name;
                report.Description = updateReportDto.Description;

                _reportRepository.Update(report);
                await _reportRepository.SaveChangesAsync();

            return new BaseResult<ReportDto>()
                {
                    Data = _mapper.Map<ReportDto>(report)
                };


            
        }
    }
}
