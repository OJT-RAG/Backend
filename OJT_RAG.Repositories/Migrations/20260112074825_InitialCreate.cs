using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace OJT_RAG.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "documenttag",
                columns: table => new
                {
                    documenttag_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_documenttag", x => x.documenttag_id);
                });

            migrationBuilder.CreateTable(
                name: "job_title_overview",
                columns: table => new
                {
                    job_title_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    job_title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    position_amount = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job_title_overview", x => x.job_title_id);
                });

            migrationBuilder.CreateTable(
                name: "major",
                columns: table => new
                {
                    major_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    major_title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    major_code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    create_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    update_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_major", x => x.major_id);
                });

            migrationBuilder.CreateTable(
                name: "semester",
                columns: table => new
                {
                    semester_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    start_date = table.Column<DateOnly>(type: "date", nullable: true),
                    end_date = table.Column<DateOnly>(type: "date", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_semester", x => x.semester_id);
                });

            migrationBuilder.CreateTable(
                name: "user_chat_message",
                columns: table => new
                {
                    user_chat_message_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sender_id = table.Column<long>(type: "bigint", nullable: false),
                    receiver_id = table.Column<long>(type: "bigint", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    sent_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_chat_message", x => x.user_chat_message_id);
                });

            migrationBuilder.CreateTable(
                name: "company",
                columns: table => new
                {
                    company_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    majorid = table.Column<long>(type: "bigint", nullable: true),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    tax_code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    address = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    website = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    contact_email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    logo_url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    create_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    update_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_verified = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_company", x => x.company_id);
                    table.ForeignKey(
                        name: "FK_company_major_majorid",
                        column: x => x.majorid,
                        principalTable: "major",
                        principalColumn: "major_id");
                });

            migrationBuilder.CreateTable(
                name: "job_position",
                columns: table => new
                {
                    job_position_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    major_id = table.Column<long>(type: "bigint", nullable: true),
                    semester_id = table.Column<long>(type: "bigint", nullable: true),
                    job_title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    requirements = table.Column<string>(type: "text", nullable: true),
                    benefit = table.Column<string>(type: "text", nullable: true),
                    location = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    salary_range = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: true),
                    create_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    update_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job_position", x => x.job_position_id);
                    table.ForeignKey(
                        name: "FK_job_position_major_major_id",
                        column: x => x.major_id,
                        principalTable: "major",
                        principalColumn: "major_id");
                    table.ForeignKey(
                        name: "FK_job_position_semester_semester_id",
                        column: x => x.semester_id,
                        principalTable: "semester",
                        principalColumn: "semester_id");
                });

            migrationBuilder.CreateTable(
                name: "semester_company",
                columns: table => new
                {
                    semester_company_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    semester_id = table.Column<long>(type: "bigint", nullable: true),
                    company_id = table.Column<long>(type: "bigint", nullable: true),
                    approved_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_semester_company", x => x.semester_company_id);
                    table.ForeignKey(
                        name: "FK_semester_company_company_company_id",
                        column: x => x.company_id,
                        principalTable: "company",
                        principalColumn: "company_id");
                    table.ForeignKey(
                        name: "FK_semester_company_semester_semester_id",
                        column: x => x.semester_id,
                        principalTable: "semester",
                        principalColumn: "semester_id");
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    user_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    major_id = table.Column<long>(type: "bigint", nullable: true),
                    company_id = table.Column<long>(type: "bigint", nullable: true),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    password = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    role = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    fullname = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    student_code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    dob = table.Column<DateOnly>(type: "date", nullable: true),
                    phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    avatar_url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    cv_url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    create_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    update_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_User_company_company_id",
                        column: x => x.company_id,
                        principalTable: "company",
                        principalColumn: "company_id");
                    table.ForeignKey(
                        name: "FK_User_major_major_id",
                        column: x => x.major_id,
                        principalTable: "major",
                        principalColumn: "major_id");
                });

            migrationBuilder.CreateTable(
                name: "job_description",
                columns: table => new
                {
                    job_description_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    job_position_id = table.Column<long>(type: "bigint", nullable: true),
                    job_description = table.Column<string>(type: "text", nullable: true),
                    hire_quantity = table.Column<int>(type: "integer", nullable: true),
                    applied_quantity = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job_description", x => x.job_description_id);
                    table.ForeignKey(
                        name: "FK_job_description_job_position_job_position_id",
                        column: x => x.job_position_id,
                        principalTable: "job_position",
                        principalColumn: "job_position_id");
                });

            migrationBuilder.CreateTable(
                name: "chat_room",
                columns: table => new
                {
                    chat_room_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    chat_room_title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    create_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    update_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chat_room", x => x.chat_room_id);
                    table.ForeignKey(
                        name: "FK_chat_room_User_user_id",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "companydocument",
                columns: table => new
                {
                    companydocument_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    semester_company_id = table.Column<long>(type: "bigint", nullable: true),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    file_url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    uploaded_by = table.Column<long>(type: "bigint", nullable: true),
                    is_public = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_companydocument", x => x.companydocument_id);
                    table.ForeignKey(
                        name: "FK_companydocument_User_uploaded_by",
                        column: x => x.uploaded_by,
                        principalTable: "User",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "FK_companydocument_semester_company_semester_company_id",
                        column: x => x.semester_company_id,
                        principalTable: "semester_company",
                        principalColumn: "semester_company_id");
                });

            migrationBuilder.CreateTable(
                name: "finalreport",
                columns: table => new
                {
                    finalreport_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    job_position_id = table.Column<long>(type: "bigint", nullable: true),
                    semester_id = table.Column<long>(type: "bigint", nullable: true),
                    student_report_file = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    student_report_text = table.Column<string>(type: "text", nullable: true),
                    company_feedback = table.Column<string>(type: "text", nullable: true),
                    company_rating = table.Column<int>(type: "integer", nullable: true),
                    company_evaluator = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    submitted_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    evaluated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_finalreport", x => x.finalreport_id);
                    table.ForeignKey(
                        name: "FK_finalreport_User_user_id",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "FK_finalreport_job_position_job_position_id",
                        column: x => x.job_position_id,
                        principalTable: "job_position",
                        principalColumn: "job_position_id");
                    table.ForeignKey(
                        name: "FK_finalreport_semester_semester_id",
                        column: x => x.semester_id,
                        principalTable: "semester",
                        principalColumn: "semester_id");
                });

            migrationBuilder.CreateTable(
                name: "job_bookmark",
                columns: table => new
                {
                    job_bookmark_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    job_position_id = table.Column<long>(type: "bigint", nullable: true),
                    create_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job_bookmark", x => x.job_bookmark_id);
                    table.ForeignKey(
                        name: "FK_job_bookmark_User_user_id",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "FK_job_bookmark_job_position_job_position_id",
                        column: x => x.job_position_id,
                        principalTable: "job_position",
                        principalColumn: "job_position_id");
                });

            migrationBuilder.CreateTable(
                name: "ojtdocument",
                columns: table => new
                {
                    ojtdocument_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    file_url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    semester_id = table.Column<long>(type: "bigint", nullable: true),
                    is_general = table.Column<bool>(type: "boolean", nullable: true),
                    uploaded_by = table.Column<long>(type: "bigint", nullable: true),
                    uploaded_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ojtdocument", x => x.ojtdocument_id);
                    table.ForeignKey(
                        name: "FK_ojtdocument_User_uploaded_by",
                        column: x => x.uploaded_by,
                        principalTable: "User",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "FK_ojtdocument_semester_semester_id",
                        column: x => x.semester_id,
                        principalTable: "semester",
                        principalColumn: "semester_id");
                });

            migrationBuilder.CreateTable(
                name: "message",
                columns: table => new
                {
                    message_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    chat_room_id = table.Column<long>(type: "bigint", nullable: true),
                    content = table.Column<string>(type: "text", nullable: true),
                    from_ai = table.Column<bool>(type: "boolean", nullable: true),
                    useful = table.Column<bool>(type: "boolean", nullable: true),
                    sources = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_message", x => x.message_id);
                    table.ForeignKey(
                        name: "FK_message_chat_room_chat_room_id",
                        column: x => x.chat_room_id,
                        principalTable: "chat_room",
                        principalColumn: "chat_room_id");
                });

            migrationBuilder.CreateTable(
                name: "companydocumenttag",
                columns: table => new
                {
                    companydocumenttag_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    companydocument_id = table.Column<long>(type: "bigint", nullable: false),
                    documenttag_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_companydocumenttag", x => x.companydocumenttag_id);
                    table.ForeignKey(
                        name: "FK_companydocumenttag_companydocument_companydocument_id",
                        column: x => x.companydocument_id,
                        principalTable: "companydocument",
                        principalColumn: "companydocument_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_companydocumenttag_documenttag_documenttag_id",
                        column: x => x.documenttag_id,
                        principalTable: "documenttag",
                        principalColumn: "documenttag_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ojtdocumenttag",
                columns: table => new
                {
                    ojtdocumenttag_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ojtdocument_id = table.Column<long>(type: "bigint", nullable: false),
                    documenttag_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ojtdocumenttag", x => x.ojtdocumenttag_id);
                    table.ForeignKey(
                        name: "FK_ojtdocumenttag_documenttag_documenttag_id",
                        column: x => x.documenttag_id,
                        principalTable: "documenttag",
                        principalColumn: "documenttag_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ojtdocumenttag_ojtdocument_ojtdocument_id",
                        column: x => x.ojtdocument_id,
                        principalTable: "ojtdocument",
                        principalColumn: "ojtdocument_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_chat_room_user_id",
                table: "chat_room",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_company_majorid",
                table: "company",
                column: "majorid");

            migrationBuilder.CreateIndex(
                name: "IX_companydocument_semester_company_id",
                table: "companydocument",
                column: "semester_company_id");

            migrationBuilder.CreateIndex(
                name: "IX_companydocument_uploaded_by",
                table: "companydocument",
                column: "uploaded_by");

            migrationBuilder.CreateIndex(
                name: "IX_companydocumenttag_companydocument_id",
                table: "companydocumenttag",
                column: "companydocument_id");

            migrationBuilder.CreateIndex(
                name: "IX_companydocumenttag_documenttag_id",
                table: "companydocumenttag",
                column: "documenttag_id");

            migrationBuilder.CreateIndex(
                name: "IX_finalreport_job_position_id",
                table: "finalreport",
                column: "job_position_id");

            migrationBuilder.CreateIndex(
                name: "IX_finalreport_semester_id",
                table: "finalreport",
                column: "semester_id");

            migrationBuilder.CreateIndex(
                name: "IX_finalreport_user_id",
                table: "finalreport",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_job_bookmark_job_position_id",
                table: "job_bookmark",
                column: "job_position_id");

            migrationBuilder.CreateIndex(
                name: "IX_job_bookmark_user_id",
                table: "job_bookmark",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_job_description_job_position_id",
                table: "job_description",
                column: "job_position_id");

            migrationBuilder.CreateIndex(
                name: "IX_job_position_major_id",
                table: "job_position",
                column: "major_id");

            migrationBuilder.CreateIndex(
                name: "IX_job_position_semester_id",
                table: "job_position",
                column: "semester_id");

            migrationBuilder.CreateIndex(
                name: "IX_message_chat_room_id",
                table: "message",
                column: "chat_room_id");

            migrationBuilder.CreateIndex(
                name: "IX_ojtdocument_semester_id",
                table: "ojtdocument",
                column: "semester_id");

            migrationBuilder.CreateIndex(
                name: "IX_ojtdocument_uploaded_by",
                table: "ojtdocument",
                column: "uploaded_by");

            migrationBuilder.CreateIndex(
                name: "IX_ojtdocumenttag_documenttag_id",
                table: "ojtdocumenttag",
                column: "documenttag_id");

            migrationBuilder.CreateIndex(
                name: "IX_ojtdocumenttag_ojtdocument_id",
                table: "ojtdocumenttag",
                column: "ojtdocument_id");

            migrationBuilder.CreateIndex(
                name: "IX_semester_company_company_id",
                table: "semester_company",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_semester_company_semester_id",
                table: "semester_company",
                column: "semester_id");

            migrationBuilder.CreateIndex(
                name: "IX_User_company_id",
                table: "User",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_User_major_id",
                table: "User",
                column: "major_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "companydocumenttag");

            migrationBuilder.DropTable(
                name: "finalreport");

            migrationBuilder.DropTable(
                name: "job_bookmark");

            migrationBuilder.DropTable(
                name: "job_description");

            migrationBuilder.DropTable(
                name: "job_title_overview");

            migrationBuilder.DropTable(
                name: "message");

            migrationBuilder.DropTable(
                name: "ojtdocumenttag");

            migrationBuilder.DropTable(
                name: "user_chat_message");

            migrationBuilder.DropTable(
                name: "companydocument");

            migrationBuilder.DropTable(
                name: "job_position");

            migrationBuilder.DropTable(
                name: "chat_room");

            migrationBuilder.DropTable(
                name: "documenttag");

            migrationBuilder.DropTable(
                name: "ojtdocument");

            migrationBuilder.DropTable(
                name: "semester_company");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "semester");

            migrationBuilder.DropTable(
                name: "company");

            migrationBuilder.DropTable(
                name: "major");
        }
    }
}
