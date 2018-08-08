namespace MyWallet.Common
{
	using System.Collections;
	using System.Collections.Generic;
	using System.Configuration;
	using System.Linq;

	/// <summary>
	/// Class which working with configuration settings.
	/// </summary>
	public class ConfigurationHelper
	{

		#region Methods: Public

		/// <summary>
		/// Get setting value from app settings section.
		/// </summary>
		/// <param name="setting">Setting name.</param>
		/// <returns>Setting value.</returns>
		public static string GetSettingValue(string setting) {
			return ConfigurationManager.AppSettings[setting];
		}

		/// <summary>
		/// Get whole configuration section.
		/// </summary>
		/// <param name="sectionName">Name of the section.</param>
		/// <returns>Dictionary with configuration section.</returns>
		public static Dictionary<string, string> GetConfigSection(string sectionName) {
			var hashTable = ConfigurationManager.GetSection(sectionName) as Hashtable;
			return hashTable?
				.Cast<DictionaryEntry>()
				.ToDictionary(entry => entry.Key.ToString(),
					entry => entry.Value.ToString()) ?? new Dictionary<string, string>();
		}

		#endregion

	}
}