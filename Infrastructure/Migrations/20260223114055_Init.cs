using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DecisionGraphs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DecisionGraphs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DecisionGraphProps",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Field = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DecisionGraphId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DecisionGraphProps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DecisionGraphProps_DecisionGraphs_DecisionGraphId",
                        column: x => x.DecisionGraphId,
                        principalTable: "DecisionGraphs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DecisionStages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GraphId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Priority = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DecisionStages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DecisionStages_DecisionGraphs_GraphId",
                        column: x => x.GraphId,
                        principalTable: "DecisionGraphs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DecisionRules",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StageId = table.Column<long>(type: "bigint", nullable: false),
                    Priority = table.Column<byte>(type: "tinyint", nullable: false),
                    LogicalOperator = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DecisionRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DecisionRules_DecisionStages_StageId",
                        column: x => x.StageId,
                        principalTable: "DecisionStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StageConnection",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GraphId = table.Column<long>(type: "bigint", nullable: false),
                    FromStageId = table.Column<long>(type: "bigint", nullable: false),
                    ToStageId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StageConnection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StageConnection_DecisionStages_FromStageId",
                        column: x => x.FromStageId,
                        principalTable: "DecisionStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StageConnection_DecisionStages_ToStageId",
                        column: x => x.ToStageId,
                        principalTable: "DecisionStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DecisionOutputs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RuleId = table.Column<long>(type: "bigint", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DecisionOutputs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DecisionOutputs_DecisionRules_RuleId",
                        column: x => x.RuleId,
                        principalTable: "DecisionRules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DecisionRuleConditions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RuleId = table.Column<long>(type: "bigint", nullable: false),
                    PropertyId = table.Column<long>(type: "bigint", nullable: true),
                    Operator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DecisionRuleConditions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DecisionRuleConditions_DecisionGraphProps_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "DecisionGraphProps",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DecisionRuleConditions_DecisionRules_RuleId",
                        column: x => x.RuleId,
                        principalTable: "DecisionRules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DecisionGraphProps_DecisionGraphId",
                table: "DecisionGraphProps",
                column: "DecisionGraphId");

            migrationBuilder.CreateIndex(
                name: "IX_DecisionOutputs_RuleId",
                table: "DecisionOutputs",
                column: "RuleId");

            migrationBuilder.CreateIndex(
                name: "IX_DecisionRuleConditions_PropertyId",
                table: "DecisionRuleConditions",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_DecisionRuleConditions_RuleId",
                table: "DecisionRuleConditions",
                column: "RuleId");

            migrationBuilder.CreateIndex(
                name: "IX_DecisionRules_StageId",
                table: "DecisionRules",
                column: "StageId");

            migrationBuilder.CreateIndex(
                name: "IX_DecisionStages_GraphId",
                table: "DecisionStages",
                column: "GraphId");

            migrationBuilder.CreateIndex(
                name: "IX_StageConnection_FromStageId",
                table: "StageConnection",
                column: "FromStageId");

            migrationBuilder.CreateIndex(
                name: "IX_StageConnection_ToStageId",
                table: "StageConnection",
                column: "ToStageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DecisionOutputs");

            migrationBuilder.DropTable(
                name: "DecisionRuleConditions");

            migrationBuilder.DropTable(
                name: "StageConnection");

            migrationBuilder.DropTable(
                name: "DecisionGraphProps");

            migrationBuilder.DropTable(
                name: "DecisionRules");

            migrationBuilder.DropTable(
                name: "DecisionStages");

            migrationBuilder.DropTable(
                name: "DecisionGraphs");
        }
    }
}
