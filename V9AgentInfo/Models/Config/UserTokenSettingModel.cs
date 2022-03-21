namespace V9AgentInfo.Models.Config
{
    public class UserTokenSettingModel
    {
        public int Expires { get; set; }
        public string Secret { get; set; }
        public string DefaultPassword { get; set; }
        public int OTPRecoverPassword { get; set; }
    }
}
