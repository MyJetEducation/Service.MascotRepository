using MyJetWallet.Sdk.Service;
using MyYamlParser;

namespace Service.UserMascotRepository.Settings
{
	public class SettingsModel
	{
		[YamlProperty("UserMascotRepository.SeqServiceUrl")]
		public string SeqServiceUrl { get; set; }

		[YamlProperty("UserMascotRepository.ZipkinUrl")]
		public string ZipkinUrl { get; set; }

		[YamlProperty("UserMascotRepository.ElkLogs")]
		public LogElkSettings ElkLogs { get; set; }

		[YamlProperty("UserMascotRepository.ServerKeyValueServiceUrl")]
		public string ServerKeyValueServiceUrl { get; set; }

		[YamlProperty("UserMascotRepository.KeyUserMascot")]
		public string KeyUserMascot { get; set; }

		[YamlProperty("UserMascotRepository.ServiceBusReader")]
		public string ServiceBusReader { get; set; }
	}
}