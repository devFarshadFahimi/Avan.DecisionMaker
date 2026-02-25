using Application.Common.Utils;
using Domain.Aggregates;
using Domain.Aggregates.Enums;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class SwitchController(IDecisionEngine decisionEngine, ApplicationDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public IActionResult CreateSwitchStages()
    {
        var transaction = dbContext.Database.BeginTransaction();
        try
        {
            var graph = new DecisionGraph("SwitchTest");
            dbContext.DecisionGraphs.Add(graph);
            dbContext.SaveChanges();

            var riskSwitchStage = new DecisionStage(
                graphId: graph.Id,
                name: "RiskAssessmentSwitch",
                priority: 2,
                hitPolicy: HitPolicyType.First,
                stageType: StageType.Decision // یا StageType.Switch
            );

            // Rule 1: CreditScore < 600 => High Risk
            var highRiskRule = new DecisionRule(riskSwitchStage.Id, LogicalOperator.And, 1);
            highRiskRule.AddCondition("Less", "600", propId: 10002);
            highRiskRule.AddOutput("riskLevel", "High");
            highRiskRule.AddOutput("interestRate", "20");
            riskSwitchStage.AddRule(highRiskRule);

            // Rule 2: 600 <= CreditScore < 750 => Medium Risk
            var medRiskRule = new DecisionRule(riskSwitchStage.Id, LogicalOperator.And, 2);
            medRiskRule.AddCondition("GreaterOrEqual", "600", propId: 10002);
            medRiskRule.AddCondition("Less", "750", propId: 10002);
            medRiskRule.AddOutput("riskLevel", "Medium");
            medRiskRule.AddOutput("interestRate", "12");
            riskSwitchStage.AddRule(medRiskRule);

            // Rule 3: CreditScore >= 750 => Low Risk
            var lowRiskRule = new DecisionRule(riskSwitchStage.Id, LogicalOperator.And, 3);
            lowRiskRule.AddCondition("GreaterOrEqual", "750", propId: 10002);
            lowRiskRule.AddOutput("riskLevel", "Low");
            lowRiskRule.AddOutput("interestRate", "8");
            riskSwitchStage.AddRule(lowRiskRule);

            var debtEvaluationStage = new DecisionStage(
                graphId: graph.Id,
                name: "DebtEvaluation",
                priority: 3,
                hitPolicy: HitPolicyType.First,
                stageType: StageType.Decision
            );

            var approvalDecisionStage = new DecisionStage(
                graphId: graph.Id,
                name: "ApprovalDecision",
                priority: 4,
                hitPolicy: HitPolicyType.First,
                stageType: StageType.Decision
            );

            var maxAmountStage = new DecisionStage(
                graphId: graph.Id,
                name: "MaxAmountCalculation",
                priority: 5,
                hitPolicy: HitPolicyType.First,
                stageType: StageType.Decision
            );


            // High Risk -> ApprovalDecision
            riskSwitchStage.OutgoingConnections.Add(new StageConnection(graph.Id, riskSwitchStage.Id, approvalDecisionStage.Id));

            // Medium Risk -> DebtEvaluation
            riskSwitchStage.OutgoingConnections.Add(new StageConnection(graph.Id, riskSwitchStage.Id, debtEvaluationStage.Id));

            // Low Risk -> DebtEvaluation
            riskSwitchStage.OutgoingConnections.Add(new StageConnection(graph.Id, riskSwitchStage.Id, debtEvaluationStage.Id));

            // Default (اگر هیچ Rule ای match نکرد) -> MaxAmountCalculation
            riskSwitchStage.OutgoingConnections.Add(new StageConnection(graph.Id, riskSwitchStage.Id, maxAmountStage.Id));


            dbContext.SaveChanges();
            transaction.Commit();

        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
        return Ok();
    }
}
