using Domain.Aggregates;
using Domain.Aggregates.Utils;

namespace WebApi.Controllers;

internal class MockGraph
{
    public static DecisionGraph BuildGraph()
    {
        var graph = new DecisionGraph("InsuranceGraph")
        {
            DecisionGraphProps = [.. DecisionGraphPropsFactory.CreateFromModel<InsuranceRequest>()]
        };
        CompleteGraph(graph);
        return graph;
    }

    public static DecisionGraph CompleteGraph(DecisionGraph graph)
    {
        //// =========================================================
        //// Stage 1 : eligibilityCheck
        //// =========================================================
        //var eligibility = graph.AddStage("eligibilityCheck");

        //var e1 = eligibility.AddRule(LogicalOperator.And);
        ////e1.AddCondition(">=", "18", graph.DecisionGraphProps.First(p => p.Field == "age").Id);
        //e1.AddCondition("age", ">=", "18", "Int32");
        //e1.AddCondition("hasChronicDisease", "==", "false", "Boolean");
        //e1.AddOutput("eligible", "true");
        //e1.AddOutput("eligibilityReason", "Adult & Healthy");

        //var e2 = eligibility.AddRule(LogicalOperator.And);
        //e2.AddCondition("age", "<", "18", "Int32");
        //e2.AddOutput("eligible", "false");
        //e2.AddOutput("eligibilityReason", "Too young");

        //var e3 = eligibility.AddRule(LogicalOperator.And);
        //e3.AddCondition("hasChronicDisease", "==", "true", "Boolean");
        //e3.AddOutput("eligible", "false");
        //e3.AddOutput("eligibilityReason", "Chronic illness");

        //var e4 = eligibility.AddRule(LogicalOperator.And);
        //e4.AddCondition("previousClaims", ">", "5", "Int32");
        //e4.AddOutput("eligible", "false");
        //e4.AddOutput("eligibilityReason", "Too many previous claims");

        //var e5 = eligibility.AddRule(LogicalOperator.And);
        //e5.AddOutput("eligible", "true");
        //e5.AddOutput("eligibilityReason", "Default eligibility");


        //// =========================================================
        //// Stage 2 : healthScore
        //// =========================================================
        //var health = graph.AddStage("healthScore");

        //var h1 = health.AddRule(LogicalOperator.And);
        //h1.AddCondition("bMI", "<", "25", "Int32");
        //h1.AddCondition("smoker", "==", "false", "Boolean");
        //h1.AddCondition("exerciseHoursPerWeek", ">=", "3", "Int32");
        //h1.AddOutput("healthScore", "Excellent");

        //var h2 = health.AddRule(LogicalOperator.And);
        //h2.AddCondition("bMI", "<", "28", "Int32");
        //h2.AddCondition("smoker", "==", "false", "Boolean");
        //h2.AddOutput("healthScore", "Good");

        //var h3 = health.AddRule(LogicalOperator.Or);
        //h3.AddCondition("bMI", ">=", "28", "Int32");
        //h3.AddCondition("smoker", "==", "true", "Boolean");
        //h3.AddOutput("healthScore", "Average");

        //var h4 = health.AddRule(LogicalOperator.And);
        //h4.AddCondition("bMI", ">=", "30", "Int32");
        //h4.AddCondition("smoker", "==", "true", "Boolean");
        //h4.AddOutput("healthScore", "Poor");

        //var h5 = health.AddRule(LogicalOperator.And);
        //h5.AddOutput("healthScore", "Unknown");


        //// =========================================================
        //// Stage 3 : riskFactors
        //// =========================================================
        //var risk = graph.AddStage("riskFactors");

        //var r1 = risk.AddRule(LogicalOperator.Or);
        //r1.AddCondition("healthScore", "==", "Poor", "String");
        //r1.AddCondition("highRiskAge", "==", "true", "Boolean");
        //r1.AddOutput("riskLevel", "High");

        //var r2 = risk.AddRule(LogicalOperator.And);
        //r2.AddCondition("healthScore", "==", "Average", "String");
        //r2.AddCondition("activeLifestyle", "==", "false", "Boolean");
        //r2.AddOutput("riskLevel", "Medium-High");

        //var r3 = risk.AddRule(LogicalOperator.And);
        //r3.AddCondition("healthScore", "==", "Good", "String");
        //r3.AddCondition("activeLifestyle", "==", "true", "Boolean");
        //r3.AddOutput("riskLevel", "Medium");

        //var r4 = risk.AddRule(LogicalOperator.And);
        //r4.AddCondition("healthScore", "==", "Excellent", "String");
        //r4.AddCondition("activeLifestyle", "==", "true", "Boolean");
        //r4.AddOutput("riskLevel", "Low");

        //var r5 = risk.AddRule(LogicalOperator.And);
        //r5.AddOutput("riskLevel", "Unknown");


        //// =========================================================
        //// Stage 4 : lifestyle
        //// =========================================================
        //var lifestyle = graph.AddStage("lifestyle");

        //var l1 = lifestyle.AddRule(LogicalOperator.And);
        //l1.AddCondition("smoker", "==", "true", "Boolean");
        //l1.AddCondition("alcoholConsumptionPerWeek", ">", "5", "Int32");
        //l1.AddOutput("lifestyleRisk", "Very High");

        //var l2 = lifestyle.AddRule(LogicalOperator.Or);
        //l2.AddCondition("smoker", "==", "true", "Boolean");
        //l2.AddCondition("alcoholConsumptionPerWeek", ">", "3", "Int32");
        //l2.AddOutput("lifestyleRisk", "High");

        //var l3 = lifestyle.AddRule(LogicalOperator.And);
        //l3.AddCondition("exerciseHoursPerWeek", ">=", "4", "Int32");
        //l3.AddCondition("smoker", "==", "false", "Boolean");
        //l3.AddOutput("lifestyleRisk", "Low");

        //var l4 = lifestyle.AddRule(LogicalOperator.And);
        //l4.AddCondition("exerciseHoursPerWeek", ">=", "2", "Int32");
        //l4.AddOutput("lifestyleRisk", "Medium");

        //var l5 = lifestyle.AddRule(LogicalOperator.And);
        //l5.AddOutput("lifestyleRisk", "Unknown");


        //// =========================================================
        //// Stage 5 : premiumCalculation
        //// =========================================================
        //var premium = graph.AddStage("premiumCalculation");

        //var p1 = premium.AddRule(LogicalOperator.Or);
        //p1.AddCondition("riskLevel", "==", "High", "String");
        //p1.AddCondition("lifestyleRisk", "==", "Very High", "String");
        //p1.AddOutput("premium", "1200");

        //var p2 = premium.AddRule(LogicalOperator.Or);
        //p2.AddCondition("riskLevel", "==", "Medium-High", "String");
        //p2.AddCondition("lifestyleRisk", "==", "High", "String");
        //p2.AddOutput("premium", "900");

        //var p3 = premium.AddRule(LogicalOperator.And);
        //p3.AddCondition("riskLevel", "==", "Medium", "String");
        //p3.AddCondition("lifestyleRisk", "==", "Medium", "String");
        //p3.AddOutput("premium", "600");

        //var p4 = premium.AddRule(LogicalOperator.And);
        //p4.AddCondition("riskLevel", "==", "Low", "String");
        //p4.AddCondition("lifestyleRisk", "==", "Low", "String");
        //p4.AddOutput("premium", "400");

        //var p5 = premium.AddRule(LogicalOperator.And);
        //p5.AddOutput("premium", "700");


        //// =========================================================
        //// Stage 6 : combinedRiskEvaluation
        //// =========================================================
        //var combined = graph.AddStage("combinedRiskEvaluation");

        //var c1 = combined.AddRule(LogicalOperator.And);
        //c1.AddCondition("riskLevel", "==", "High", "String");
        //c1.AddCondition("lifestyleRisk", "==", "Very High", "String");
        //c1.AddCondition("premium", ">=", "1000", "Decimal");
        //c1.AddOutput("combinedRisk", "Extreme");
        //c1.AddOutput("coverageLevel", "Basic");

        //var c2 = combined.AddRule(LogicalOperator.And);
        //c2.AddCondition("riskLevel", "==", "Medium-High", "String");
        //c2.AddCondition("lifestyleRisk", "==", "High", "String");
        //c2.AddCondition("premium", ">=", "900", "Decimal");
        //c2.AddOutput("combinedRisk", "High");
        //c2.AddOutput("coverageLevel", "Standard");

        //var c3 = combined.AddRule(LogicalOperator.And);
        //c3.AddCondition("riskLevel", "==", "Medium", "String");
        //c3.AddCondition("lifestyleRisk", "==", "Medium", "String");
        //c3.AddCondition("premium", ">=", "600", "Decimal");
        //c3.AddOutput("combinedRisk", "Moderate");
        //c3.AddOutput("coverageLevel", "Extended");

        //var c4 = combined.AddRule(LogicalOperator.And);
        //c4.AddCondition("riskLevel", "==", "Low", "String");
        //c4.AddCondition("lifestyleRisk", "==", "Low", "String");
        //c4.AddCondition("premium", "<=", "500", "Decimal");
        //c4.AddOutput("combinedRisk", "Low");
        //c4.AddOutput("coverageLevel", "Premium");

        //var c5 = combined.AddRule(LogicalOperator.And);
        //c5.AddOutput("combinedRisk", "Unknown");
        //c5.AddOutput("coverageLevel", "Standard");


        //// =========================================================
        //// Stage 7 : fraudCheck
        //// =========================================================
        //var fraud = graph.AddStage("fraudCheck");

        //var f1 = fraud.AddRule(LogicalOperator.And);
        //f1.AddCondition("previousClaims", ">", "5", "Int32");
        //f1.AddOutput("fraudFlag", "true");

        //var f2 = fraud.AddRule(LogicalOperator.And);
        //f2.AddCondition("familyMedicalHistoryScore", ">", "25", "Int32");
        //f2.AddCondition("combinedRisk", "==", "Extreme", "String");
        //f2.AddOutput("fraudFlag", "true");

        //var f3 = fraud.AddRule(LogicalOperator.And);
        //f3.AddCondition("previousClaims", "==", "0", "Int32");
        //f3.AddCondition("combinedRisk", "!=", "Extreme", "String");
        //f3.AddOutput("fraudFlag", "false");

        //var f4 = fraud.AddRule(LogicalOperator.And);
        //f4.AddCondition("smoker", "==", "true", "Boolean");
        //f4.AddCondition("alcoholConsumptionPerWeek", ">", "10", "Int32");
        //f4.AddOutput("fraudFlag", "true");

        //var f5 = fraud.AddRule(LogicalOperator.And);
        //f5.AddOutput("fraudFlag", "false");


        //// =========================================================
        //// Stage 8 : finalApproval
        //// =========================================================
        //var final = graph.AddStage("finalApproval");

        //var fa1 = final.AddRule(LogicalOperator.And);
        //fa1.AddCondition("eligible", "==", "true", "Boolean");
        //fa1.AddCondition("fraudFlag", "==", "false", "Boolean");
        //fa1.AddCondition("combinedRisk", "!=", "Extreme", "String");
        //fa1.AddOutput("approved", "true");

        //var fa2 = final.AddRule(LogicalOperator.And);
        //fa2.AddCondition("eligible", "==", "true", "Boolean");
        //fa2.AddCondition("fraudFlag", "==", "false", "Boolean");
        //fa2.AddCondition("combinedRisk", "==", "Extreme", "String");
        //fa2.AddCondition("premium", "<=", "1000", "Decimal");
        //fa2.AddOutput("approved", "true");

        //var fa3 = final.AddRule(LogicalOperator.And);
        //fa3.AddOutput("approved", "false");


        //// =========================================================
        //// Connections
        //// =========================================================

        ////graph.ConnectStages(eligibility.Id, health.Id);
        ////graph.ConnectStages(health.Id, risk.Id);
        ////graph.ConnectStages(risk.Id, lifestyle.Id);
        ////graph.ConnectStages(lifestyle.Id, premium.Id);

        ////graph.ConnectStages(risk.Id, combined.Id);
        ////graph.ConnectStages(lifestyle.Id, combined.Id);
        ////graph.ConnectStages(premium.Id, combined.Id);

        ////graph.ConnectStages(combined.Id, fraud.Id);
        ////graph.ConnectStages(fraud.Id, final.Id);

        return graph;
    }
}