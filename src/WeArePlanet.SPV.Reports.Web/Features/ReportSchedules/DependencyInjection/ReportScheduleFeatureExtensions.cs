using System.Diagnostics.CodeAnalysis;

using FluentValidation.Validators;

using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Validators;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Repositories;
using WeArePlanet.SPV.Reports.Persistence.ReportSchedules;

namespace WeArePlanet.SPV.Reports.Web.Features.ReportSchedules.DependencyInjection;

[ExcludeFromCodeCoverage]
internal static class ReportScheduleFeatureExtensions
{
    public static IServiceCollection AddReportSchedulesFeature(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddScoped<
                IPropertyValidator<ReportSchedule, ReportScheduleInput>,
                ReportScheduleInputValidator>()
            .AddTransient<IReportScheduleBuilder, ReportScheduleBuilder>()
            .AddScoped<IReportScheduleRepository, MongoReportScheduleRepository>()
            .AddSingleton<IReportScheduler, HangfireReportScheduler>();
    }
}
