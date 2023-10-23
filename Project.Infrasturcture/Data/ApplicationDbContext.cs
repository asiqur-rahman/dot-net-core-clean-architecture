using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.NameObjectCollectionBase;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using System.Xml.Linq;
using Project.Core.Entities.Common;

namespace Project.Infrasturcture.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, int>
    {
        private readonly ILoggerFactory _loggerFactory;
        protected ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ILoggerFactory loggerFactory) : base(options)
        {
            this._loggerFactory = loggerFactory;
        }

        //#region COMMON DBSETS
        //public DbSet<Point> Branches { get; set; }
        //public DbSet<Country> Countries { get; set; }
        //public DbSet<Division> Divisions { get; set; }
        //public DbSet<District> Districts { get; set; }
        //public DbSet<Thana> Thanas { get; set; }
        //public DbSet<Paurosava> Paurosavas { get; set; }
        //public DbSet<Union> Unions { get; set; }
        //public DbSet<Currency> Currencies { get; set; }
        //public DbSet<GlobalConfig> GlobalConfigs { get; set; }
        //public DbSet<ConfigValue> ConfigValues { get; set; }
        //public DbSet<MenuConfig> MenuConfigs { get; set; }
        //public DbSet<SolutionConfig> SolutionConfigs { get; set; }
        //public DbSet<OrganizationInformation> OrganizationInformation { get; set; }
        //public DbSet<Designation> Designations { get; set; }
        //public DbSet<AllBank> AllBank { get; set; }
        //public DbSet<BankBranches> BankBranches { get; set; }
        //public DbSet<NatureOfBusiness> NatureOfBusiness { get; set; }
        //public DbSet<Occupation> Occupations { get; set; }
        //public DbSet<UserAccess> UserAccess { get; set; }
        //public DbSet<UserPrivilege> UserPrivileges { get; set; }
        //public DbSet<RolePrivilege> RolePrivileges { get; set; }
        //public DbSet<GlobalConfigAuthorization> GlobalConfigAuth { get; set; }
        //public DbSet<UserAccessAuthorization> UserAccessAuth { get; set; }
        //public DbSet<UsersAuthorization> UsersAuth { get; set; }
        //public DbSet<BranchAuthorization> BranchAuth { get; set; }
        //public DbSet<LoginInfo> LoginInfos { get; set; }
        //public DbSet<LoginHistory> LoginHistories { get; set; }
        //public DbSet<AuditTrail> AuditTrails { get; set; }
        //public DbSet<Relationship> Relationships { get; set; }
        //public DbSet<Education> Education { get; set; }
        //public DbSet<Holiday> Holiday { get; set; }
        //public DbSet<Hyperlink> Hyperlinks { get; set; }
        //public DbSet<FileTemplate> FileTemplates { get; set; }
        //public DbSet<DocumentType> DocumentType { get; set; }
        //public DbSet<WorkFlowConfig> WorkFlowConfigs { get; set; }
        //public DbSet<WorkFlowHistory> WorkFlowHistories { get; set; }
        //public DbSet<StepLevel> StepLevel { get; set; }
        //public DbSet<WorkFlowPanel> WorkFlowPanels { get; set; }
        //public DbSet<WorkFlowFunction> WorkFlowFunction { get; set; }
        //public DbSet<Tasks> Tasks { get; set; }
        //public DbSet<Accessor> Accessor { get; set; }
        //public DbSet<AccessorTask> AccessorTask { get; set; }
        //public DbSet<AccessorPanel> AccessorPanel { get; set; }
        //public DbSet<Notification> Notifications { get; set; }
        //public DbSet<UserNotification> UserNotifications { get; set; }
        //public DbSet<AddressType> AddressTypes { get; set; }
        //public DbSet<Address> Addresses { get; set; }
        //public DbSet<ContactType> ContactTypes { get; set; }
        //public DbSet<Contact> Contacts { get; set; }
        //public DbSet<Vendor> Vendors { get; set; }
        //public DbSet<BulkTransaction> BulkTransaction { get; set; }
        //public DbSet<FundHoldingCategory> FundHoldingCategory { get; set; }
        //public DbSet<BalanceHolding> BalanceHolding { get; set; }
        //public DbSet<FundHoldingReviewHistory> FundHoldingReviewHistory { get; set; }
        //public DbSet<SchemeInformation> SchemeInformation { get; set; }
        //public DbSet<Taxation> Taxation { get; set; }
        //public DbSet<TaxationConfigure> TaxationConfigures { get; set; }
        //public DbSet<SavingsProductDefinition> SavingsProduct { get; set; }
        //public DbSet<Savings> Savings { get; set; }
        //public DbSet<SavingsRelatedParty> SavingsRelatedParty { get; set; }
        //public DbSet<Tenure> Tenure { get; set; }
        //public DbSet<ChargeDefinition> ChargeDefinition { get; set; }
        //public DbSet<CommissionDefinition> InitialDepositDefination { get; set; }
        //public DbSet<InterestDefinition> InterestDefinition { get; set; }
        ////public DbSet<SavingProductInterestConfig> SavingProductInterestConfig { get; set; }
        ////public DbSet<TransactionAmountWiseChargeConfiguration> TransactionAmountWiseChargeConfiguration { get; set; }
        ////public DbSet<SavingProductInterestConfig> SavingProductInterestConfig { get; set; }        

        //public DbSet<Commission> Commissions { get; set; }
        //public DbSet<CommissionDetails> CommissionDetails { get; set; }
        //public DbSet<NatureOfCharges> NatureOfCharges { get; set; }
        //public DbSet<CommissionAccrue> CommissionAccrue { get; set; }
        //public DbSet<CashRegister> CashRegister { get; set; }
        //public DbSet<RssFeed> RssFeed { get; set; }
        //public DbSet<RiskGradingType> RiskGradingType { get; set; }
        //public DbSet<RiskGrading> RiskGrading { get; set; }
        //public DbSet<RiskScoreGrading> RiskGradingScoring { get; set; }
        //public DbSet<PassportFee> PassportFee { get; set; }
        //public DbSet<ProductTransactionProfile> ProductTransactionProfile { get; set; }
        //public DbSet<TransactionProfileConfigMaster> TransactionProfileConfigMaster { get; set; }
        //public DbSet<TransactionProfileConfigDetails> TransactionProfileConfigDetails { get; set; }

        //public DbSet<CalendarConfig> CalendarConfig { get; set; }
        //public DbSet<WorkingDaysConfig> WorkingDaysConfig { get; set; }
        //public DbSet<SchedulerJob> SchedulerJobs { get; set; }
        //public DbSet<AccountOpeningWay> AccountOpeningWays { get; set; }
        //public DbSet<ApiBaseAddress> ApiBaseAddress { get; set; }
        //public DbSet<ApiConfig> ApiConfig { get; set; }

        //public DbSet<DynamicFieldRecord> DynamicFieldRecord { get; set; }
        //public DbSet<RMCode> RMCode { get; set; }
        //public DbSet<AffliationType> AffliationType { get; set; }
        //public DbSet<Language> Languages { get; set; }
        //public DbSet<StringResource> StringResources { get; set; }
        //public DbSet<City> Cities { get; set; }
        //public DbSet<ApplicationModule> ApplicationModule { get; set; }
        //public DbSet<UserModule> UserModule { get; set; }
        //public DbSet<AuthRecord> AuthRecord { get; set; }
        //public DbSet<ServiceRequestType> ServiceRequestType { get; set; }
        //public DbSet<ServiceRequest> ServiceRequest { get; set; }


        //#endregion

        //#region AGENT DBSETS
        //public DbSet<CustomerInformation> CustomerInformation { get; set; }
        ////public DbSet<ImageFile> ImageFiles { get; set; }
        //public DbSet<ProductType> ProductType { get; set; }
        //public DbSet<ProductCategory> ProductCategory { get; set; }
        //public DbSet<MandateOfAccountOperation> MandateOfAccountOperation { get; set; }
        //public DbSet<AccountInformation> AccountInformation { get; set; }
        //public DbSet<EntityAccountConfig> EntityAccountConfig { get; set; }
        //public DbSet<RequestedAccountNumber> RequestedAccountNumber { get; set; }
        //public DbSet<RelatedPartyType> RelatedPartyTypes { get; set; }
        //public DbSet<RelatedParty> RelatedParties { get; set; }
        //public DbSet<TransactionProfileType> TransactionProfileType { get; set; }
        //public DbSet<TransactionProfile> TransactionProfile { get; set; }
        //public DbSet<MiniKyc> MiniKyc { get; set; }
        //public DbSet<KycRiskFactor> KycRiskFactors { get; set; }
        //public DbSet<BankingService> Services { get; set; }
        //public DbSet<TransactionMedia> TransactionMedia { get; set; }
        //public DbSet<Transaction> Transactions { get; set; }
        //public DbSet<TransactionHistory> TransactionHistory { get; set; }
        //public DbSet<DailyTransactionHistoryNumeric> DailyTransactionHistoryNumeric { get; set; }
        //public DbSet<DailyTransactionHistoryVolume> DailyTransactionHistoryVolume { get; set; }
        //public DbSet<DepositOrWithdraw> DepositOrWithdraws { get; set; }
        //public DbSet<MobileRecharge> MobileRecharge { get; set; }
        //public DbSet<MFSRecords> MFSRecords { get; set; }
        //public DbSet<ZaynaxHealthRecords> ZaynaxHealthRecords { get; set; }
        //public DbSet<UtilityBill> UtilityBill { get; set; }
        //public DbSet<RemittanceInformation> RemittanceInformation { get; set; }
        //public DbSet<RemittanceCustomerInformation> RemittanceCustomerInformation { get; set; }
        //public DbSet<DayBasisInterest> DayBasisInterest { get; set; }
        //public DbSet<MaintenanceFeeCriteria> MaintenanceFeeCriteria { get; set; }
        //public DbSet<CustomerDailyAccrue> CustomerDailyAccrue { get; set; }
        //public DbSet<DailyInterestAccrue> DailyInterestAccrues { get; set; }
        //public DbSet<AgentDailyAccrue> AgentDailyAccrue { get; set; }
        //public DbSet<DayStartDayEnd> DayStartDayEnd { get; set; }
        //public DbSet<RelatedDocument> RelatedDocuments { get; set; }
        //public DbSet<FingerPrint> FingerPrints { get; set; }
        //public DbSet<CardType> CardTypes { get; set; }
        //public DbSet<CardTypeChoice> CardTypeChoices { get; set; }
        //public DbSet<ApplicationForm> ApplicationForms { get; set; }
        //public DbSet<DebitCardInformation> DebitCardInformation { get; set; }
        //public DbSet<GuardianInformation> GuardianInformation { get; set; }
        //public DbSet<ChequeBook> ChequeBooks { get; set; }
        //public DbSet<FeesCollection> FeesCollections { get; set; }
        //public DbSet<LanguageResources> LanguageResources { get; set; }
        //public DbSet<Module> Modules { get; set; }
        //public DbSet<ResourcePage> ResourcePages { get; set; }
        //public DbSet<Function> Functions { get; set; }
        //public DbSet<CustomerInformationChange> CustomerInformationChanges { get; set; }
        //public DbSet<ExchangeHouseConfig> ExchangeHouseConfigs { get; set; }
        //public DbSet<FcyTransaction> FcyTransactions { get; set; }
        //public DbSet<ReverseTransaction> ReverseTransaction { get; set; }
        //public DbSet<AtmTransactionDetail> AtmTransactionDetails { get; set; }
        //public DbSet<VendorType> VendorTypes { get; set; }
        //public DbSet<EFTRecords> EFTRecords { get; set; }
        //public DbSet<RTGSRecords> RTGSRecords { get; set; }
        //public DbSet<NPSBRecords> NPSBRecords { get; set; }

        //public DbSet<CommissionCalculationRules> ChargeCalculationRules { get; set; }
        //public DbSet<OutstandingTransaction> OutstandingTransaction { get; set; }
        //public DbSet<JointAccountInformation> JointAccountInformation { get; set; }
        //public DbSet<EntityCustomer> EntityCustomer { get; set; }
        //public DbSet<ChargeAndFees> ChargeAndFees { get; set; }
        //public DbSet<ChargeAndFeeConfiguration> ChargeAndFeeConfiguration { get; set; }
        //public DbSet<Product> Products { get; set; }
        //public DbSet<ProductCharge> ProductCharges { get; set; }
        //public DbSet<InterestConfig> ProductInterests { get; set; }
        //public DbSet<ProductCapitalization> ProductCapitalization { get; set; }
        //public DbSet<FeederAccountInfo> FeederAccountInfo { get; set; }
        //public DbSet<FeederAccountTransaction> FeederAccountTransaction { get; set; }
        //public DbSet<ChequeClearance> ChequeClearance { get; set; }
        //public DbSet<OtpData> OtpData { get; set; }
        //public DbSet<OtherBankAccountInfo> OtherBankAccountInfo { get; set; }
        //public DbSet<CreditCardInformation> CreditCardInformation { get; set; }
        //public DbSet<EmergencyContactPerson> EmergencyContactPerson { get; set; }
        //public DbSet<AccountStatementRecord> AccountStatementRecord { get; set; }
        //public DbSet<TieredRateCondition> TieredRateCondition { get; set; }
        //public DbSet<KycInformation> KycInformation { get; set; }
        //public DbSet<ProductDocument> ProductDocument { get; set; }
        //public DbSet<ProductDocumentConfig> ProductDocumentConfig { get; set; }
        //public DbSet<DispatchRecord> DispatchRecord { get; set; }
        //public DbSet<DispatchItem> DispatchItem { get; set; }
        //public DbSet<TypeOfOrganization> TypeOfOrganization { get; set; }
        //public DbSet<AuthTransactionProfile> AuthTransactionProfile { get; set; }
        //public DbSet<SectorCode> SectorCode { get; set; }
        //public DbSet<CbsAcBalance> CbsAcBalance { get; set; }
        //public DbSet<CbsTransactionSync> CbsTransactionSync { get; set; }
        //public DbSet<CustomerApiConfig> CustomerApiConfig { get; set; }
        //public DbSet<OtpMobileData> OtpMobileData { get; set; }

        //#endregion

        //#region AIS DBSETS

        //public DbSet<Ledger> Ledgers { get; set; }
        //public DbSet<Voucher> Vouchers { get; set; }
        //public DbSet<VoucherDetail> VoucherDetails { get; set; }
        //public DbSet<VoucherType> VoucherTypes { get; set; }

        //#endregion

        //#region LOAN DBSETS
        //public DbSet<LoanPurposeCategory> LoanPurposeCategories { get; set; }
        //public DbSet<LoanPurpose> LoanPurposes { get; set; }
        //public DbSet<LoanProductDefinition> LoanProducts { get; set; }
        //public DbSet<Loans> Loans { get; set; }
        //public DbSet<LoanAuditTrail> LoanAuditTrail { get; set; }
        //public DbSet<LoanEligibleSegment> LoanEligibleSegment { get; set; }
        //public DbSet<LoanDocument> LoanDocument { get; set; }
        //public DbSet<Limits> Limits { get; set; }
        //public DbSet<CreditLimits> CreditLimits { get; set; }
        //public DbSet<LoanRepaymentSchedule> LoanRepaymentSchedules { get; set; }
        //public DbSet<LoanPaymentProcessRecord> LoanPaymentProcessRecords { get; set; }
        //public DbSet<LoanReferences> LoanReferences { get; set; }
        //public DbSet<CreditExposure> CreditExposure { get; set; }
        //public DbSet<IncomeExpense> IncomeExpense { get; set; }
        //public DbSet<DocumentChecklist> DocumentChecklist { get; set; }
        //#endregion

        //#region Saving DBSETS
        //public DbSet<PaymentSize> PaymentSize { get; set; }
        //public DbSet<SavingsSchemeSchedule> SavingsSchemeSchedule { get; set; }
        //public DbSet<SavingsAuditTrail> SavingsAuditTrail { get; set; }
        //public DbSet<SavingsMonthlyInterestSchedule> SavingsMonthlyInterestSchedules { get; set; }
        //#endregion

        //#region DBQUERY
        //public DbQuery<AgentPointViewModel> AgentPointQuery { get; set; }
        //public DbQuery<BranchAndAccountTagging> BranchAndAccountTagging { get; set; }
        //public DbQuery<UserDto> UserAuthorization { get; set; }
        //public DbQuery<PendingAccountAndTransaction> PendingAccountAndTransactions { get; set; }
        //public DbQuery<PermittedUserViewModel> PermittedUserViewModels { get; set; }
        //public DbQuery<LoginInfoViewModel> LoginInfoViewModels { get; set; }
        //public DbQuery<VmCommissionConfiguration> CommissionCalculations { get; set; }
        //public DbQuery<VmCustomerInfo> VmCustomerInfo { get; set; }
        //public DbQuery<VmCustomerGeneralInfoIndividual> VmCustomerGeneralInfoIndividual { get; set; }
        //public DbQuery<VmCustomerAddressAndContact> VmCustomerAddressAndContact { get; set; }
        //public DbQuery<VmCustomerInformation> VmCustomerInformation { get; set; }
        //public DbQuery<VmAccountInformation> VmAccountInformation { get; set; }
        //public DbQuery<VmEntityAccountConfig> VmEntityAccountConfig { get; set; }
        //public DbQuery<VmTransactionData> VmTransactionData { get; set; }
        //public DbQuery<VmWithdrawalTransactionData> VmWithdrawalTransactionData { get; set; }
        //public DbQuery<VmLoginInfo> VmLoginInfos { get; set; }
        //public DbQuery<VmFilteredAccountInformation> VmFilteredAccountInformation { get; set; }
        //public DbQuery<VmFilteredCustomerInformation> VmFilteredCustomerInformation { get; set; }
        //public DbQuery<VmAgentDayEnd> VmAgentDayEnd { get; set; }
        //public DbQuery<VmFeesCollection> VmFeesCollections { get; set; }
        //public DbQuery<VmCustomerCommonInformation> VmCustomerCommonInformation { get; set; }
        //public DbQuery<VmLinkAccountList> VmLinkAccountList { get; set; }
        //public DbQuery<VmLoanInformation> VmLoanInformation { get; set; }
        //public DbQuery<VmLoanInformationForReschedule> VmLoanInformationForReschedule { get; set; }
        //public DbQuery<VmLoanInformationForPremature> VmLoanInformationForPremature { get; set; }
        //public DbQuery<VmSavingsInformationForPremature> VmSavingsInformationForPremature { get; set; }
        //public DbQuery<VmAgentPointUserWise> VmAgentPointDataUserWise { get; set; }
        //public DbQuery<VmAgentPointAccount> VmAgentPointAccount { get; set; }
        //public DbQuery<VmCommonBankingService> VmCommonBankingService { get; set; }
        //public DbQuery<VmCalendarConfig> VmCalendarConfig { get; set; }
        //public DbQuery<VmTransactionDataForExcise> VmTransactionDataForExcise { get; set; }
        //public DbQuery<VmUtilityBillGridData> VmUtilityBillGridData { get; set; }
        //public DbQuery<VmChequeBookGridData> VmChequeBookGridData { get; set; }
        //public DbQuery<VmCardGridData> VmCardGridData { get; set; }
        //public DbQuery<VmSavingsGridData> VmSavingsGridData { get; set; }
        //public DbQuery<VmRemittanceInformationGridData> VmRemittanceInformationGridData { get; set; }
        //public DbQuery<VmExchangeHouseInfo> VmExchangeHouseInfo { get; set; }
        //public DbQuery<VmUserCommonInfo> VmUserCommonInfo { get; set; }
        //public DbQuery<VmUniqueCustomerInformation> VmUniqueCustomer { get; set; }
        //public DbQuery<VmRiskGradingDdl> VmRiskGradingDdl { get; set; }
        //public DbQuery<VmContactNoCheck> VmContactNoCheck { get; set; }
        //public DbQuery<VmCustomerSteps> VmCustomerSteps { get; set; }
        //public DbQuery<VmCustomerListForScreening> VmCustomerListForScreening { get; set; }
        //public DbQuery<VmChangeCustInfos> VmChangeCustInfos { get; set; }
        //public DbQuery<VmAgentServiceCommission> AgentServiceCommissions { get; set; }
        //#endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(_loggerFactory);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
