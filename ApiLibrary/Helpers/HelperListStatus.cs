using ApiLibrary.Enums;

namespace ApiLibrary.Helpers
{
    public class HelperListStatus
    {
        public static List<string> GetGameStatusList()
        {
            return Enum.GetNames(typeof(GameStatus)).ToList();
        }
    }
}
