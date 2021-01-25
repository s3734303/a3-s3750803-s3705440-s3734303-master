using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WDTAssignment02.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    CustomerID = table.Column<int>(nullable: false),
                    CustomerName = table.Column<string>(maxLength: 50, nullable: false),
                    TFN = table.Column<string>(maxLength: 11, nullable: true),
                    CustAddress = table.Column<string>(maxLength: 50, nullable: true),
                    CustCity = table.Column<string>(maxLength: 40, nullable: true),
                    CustState = table.Column<string>(maxLength: 3, nullable: true),
                    PostCode = table.Column<string>(maxLength: 4, nullable: true),
                    PhoneNum = table.Column<string>(maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.CustomerID);
                });

            migrationBuilder.CreateTable(
                name: "Payee",
                columns: table => new
                {
                    PayeeId = table.Column<int>(nullable: false),
                    PayeeName = table.Column<string>(maxLength: 50, nullable: false),
                    PayeeAddress = table.Column<string>(maxLength: 50, nullable: true),
                    PayeeCity = table.Column<string>(maxLength: 40, nullable: true),
                    PayeeState = table.Column<string>(maxLength: 3, nullable: true),
                    PostCode = table.Column<string>(maxLength: 4, nullable: true),
                    PhoneNum = table.Column<string>(maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payee", x => x.PayeeId);
                });

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    AccountNumber = table.Column<int>(nullable: false),
                    AccountType = table.Column<int>(nullable: false),
                    CustomerID = table.Column<int>(nullable: false),
                    Balance = table.Column<decimal>(type: "money", nullable: false),
                    ModifyDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.AccountNumber);
                    table.CheckConstraint("CH_Account_Balance", "Balance >= 0");
                    table.ForeignKey(
                        name: "FK_Account_Customer_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Login",
                columns: table => new
                {
                    UserID = table.Column<string>(maxLength: 8, nullable: false),
                    CustomerID = table.Column<int>(nullable: false),
                    Password = table.Column<string>(maxLength: 64, nullable: false),
                    ModifyDate = table.Column<DateTime>(nullable: false),
                    Flag = table.Column<int>(nullable: false),
                    LoginStatus = table.Column<bool>(nullable: false),
                    Timer = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Login", x => x.UserID);
                    table.CheckConstraint("CH_Login_UserID", "len(UserID) = 8");
                    table.CheckConstraint("CH_Login_Password", "len(Password) = 64");
                    table.ForeignKey(
                        name: "FK_Login_Customer_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BillPay",
                columns: table => new
                {
                    BillPayId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountNumber = table.Column<int>(nullable: false),
                    PayeeId = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(type: "money", nullable: false),
                    ScheduleDate = table.Column<DateTime>(nullable: false),
                    Period = table.Column<int>(nullable: false),
                    ModifyDate = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillPay", x => x.BillPayId);
                    table.ForeignKey(
                        name: "FK_BillPay_Account_AccountNumber",
                        column: x => x.AccountNumber,
                        principalTable: "Account",
                        principalColumn: "AccountNumber",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BillPay_Payee_PayeeId",
                        column: x => x.PayeeId,
                        principalTable: "Payee",
                        principalColumn: "PayeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    TransactionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionType = table.Column<int>(nullable: false),
                    AccountNumber = table.Column<int>(nullable: false),
                    DestinationAccountNumber = table.Column<int>(nullable: true),
                    Amount = table.Column<decimal>(type: "money", nullable: false),
                    Comment = table.Column<string>(maxLength: 250, nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.TransactionId);
                    table.CheckConstraint("CH_Transaction_Amount", "Amount > 0");
                    table.ForeignKey(
                        name: "FK_Transaction_Account_AccountNumber",
                        column: x => x.AccountNumber,
                        principalTable: "Account",
                        principalColumn: "AccountNumber",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transaction_Account_DestinationAccountNumber",
                        column: x => x.DestinationAccountNumber,
                        principalTable: "Account",
                        principalColumn: "AccountNumber",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_CustomerID",
                table: "Account",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_BillPay_AccountNumber",
                table: "BillPay",
                column: "AccountNumber");

            migrationBuilder.CreateIndex(
                name: "IX_BillPay_PayeeId",
                table: "BillPay",
                column: "PayeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Login_CustomerID",
                table: "Login",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_AccountNumber",
                table: "Transaction",
                column: "AccountNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_DestinationAccountNumber",
                table: "Transaction",
                column: "DestinationAccountNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BillPay");

            migrationBuilder.DropTable(
                name: "Login");

            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropTable(
                name: "Payee");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "Customer");
        }
    }
}
