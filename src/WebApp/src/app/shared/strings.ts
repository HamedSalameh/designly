export class Strings {
  // Global strings
  static Yes = $localize`:@@Yes:Yes`;
  static No = $localize`:@@No:No`;
  static Ok = $localize`:@@Ok:Ok`;
  static Cancel = $localize`:@@Cancel:Cancel`;
  static Save = $localize`:@@Save:Save`;
  static Edit = $localize`:@@Edit:Edit`;
  static Delete = $localize`:@@Delete:Delete`;
  static Share = $localize`:@@Share:Share`;
  static Close = $localize`:@@Close:Close`;

  static Logout = $localize`:@@Global.Logout:Logout`;
  static Login = $localize`:@@Global.Login:Login`;
  static Username = $localize`:@@Global.Username:Username`;
  static Password = $localize`:@@Global.Password:Password`;

  // Form validation
  static RequiredField = $localize`:@@Global.Validation.Required:Required`;

  // Global Basic Info
  static FirstName = $localize`:@@Global.BasicInfo.FirstName:FirstName`;
  static FamilyName = $localize`:@@Global.BasicInfo.FamilyName:FamilyName`;

  // Global Address Info
  static City = $localize`:@@Global.AddressInfo.City:City`;
  static Street = $localize`:@@Global.AddressInfo.Street:Street`;
  static BuildingNumber = $localize`:@@Global.AddressInfo.BuildingNumber:BuildingNumber`;
  static Address = $localize`:@@Global.AddressInfo.Address:Address`;
  static PrimaryPhoneNumer = $localize`:@@Global.ContactInfo.PrimaryPhoneNumber:PrimaryPhoneNumber`;
  static AddressLine1 = $localize`:@@Global.AddressInfo.AddressLine1:AddressLine1`;
  static EmailAddress = $localize`:@@Global.ContactInfo.EmailAddress:EmailAddress`;

  // Asset Info
  static AssetDetails = $localize`:@@Global.AssetInfo:AssetDetails`;
  static AreaMeasurementUnit = $localize`:@@Global.AssetInfo.AreaMeasurementUnit:AreaMeasurementUnit`;

  // Project Info
  static ProjectDetails = $localize`:@@Global.ProjectInfo:ProjectDetails`;
  static ProjectDescription = $localize`:@@Global.ProjectInfo.ProjectDescription:ProjectDescription`;
  static ProjectClient = $localize`:@@Global.ProjectInfo.ProjectClient:ProjectClient`;
  static ProjectLead = $localize`:@@Global.ProjectInfo.ProjectLead:ProjectLead`;
  static ProjectStartDate = $localize`:@@Global.ProjectInfo.ProjectStartDate:ProjectStartDate`;
  static ProjectDeadline = $localize`:@@Global.ProjectInfo.ProjectEndDate:ProjectDeadline`;

  // Message titles
  static MessageTitle_Error = $localize`:@@MessageTitle_Error:Error`;
  static MessageTitle_Warning = $localize`:@@MessageTitle_Warning:Warning`;
  static MessageTitle_Information = $localize`:@@MessageTitle_Information:Information`;
  static MessageTitle_Success = $localize`:@@MessageTitle_Success:Success`;

  // Message codes from server
  static messageFromCodeS001 = $localize`:@@messageFromCodeS001:CannotDeleteClient`;
  static messageFromCodeS002 = $localize`:@@messageFromCodeS002:RequiredField`;

  // Error messages
  static UnableToLoadClientsList = $localize`:@@Errors.Client.UnableToLoadClients:We were unable to load the clients list. Please try again.`;
  static UnableToLoadProjectsList = $localize`:@@Errors.Client.UnableToLoadClients:We were unable to load the projects list. Please try again.`;
  static UnableToLoadAccountUsers = $localize`:@@Errors.Client.UnableToLoadClients:We were unable to load the account users. Please try again.`;

  static BadRequest: string = $localize`:@@Errors.BadRequest:Your request is invalid.`;
  static Unauthorized: string = $localize`:@@Errors.Unauthorized:You are not authorized to perform this action.`;
  static Forbidden: string = $localize`:@@Errors.Forbidden:You are not authorized to perform this action.`;
  static NotFound: string = $localize`:@@Errors.NotFound:We were unable to find the requested resource.`;
  static InternalServerError = $localize`:@@Errors.InternalServerError:An error occurred while processing your request.`;
  static ServiceUnavailable = $localize`:@@Errors.ServiceUnavailable:The service is currently unavailable. Please try again later.`;
  static UnexpectedError = $localize`:@@Errors.UnexpectedError:An unexpected error occurred. Please try again later.`;
  static InvalidCredentials: string = $localize`:@@Errors.InvalidCredentials:Invalid username or password.`;
  static PasswordAttemptsExceeded: string = $localize`:@@Errors.PassweordAttemptsExceeded:Password attempts exceeded.`;
  static UnknownError: string = $localize`:@@Errors.UnknownError:An unknown error occurred.`;
  static NoAssetsFound: string = $localize`:@@Errors.NoAssetsFound:No assets found.`;
}
