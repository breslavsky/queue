namespace Queue.Services.Common
{
    public class PortalClientServiceMap
    {
        public const string Index = "/";
        public const string Login = "/login?email={email}&password={password}";
        public const string ClientULogin = "/ulogin";
        public const string RestorePassword = "/restore-password?email={email}";
        public const string GetProfile = "/get-profile";
        public const string EditProfile = "/edit-profile?surname={surname}&name={name}&patronymic={patronymic}&mobile={mobile}";
        public const string GetRequests = "/get-requests";
        public const string CancelRequest = "/cancel-request?requestId={requestId}";
        public const string SendPINToEmail = "/send-pin-to-email?email={email}";
        public const string CheckPIN = "/check-pin?email={email}&PIN={PIN}";
        public const string Register = "/register?email={email}&PIN={PIN}&surname={surname}&name={name}&patronymic={patronymic}&mobile={mobile}";
        public const string GetRootServiceGroups = "/root-service-groups";
        public const string GetServiceGroups = "/service-group/{serviceGroupId}/child-groups";
        public const string GetRootServices = "/root-services";
        public const string GetServices = "/service-group/{serviceGroupId}/services";
        public const string GetServiceFreeTime = "/queue-plan/{planDate}/{queueType}/service/{serviceId}/free-time";
        public const string GetRequestCoupon = "/request/{requestId}/coupon";
        public const string AddRequest = "/add-request?serviceId={serviceId}&requestDate={requestDate}&requestTime={requestTime}&subjects={subjects}";
    }
}