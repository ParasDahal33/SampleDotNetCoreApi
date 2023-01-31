using System.Text;

namespace BusReservationSystemApi.Utils;

public static class ConfigUtils
{

    const string CONFIG_FILE = "local-config.txt";

    /// <summary>
    /// Gets/Sets the database connection string from local-config file.
    /// </summary>
    /// <returns></returns>
    public static string GetConnectionString(ConfigurationManager manager)
    {

        string connectionString = "";

        if (File.Exists(CONFIG_FILE))
        {
            connectionString = File.ReadAllLines(CONFIG_FILE).FirstOrDefault();
        }
        else
        {
            connectionString = manager.GetConnectionString("sqlServer");
        }

        return connectionString;
    }
}