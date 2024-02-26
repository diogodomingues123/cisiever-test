using System.Diagnostics.CodeAnalysis;

using FluentValidation.Results;

using LanguageExt;

using WeArePlanet.SPV.Reports.Domain.Common;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Events;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models.Extensions;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.State;
using WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.State.Exceptions;
using WeArePlanet.SPV.Reports.Domain.Features.ReportTemplates.Queries;

namespace WeArePlanet.SPV.Reports.Domain.Features.ReportSchedules.Commands.Models;

public class ReportSchedule : Entity<ReportSchedule, ReportScheduleId>, IOwned
{
    internal ReportSchedule(ReportScheduleId id, string name, Owner owner, ReportTemplateConfiguration template,
        IReportExecutionPlan executionPlan, IReportScheduleState state, ReportScheduleInput? input = null)
        : base(id)
    {
        this.Name = name;
        this.Template = template;
        this.ExecutionPlan = executionPlan;
        this.Owner = owner;
        this.Input = input;
        this.State = state;
    }

    public string Name { get; internal init; }

    public ReportTemplateConfiguration Template { get; internal init; }

    public IReportExecutionPlan ExecutionPlan { get; internal init; }

    public ReportScheduleInput? Input { get; internal init; }

    public WebhookConfiguration? WebhookConfiguration { get; internal init; }

    public IReportScheduleState State { get; private set; }

    public DateTime CreatedAt { get; internal init; }

    public DateTime UpdatedAt { get; internal set; }

    public bool IsActive => this.State is ActiveReportScheduleState;

    public Owner Owner { get; internal init; }

    public async Task ScheduleAsync(IReportScheduler reportScheduler, CancellationToken cancellationToken = default)
    {
        await this.ExecutionPlan.ScheduleAsync(this, reportScheduler, cancellationToken);
    }

    public async Task DescheduleAsync(IReportScheduler reportScheduler, CancellationToken cancellationToken = default)
    {
        await this.ExecutionPlan.DescheduleAsync(this, reportScheduler, cancellationToken);
    }

    public void ToggleActivation(bool toggle, IDateTimeProvider dateTimeProvider)
    {
        IReportScheduleState desiredState =
            toggle ? new ActiveReportScheduleState() : new InactiveReportScheduleState();

        if (!this.State.CanTransitionTo(desiredState))
        {
            throw new StateTransitionNotPossibleException(this, desiredState);
        }

        if (toggle == this.IsActive)
        {
            // Nothing changed, don't change anything
            return;
        }

        // Here we don't need to query "ExecutionPlan.EvaluateStateTransition()" because we're simply toggling the schedule.
        // So we will never transition to an Executed state at this point.
        this.State = desiredState;
        this.RefreshUpdateDate(dateTimeProvider);
        this.RaiseEvent(new ReportScheduleToggledEvent(this, dateTimeProvider));
    }

    public void Archive(IDateTimeProvider dateTimeProvider)
    {
        var desiredState = new ArchivedReportScheduleState();

        if (!this.State.CanTransitionTo(desiredState))
        {
            throw new StateTransitionNotPossibleException(this, desiredState);
        }

        var previousState = (IReportScheduleState)this.State.Clone();

        // Here we don't need to query "ExecutionPlan.EvaluateStateTransition()" because we're simply toggling the schedule.
        // So we will never transition to an Executed state at this point.
        this.State = desiredState;
        this.RefreshUpdateDate(dateTimeProvider);
        this.RaiseEvent(new ReportScheduleArchivedEvent(this, previousState, dateTimeProvider));
    }

    // TODO Remove this attribute and test on SIP-4073
    [ExcludeFromCodeCoverage]
    public bool HasExecuted()
    {
        return this.State is ExecutedReportScheduleState;
    }

    // TODO Remove this attribute and test on SIP-4073
    [ExcludeFromCodeCoverage]
    public void MarkAsExecuted(IDateTimeProvider dateTimeProvider)
    {
        var desiredState = new ExecutedReportScheduleState();

        if (!this.State.CanTransitionTo(desiredState))
        {
            throw new StateTransitionNotPossibleException(this, desiredState);
        }

        var resultingState = this.ExecutionPlan.EvaluateStateTransition(this, desiredState);

        this.State = resultingState;
        this.RefreshUpdateDate(dateTimeProvider);
        this.RaiseEvent(new ReportScheduleExecutedEvent(this, dateTimeProvider));
    }

    public DateTime? GetNextExecutionDate(IDateTimeProvider dateTimeProvider)
    {
        if (!this.State.IsActive())
        {
            return null;
        }

        return this.ExecutionPlan.GetNextExecutionDateInUtc(dateTimeProvider);
    }

    public static async Task<Either<ValidationResult, ReportSchedule>> CreateAsync(
        IReportScheduleBuilder builder,
        IDateTimeProvider dateTimeProvider)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        if (dateTimeProvider == null)
        {
            throw new ArgumentNullException(nameof(dateTimeProvider));
        }

        var reportScheduleBuildResult = await builder.BuildAsync();

        return reportScheduleBuildResult.Map(reportConfiguration =>
        {
            reportConfiguration.RaiseEvent(new ReportScheduleCreatedEvent(reportConfiguration, dateTimeProvider));
            reportConfiguration.ToggleActivation(true, dateTimeProvider);

            return reportConfiguration;
        });
    }

    public static async Task<Either<ValidationResult, ReportSchedule>> UpdateAsync(
        IReportScheduleBuilder builder,
        IDateTimeProvider dateTimeProvider)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        if (dateTimeProvider == null)
        {
            throw new ArgumentNullException(nameof(dateTimeProvider));
        }

        var reportScheduleBuildResult = await builder.BuildAsync();

        return reportScheduleBuildResult.Map(reportConfiguration =>
        {
            reportConfiguration.RaiseEvent(new ReportScheduleUpdatedEvent(reportConfiguration, dateTimeProvider));
            return reportConfiguration;
        });
    }

    private void RefreshUpdateDate(IDateTimeProvider dateTimeProvider)
    {
        this.UpdatedAt = dateTimeProvider.GetUtcNow().UtcDateTime;
    }
}
