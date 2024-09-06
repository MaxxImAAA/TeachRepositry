using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teach.Domain.Dto.Report;
using Teach.Domain.Result;

namespace Teach.Domain.Interfaces.Services
{
    public interface IReportService
    {
        Task<CollectionResult<ReportDto>> GetAllReportsAsync(long userId);

        /// <summary>
        /// Получение отчета по id
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        Task<BaseResult<ReportDto>> GetReportByIdAsync(long reportId);

        /// <summary>
        /// Создание отчета с базовыми параметрами
        /// </summary>
        /// <param name="reportDto"></param>
        /// <returns></returns>
        Task<BaseResult<ReportDto>> CreateReportAsync(CreateReportDto reportDto);

       // Task<BaseResult<ReportDto>> UpdateReportAsync(long reportId);

        /// <summary>
        /// Удаление отчета по id
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        Task<BaseResult<ReportDto>> DeleteReportAsync(long reportId);

        /// <summary>
        /// Обновление отчета по id
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        Task<BaseResult<ReportDto>> UpdateReportAsync(UpdateReportDto updateReportDto);
    }
}
