using Blazored.LocalStorage;

namespace ClientLib.Helpers
{
    public class LocalStorageService(ILocalStorageService localStorage)
    {
        private const string StorageKey = "BlazexStorage";
        public async Task<string> GetToken() => await localStorage.GetItemAsStringAsync(StorageKey);

        public async Task SetToken(string token) => await localStorage.SetItemAsStringAsync(StorageKey, token);

        public async Task RemoveToken() => await localStorage.RemoveItemAsync(StorageKey);
    }
}
