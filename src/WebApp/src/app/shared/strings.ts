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

  static Unauthorized: string = $localize`:@@Errors.Unauthorized:You are not authorized to perform this action.`;
  static Forbidden: string = $localize`:@@Errors.Forbidden:You are not authorized to perform this action.`;
  static NotFound: string = $localize`:@@Errors.NotFound:We were unable to find the requested resource.`;
  static InternalServerError = $localize`:@@Errors.InternalServerError:An error occurred while processing your request.`;
  static ServiceUnavailable = $localize`:@@Errors.ServiceUnavailable:The service is currently unavailable. Please try again later.`;
  static UnexpectedError = $localize`:@@Errors.UnexpectedError:An unexpected error occurred. Please try again later.`;
}
