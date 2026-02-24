using Application.Common.Utils;
using Domain.Aggregates;
using Domain.Aggregates.Enums;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace WebApi.Controllers;

public class InsuranceRequest
{
    public int Age { get; set; }
    public bool HasChronicDisease { get; set; }
    public decimal BMI { get; set; }
    public bool Smoker { get; set; }
    public int AlcoholConsumptionPerWeek { get; set; }
    public int ExerciseHoursPerWeek { get; set; }
    public int PreviousClaims { get; set; }
    public int FamilyMedicalHistoryScore { get; set; }
}


public record GraphPropDTO(string PropName, string DataType);
public record GraphCreateDTO(string Name, List<GraphPropDTO> GraphPropDTOs);



public record StageOutputDTO(string Key, string? Value);
public record StageConditionDTO(string Op, string Value, long PropId);
public record StageRuleDTO(LogicalOperator LogicalOperator, byte Priority, List<StageConditionDTO> ConditionDTOs, List<StageOutputDTO> OutputDTOs);
public record StageCreateDTO(string Name, byte Order, long GraphId, List<StageRuleDTO> Rules);



public record StageConnectionBoundsDTO(long FromStageId, long ToStageId);
public record StageConnectionDTO(long GraphId, List<StageConnectionBoundsDTO> Bounds);

