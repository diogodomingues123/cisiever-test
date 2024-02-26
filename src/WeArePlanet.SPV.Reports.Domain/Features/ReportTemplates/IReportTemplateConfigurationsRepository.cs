using LanguageExt;

using WeArePlanet.SPV.Reports.Domain.Common.Exceptions;
using WeArePlanet.SPV.Reports.Domain.Features.ReportTemplates.Queries;

namespace WeArePlanet.SPV.Reports.Domain.Features.ReportTemplates;

public interface IReportTemplateConfigurationsRepository
{
    Task<Either<NotFoundException, ReportTemplateConfiguration>> GetAsync(Guid id,
        CancellationToken cancellationToken = default);
}
