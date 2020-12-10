using Newtonsoft.Json;
using Plugin.Settings;
using WorkTimeSheet.Interfaces;
using WorkTimeSheet.Models;

namespace WorkTimeSheet.Settings
{
    public class UserSettings : IUserSettings
    {
        public Plugin.Settings.Abstractions.ISettings AppSettings => CrossSettings.Current;

        public AuthorizedUser AuthorizedUser
        {
            get
            {
                var authorizedUserJson = AppSettings.GetValueOrDefault(nameof(AuthorizedUser), string.Empty);
                if (string.IsNullOrEmpty(authorizedUserJson))
                    return null;
                return JsonConvert.DeserializeObject<AuthorizedUser>(authorizedUserJson);
            }
            set
            {
                AppSettings.AddOrUpdateValue(nameof(AuthorizedUser), JsonConvert.SerializeObject(value));
            }
        }
    }
}