[ApiController]
[Route("api/[controller]/[action]")]
public class DecisionController(IDecisionEngine decisionEngine, ApplicationDbContext dbContext) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> ConnectStagesAsync([FromBody] StageConnectionDTO stageConnection)
    {
        DecisionGraph? graph = dbContext.DecisionGraphs
            .Include(p => p.Stages)
                .ThenInclude(p => p.IncomingConnections)
            .Include(p => p.Stages)
                .ThenInclude(p => p.OutgoingConnections)
            .AsNoTracking()
            .FirstOrDefault(p => p.Id == stageConnection.GraphId);

        if (graph == null) return NotFound();

        var connections = new List<StageConnection>();


        #region For Insurance Scenario
        //graph.Connect("eligibilityCheck", "healthScore");
        //graph.Connect("healthScore", "riskFactors");
        //graph.Connect("riskFactors", "lifestyle");
        //graph.Connect("lifestyle", "premiumCalculation");

        //// Stage 6 (CombinedRiskEvaluation) ورودی از 3,4,5
        //graph.Connect("riskFactors", "combinedRiskEvaluation");
        //graph.Connect("lifestyle", "combinedRiskEvaluation");
        //graph.Connect("premiumCalculation", "combinedRiskEvaluation");

        //graph.Connect("combinedRiskEvaluation", "fraudCheck");
        //graph.Connect("fraudCheck", "finalApproval");
        //var collection = new List<(long FromStageId, long ToStageId)>()
        //{
        //    new (1,2),
        //    new (2,3),
        //    new (3,4),
        //    new (4,5),

        //    new (3,6),
        //    new (4,6),
        //    new (5,6),

        //    new (6,7),
        //    new (7,8)
        //}; 
        #endregion

        ////loan demo
        //var collection = new List<(long FromStageId, long ToStageId)>()
        //    {
        //    new (10004,10005),
        //    new (10005,10006),
        //};



        var collection = new List<(long FromStageId, long ToStageId)>()
            {
            new (10004,10005),
            new (10005,10006),
        };

        foreach (var (FromStageId, ToStageId) in collection)
        {
            var connection = new StageConnection(
                stageConnection.GraphId,
                FromStageId,
                ToStageId);

            dbContext.StageConnection.Add(connection);
        }

        dbContext.SaveChanges();
        return Ok();
    }


    [HttpPost("graphId")]
    public async Task<IActionResult> CreateStageAsync([FromQuery] long graphId, [FromBody] List<StageCreateDTO> stages)
    {
        DecisionGraph? graph = dbContext.DecisionGraphs.FirstOrDefault(p => p.Id == graphId);

        if (graph == null) return NotFound();
        foreach (var stageCreateDTO in stages)
        {
            var stage = new DecisionStage(graphId, stageCreateDTO.Name, stageCreateDTO.Order);
            foreach (var item in stageCreateDTO.Rules)
            {
                var stageRule = new DecisionRule(stage.Id, item.LogicalOperator, item.Priority);
                foreach (var condition in item.ConditionDTOs)
                {
                    stageRule.AddCondition(condition.Op, condition.Value, condition.PropId);
                }
                foreach (var output in item.OutputDTOs)
                {
                    stageRule.AddOutput(output.Key, output.Value);
                }

                stage.AddRule(stageRule);
            }
            dbContext.DecisionStages.Add(stage);
            dbContext.SaveChanges();
        }
        return Ok();
    }


    [HttpPost]
    public async Task<IActionResult> CreateStageAsync([FromBody] StageCreateDTO stageCreateDTO)
    {
        DecisionGraph? graph = dbContext.DecisionGraphs.FirstOrDefault(p => p.Id == stageCreateDTO.GraphId);

        if (graph == null) return NotFound();

        var stage = new DecisionStage(stageCreateDTO.GraphId, stageCreateDTO.Name, stageCreateDTO.Order);
        foreach (var item in stageCreateDTO.Rules)
        {
            var stageRule = new DecisionRule(stage.Id, item.LogicalOperator, item.Priority);
            foreach (var condition in item.ConditionDTOs)
            {
                stageRule.AddCondition(condition.Op, condition.Value, condition.PropId);
            }
            foreach (var output in item.OutputDTOs)
            {
                stageRule.AddOutput(output.Key, output.Value);
            }

            stage.AddRule(stageRule);
        }
        dbContext.DecisionStages.Add(stage);
        dbContext.SaveChanges();
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> CreateGraphAsync([FromBody] GraphCreateDTO graphCreateDTO)
    {
        var graph = new DecisionGraph(graphCreateDTO.Name)
        {
            //DecisionGraphProps = [.. DecisionGraphPropsFactory.CreateFromModel<InsuranceRequest>()]
            DecisionGraphProps = [.. graphCreateDTO.GraphPropDTOs.Select(p => new DecisionGraphProps(p.PropName, p.DataType))]
        };
        dbContext.DecisionGraphs.Add(graph);
        dbContext.SaveChanges();
        return Ok();
    }

    [HttpGet("Insurance-Demo")]
    public async Task<IActionResult> InsuranceDemo()
    {
        DecisionGraph? graph = await dbContext.DecisionGraphs
            .Include(p => p.DecisionGraphProps)
             .Include(x => x.Stages)
                 .ThenInclude(s => s.OutgoingConnections)
             .Include(p => p.Stages)
             .ThenInclude(p => p.Rules)
             .ThenInclude(p => p.Conditions)
             .Include(p => p.Stages)
             .ThenInclude(p => p.Rules)
             .ThenInclude(p => p.Outputs)

             .FirstOrDefaultAsync();

        ArgumentNullException.ThrowIfNull(graph);

        InsuranceRequest[] scenarios =
               [
                    new InsuranceRequest
                    {
                        Age = 28,
                        HasChronicDisease = false,
                        BMI = 22,
                        Smoker = false,
                        AlcoholConsumptionPerWeek = 2,
                        ExerciseHoursPerWeek = 5,
                        PreviousClaims = 0,
                        FamilyMedicalHistoryScore = 10
                    },
                    new InsuranceRequest
                    {
                        Age = 45,
                        HasChronicDisease = true,
                        BMI = 29,
                        Smoker = true,
                        AlcoholConsumptionPerWeek = 7,
                        ExerciseHoursPerWeek = 1,
                        PreviousClaims = 3,
                        FamilyMedicalHistoryScore = 30
                    }
               ];

        var results = new List<object>();
        var computed = new Dictionary<string, Func<InsuranceRequest, object?>>
        {
            ["highRiskAge"] = r => r.Age >= 50,
            ["obese"] = r => r.BMI >= 30,
            ["activeLifestyle"] = r => r.ExerciseHoursPerWeek >= 3
                                     && r.AlcoholConsumptionPerWeek <= 5
        };

        foreach (var scenario in scenarios)
        {
            var context = DecisionContextBuilder.Build(scenario, computed);

            await decisionEngine.ExecuteAsync(graph, context);
            results.Add(context.Snapshot());
        }

        return Ok(results);
    }

    [HttpGet("LoanDemo")]
    public async Task<IActionResult> LoanDemo()
    {
        DecisionGraph? graph = await dbContext.DecisionGraphs
            .Include(p => p.DecisionGraphProps)
            .Include(x => x.Stages)
                .ThenInclude(s => s.OutgoingConnections)
            .Include(p => p.Stages)
            .ThenInclude(p => p.Rules)
            .ThenInclude(p => p.Conditions)
            .Include(p => p.Stages)
            .ThenInclude(p => p.Rules)
            .ThenInclude(p => p.Outputs)
            .Where(p => p.Id == 2)
            .FirstOrDefaultAsync();

        ArgumentNullException.ThrowIfNull(graph);

        LoanApprovalDecisionContext[] scenarios =
        [
            new(){
                CreditScore = 780,
                MonthlyIncome = 9000m,
                ExistingDebt = 10000m,
                EmploymentYears = 6,
                HasLatePayments = false,
                LoanAmount = 80000m
            }
        ];

        var results = new List<object>();

        foreach (var scenario in scenarios)
        {
            var context = DecisionContextBuilder.Build(scenario);

            await decisionEngine.ExecuteAsync(graph, context);
            results.Add(context.Snapshot());
        }

        return Ok(results);
    }

    public sealed class LoanApprovalDecisionContext
    {
        public int CreditScore { get; init; }
        public decimal MonthlyIncome { get; init; }
        public decimal ExistingDebt { get; init; }
        public int EmploymentYears { get; init; }
        public bool HasLatePayments { get; init; }
        public decimal LoanAmount { get; init; }
    }

}

public static class DecisionContextBuilder
{
    private static readonly ConcurrentDictionary<Type, Func<object, Dictionary<string, object?>>> _cache
        = new();

    public static DecisionContext Build<T>(
    T input,
    IDictionary<string, Func<T, object?>>? computed = null)
    {
        if (input is null)
            throw new ArgumentNullException(nameof(input));

        var context = new DecisionContext();

        var extractor = _cache.GetOrAdd(typeof(T), CreateExtractor);
        var values = extractor(input!);

        foreach (var kv in values)
        {
            context.Set(kv.Key, kv.Value);
        }

        if (computed is not null)
        {
            foreach (var kv in computed)
            {
                context.Set(kv.Key, kv.Value(input));
            }
        }

        return context;
    }


    private static Func<object, Dictionary<string, object?>> CreateExtractor(Type type)
    {
        var param = Expression.Parameter(typeof(object), "input");
        var cast = Expression.Convert(param, type);

        var dictVar = Expression.Variable(typeof(Dictionary<string, object?>), "dict");

        var newDict = Expression.Assign(
            dictVar,
            Expression.New(typeof(Dictionary<string, object?>)));

        var expressions = new List<Expression> { newDict };

        foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (!prop.CanRead)
            {
                continue;
            }

            var propertyAccess = Expression.Property(cast, prop);
            var box = Expression.Convert(propertyAccess, typeof(object));

            var addMethod = typeof(Dictionary<string, object?>)
                .GetMethod("Add")!;

            //this Item can fill from [DisplayName] attribute value
            var key = ToCamelCase(prop.Name);

            var addCall = Expression.Call(
                dictVar,
                addMethod,
                Expression.Constant(key),
                box);

            expressions.Add(addCall);
        }

        expressions.Add(dictVar);

        var body = Expression.Block(new[] { dictVar }, expressions);

        return Expression.Lambda<Func<object, Dictionary<string, object?>>>(body, param)
            .Compile();
    }

    private static string ToCamelCase(string name)
        => char.ToLowerInvariant(name[0]) + name[1..];
}
